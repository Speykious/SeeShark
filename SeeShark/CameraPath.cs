// Copyright (c) Speykious
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 3-Clause License. See LICENSE for details.

namespace SeeShark;

public struct CameraPath
{
    /// <summary>
    /// Path of the camera device.
    /// On Linux, this represents a file in /dev/.
    /// On Windows, this represents a UUID.
    /// </summary>
    public string Path { get; init; }

    public override string ToString() => Path;
}
