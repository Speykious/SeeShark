// Copyright (c) Speykious
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 3-Clause License. See LICENSE for details.

namespace SeeShark.Linux;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using SeeShark.Interop.Libc;
using static SeeShark.Interop.Libc.Libc;

public static class V4l2
{
    // Helpful resource and reference: https://github.com/kmdouglass/v4l2-examples

    internal static unsafe List<CameraInfo> AvailableCameras()
    {
        var captureDevices = new List<CameraInfo>();

        v4l2_capability capability;

        foreach (string path in Directory.EnumerateFiles("/dev/"))
        {
            int deviceFd = open(path, FileOpenFlags.O_RDWR);
            if (deviceFd < 0)
                continue;

            if (xioctl(deviceFd, Ioctl.VidIOC.QUERYCAP, &capability))
                captureDevices.Add(new CameraInfo { Path = path });

            close(deviceFd);
        }

        return captureDevices;
    }

    /// <summary>
    /// Get available video input options of a V4l2 device.
    /// Inspired from https://github.com/ZhangGaoxing/v4l2.net
    /// </summary>
    internal static unsafe List<VideoInputOptions> AvailableInputOptions(CameraInfo device)
    {
        List<VideoInputOptions> options = new List<VideoInputOptions>();

        int deviceFd = open(device.Path, FileOpenFlags.O_RDWR);
        if (deviceFd < 0)
            throw new IOException($"Error {Marshal.GetLastWin32Error()}: Cannot open video device {device}");

        v4l2_fmtdesc fmtdesc = new v4l2_fmtdesc
        {
            index = 0,
            type = v4l2_buf_type.V4L2_BUF_TYPE_VIDEO_CAPTURE
        };

        List<V4l2InputFormat> supportedInputFormats = new List<V4l2InputFormat>();
        while (xioctl(deviceFd, Ioctl.VidIOC.ENUM_FMT, &fmtdesc) == false)
        {
            supportedInputFormats.Add(fmtdesc.pixelformat);
            fmtdesc.index++;
        }

        foreach (V4l2InputFormat inputFormat in supportedInputFormats)
        {
            v4l2_frmsizeenum frmsize = new v4l2_frmsizeenum
            {
                index = 0,
                pixel_format = inputFormat,
            };

            while (xioctl(deviceFd, Ioctl.VidIOC.ENUM_FRAMESIZES, &frmsize) == false)
            {
                if (frmsize.type == v4l2_frmsizetypes.V4L2_FRMSIZE_TYPE_DISCRETE)
                {
                    fillFrameIntervalOptions(options, deviceFd, inputFormat, frmsize.discrete.width, frmsize.discrete.height);
                }
                else
                {
                    for (uint width = frmsize.stepwise.min_width; width < frmsize.stepwise.max_width; width += frmsize.stepwise.step_width)
                    {
                        for (uint height = frmsize.stepwise.min_height; height < frmsize.stepwise.max_height; height += frmsize.stepwise.step_height)
                            fillFrameIntervalOptions(options, deviceFd, inputFormat, width, height);
                    }
                }
                frmsize.index++;
            }
        }

        close(deviceFd);
        return options;
    }

    private static unsafe void fillFrameIntervalOptions(List<VideoInputOptions> options, int deviceFd, V4l2InputFormat inputFormat, uint width, uint height)
    {
        v4l2_frmivalenum frmival = new v4l2_frmivalenum
        {
            index = 0,
            pixel_format = inputFormat,
            width = width,
            height = height,
        };

        while (xioctl(deviceFd, Ioctl.VidIOC.ENUM_FRAMEINTERVALS, &frmival) == false)
        {
            if (frmival.type == v4l2_frmivaltypes.V4L2_FRMIVAL_TYPE_DISCRETE)
            {
                options.Add(new VideoInputOptions
                {
                    InputFormat = inputFormat,
                    VideoSize = ((int)width, (int)height),
                    Framerate = new VideoInputOptions.FramerateRatio
                    {
                        Numerator = frmival.discrete.denominator,
                        Denominator = frmival.discrete.numerator,
                    },
                });
            }
            frmival.index++;
        }
    }

    internal class Camera
    {
        internal CameraInfo Info { get; init; }
        internal int DeviceFd;
        internal v4l2_requestbuffers RequestBuffers { get; init; }

        internal ReqBuffer[] Buffers { get; init; } = Array.Empty<ReqBuffer>();

        ~Camera()
        {
            foreach (ReqBuffer buffer in Buffers)
                buffer.Free();

            close(DeviceFd);
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

    internal static unsafe Camera OpenCamera(CameraInfo cameraInfo)
    {
        int deviceFd = open(cameraInfo.Path, FileOpenFlags.O_RDWR);
        if (deviceFd < 0)
            throw new IOException($"Cannot open camera {cameraInfo.Path}");

        // get video format
        v4l2_fmtdesc formatDesc = new v4l2_fmtdesc
        {
            type = v4l2_buf_type.V4L2_BUF_TYPE_VIDEO_CAPTURE,
            index = 0,
            flags = 0,
            pixelformat = 0,
        };

        if (xioctl(deviceFd, Ioctl.VidIOC.ENUM_FMT, &formatDesc) == false)
            throw new IOException($"Cannot get video format for camera {cameraInfo.Path}: {Marshal.GetLastWin32Error()}");

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
                    pixelformat = formatDesc.pixelformat,
                    field = v4l2_field.V4L2_FIELD_NONE,
                }
            }
        };

