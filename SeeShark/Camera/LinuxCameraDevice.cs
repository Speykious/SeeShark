// Copyright (c) Speykious
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 2-Clause License. See LICENSE for details.

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using SeeShark.Interop.Linux.Libc;
using static SeeShark.Interop.Linux.Libc.Libc;
using static SeeShark.Linux.V4l2;

namespace SeeShark.Camera;

[SupportedOSPlatform("Linux")]
public sealed class LinuxCameraDevice : CameraDevice
{
    private int deviceFd { get; set; }
    private v4l2_requestbuffers requestBuffers { get; init; }

    private ReqBuffer[] buffers { get; init; } = [];

    internal LinuxCameraDevice(CameraPath cameraInfo, VideoFormatOptions options)
    {
        int deviceFd = open(cameraInfo.Path, FileOpenFlags.O_RDWR);
        if (deviceFd < 0)
            throw new CameraDeviceIOException(cameraInfo, "Cannot open camera");

        // Get a suited pixel format
        ImageFormat imageFormat;
        V4l2InputFormat pixelFormat;

        if (options.ImageFormat is ImageFormat specifiedImageFormat)
        {
            imageFormat = specifiedImageFormat;
            pixelFormat = ImageFormatIntoV4l2InputFormat_OrThrowIfUnsupported(imageFormat);
        }
        else
        {
            // Default to the first SeeShark-supported pixel format when not specified
            unsafe
            {
                v4l2_fmtdesc formatDesc = new v4l2_fmtdesc
                {
                    index = 0,
                    type = v4l2_buf_type.V4L2_BUF_TYPE_VIDEO_CAPTURE
                };

                (ImageFormat, V4l2InputFormat)? maybeImageFormat = null;
                while (Xioctl(deviceFd, Ioctl.VidIOC.ENUM_FMT, &formatDesc))
                {
                    if (V4l2InputFormatIntoImageFormat(formatDesc.pixelformat) is ImageFormat supportedImageFormat)
                    {
                        maybeImageFormat = (supportedImageFormat, formatDesc.pixelformat);
                        break;
                    }

                    formatDesc.index++;
                }

                if (maybeImageFormat is (ImageFormat supportedImageFmt, V4l2InputFormat supportedInputFmt))
                {
                    imageFormat = supportedImageFmt;
                    pixelFormat = supportedInputFmt;
                }
                else
                {
                    throw new CameraDeviceIOException(cameraInfo, "Cannot find any supported image format");
                }
            }
        }

        // Set video format
        v4l2_format format;
        {
            uint width, height;
            if (options.VideoSize is (uint w, uint h))
            {
                width = w;
                height = h;
            }
            else
            {
                // Default to the first queried framesize's max value when not specified
                unsafe
                {
                    v4l2_frmsizeenum frameSize = new v4l2_frmsizeenum
                    {
                        index = 0,
                        pixel_format = pixelFormat,
                    };

                    if (Xioctl(deviceFd, Ioctl.VidIOC.ENUM_FRAMESIZES, &frameSize))
                    {
                        if (frameSize.type == v4l2_frmsizetypes.V4L2_FRMSIZE_TYPE_DISCRETE)
                        {
                            width = frameSize.frame_size.discrete.width;
                            height = frameSize.frame_size.discrete.height;
                        }
                        else
                        {
                            width = frameSize.frame_size.stepwise.max_width;
                            height = frameSize.frame_size.stepwise.max_height;
                        }
                    }
                    else
                    {
                        throw new CameraDeviceIOException(cameraInfo, "Cannot query any frame size");
                    }
                }
            }

            format = new v4l2_format
            {
                type = v4l2_buf_type.V4L2_BUF_TYPE_VIDEO_CAPTURE,
                fmt = new v4l2_format.fmt_union
                {
                    pix = new v4l2_pix_format
                    {
                        width = width,
                        height = height,
                        pixelformat = pixelFormat,
                        field = v4l2_field.V4L2_FIELD_NONE,
                    }
                }
            };

            unsafe
            {
                if (Xioctl(deviceFd, Ioctl.VidIOC.S_FMT, &format) == false)
                    throw new IOException($"Cannot set video format for camera {cameraInfo}");
            }
        }

        // Get a suited framerate
        FramerateRatio framerate;

        // Helpful documentation on G_PARM and S_PARM:
        // https://www.kernel.org/doc/html/latest/userspace-api/media/v4l/vidioc-g-parm.html
        if (options.Framerate is FramerateRatio framerateOption)
        {
            // Set framerate
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

            unsafe
            {
                if (Xioctl(deviceFd, Ioctl.VidIOC.S_PARM, &parameter) == false)
                    throw new IOException($"Cannot set video format for camera {cameraInfo}");
            }

            v4l2_fract timeperframe = parameter.parm.capture.timeperframe;
            framerate = new FramerateRatio
            {
                Numerator = timeperframe.denominator,
                Denominator = timeperframe.numerator,
            };
        }
        else
        {
            // Get framerate
            v4l2_streamparm parameter = new v4l2_streamparm
            {
                type = v4l2_buf_type.V4L2_BUF_TYPE_VIDEO_CAPTURE,
            };

            unsafe
            {
                if (Xioctl(deviceFd, Ioctl.VidIOC.G_PARM, &parameter) == false)
                    throw new IOException($"Cannot set video format for camera {cameraInfo}");
            }

            v4l2_fract timeperframe = parameter.parm.capture.timeperframe;
            framerate = new FramerateRatio
            {
                Numerator = timeperframe.denominator,
                Denominator = timeperframe.numerator,
            };
        }

        v4l2_pix_format pix = format.fmt.pix;

        VideoFormat videoFormat = new VideoFormat
        {
            VideoSize = (pix.width, pix.height),
            VideoPosition = (0, 0),
            Framerate = framerate,
            ImageFormat = imageFormat,
            DrawMouse = false,
        };

        // Request buffers
        v4l2_requestbuffers requestBuffers = new v4l2_requestbuffers
        {
            type = v4l2_buf_type.V4L2_BUF_TYPE_VIDEO_CAPTURE,
            memory = v4l2_memory.V4L2_MEMORY_MMAP,
            count = 4,
        };

        unsafe
        {
            if (Xioctl(deviceFd, Ioctl.VidIOC.REQBUFS, &requestBuffers) == false)
                throw new CameraDeviceIOException(cameraInfo, "Cannot request buffers for camera");
        }

        if (requestBuffers.count < 2)
            throw new CameraDeviceIOException(cameraInfo, "Didn't get enough request buffers");

        ReqBuffer[] buffers = new ReqBuffer[requestBuffers.count];
        for (uint i = 0; i < requestBuffers.count; i++)
        {
            v4l2_buffer vbuf = new v4l2_buffer
            {
                type = requestBuffers.type,
                memory = v4l2_memory.V4L2_MEMORY_MMAP,
                index = i,
            };

            unsafe
            {
                if (Xioctl(deviceFd, Ioctl.VidIOC.QUERYBUF, &vbuf) == false)
                    throw new CameraDeviceIOException(cameraInfo, "Cannot query buffer");
            }

            buffers[i] = new ReqBuffer(ref vbuf, deviceFd);
        }

        Path = cameraInfo;
        this.deviceFd = deviceFd;
        CurrentFormat = videoFormat;
        this.requestBuffers = requestBuffers;
        this.buffers = buffers;
    }

