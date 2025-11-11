// Copyright (c) Speykious
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 2-Clause License. See LICENSE for details.

using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace SeeShark.Interop.Linux.X11;

using Atom = Int64;
using Display = nint;
using Window = nint;

[SupportedOSPlatform("Linux")]
internal static class Xrandr
{
    private const string lib_x_randr = "libXrandr";

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct XRRMonitorInfo
    {
        internal Atom Name;
        internal int Primary;
        internal int Automatic;
        internal int NOutput;
        internal int X;
        internal int Y;
        internal int Width;
        internal int Height;
        internal int MWidth;
        internal int MHeight;
        internal nint Outputs;
    }

    [DllImport(lib_x_randr, EntryPoint = "XRRGetMonitors")]
    internal static extern unsafe XRRMonitorInfo* XRRGetMonitors(Display dpy, Window window, bool getActive, out int nmonitors);
}
