// Copyright (c) The Vignette Authors
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 3-Clause License. See LICENSE for details.

using SeeShark.Decode;

namespace SeeShark.Device
{
    public class Display : VideoDevice
    {
        public Display(VideoDeviceInfo info, DeviceInputFormat inputFormat, VideoInputOptions? options = null)
            : base(info, inputFormat, options)
        {
        }
    }
}
