// Copyright (c) Speykious
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 2-Clause License. See LICENSE for details.

namespace SeeShark.Linux;

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using SeeShark.Camera;
using SeeShark.Interop.Linux.Libc;
using static SeeShark.Interop.Linux.Libc.Libc;

[SupportedOSPlatform("Linux")]
internal static class V4l2
{
    // Helpful resource and reference: https://github.com/kmdouglass/v4l2-examples

    internal static unsafe List<CameraPath> AvailableCameras()
    {
        var captureDevices = new List<CameraPath>();

        v4l2_capability capability;

        foreach (string path in Directory.EnumerateFiles("/dev/"))
        {
            int deviceFd = open(path, FileOpenFlags.O_RDWR);
            if (deviceFd < 0)
                continue;

            if (Xioctl(deviceFd, Ioctl.VidIOC.QUERYCAP, &capability))
            {
                string card = Encoding.ASCII.GetString(capability.card, 32);
                string bus = Encoding.ASCII.GetString(capability.bus_info, 32);
                string name = $"{card} ({bus})";

                captureDevices.Add(new CameraPath { Path = path, Name = name });
            }

            close(deviceFd);
        }

        return captureDevices;
    }

    /// <summary>
    /// Get available video input options of a V4l2 device.
    /// Inspired from https://github.com/ZhangGaoxing/v4l2.net
    /// </summary>
    internal static unsafe List<VideoFormat> AvailableFormats(CameraPath device)
    {
        List<VideoFormat> formats = new List<VideoFormat>();

        int deviceFd = open(device.Path, FileOpenFlags.O_RDWR);
        if (deviceFd < 0)
            throw new IOException($"Error {Marshal.GetLastWin32Error()}: Cannot open video device {device}");

        v4l2_fmtdesc formatDesc = new v4l2_fmtdesc
        {
            index = 0,
            type = v4l2_buf_type.V4L2_BUF_TYPE_VIDEO_CAPTURE
        };

        List<V4l2InputFormat> supportedInputFormats = new List<V4l2InputFormat>();
        while (Xioctl(deviceFd, Ioctl.VidIOC.ENUM_FMT, &formatDesc))
        {
            supportedInputFormats.Add(formatDesc.pixelformat);
            formatDesc.index++;
        }

        foreach (V4l2InputFormat inputFormat in supportedInputFormats)
        {
            ImageFormat? maybeImageFormat = V4l2InputFormatIntoImageFormat(inputFormat);
            if (maybeImageFormat is not ImageFormat imageFormat)
                continue;

            v4l2_frmsizeenum frameSize = new v4l2_frmsizeenum
            {
                index = 0,
                pixel_format = inputFormat,
            };

            while (Xioctl(deviceFd, Ioctl.VidIOC.ENUM_FRAMESIZES, &frameSize))
            {
                if (frameSize.type == v4l2_frmsizetypes.V4L2_FRMSIZE_TYPE_DISCRETE)
                {
                    v4l2_frmsize_discrete discrete = frameSize.frame_size.discrete;
                    fillFrameIntervalFormats(formats, deviceFd, imageFormat, inputFormat, discrete.width, discrete.height);
                }
                else
                {
                    v4l2_frmsize_stepwise stepwise = frameSize.frame_size.stepwise;
                    for (uint width = stepwise.min_width; width < stepwise.max_width; width += stepwise.step_width)
                    {
                        for (uint height = stepwise.min_height; height < stepwise.max_height; height += stepwise.step_height)
                            fillFrameIntervalFormats(formats, deviceFd, imageFormat, inputFormat, width, height);
                    }
                }
                frameSize.index++;
            }
        }

        close(deviceFd);
        return formats;
    }

    private static unsafe void fillFrameIntervalFormats(List<VideoFormat> formats, int deviceFd, ImageFormat imageFormat, V4l2InputFormat inputFormat, uint width, uint height)
    {
        v4l2_frmivalenum frameInterval = new v4l2_frmivalenum
        {
            index = 0,
            pixel_format = inputFormat,
            width = width,
            height = height,
        };

        while (Xioctl(deviceFd, Ioctl.VidIOC.ENUM_FRAMEINTERVALS, &frameInterval))
        {
            if (frameInterval.type == v4l2_frmivaltypes.V4L2_FRMIVAL_TYPE_DISCRETE)
            {
                v4l2_fract intervalRatio = frameInterval.frame_interval.discrete;
                formats.Add(new VideoFormat
                {
                    ImageFormat = imageFormat,
                    VideoSize = (width, height),
                    VideoPosition = (0, 0),
                    Framerate = new FramerateRatio
                    {
                        Numerator = intervalRatio.denominator,
                        Denominator = intervalRatio.numerator,
                    },
                });
            }
            frameInterval.index++;
        }
    }

