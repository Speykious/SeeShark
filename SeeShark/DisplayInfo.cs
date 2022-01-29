// Copyright (c) The Vignette Authors
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 3-Clause License. See LICENSE for details.

namespace SeeShark;

public class DisplayInfo : VideoDeviceInfo
{
    public int X { get; }
    public int Y { get; }
    public int Width { get; }
    public int Height { get; }
    public bool Primary { get; }

    public DisplayInfo(string? name, string path) : base(name, path)
    {
    }

    public DisplayInfo(string? name, string path, int x, int y, int width, int height, bool primary) : base(name, path)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
        Primary = primary;
    }
}
