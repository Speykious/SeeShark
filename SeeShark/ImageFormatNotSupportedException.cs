// Copyright (c) Speykious
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 2-Clause License. See LICENSE for details.

using System;

namespace SeeShark;

public class ImageFormatNotSupportedException : Exception
{
    public ImageFormatNotSupportedException()
    {
    }

    public ImageFormatNotSupportedException(string message) : base(message)
    {
    }

    public ImageFormatNotSupportedException(string message, Exception inner) : base(message, inner)
    {
    }
}
