// Copyright (c) Speykious
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 2-Clause License. See LICENSE for details.

namespace SeeShark.Linux;

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using SeeShark.Camera;
using SeeShark.Interop.Libc;
using static SeeShark.Interop.Libc.Libc;

[SupportedOSPlatform("Linux")]
public static class V4l2
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
                captureDevices.Add(new CameraPath { Path = path });

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
                    fillFrameIntervalFormats(formats, deviceFd, inputFormat, discrete.width, discrete.height);
                }
                else
                {
                    v4l2_frmsize_stepwise stepwise = frameSize.frame_size.stepwise;
                    for (uint width = stepwise.min_width; width < stepwise.max_width; width += stepwise.step_width)
                    {
                        for (uint height = stepwise.min_height; height < stepwise.max_height; height += stepwise.step_height)
                            fillFrameIntervalFormats(formats, deviceFd, inputFormat, width, height);
                    }
                }
                frameSize.index++;
            }
        }

        close(deviceFd);
        return formats;
    }

    private static unsafe void fillFrameIntervalFormats(List<VideoFormat> formats, int deviceFd, V4l2InputFormat inputFormat, uint width, uint height)
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
                    ImageFormat = new ImageFormat((uint)inputFormat),
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

    #region Open
    internal unsafe struct ReqBuffer
    {
        internal byte* Ptr { get; private set; }
        internal uint Length { get; private set; }

        internal ReqBuffer(ref v4l2_buffer vbuf, int deviceFd)
        {
            MmapProtFlags mmp = MmapProtFlags.PROT_READ | MmapProtFlags.PROT_WRITE;
            byte* ptr = (byte*)mmap(null, (int)vbuf.length, mmp, MmapFlags.MAP_SHARED, deviceFd, (int)vbuf.m.offset);

            if ((nint)ptr == -1)
                throw new Exception($"Could not allocate request buffer");

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

    internal static unsafe LinuxCameraDevice OpenCamera(CameraPath cameraInfo, VideoFormatOptions options)
    {
        int deviceFd = open(cameraInfo.Path, FileOpenFlags.O_RDWR);
        if (deviceFd < 0)
            throw new IOException($"Cannot open camera {cameraInfo}");

        // get a suited pixel format
        V4l2InputFormat pixelFormat;

        if (options.ImageFormat is ImageFormat imageFormatOption)
        {
            pixelFormat = (V4l2InputFormat)imageFormatOption.FourCC;
        }
        else
        {
            // default to the first pixel format when not specified
            v4l2_fmtdesc formatDesc = new v4l2_fmtdesc
            {
                type = v4l2_buf_type.V4L2_BUF_TYPE_VIDEO_CAPTURE,
                index = 0,
                flags = 0,
                pixelformat = 0,
            };

            if (V4l2.Xioctl(deviceFd, Ioctl.VidIOC.ENUM_FMT, &formatDesc) == false)
                throw new IOException($"Cannot get video format for camera {cameraInfo}: {Marshal.GetLastWin32Error()}");

            pixelFormat = formatDesc.pixelformat;
        }

        // set video format
        v4l2_format format = new v4l2_format
        {
            type = v4l2_buf_type.V4L2_BUF_TYPE_VIDEO_CAPTURE,
            fmt = new v4l2_format.fmt_union
            {
                pix = new v4l2_pix_format
                {
                    width = 1280,
                    height = 720,
                    pixelformat = pixelFormat,
                    field = v4l2_field.V4L2_FIELD_NONE,
                }
            }
        };

        if (V4l2.Xioctl(deviceFd, Ioctl.VidIOC.S_FMT, &format) == false)
            throw new IOException($"Cannot set video format for camera {cameraInfo}");

        // get a suited framerate
        FramerateRatio framerate;

        // Helpful documentation on G_PARM and S_PARM:
        // https://www.kernel.org/doc/html/latest/userspace-api/media/v4l/vidioc-g-parm.html
        if (options.Framerate is FramerateRatio framerateOption)
        {
            // set framerate
            v4l2_streamparm parameter = new v4l2_streamparm
            {
                type = v4l2_buf_type.V4L2_BUF_TYPE_VIDEO_CAPTURE,
                parm = new v4l2_streamparm.parm_union
                {
                    capture = new v4l2_captureparm
                    {
                        timeperframe = new v4l2_fract
                        {
                            numerator = framerateOption.Denominator,
                            denominator = framerateOption.Numerator,
                        }
                    }
                }
            };

            if (Xioctl(deviceFd, Ioctl.VidIOC.S_PARM, &parameter) == false)
                throw new IOException($"Cannot set video format for camera {cameraInfo}");

            v4l2_fract timeperframe = parameter.parm.capture.timeperframe;
            framerate = new FramerateRatio
            {
                Numerator = timeperframe.denominator,
                Denominator = timeperframe.numerator,
            };
        }
        else
        {
            // get framerate
            v4l2_streamparm parameter = new v4l2_streamparm
            {
                type = v4l2_buf_type.V4L2_BUF_TYPE_VIDEO_CAPTURE,
            };

            if (Xioctl(deviceFd, Ioctl.VidIOC.G_PARM, &parameter) == false)
                throw new IOException($"Cannot set video format for camera {cameraInfo}");

            v4l2_fract timeperframe = parameter.parm.capture.timeperframe;
            framerate = new FramerateRatio
            {
                Numerator = timeperframe.denominator,
                Denominator = timeperframe.numerator,
            };
        }

        v4l2_pix_format pix = format.fmt.pix;
        Console.WriteLine("Format:");
        Console.WriteLine($"| width     : {pix.width}");
        Console.WriteLine($"| height    : {pix.height}");
        Console.WriteLine($"| format    : {pix.pixelformat}");
        Console.WriteLine($"| field     : {pix.field}");
        Console.WriteLine($"| framerate : {framerate.Value}");

        VideoFormat videoFormat = new VideoFormat
        {
            VideoSize = (pix.width, pix.height),
            VideoPosition = (0, 0),
            Framerate = framerate,
            ImageFormat = new ImageFormat((uint)pix.pixelformat),
            DrawMouse = false,
        };

        // request buffers
        v4l2_requestbuffers requestBuffers = new v4l2_requestbuffers
        {
            type = v4l2_buf_type.V4L2_BUF_TYPE_VIDEO_CAPTURE,
            memory = v4l2_memory.V4L2_MEMORY_MMAP,
            count = 4,
        };

        if (Xioctl(deviceFd, Ioctl.VidIOC.REQBUFS, &requestBuffers) == false)
            throw new IOException($"Cannot request buffers for camera {cameraInfo}");

        if (requestBuffers.count < 2)
            throw new IOException($"Didn't get enough buffers");

        ReqBuffer[] buffers = new ReqBuffer[requestBuffers.count];
        for (uint i = 0; i < requestBuffers.count; i++)
        {
            v4l2_buffer vbuf = new v4l2_buffer
            {
                type = requestBuffers.type,
                memory = v4l2_memory.V4L2_MEMORY_MMAP,
                index = i,
            };

            if (V4l2.Xioctl(deviceFd, Ioctl.VidIOC.QUERYBUF, &vbuf) == false)
                throw new IOException($"Could not query buffer for camera {cameraInfo}");

            buffers[i] = new ReqBuffer(ref vbuf, deviceFd);
        }

        return new LinuxCameraDevice
        {
            Path = cameraInfo,
            DeviceFd = deviceFd,
            CurrentFormat = videoFormat,
            RequestBuffers = requestBuffers,
            Buffers = buffers,
        };
    }
    #endregion

    internal static unsafe bool Xioctl(int fd, Ioctl.VidIOC request, void* argp)
    {
        int r = -1;

        do
        {
            r = ioctl(fd, request, argp);
        } while (-1 == r && Marshal.GetLastWin32Error() == EINTR);

        return r != -1;
    }
}
