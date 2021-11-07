// Copyright (c) The Vignette Authors
// This file is part of SeeShark.
// SeeShark is licensed under LGPL v3. See LICENSE.LESSER.md for details.

using System.Runtime.InteropServices;
using FFmpeg.AutoGen;

namespace SeeShark
{
    internal static class FFmpegHelper
    {
        public static unsafe string? AvStrerror(int error)
        {
            var bufferSize = 1024;
            var buffer = stackalloc byte[bufferSize];
            ffmpeg.av_strerror(error, buffer, (ulong)bufferSize);
            var message = Marshal.PtrToStringAnsi((IntPtr)buffer);
            return message;
        }

        public static int ThrowExceptionIfError(this int error)
        {
            if (error < 0)
                throw new ApplicationException(AvStrerror(error));
            return error;
        }
    }
}
