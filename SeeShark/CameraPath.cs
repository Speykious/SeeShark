// Copyright (c) Speykious
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 2-Clause License. See LICENSE for details.

namespace SeeShark;

public struct CameraPath
{
    /// <summary>
    /// Name of the camera device, if it exists.
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// Path of the camera device.
    /// <list type="bullet">
    ///   <item>On Linux, this represents a file in /dev/.</item>
    ///   <item>On Windows, this represents a UUID.</item>
    ///   <item>On MacOS, this represents a unique identifier.</item>
    /// </list>
    /// </summary>
    public string Path { get; init; }

    public override string ToString() => Name == null ? Path : $"{Name} ({Path})";
}
