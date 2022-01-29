// Copyright (c) The Vignette Authors
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 3-Clause License. See LICENSE for details.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using FFmpeg.AutoGen;
using SeeShark.FFmpeg;
using SeeShark.Interop.X11;

namespace SeeShark
{
    public class DisplayManager : VideoDeviceManager<DisplayInfo, Display>
    {
        public override event Action<DisplayInfo>? OnNewDevice;
        public override event Action<DisplayInfo>? OnLostDevice;
        public override Display GetDevice(DisplayInfo info) => new Display(info, InputFormat);

        /// <summary>
        /// Enumerates available devices.
        /// </summary>
        private unsafe DisplayInfo[] enumerateDevices()
        {
            if (InputFormat == DeviceInputFormat.X11Grab)
            {
                var display = XLib.XOpenDisplay(null);
                var rootWindow = XLib.XDefaultRootWindow(display);
                var monitors = getXRandrDisplays(display, rootWindow).ToList();
                DisplayInfo[] info = new DisplayInfo[monitors.Count];
                for (int i = 0; i < monitors.Count; i++)
                {
                    info[i] = new DisplayInfo($"Display {i}", ":0", monitors[i].X,
                        monitors[i].Y, monitors[i].Width, monitors[i].Height, monitors[i].Primary > 0);
                }

                return info;
            }
            else
            {
            }

            return Array.Empty<DisplayInfo>();
        }

        private unsafe IEnumerable<XRRMonitorInfo> getXRandrDisplays(IntPtr display, IntPtr rootWindow)
        {
            ICollection<XRRMonitorInfo> monitors = new List<XRRMonitorInfo>();
            var xRandrMonitors = XRandr.XRRGetMonitors(display, rootWindow, true, out var count);
            for (int i = 0; i < count; i++)
                monitors.Add(xRandrMonitors[i]);
            return monitors;
        }

        public DisplayManager(DeviceInputFormat? inputFormat = null)
        {
            InputFormat = inputFormat ?? (
                RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? DeviceInputFormat.GdiGrab
                : RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? DeviceInputFormat.X11Grab
                : throw new NotSupportedException($"Cannot find adequate input format for RID '{RuntimeInformation.RuntimeIdentifier}'."));
            SyncDevices();
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
