// Copyright (c) The Vignette Authors
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 3-Clause License. See LICENSE for details.

using System;
using System.Collections.Immutable;
using System.Linq;
using SeeShark.FFmpeg;

namespace SeeShark
{
    public class DisplayManager : VideoDeviceManager<Display>
    {
        public override event Action<VideoDeviceInfo>? OnNewDevice;
        public override event Action<VideoDeviceInfo>? OnLostDevice;
        public override Display GetDevice(VideoDeviceInfo info) => new Display(info, InputFormat);

        /// <summary>
        /// Enumerates available devices.
        /// </summary>
        private VideoDeviceInfo[] enumerateDevices()
        {
            if (InputFormat == DeviceInputFormat.X11Grab)
            {

            }
            else
            {
            }
        }

        public override void SyncDevices()
        {
            var newDevices = enumerateDevices().ToImmutableList();

            if (Devices.SequenceEqual(newDevices))
                return;

            foreach (var device in newDevices.Except(Devices))
                OnNewDevice?.Invoke(device);

            foreach (var device in Devices.Except(newDevices))
                OnLostDevice?.Invoke(device);

            Devices = newDevices;
        }
    }
}
