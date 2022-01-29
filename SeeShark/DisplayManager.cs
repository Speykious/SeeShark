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
        public static DeviceInputFormat DefaultInputFormat
        {
            get
            {
                return OperatingSystem.IsWindows() ? DeviceInputFormat.GdiGrab
                    : OperatingSystem.IsLinux() ? DeviceInputFormat.X11Grab
                    : OperatingSystem.IsMacOS() ? DeviceInputFormat.AVFoundation
                    : throw new NotSupportedException(
                        $"Cannot find adequate display input format for RID '{RuntimeInformation.RuntimeIdentifier}'.");
            }
        }

        public DisplayManager(DeviceInputFormat? inputFormat = null) : base(inputFormat ?? DefaultInputFormat)
        {
        }

        public override Display GetDevice(DisplayInfo info) => new Display(info, InputFormat);

        /// <summary>
        /// Enumerates available devices.
        /// </summary>
        protected override DisplayInfo[] EnumerateDevices()
        {
            if (InputFormat == DeviceInputFormat.X11Grab)
            {
                unsafe
                {
                    IntPtr display = XLib.XOpenDisplay(null);
                    IntPtr rootWindow = XLib.XDefaultRootWindow(display);
                    List<XRRMonitorInfo> monitors = getXRandrDisplays(display, rootWindow).ToList();

                    DisplayInfo[] info = new DisplayInfo[monitors.Count];
                    for (int i = 0; i < monitors.Count; i++)
                    {
                        info[i] = new DisplayInfo
                        {
                            Name = $"Display {i}",
                            Path = ":0",
                            X = monitors[i].X,
                            Y = monitors[i].Y,
                            Width = monitors[i].Width,
                            Height = monitors[i].Height,
                            Primary = monitors[i].Primary > 0,
                        };
                    }

                    return info;
                }
            }

            return base.EnumerateDevices();
        }

        private unsafe IEnumerable<XRRMonitorInfo> getXRandrDisplays(IntPtr display, IntPtr rootWindow)
        {
            ICollection<XRRMonitorInfo> monitors = new List<XRRMonitorInfo>();
            var xRandrMonitors = XRandr.XRRGetMonitors(display, rootWindow, true, out var count);
            for (int i = 0; i < count; i++)
                monitors.Add(xRandrMonitors[i]);
            return monitors;
        }
    }
}
