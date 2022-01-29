// Copyright (c) The Vignette Authors
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 3-Clause License. See LICENSE for details.

using System;
using System.Runtime.InteropServices;

namespace SeeShark.Interop.X11
{
    using Display = IntPtr;
    using IntPtr = IntPtr;
    using KeySym = IntPtr;
    using Window = IntPtr;

    public class XLib
    {
        private const string lib_x11 = "libX11";
        private static object Lock = new object();

        [DllImport(lib_x11, EntryPoint = "XOpenDisplay")]
        private static extern unsafe IntPtr sys_XOpenDisplay(char* display);
        public static unsafe IntPtr XOpenDisplay(char* display)
        {
            lock (Lock)
                return sys_XOpenDisplay(display);
        }

        [DllImport(lib_x11, EntryPoint = "XCloseDisplay")]
        public static extern int XCloseDisplay(IntPtr display);

        [DllImport(lib_x11, EntryPoint = "XDefaultRootWindow")]
        public static extern Window XDefaultRootWindow(Display display);

        [DllImport(lib_x11, EntryPoint = "XDisplayWidth")]
        public static extern int XDisplayWidth(Display display, int screenNumber);

        [DllImport(lib_x11, EntryPoint = "XDisplayHeight")]
        public static extern int XDisplayHeight(Display display, int screenNumber);
    }
}
