// Copyright (c) Speykious
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 2-Clause License. See LICENSE for details.

using System;
using System.IO;

namespace SeeShark.Camera;

public class CameraDeviceIOException : IOException
{
    public readonly CameraPath CameraPath;

    public CameraDeviceIOException(CameraPath cameraPath)
    {
        CameraPath = cameraPath;
    }

    public CameraDeviceIOException(CameraPath cameraPath, string message) : base(message)
    {
        CameraPath = cameraPath;
    }

    public CameraDeviceIOException(CameraPath cameraPath, string message, Exception inner) : base(message, inner)
    {
        CameraPath = cameraPath;
    }
}
