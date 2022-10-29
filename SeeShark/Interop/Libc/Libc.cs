// Copyright (c) The Vignette Authors
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 3-Clause License. See LICENSE for details.

using System;
using System.Runtime.InteropServices;

namespace SeeShark.Interop.Libc
{
    internal class Libc
    {
        private const string libc_library = "libc";
        private const string explain_library = "explain";

        [DllImport(libc_library, SetLastError = true)]
        internal static extern int open([MarshalAs(UnmanagedType.LPStr)] string pathname, FileOpenFlags flags);

        [DllImport(libc_library)]
        internal static extern int close(int fd);

        [DllImport(libc_library, SetLastError = true)]
        internal static extern int read(int fd, IntPtr buf, int count);

        [DllImport(libc_library, SetLastError = true)]
        internal static extern int write(int fd, IntPtr buf, int count);

        #region ioctl
        [DllImport(libc_library, SetLastError = true)]
        internal static extern int ioctl(int fd, int request, IntPtr argp);

        [DllImport(explain_library, SetLastError = true)]
        internal static extern unsafe sbyte* explain_ioctl(int fd, int request, IntPtr argp);

        [DllImport(explain_library, SetLastError = true)]
        internal static extern unsafe sbyte* explain_errno_ioctl(int errno, int fd, int request, IntPtr argp);
        #endregion

        [DllImport(libc_library, SetLastError = true)]
        internal static extern IntPtr mmap(IntPtr addr, int length, MemoryMappedProtections prot, MemoryMappedFlags flags, int fd, int offset);

        [DllImport(libc_library)]
        internal static extern int munmap(IntPtr addr, int length);
    }
}
