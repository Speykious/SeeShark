// Copyright (c) Speykious
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 2-Clause License. See LICENSE for details.

using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace SeeShark.Interop.X11;

using Atom = Int64;
using Display = nint;
using Window = nint;

[SupportedOSPlatform("Linux")]
internal static class Xrandr
{
    private const string lib_x_randr = "libXrandr";

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct XRRMonitorInfo
    {
        public Atom Name;
        public int Primary;
        public int Automatic;
        public int NOutput;
        public int X;
        public int Y;
        public int Width;
        public int Height;
        public int MWidth;
        public int MHeight;
        public nint Outputs;
    }

    [DllImport(lib_x_randr, EntryPoint = "XRRGetMonitors")]
    public static extern unsafe XRRMonitorInfo* XRRGetMonitors(Display dpy, Window window, bool getActive, out int nmonitors);
}
