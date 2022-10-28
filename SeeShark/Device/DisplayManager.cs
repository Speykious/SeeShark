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

                    DisplayInfo[] info = new DisplayInfo[monitors.Length + 1];

                    int compositeLeft = Int32.MaxValue;
                    int compositeRight = 0;
                    int compositeTop = Int32.MaxValue;
                    int compositeBottom = 0;

                    for (int i = 0; i < monitors.Length; i++)
                    {
                        XRRMonitorInfo monitor = monitors[i];
                        info[i + 1] = new DisplayInfo
                        {
                            Name = $"Display {i}",
                            Path = ":0",
                            X = monitor.X,
                            Y = monitor.Y,
                            Width = monitor.Width,
                            Height = monitor.Height,
                            Primary = monitor.Primary > 0,
                        };

                        if (monitor.X < compositeLeft)
                            compositeLeft = monitor.X;

                        if ((monitor.X + monitor.Width) > compositeRight)
                            compositeRight = (monitor.X + monitor.Width);

                        if (monitor.Y < compositeTop)
                            compositeTop = monitor.Y;

                        if ((monitor.Y + monitor.Height) > compositeBottom)
                            compositeBottom = (monitor.Y + monitor.Height);
                    }

                    info[0] = new DisplayInfo
                    {
                        Name = $"Composite X11 Display",
                        Path = ":0",
                        X = compositeLeft,
                        Y = compositeTop,
                        Width = compositeRight - compositeLeft,
                        Height = compositeBottom - compositeTop,
                        Primary = false,
                        IsComposite = true
                    };
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
