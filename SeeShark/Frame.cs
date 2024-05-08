// Copyright (c) Speykious
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 2-Clause License. See LICENSE for details.

using System;
using System.Runtime.Versioning;
using SeeShark.Interop.Libc;

namespace SeeShark;

public class Frame
{
    public uint Width { get; set; }
    public uint Height { get; set; }

    [SupportedOSPlatform("Linux")]
    internal V4l2InputFormat InputFormat { get; set; }

    public byte[] Data { get; set; } = Array.Empty<byte>();

    public override string ToString()
    {
        if (OperatingSystem.IsLinux())
            return $"{Width}x{Height} w/ {Data.Length}b ({InputFormat})";
        else
            return $"{Width}x{Height} w/ {Data.Length}b";
    }
}
