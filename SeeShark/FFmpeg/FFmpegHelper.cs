// Copyright (c) Speykious
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 3-Clause License. See LICENSE for details.

using System;
using System.Runtime.InteropServices;
using FFmpeg.AutoGen;

namespace SeeShark.FFmpeg;

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
            throw new FFmpegException(AvStrerror(error));
        return error;
    }
}
