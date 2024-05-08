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
internal static class Xlib
{
    private const string lib_x11 = "libX11";
    private static readonly object displayLock = new object();

    [DllImport(lib_x11, EntryPoint = "XOpenDisplay")]
    private static extern unsafe Display sys_XOpenDisplay(sbyte* display);
    internal static unsafe Display XOpenDisplay(sbyte* display)
    {
        lock (displayLock)
            return sys_XOpenDisplay(display);
    }

    [DllImport(lib_x11, EntryPoint = "XCloseDisplay")]
    internal static extern int XCloseDisplay(Display display);

    [DllImport(lib_x11, EntryPoint = "XDefaultRootWindow")]
    internal static extern Window XDefaultRootWindow(Display display);

    [DllImport(lib_x11, EntryPoint = "XDisplayWidth")]
    internal static extern int XDisplayWidth(Display display, int screenNumber);

    [DllImport(lib_x11, EntryPoint = "XDisplayHeight")]
    internal static extern int XDisplayHeight(Display display, int screenNumber);

    [DllImport(lib_x11, EntryPoint = "XGetAtomName")]
    internal static extern IntPtr XGetAtomName(Display display, Atom atom);
}