    #region Capture
    public override void StartCapture()
    {
        for (uint i = 0; i < buffers.Length; i++)
        {
            v4l2_buffer vbuf = new v4l2_buffer
            {
                type = v4l2_buf_type.V4L2_BUF_TYPE_VIDEO_CAPTURE,
                memory = v4l2_memory.V4L2_MEMORY_MMAP,
                index = i,
            };

            unsafe
            {
                if (Xioctl(deviceFd, Ioctl.VidIOC.QBUF, &vbuf) == false)
                    throw new IOException($"Could not enqueue buffer for camera {Path}");
            }
        }

        v4l2_buf_type type = v4l2_buf_type.V4L2_BUF_TYPE_VIDEO_CAPTURE;

        unsafe
        {
            if (Xioctl(deviceFd, Ioctl.VidIOC.STREAMON, &type) == false)
                throw new IOException($"Could not enable data streaming for camera {Path}");
        }
    }

    public override void StopCapture()
    {
        v4l2_buf_type type = v4l2_buf_type.V4L2_BUF_TYPE_VIDEO_CAPTURE;

        unsafe
        {
            if (Xioctl(deviceFd, Ioctl.VidIOC.STREAMOFF, &type) == false)
                throw new Exception($"Could not disable data streaming for camera {Path}");
        }
    }

    public override bool TryReadFrame(ref Frame frame)
    {
        fd_set fds;
        FD_ZERO(ref fds);
        FD_SET(deviceFd, ref fds);

        timeval_t timeout = new timeval_t
        {
            tv_sec = 2,
            tv_usec = 0,
        };

        int res;
        unsafe
        {
            res = select(deviceFd + 1, &fds, null, null, &timeout);
        }

        if (res == -1)
        {
            int errno = Marshal.GetLastWin32Error();
            if (errno == EINTR)
                return false;

            throw new IOException($"Could not poll camera {Path} (error {errno})");
        }
        else if (res == 0)
        {
            throw new IOException($"Timeout when polling camera {Path}");
        }

        v4l2_buffer vbuf = new v4l2_buffer
        {
            type = v4l2_buf_type.V4L2_BUF_TYPE_VIDEO_CAPTURE,
            memory = v4l2_memory.V4L2_MEMORY_MMAP,
            index = 0,
        };

        // dequeue a buffer
        bool dqbuf;
        unsafe
        {
            dqbuf = Xioctl(deviceFd, Ioctl.VidIOC.DQBUF, &vbuf);
        }

        if (dqbuf == false)
        {
            switch (Marshal.GetLastWin32Error())
            {
                case EAGAIN:
                    return false;

                case EIO:
                default:
                    throw new Exception($"Could not dequeue buffer for camera {Path}");
            }
        }

        Debug.Assert(vbuf.index < buffers.Length);

        // copy image to frame
        if (frame.Data.Length != vbuf.bytesused)
            frame.Data = new byte[vbuf.bytesused];

        (frame.Width, frame.Height) = CurrentFormat.VideoSize;
        frame.ImageFormat = CurrentFormat.ImageFormat;

        ReqBuffer reqBuffer = buffers[vbuf.index];
        unsafe
        {
            Marshal.Copy((nint)reqBuffer.Ptr, frame.Data, 0, (int)vbuf.bytesused);
        }

        // reenqueue the buffer
        bool qbuf;
        unsafe
        {
            qbuf = Xioctl(deviceFd, Ioctl.VidIOC.QBUF, &vbuf);
        }

        if (qbuf == false)
            throw new Exception($"Could not reenqueue buffer for camera {Path}");

        return true;
    }
    #endregion

    protected override void DisposeUnmanagedResources()
    {
        if (deviceFd != -1)
        {
            foreach (ReqBuffer buffer in buffers)
                buffer.Free();

            close(deviceFd);
            deviceFd = -1;
        }
    }
}
