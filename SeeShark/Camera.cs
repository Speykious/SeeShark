// Copyright (c) The Vignette Authors
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 3-Clause License. See LICENSE for details.

using System;
using System.Threading;
using SeeShark.FFmpeg;

namespace SeeShark
{
    public class Camera : VideoDevice
    {
        public Camera(VideoDeviceInfo info, DeviceInputFormat inputFormat) : base(info, inputFormat)
        {
        }
    }
}
