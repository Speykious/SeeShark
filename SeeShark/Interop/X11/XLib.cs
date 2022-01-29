// Copyright (c) The Vignette Authors
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 3-Clause License. See LICENSE for details.

using System;
using System.Runtime.InteropServices;

namespace SeeShark.Interop.X11
{
    using Display = IntPtr;
    using Window = IntPtr;

    internal class XLib
    {
        private const string lib_x11 = "libX11";
        private static readonly object displayLock = new object();

        [DllImport(lib_x11, EntryPoint = "XOpenDisplay")]
        private static extern unsafe Display sys_XOpenDisplay(sbyte* display);
        public static unsafe Display XOpenDisplay(sbyte* display)
        {
            lock (displayLock)
                return sys_XOpenDisplay(display);
        }

        [DllImport(lib_x11, EntryPoint = "XCloseDisplay")]
        public static extern int XCloseDisplay(Display display);

        [DllImport(lib_x11, EntryPoint = "XDefaultRootWindow")]
        public static extern Window XDefaultRootWindow(Display display);

        [DllImport(lib_x11, EntryPoint = "XDisplayWidth")]
        public static extern int XDisplayWidth(Display display, int screenNumber);

        [DllImport(lib_x11, EntryPoint = "XDisplayHeight")]
        public static extern int XDisplayHeight(Display display, int screenNumber);
    }
}
