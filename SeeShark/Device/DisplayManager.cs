// Copyright (c) The Vignette Authors
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 3-Clause License. See LICENSE for details.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using SeeShark.Interop.Windows;
using SeeShark.Interop.X11;

namespace SeeShark.Device;

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

    public override Display GetDevice(DisplayInfo info, VideoInputOptions? options = null)
    {
        if (options is { } o)
        {
            return new Display(info, InputFormat, o);
        }
        else
        {
            return new Display(info, InputFormat, generateInputOptions(info));
        }
    }

    private VideoInputOptions generateInputOptions(DisplayInfo info)
    {
        return new VideoInputOptions
        {
            VideoSize = (info.Width, info.Height),
            VideoPosition = (info.X, info.Y)
        };
    }

    /// <summary>
    /// Enumerates available devices.
    /// </summary>
    protected override DisplayInfo[] EnumerateDevices()
    {
        switch (InputFormat)
        {
            case DeviceInputFormat.X11Grab:
                return enumerateDevicesX11();
            case DeviceInputFormat.GdiGrab:
                return enumerateDevicesGdi();
            default:
                return base.EnumerateDevices();
        }
    }

    private DisplayInfo[] enumerateDevicesX11()
    {
        unsafe
        {
            IntPtr display = XLib.XOpenDisplay(null);
            IntPtr rootWindow = XLib.XDefaultRootWindow(display);
            XRRMonitorInfo[] monitors = getXRandrDisplays(display, rootWindow);

            DisplayInfo[] info = new DisplayInfo[monitors.Length + 1];

            int compositeLeft = int.MaxValue;
            int compositeRight = int.MinValue;
            int compositeTop = int.MaxValue;
            int compositeBottom = int.MinValue;

            for (int i = 0; i < monitors.Length; i++)
            {
                XRRMonitorInfo monitor = monitors[i];
                var monitorNamePtr = XLib.XGetAtomName(display, monitor.Name);
                var monitorName = Marshal.PtrToStringAnsi(monitorNamePtr)!;

                string nameAddition = string.IsNullOrEmpty(monitorName) ? "" : $" ({monitorName})";
                info[i + 1] = new DisplayInfo
                {
                    Name = $"Display {i}{nameAddition}",
                    Path = ":0",
                    X = monitor.X,
                    Y = monitor.Y,
                    Width = monitor.Width,
                    Height = monitor.Height,
                    Primary = monitor.Primary > 0,
                };

                if (monitor.X < compositeLeft)
                    compositeLeft = monitor.X;

                if (monitor.X + monitor.Width > compositeRight)
                    compositeRight = monitor.X + monitor.Width;

                if (monitor.Y < compositeTop)
                    compositeTop = monitor.Y;

                if (monitor.Y + monitor.Height > compositeBottom)
                    compositeBottom = monitor.Y + monitor.Height;
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

    private DisplayInfo[] enumerateDevicesGdi()
    {
        var displayInfo = new List<DisplayInfo>();

        int count = 0;

        int compositeLeft = int.MaxValue;
        int compositeRight = int.MinValue;
        int compositeTop = int.MaxValue;
        int compositeBottom = int.MinValue;

        bool MonitorDelegate(IntPtr hMonitor, IntPtr hdcMonitor, ref Rect lprcMonitor, IntPtr dwData)
        {
            var monitorInfo = new MonitorInfoEx();
            monitorInfo.size = (uint)Marshal.SizeOf(monitorInfo);

            if (User32.GetMonitorInfo(hMonitor, ref monitorInfo))
            {
                var info = new DevMode();
                User32.EnumDisplaySettings(monitorInfo.deviceName, -1, ref info);

                var d = new DISPLAY_DEVICE();
                d.cb = Marshal.SizeOf(d);
                User32.EnumDisplayDevices(monitorInfo.deviceName, 0, ref d, 0);

                string nameAddition = string.IsNullOrEmpty(d.DeviceString) ? "" : $" ({d.DeviceString})";
                displayInfo.Add(new DisplayInfo
                {
                    Name = $"Display {count}{nameAddition}",
                    Path = "desktop",
                    X = info.dmPositionX,
                    Y = info.dmPositionY,
                    Width = info.dmPelsWidth,
                    Height = info.dmPelsHeight,
                    Primary = count == 0
                });
                count++;

                if (info.dmPositionX < compositeLeft)
                    compositeLeft = info.dmPositionX;

                if (info.dmPositionX + info.dmPelsWidth > compositeRight)
                    compositeRight = (info.dmPositionX + info.dmPelsWidth);

                if (info.dmPositionY < compositeTop)
                    compositeTop = info.dmPositionY;

                if (info.dmPositionY + info.dmPelsHeight > compositeBottom)
                    compositeBottom = (info.dmPositionY + info.dmPelsHeight);

            }
            return true;
        }

        User32.EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, MonitorDelegate, IntPtr.Zero);

        displayInfo.Insert(0, new DisplayInfo
        {
            Name = $"Composite GDI Display",
            Path = "desktop",
            X = compositeLeft,
            Y = compositeTop,
            Width = compositeRight - compositeLeft,
            Height = compositeBottom - compositeTop,
            Primary = false,
            IsComposite = true
        });

        // TODO: using a list and converting to array is ugly, try to find alternative
        return displayInfo.ToArray();
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
