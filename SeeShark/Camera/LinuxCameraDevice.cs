// Copyright (c) Speykious
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 2-Clause License. See LICENSE for details.

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using SeeShark.Interop.Libc;
using static SeeShark.Interop.Libc.Libc;
using static SeeShark.Linux.V4l2;

namespace SeeShark.Camera;

[SupportedOSPlatform("Linux")]
public sealed class LinuxCameraDevice : CameraDevice
{
    internal int DeviceFd { get; set; }
    internal v4l2_requestbuffers RequestBuffers { get; init; }

    internal ReqBuffer[] Buffers { get; init; } = [];

    #region Capture
    public override void StartCapture()
    {
        for (uint i = 0; i < Buffers.Length; i++)
        {
            v4l2_buffer vbuf = new v4l2_buffer
            {
                type = v4l2_buf_type.V4L2_BUF_TYPE_VIDEO_CAPTURE,
                memory = v4l2_memory.V4L2_MEMORY_MMAP,
                index = i,
            };

            unsafe
            {
                if (Xioctl(DeviceFd, Ioctl.VidIOC.QBUF, &vbuf) == false)
                    throw new IOException($"Could not enqueue buffer for camera {Path}");
            }
        }

        v4l2_buf_type type = v4l2_buf_type.V4L2_BUF_TYPE_VIDEO_CAPTURE;

        unsafe
        {
            if (Xioctl(DeviceFd, Ioctl.VidIOC.STREAMON, &type) == false)
                throw new IOException($"Could not enable data streaming for camera {Path}");
        }
    }

    public override void StopCapture()
    {
        v4l2_buf_type type = v4l2_buf_type.V4L2_BUF_TYPE_VIDEO_CAPTURE;

        unsafe
        {
            if (Xioctl(DeviceFd, Ioctl.VidIOC.STREAMOFF, &type) == false)
                throw new Exception($"Could not disable data streaming for camera {Path}");
        }
    }

    public override bool TryReadFrame(ref Frame frame)
    {
        fd_set fds;
        FD_ZERO(ref fds);
        FD_SET(DeviceFd, ref fds);

        timeval_t timeout = new timeval_t
        {
            tv_sec = 2,
            tv_usec = 0,
        };

        int res;
        unsafe
        {
            res = select(DeviceFd + 1, &fds, null, null, &timeout);
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
            dqbuf = Xioctl(DeviceFd, Ioctl.VidIOC.DQBUF, &vbuf);
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

        Debug.Assert(vbuf.index < Buffers.Length);

        // copy image to frame
        if (frame.Data.Length != vbuf.bytesused)
            frame.Data = new byte[vbuf.bytesused];

        (frame.Width, frame.Height) = CurrentFormat.VideoSize;
        frame.ImageFormat = CurrentFormat.ImageFormat;

        ReqBuffer reqBuffer = Buffers[vbuf.index];
        unsafe
        {
            Marshal.Copy((nint)reqBuffer.Ptr, frame.Data, 0, (int)vbuf.bytesused);
        }

        // reenqueue the buffer
        bool qbuf;
        unsafe
        {
            qbuf = Xioctl(DeviceFd, Ioctl.VidIOC.QBUF, &vbuf);
        }

        if (qbuf == false)
            throw new Exception($"Could not reenqueue buffer for camera {Path}");

        return true;
    }
    #endregion

    protected override void DisposeUnmanagedResources()
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
