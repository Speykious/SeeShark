// Copyright (c) Speykious
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 2-Clause License. See LICENSE for details.

namespace SeeShark;

public struct CameraPath
{
    /// <summary>
    /// Path of the camera device.
    /// <list type="bullet">
    ///   <item>On Linux, this represents a file in /dev/.</item>
    ///   <item>On Windows, this represents a UUID.</item>
    ///   <item>On MacOS, this represents a unique identifier.</item>
    /// </list>
    /// </summary>
    public string Path { get; init; }

    public override string ToString() => Path;
}
