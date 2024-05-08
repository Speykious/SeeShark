// Copyright (c) Speykious
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 3-Clause License. See LICENSE for details.

using System;
using System.Runtime.InteropServices;

namespace SeeShark.Interop.Libc;

internal class Libc
{
    private const string libc_library = "libc";
    private const string explain_library = "explain";

    internal const int EPERM = 1;
    internal const int ENOENT = 2;
    internal const int ESRCH = 3;
    internal const int EINTR = 4;
    internal const int EIO = 5;
    internal const int ENXIO = 6;
    internal const int E2BIG = 7;
    internal const int ENOEXEC = 8;
    internal const int EBADF = 9;
    internal const int ECHILD = 10;
    internal const int EAGAIN = 11;
    internal const int ENOMEM = 12;
    internal const int EACCES = 13;
    internal const int EFAULT = 14;
    internal const int ENOTBLK = 15;
    internal const int EBUSY = 16;
    internal const int EEXIST = 17;
    internal const int EXDEV = 18;
    internal const int ENODEV = 19;
    internal const int ENOTDIR = 20;
    internal const int EISDIR = 21;
    internal const int EINVAL = 22;
    internal const int ENFILE = 23;
    internal const int EMFILE = 24;
    internal const int ENOTTY = 25;
    internal const int ETXTBSY = 26;
    internal const int EFBIG = 27;
    internal const int ENOSPC = 28;
    internal const int ESPIPE = 29;
    internal const int EROFS = 30;
    internal const int EMLINK = 31;
    internal const int EPIPE = 32;
    internal const int EDOM = 33;
    internal const int ERANGE = 34;

    internal enum FileOpenFlags
    {
        O_RDONLY = 0x00,
        O_RDWR = 0x02,
        O_NONBLOCK = 0x800,
        O_SYNC = 0x101000
    }

    [Flags]
    internal enum MmapFlags
    {
        MAP_SHARED = 0x01,
        MAP_PRIVATE = 0x02,
        MAP_FIXED = 0x10
    }

    [Flags]
    internal enum MmapProtFlags
    {
        PROT_NONE = 0x0,
        PROT_READ = 0x1,
        PROT_WRITE = 0x2,
        PROT_EXEC = 0x4
    }

    [DllImport(libc_library, SetLastError = true)]
    internal static extern int open([MarshalAs(UnmanagedType.LPStr)] string pathname, FileOpenFlags flags);

    [DllImport(libc_library)]
    internal static extern int close(int fd);

    [DllImport(libc_library, SetLastError = true)]
    internal static extern unsafe int read(int fd, void* buf, int count);

    [DllImport(libc_library, SetLastError = true)]
    internal static extern unsafe int write(int fd, void* buf, int count);

    #region ioctl
    [DllImport(libc_library, SetLastError = true)]
    internal static extern unsafe int ioctl(int fd, Ioctl.VidIOC request, void* argp);

    [DllImport(explain_library, SetLastError = true)]
    internal static extern unsafe sbyte* explain_ioctl(int fd, Ioctl.VidIOC request, void* argp);

    [DllImport(explain_library, SetLastError = true)]
    internal static extern unsafe sbyte* explain_errno_ioctl(int errno, int fd, Ioctl.VidIOC request, void* argp);
    #endregion

    [DllImport(libc_library, SetLastError = true)]
    internal static extern unsafe void* mmap(void* addr, int length, MmapProtFlags prot, MmapFlags flags, int fd, int offset);

    [DllImport(libc_library)]
    internal static extern unsafe int munmap(void* addr, int length);
}
