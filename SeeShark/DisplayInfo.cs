// Copyright (c) The Vignette Authors
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 3-Clause License. See LICENSE for details.

namespace SeeShark;

public class DisplayInfo : VideoDeviceInfo
{
    public DisplayInfo(string path) : base(path)
    {
    }

    public DisplayInfo(string? name, string path) : base(name, path)
    {
    }
}
