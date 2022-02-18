// Copyright (c) The Vignette Authors
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 3-Clause License. See LICENSE for details.

using System;
using System.Runtime.InteropServices;
using SeeShark.Interop.X11;

namespace SeeShark.Device
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

        public override Display GetDevice(DisplayInfo info, VideoInputOptions? options = null) =>
            new Display(info, InputFormat, options);

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
                    XRRMonitorInfo[] monitors = getXRandrDisplays(display, rootWindow);

                    DisplayInfo[] info = new DisplayInfo[monitors.Length];
                    for (int i = 0; i < info.Length; i++)
                    {
                        XRRMonitorInfo monitor = monitors[i];
                        string nameAddition = monitor.Name == null ? "" : $" ({new string(monitor.Name)})";
                        info[i] = new DisplayInfo
                        {
                            Name = $"Display {i}{nameAddition}",
                            Path = ":0",
                            X = monitor.X,
                            Y = monitor.Y,
                            Width = monitor.Width,
                            Height = monitor.Height,
                            Primary = monitor.Primary > 0,
                        };
                    }

                    return info;
                }
            }

            return base.EnumerateDevices();
        }

        private unsafe XRRMonitorInfo[] getXRandrDisplays(IntPtr display, IntPtr rootWindow)
        {
            XRRMonitorInfo* xRandrMonitors = XRandr.XRRGetMonitors(display, rootWindow, true, out int count);
            XRRMonitorInfo[] monitors = new XRRMonitorInfo[count];
            for (int i = 0; i < count; i++)
                monitors[i] = xRandrMonitors[i];
            return monitors;
        }
    }
}
