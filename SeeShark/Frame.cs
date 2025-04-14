// Copyright (c) Speykious
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 2-Clause License. See LICENSE for details.

namespace SeeShark;

public class Frame
{
    public uint Width { get; set; }
    public uint Height { get; set; }

    public ImageFormat ImageFormat { get; set; }

    public byte[] Data { get; set; } = [];

    public override string ToString() => $"{Width}x{Height} w/ {Data.Length}b ({ImageFormat})";
}
