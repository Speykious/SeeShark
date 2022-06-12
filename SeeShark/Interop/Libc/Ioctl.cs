// Copyright (c) The Vignette Authors
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 3-Clause License. See LICENSE for details.

using System;
using System.Runtime.InteropServices;

namespace SeeShark.Interop.Libc
{
    internal partial class Ioctl
    {
        const int ioc_nrbits = 8;
        const int ioc_typebits = 8;
        const int ioc_sizebits = 14;
        // const int ioc_dirbits = 2;

        // const int ioc_nrmask = (1 << ioc_nrbits) - 1;
        // const int ioc_typemask = (1 << ioc_typebits) - 1;
        // const int ioc_sizemask = (1 << ioc_sizebits) - 1;
        // const int ioc_dirmask = (1 << ioc_dirbits) - 1;

        const int ioc_nrshift = 0;
        const int ioc_typeshift = ioc_nrshift + ioc_nrbits;
        const int ioc_sizeshift = ioc_typeshift + ioc_typebits;
        const int ioc_dirshift = ioc_sizeshift + ioc_sizebits;

        const int ioc_none = 0;
        const int ioc_write = 1;
        const int ioc_read = 2;

        internal static int IOC(int dir, int type, int nr, int size)
                => dir << ioc_dirshift | type << ioc_typeshift | nr << ioc_nrshift | size << ioc_sizeshift;

        internal static int IO(int type, int nr) => IOC(ioc_none, type, nr, 0);
        internal static int IOR(int type, int nr, Type size) => IOC(ioc_read, type, nr, IOC_TYPECHECK(size));
        internal static int IOW(int type, int nr, Type size) => IOC(ioc_write, type, nr, IOC_TYPECHECK(size));
        internal static int IOWR(int type, int nr, Type size) => IOC(ioc_read | ioc_write, type, nr, IOC_TYPECHECK(size));
        internal static int IOC_TYPECHECK(Type t) => Marshal.SizeOf(t);
    }
}
