// Copyright (c) Speykious
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 2-Clause License. See LICENSE for details.

using System;

namespace SeeShark.Camera;

public class CameraDeviceInvalidFrameException : Exception
{
    public CameraDeviceInvalidFrameException()
    {
    }

    public CameraDeviceInvalidFrameException(string message) : base(message)
    {
    }

    public CameraDeviceInvalidFrameException(string message, Exception inner) : base(message, inner)
    {
    }
}
