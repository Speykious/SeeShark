// Copyright (c) The Vignette Authors
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 3-Clause License. See LICENSE for details.

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace SeeShark.Interop.Windows;

[StructLayout(LayoutKind.Sequential)]
internal struct DevMode
{
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
    internal string dmDeviceName;
    internal short dmSpecVersion;
    internal short dmDriverVersion;
    internal short dmSize;
    internal short dmDriverExtra;
    internal int dmFields;
    internal int dmPositionX;
    internal int dmPositionY;
    internal int dmDisplayOrientation;
    internal int dmDisplayFixedOutput;
    internal short dmColor;
    internal short dmDuplex;
    internal short dmYResolution;
    internal short dmTTOption;
    internal short dmCollate;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
    internal string dmFormName;
    internal short dmLogPixels;
    internal int dmBitsPerPel;
    internal int dmPelsWidth;
    internal int dmPelsHeight;
    internal int dmDisplayFlags;
    internal int dmDisplayFrequency;
    internal int dmICMMethod;
    internal int dmICMIntent;
    internal int dmMediaType;
    internal int dmDitherType;
    internal int dmReserved1;
    internal int dmReserved2;
    internal int dmPanningWidth;
    internal int dmPanningHeight;
}

[StructLayout(LayoutKind.Sequential)]
internal struct Rect
{
    internal int left;
    internal int top;
    internal int right;
    internal int bottom;
}

internal enum MONITORINFOF : ulong
{
    PRIMARY = 1
}

[StructLayout(LayoutKind.Sequential)]
internal struct MonitorInfoEx
{
    internal uint size;
    internal Rect monitor;
    internal Rect work;
    internal uint flags;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
    internal string deviceName;
}

internal enum DpiType
{
    Effective,
    Angular,
    Raw
}


[Flags]
internal enum DisplayDeviceStateFlags : int
{
    /// <summary>
    /// The device is part of the desktop.
    /// </summary>
    AttachedToDesktop = 0x1,
    MultiDriver = 0x2,
    /// <summary>The device is part of the desktop.</summary>
    PrimaryDevice = 0x4,
    /// <summary>Represents a pseudo device used to mirror application drawing for remoting or other purposes.</summary>
    MirroringDriver = 0x8,
    /// <summary>The device is VGA compatible.</summary>
    VGACompatible = 0x10,
    /// <summary>The device is removable; it cannot be the primary display.</summary>
    Removable = 0x20,
    /// <summary>The device has more display modes than its output devices support.</summary>
    ModesPruned = 0x8000000,
    Remote = 0x4000000,
    Disconnect = 0x2000000
}


#pragma warning disable CA1815

// This is never compared in the code, so we can suppress the warning.
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
internal struct DISPLAY_DEVICE
{
    [MarshalAs(UnmanagedType.U4)]
    internal int cb;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
    internal string DeviceName;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
    internal string DeviceString;
    [MarshalAs(UnmanagedType.U4)]
    internal DisplayDeviceStateFlags StateFlags;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
    internal string DeviceID;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
    internal string DeviceKey;
}
#pragma warning restore CA1815

internal static partial class User32
{
    internal delegate bool MonitorEnumDelegate(IntPtr hMonitor, IntPtr hdcMonitor, ref Rect lprcMonitor, IntPtr dwData);

    [DllImport("user32.dll")]
    internal static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, MonitorEnumDelegate lpfnEnum, IntPtr dwData);

    [DllImport("user32.dll")]
    internal static extern bool GetMonitorInfo(IntPtr hmon, ref MonitorInfoEx mi);

#pragma warning disable CA2101
    [DllImport("user32.dll", CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
    internal static extern bool EnumDisplaySettings(string deviceName, int modeNum, ref DevMode devMode);

    [DllImport("user32.dll", CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
    internal static extern bool EnumDisplayDevices(string lpDevice, uint iDevNum, ref DISPLAY_DEVICE lpDisplayDevice, uint dwFlags);

#pragma warning restore CA2101

    [DllImport("Shcore.dll")]
    internal static extern int GetDpiForMonitor(IntPtr hmon, DpiType dpiType, out uint dpiX, out uint dpiY);

    [DllImport("Shcore.dll")]
    internal static extern int SetProcessDpiAwareness(int awareness);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    internal static extern int GetWindowText(IntPtr hWnd, StringBuilder strText, int maxCount);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    internal static extern int GetWindowTextLength(IntPtr hWnd);

    [DllImport("user32.dll")]
    internal static extern bool EnumDesktopWindows(IntPtr hDesktop, EnumWindowsProc enumProc, IntPtr lParam);

    [DllImport("user32.dll")]
    internal static extern bool IsWindowVisible(IntPtr hWnd);

    [DllImport("user32.dll")]
    internal static extern IntPtr GetShellWindow();

    internal delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);
}