    internal unsafe struct ReqBuffer
    {
        internal byte* Ptr { get; private set; }
        internal uint Length { get; private set; }

        internal ReqBuffer(ref v4l2_buffer vbuf, int deviceFd)
        {
            MmapProtFlags mmp = MmapProtFlags.PROT_READ | MmapProtFlags.PROT_WRITE;
            byte* ptr = (byte*)mmap(null, (int)vbuf.length, mmp, MmapFlags.MAP_SHARED, deviceFd, (int)vbuf.m.offset);

            if ((nint)ptr == -1)
                throw new Exception("Cannot allocate request buffer");

            Ptr = ptr;
            Length = vbuf.length;
        }

        internal void Free()
        {
            if (Ptr == null)
                return;

            munmap(Ptr, (int)Length);
            Ptr = null;
            Length = 0;
        }
    }

    internal static unsafe bool Xioctl(int fd, Ioctl.VidIOC request, void* argp)
    {
        int r;

        do
        {
            r = ioctl(fd, request, argp);
        } while (-1 == r && Marshal.GetLastWin32Error() == EINTR);

        return r != -1;
    }

    internal static ImageFormat? V4l2InputFormatIntoImageFormat(V4l2InputFormat inputFormat)
    {
        return inputFormat switch
        {
            V4l2InputFormat.ARGB32 => ImageFormat.ARGB_32,
            V4l2InputFormat.BGRA32 => ImageFormat.BGRA_32,
            V4l2InputFormat.ABGR32 => ImageFormat.ABGR_32,
            V4l2InputFormat.RGBA32 => ImageFormat.RGBA_32,

            V4l2InputFormat.YUYV => ImageFormat.YUYV,
            V4l2InputFormat.YVYU => ImageFormat.YVYU,
            V4l2InputFormat.UYVY => ImageFormat.UYVY,
            V4l2InputFormat.Y41P => ImageFormat.Y41P,
            V4l2InputFormat.YUV444 => ImageFormat.YUV_444,

            V4l2InputFormat.MJPEG => ImageFormat.MJPG,
            _ => null,
        };
    }

    internal static V4l2InputFormat? ImageFormatIntoV4l2InputFormat(ImageFormat imageFormat)
    {
        return imageFormat switch
        {
            ImageFormat.ARGB_32 => V4l2InputFormat.ARGB32,
            ImageFormat.BGRA_32 => V4l2InputFormat.BGRA32,
            ImageFormat.ABGR_32 => V4l2InputFormat.ABGR32,
            ImageFormat.RGBA_32 => V4l2InputFormat.RGBA32,

            ImageFormat.YUYV => V4l2InputFormat.YUYV,
            ImageFormat.YVYU => V4l2InputFormat.YVYU,
            ImageFormat.UYVY => V4l2InputFormat.UYVY,
            ImageFormat.Y41P => V4l2InputFormat.Y41P,
            ImageFormat.YUV_444 => V4l2InputFormat.YUV444,

            ImageFormat.MJPG => V4l2InputFormat.MJPEG,
            _ => null,
        };
    }

    internal static ImageFormat V4l2InputFormatIntoImageFormat_OrThrowIfUnsupported(V4l2InputFormat inputFormat)
    {
        ImageFormat? imageFormat = V4l2InputFormatIntoImageFormat(inputFormat);
        if (imageFormat is ImageFormat format)
            return format;
        else
            throw new ImageFormatNotSupportedException($"SeeShark does not support Linux input format {inputFormat}");
    }

    internal static V4l2InputFormat ImageFormatIntoV4l2InputFormat_OrThrowIfUnsupported(ImageFormat imageFormat)
    {
        V4l2InputFormat? inputFormat = ImageFormatIntoV4l2InputFormat(imageFormat);
        if (inputFormat is V4l2InputFormat format)
            return format;
        else
            throw new ImageFormatNotSupportedException($"SeeShark does not have a Linux counterpart for image format {imageFormat}");
    }
}
