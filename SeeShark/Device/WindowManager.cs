// Copyright (c) The Vignette Authors
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 3-Clause License. See LICENSE for details.

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using SeeShark.Interop.Windows;
using SeeShark.Interop.X11;

namespace SeeShark.Device;

public class WindowManager : VideoDeviceManager<WindowInfo, Window>
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


    public WindowManager(DeviceInputFormat? inputFormat = null) : base(inputFormat ?? DefaultInputFormat)
    {
    }

    public override Window GetDevice(WindowInfo info, VideoInputOptions? options = null)
    {
        if (options is { } o)
        {
            return new Window(info, InputFormat, o);
        }
        else
        {
            return new Window(info, InputFormat, generateInputOptions(info));
        }
    }

    /// <summary>
    /// Enumerates available devices.
    /// </summary>
    protected override WindowInfo[] EnumerateDevices()
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

    private WindowInfo[] enumerateDevicesX11()
    {
        List<WindowInfo> windows = new List<WindowInfo>();
        unsafe
        {
            IntPtr display = XLib.XOpenDisplay(null);
            IntPtr rootWindow = XLib.XDefaultRootWindow(display);
            findWindowsX11(display, rootWindow, ref windows);
        }

        return windows.ToArray();
    }

    void findWindowsX11(IntPtr display, IntPtr window, ref List<WindowInfo> windows)
    {
        IntPtr[] childWindows = Array.Empty<IntPtr>();

        XLib.XQueryTree(display, window, out IntPtr rootWindow, out IntPtr parentWindow, out childWindows,
            out int nChildren);

        childWindows = new IntPtr[nChildren];

        XLib.XQueryTree(display, window,
            out rootWindow, out parentWindow,
            out childWindows, out nChildren);

        XLib.XFetchName(display, window, out string title);

        windows.Add(new WindowInfo
        {
            Path = ":0",
            Title = title,
            Id = window
        });

        for (int i = 0; i < childWindows.Length; i++)
        {
            XLib.XFetchName(display, childWindows[i], out string childTitle);

            windows.Add(new WindowInfo
            {
                Path = ":0",
                Title = childTitle,
                Id = childWindows[i]
            });

            findWindowsX11(display, childWindows[i], ref windows);
        }
    }

    private WindowInfo[] enumerateDevicesGdi()
    {
        List<WindowInfo> windows = new List<WindowInfo>();
        User32.EnumWindows(delegate(IntPtr wnd, IntPtr param)
        {
            int size = User32.GetWindowTextLength(wnd);
            string title = string.Empty;
            if (size > 0)
            {
                var builder = new StringBuilder(size + 1);
                User32.GetWindowText(wnd, builder, builder.Capacity);
                title = builder.ToString();
            }

            windows.Add(new WindowInfo
            {
                Path = $"title={title}",
                Title = title,
                Id = wnd
            });
            return true;
        }, IntPtr.Zero);

        return windows.ToArray();
    }

    private VideoInputOptions generateInputOptions(WindowInfo info)
    {
        return new VideoInputOptions
        {
            WindowId = $"0x{new IntPtr(0x3600003).ToString("X2")}"
        };
    }
}
