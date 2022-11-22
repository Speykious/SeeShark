// Copyright (c) The Vignette Authors
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 3-Clause License. See LICENSE for details.

using System;

namespace SeeShark.Device;

public class WindowInfo : VideoDeviceInfo
{
    public string Title { get; init; } = string.Empty;

    public IntPtr Id { get; init; }
}
