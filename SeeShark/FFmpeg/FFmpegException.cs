// Copyright (c) Speykious
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 3-Clause License. See LICENSE for details.

using System;

namespace SeeShark.FFmpeg;

public class FFmpegException : Exception
{
    public FFmpegException()
    {
    }

    public FFmpegException(string? message) : base(message)
    {
    }

    public FFmpegException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