        if (xioctl(deviceFd, Ioctl.VidIOC.S_FMT, &format) == false)
            throw new IOException($"Cannot set video format for camera {cameraInfo.Path}");

        Console.WriteLine("Format:");
        Console.WriteLine($"| width  : {format.fmt.pix.width}");
        Console.WriteLine($"| height : {format.fmt.pix.height}");
        Console.WriteLine($"| format : {format.fmt.pix.pixelformat}");
        Console.WriteLine($"| field  : {format.fmt.pix.field}");

        // request buffers
        v4l2_requestbuffers requestBuffers = new v4l2_requestbuffers
        {
            type = v4l2_buf_type.V4L2_BUF_TYPE_VIDEO_CAPTURE,
            memory = v4l2_memory.V4L2_MEMORY_MMAP,
            count = 4,
        };

        if (xioctl(deviceFd, Ioctl.VidIOC.REQBUFS, &requestBuffers) == false)
            throw new IOException($"Cannot request buffers for camera {cameraInfo.Path}");

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

            if (xioctl(deviceFd, Ioctl.VidIOC.QUERYBUF, &vbuf) == false)
                throw new IOException($"Could not query buffer for camera {cameraInfo.Path}");

            buffers[i] = new ReqBuffer(ref vbuf, deviceFd);
        }

        return new Camera
        {
            Info = cameraInfo,
            DeviceFd = deviceFd,
            RequestBuffers = requestBuffers,
            Buffers = buffers,
        };
    }
    #endregion

    #region Capture
    internal static unsafe void StartCapture(Camera camera)
    {
        for (uint i = 0; i < camera.Buffers.Length; i++)
        {
            v4l2_buffer vbuf = new v4l2_buffer
            {
                type = v4l2_buf_type.V4L2_BUF_TYPE_VIDEO_CAPTURE,
                memory = v4l2_memory.V4L2_MEMORY_MMAP,
                index = i,
            };

            if (xioctl(camera.DeviceFd, Ioctl.VidIOC.QBUF, &vbuf) == false)
                throw new IOException($"Could not enqueue buffer for camera {camera.Info.Path}");
        }

        v4l2_buf_type type = v4l2_buf_type.V4L2_BUF_TYPE_VIDEO_CAPTURE;

        if (xioctl(camera.DeviceFd, Ioctl.VidIOC.STREAMON, &type) == false)
            throw new IOException($"Could not enable data streaming for camera {camera.Info.Path}");
    }

    internal static unsafe void StopCapture(Camera camera)
    {
        v4l2_buf_type type = v4l2_buf_type.V4L2_BUF_TYPE_VIDEO_CAPTURE;

        if (xioctl(camera.DeviceFd, Ioctl.VidIOC.STREAMOFF, &type) == false)
            throw new Exception($"Could not disable data streaming for camera {camera.Info.Path}");
    }

    internal static unsafe bool ReadFrame(Camera camera, ref Frame frame)
    {
        v4l2_buffer vbuf = new v4l2_buffer
        {
            type = v4l2_buf_type.V4L2_BUF_TYPE_VIDEO_CAPTURE,
            memory = v4l2_memory.V4L2_MEMORY_MMAP,
            index = 0,
        };

        // dequeue a buffer
        if (xioctl(camera.DeviceFd, Ioctl.VidIOC.DQBUF, &vbuf) == false)
        {
            switch (Marshal.GetLastWin32Error())
            {
                case EAGAIN:
                    return false;

                case EIO:
                default:
                    throw new Exception($"Could not dequeue buffer for camera {camera.Info.Path}");
            }
        }

        Debug.Assert(vbuf.index < camera.Buffers.Length);

        // copy image to frame
        if (frame.Data.Length != vbuf.bytesused)
            frame.Data = new byte[vbuf.bytesused];

        // TODO: set width and height

        ReqBuffer reqBuffer = camera.Buffers[vbuf.index];
        Marshal.Copy((nint)reqBuffer.Ptr, frame.Data, 0, (int)vbuf.bytesused);

        // reenqueue the buffer
        if (xioctl(camera.DeviceFd, Ioctl.VidIOC.QBUF, &vbuf) == false)
            throw new Exception($"Could not reenqueue buffer for camera {camera.Info.Path}");

        return true;
    }
    #endregion

    private static unsafe bool xioctl(int fd, Ioctl.VidIOC request, void* argp)
    {
        int r = -1;

        do
        {
            r = ioctl(fd, request, argp);
        } while (-1 == r && Marshal.GetLastWin32Error() == EINTR);

        return r != -1;
    }
}
