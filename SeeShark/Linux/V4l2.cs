// Copyright (c) Speykious
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 2-Clause License. See LICENSE for details.

namespace SeeShark.Linux;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
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

            if (xioctl(deviceFd, Ioctl.VidIOC.QUERYCAP, &capability))
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
        while (xioctl(deviceFd, Ioctl.VidIOC.ENUM_FMT, &formatDesc))
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

            while (xioctl(deviceFd, Ioctl.VidIOC.ENUM_FRAMESIZES, &frameSize))
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

        while (xioctl(deviceFd, Ioctl.VidIOC.ENUM_FRAMEINTERVALS, &frameInterval))
        {
            if (frameInterval.type == v4l2_frmivaltypes.V4L2_FRMIVAL_TYPE_DISCRETE)
            {
                v4l2_fract intervalRatio = frameInterval.frame_interval.discrete;
                formats.Add(new VideoFormat
                {
                    InputFormat = inputFormat,
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

    internal class Camera : IDisposable
    {
        internal int DeviceFd { get; set; }
        internal CameraPath Path { get; init; }
        public VideoFormat CurrentFormat { get; set; }

        internal v4l2_requestbuffers RequestBuffers { get; init; }
        internal ReqBuffer[] Buffers { get; init; } = Array.Empty<ReqBuffer>();

        ~Camera() => dispose();

        public void Dispose()
        {
            dispose();
            GC.SuppressFinalize(this);
        }

        private void dispose()
        {
            if (DeviceFd != -1)
            {
                foreach (ReqBuffer buffer in Buffers)
                    buffer.Free();

                close(DeviceFd);
                DeviceFd = -1;
            }
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

    internal static unsafe Camera OpenCamera(CameraPath cameraInfo) => OpenCamera(cameraInfo, new VideoFormatOptions());

    internal static unsafe Camera OpenCamera(CameraPath cameraInfo, VideoFormatOptions options)
    {
        int deviceFd = open(cameraInfo.Path, FileOpenFlags.O_RDWR);
        if (deviceFd < 0)
            throw new IOException($"Cannot open camera {cameraInfo.Path}");

        // get a suited pixel format
        V4l2InputFormat pixelFormat;

        if (options.InputFormat is V4l2InputFormat inputFormatOption)
        {
            pixelFormat = inputFormatOption;
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

            if (xioctl(deviceFd, Ioctl.VidIOC.ENUM_FMT, &formatDesc) == false)
                throw new IOException($"Cannot get video format for camera {cameraInfo.Path}: {Marshal.GetLastWin32Error()}");

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

        if (xioctl(deviceFd, Ioctl.VidIOC.S_FMT, &format) == false)
            throw new IOException($"Cannot set video format for camera {cameraInfo.Path}");

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

            if (xioctl(deviceFd, Ioctl.VidIOC.S_PARM, &parameter) == false)
                throw new IOException($"Cannot set video format for camera {cameraInfo.Path}");

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

            if (xioctl(deviceFd, Ioctl.VidIOC.G_PARM, &parameter) == false)
                throw new IOException($"Cannot set video format for camera {cameraInfo.Path}");

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
            InputFormat = pix.pixelformat,
            DrawMouse = false,
        };

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
            Path = cameraInfo,
            DeviceFd = deviceFd,
            CurrentFormat = videoFormat,
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
                throw new IOException($"Could not enqueue buffer for camera {camera.Path.Path}");
        }

        v4l2_buf_type type = v4l2_buf_type.V4L2_BUF_TYPE_VIDEO_CAPTURE;

        if (xioctl(camera.DeviceFd, Ioctl.VidIOC.STREAMON, &type) == false)
            throw new IOException($"Could not enable data streaming for camera {camera.Path.Path}");
    }

    internal static unsafe void StopCapture(Camera camera)
    {
        v4l2_buf_type type = v4l2_buf_type.V4L2_BUF_TYPE_VIDEO_CAPTURE;

        if (xioctl(camera.DeviceFd, Ioctl.VidIOC.STREAMOFF, &type) == false)
            throw new Exception($"Could not disable data streaming for camera {camera.Path.Path}");
    }

    internal static unsafe void ReadFrame(Camera camera, ref Frame frame)
    {
        while (true)
        {
            if (TryReadFrame(camera, ref frame))
                break;
        }
    }

    internal static unsafe bool TryReadFrame(Camera camera, ref Frame frame)
    {
        fd_set fds;
        FD_ZERO(ref fds);
        FD_SET(camera.DeviceFd, ref fds);

        timeval_t timeout = new timeval_t
        {
            tv_sec = 2,
            tv_usec = 0,
        };

        int res = select(camera.DeviceFd + 1, &fds, null, null, &timeout);
        if (res == -1)
        {
            int errno = Marshal.GetLastWin32Error();
            if (errno == EINTR)
                return false;

            throw new IOException($"Could not poll camera {camera.Path} (error {errno})");
        }
        else if (res == 0)
        {
            throw new IOException($"Timeout when polling camera {camera.Path}");
        }

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
                    throw new Exception($"Could not dequeue buffer for camera {camera.Path.Path}");
            }
        }

        Debug.Assert(vbuf.index < camera.Buffers.Length);

        // copy image to frame
        if (frame.Data.Length != vbuf.bytesused)
            frame.Data = new byte[vbuf.bytesused];

        (frame.Width, frame.Height) = camera.CurrentFormat.VideoSize;
        frame.InputFormat = camera.CurrentFormat.InputFormat;

        ReqBuffer reqBuffer = camera.Buffers[vbuf.index];
        Marshal.Copy((nint)reqBuffer.Ptr, frame.Data, 0, (int)vbuf.bytesused);

        // reenqueue the buffer
        if (xioctl(camera.DeviceFd, Ioctl.VidIOC.QBUF, &vbuf) == false)
            throw new Exception($"Could not reenqueue buffer for camera {camera.Path.Path}");

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
