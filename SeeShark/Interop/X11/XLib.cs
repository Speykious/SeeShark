// Copyright (c) The Vignette Authors
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 3-Clause License. See LICENSE for details.

using System;
using System.Runtime.InteropServices;

namespace SeeShark.Interop.X11;

using Atom = Int64;
using Display = IntPtr;
using Window = IntPtr;

internal class XLib
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

    [DllImport(lib_x11, EntryPoint = "XQueryTree")]
    internal static extern int XQueryTree(IntPtr display, IntPtr w, out IntPtr rootReturn, out IntPtr parentReturn, out IntPtr[] childrenReturn, out int nChildrenReturn);

    [DllImport(lib_x11, EntryPoint = "XFetchName")]
    internal static extern int XFetchName(IntPtr display, IntPtr w, out string windowNameReturn);
}
