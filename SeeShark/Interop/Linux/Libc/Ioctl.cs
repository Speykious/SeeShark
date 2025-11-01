// Copyright (c) Speykious
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 2-Clause License. See LICENSE for details.

using System;
using System.Runtime.Versioning;

namespace SeeShark.Interop.Linux.Libc;

[SupportedOSPlatform("Linux")]
internal static class Ioctl
{
    private const int ioc_nrbits = 8;
    private const int ioc_typebits = 8;
    private const int ioc_sizebits = 14;

    // const int ioc_dirbits = 2;

    // const int ioc_nrmask = (1 << ioc_nrbits) - 1;
    // const int ioc_typemask = (1 << ioc_typebits) - 1;
    // const int ioc_sizemask = (1 << ioc_sizebits) - 1;
    // const int ioc_dirmask = (1 << ioc_dirbits) - 1;

    private const int ioc_nrshift = 0;
    private const int ioc_typeshift = ioc_nrshift + ioc_nrbits;
    private const int ioc_sizeshift = ioc_typeshift + ioc_typebits;
    private const int ioc_dirshift = ioc_sizeshift + ioc_sizebits;

    private const int ioc_none = 0;
    private const int ioc_write = 1;
    private const int ioc_read = 2;

    internal static readonly int VIDIOC_QUERYCAP = IOR<v4l2_capability>('V', 0);
    internal static readonly int VIDIOC_ENUM_FMT = IOWR<v4l2_fmtdesc>('V', 2);
    internal static readonly int VIDIOC_G_FMT = IOWR<v4l2_format>('V', 4);
    internal static readonly int VIDIOC_S_FMT = IOWR<v4l2_format>('V', 5);
    internal static readonly int VIDIOC_REQBUFS = IOWR<v4l2_requestbuffers>('V', 8);
    internal static readonly int VIDIOC_QUERYBUF = IOWR<v4l2_buffer>('V', 9);
    internal static readonly int VIDIOC_OVERLAY = IOW<int>('V', 14);
    internal static readonly int VIDIOC_QBUF = IOWR<v4l2_buffer>('V', 15);
    internal static readonly int VIDIOC_DQBUF = IOWR<v4l2_buffer>('V', 17);
    internal static readonly int VIDIOC_STREAMON = IOW<int>('V', 18);
    internal static readonly int VIDIOC_STREAMOFF = IOW<int>('V', 19);
    internal static readonly int VIDIOC_G_PARM = IOWR<v4l2_streamparm>('V', 21);
    internal static readonly int VIDIOC_S_PARM = IOWR<v4l2_streamparm>('V', 22);
    internal static readonly int VIDIOC_G_CTRL = IOWR<v4l2_control>('V', 27);
    internal static readonly int VIDIOC_S_CTRL = IOWR<v4l2_control>('V', 28);
    internal static readonly int VIDIOC_QUERYCTRL = IOWR<v4l2_queryctrl>('V', 36);
    internal static readonly int VIDIOC_G_INPUT = IOR<int>('V', 38);
    internal static readonly int VIDIOC_S_INPUT = IOWR<int>('V', 39);
    internal static readonly int VIDIOC_G_OUTPUT = IOR<int>('V', 46);
    internal static readonly int VIDIOC_S_OUTPUT = IOWR<int>('V', 47);
    internal static readonly int VIDIOC_CROPCAP = IOWR<v4l2_cropcap>('V', 58);
    internal static readonly int VIDIOC_G_CROP = IOWR<v4l2_crop>('V', 59);
    internal static readonly int VIDIOC_S_CROP = IOW<v4l2_crop>('V', 60);
    internal static readonly int VIDIOC_TRY_FMT = IOWR<v4l2_format>('V', 64);
    internal static readonly int VIDIOC_G_PRIORITY = IOR<uint>('V', 67);
    internal static readonly int VIDIOC_S_PRIORITY = IOW<uint>('V', 68);
    internal static readonly int VIDIOC_ENUM_FRAMESIZES = IOWR<v4l2_frmsizeenum>('V', 74);
    internal static readonly int VIDIOC_ENUM_FRAMEINTERVALS = IOWR<v4l2_frmivalenum>('V', 75);
    internal static readonly int VIDIOC_PREPARE_BUF = IOWR<v4l2_buffer>('V', 93);

    internal enum VidIOC : int
    {
        QUERYCAP = -2140645888,
        ENUM_FMT = -1069525502,
        G_FMT = -1060088316,
        S_FMT = -1060088315,
        REQBUFS = -1072409080,
        QUERYBUF = -1067952631,
        OVERLAY = 1074025998,
        QBUF = -1067952625,
        DQBUF = -1067952623,
        STREAMON = 1074026002,
        STREAMOFF = 1074026003,
        G_PARM = -1060350443,
        S_PARM = -1060350442,
        G_CTRL = -1073195493,
        S_CTRL = -1073195492,
        QUERYCTRL = -1069263324,
        G_INPUT = -2147199450,
        S_INPUT = -1073457625,
        G_OUTPUT = -2147199442,
        S_OUTPUT = -1073457617,
        CROPCAP = -1070836166,
        G_CROP = -1072409029,
        S_CROP = 1075074620,
        TRY_FMT = -1060088256,
        G_PRIORITY = -2147199421,
        S_PRIORITY = 1074026052,
        ENUM_FRAMESIZES = -1070836150,
        ENUM_FRAMEINTERVALS = -1070311861,
        PREPARE_BUF = -1067952547,
    }

    internal static void PrintEnumVidIOC()
    {
        // I can't just put the values inside the enum directly...
        // It is what it is.

        Console.WriteLine("internal enum VidIOC : int");
        Console.WriteLine("{");
        Console.WriteLine($"    QUERYCAP = {VIDIOC_QUERYCAP},");
        Console.WriteLine($"    ENUM_FMT = {VIDIOC_ENUM_FMT},");
        Console.WriteLine($"    G_FMT = {VIDIOC_G_FMT},");
        Console.WriteLine($"    S_FMT = {VIDIOC_S_FMT},");
        Console.WriteLine($"    REQBUFS = {VIDIOC_REQBUFS},");
        Console.WriteLine($"    QUERYBUF = {VIDIOC_QUERYBUF},");
        Console.WriteLine($"    OVERLAY = {VIDIOC_OVERLAY},");
        Console.WriteLine($"    QBUF = {VIDIOC_QBUF},");
        Console.WriteLine($"    DQBUF = {VIDIOC_DQBUF},");
        Console.WriteLine($"    STREAMON = {VIDIOC_STREAMON},");
        Console.WriteLine($"    STREAMOFF = {VIDIOC_STREAMOFF},");
        Console.WriteLine($"    G_PARM = {VIDIOC_G_PARM},");
        Console.WriteLine($"    S_PARM = {VIDIOC_S_PARM},");
        Console.WriteLine($"    G_CTRL = {VIDIOC_G_CTRL},");
        Console.WriteLine($"    S_CTRL = {VIDIOC_S_CTRL},");
        Console.WriteLine($"    QUERYCTRL = {VIDIOC_QUERYCTRL},");
        Console.WriteLine($"    G_INPUT = {VIDIOC_G_INPUT},");
        Console.WriteLine($"    S_INPUT = {VIDIOC_S_INPUT},");
        Console.WriteLine($"    G_OUTPUT = {VIDIOC_G_OUTPUT},");
        Console.WriteLine($"    S_OUTPUT = {VIDIOC_S_OUTPUT},");
        Console.WriteLine($"    CROPCAP = {VIDIOC_CROPCAP},");
        Console.WriteLine($"    G_CROP = {VIDIOC_G_CROP},");
        Console.WriteLine($"    S_CROP = {VIDIOC_S_CROP},");
        Console.WriteLine($"    TRY_FMT = {VIDIOC_TRY_FMT},");
        Console.WriteLine($"    G_PRIORITY = {VIDIOC_G_PRIORITY},");
        Console.WriteLine($"    S_PRIORITY = {VIDIOC_S_PRIORITY},");
        Console.WriteLine($"    ENUM_FRAMESIZES = {VIDIOC_ENUM_FRAMESIZES},");
        Console.WriteLine($"    ENUM_FRAMEINTERVALS = {VIDIOC_ENUM_FRAMEINTERVALS},");
        Console.WriteLine($"    PREPARE_BUF = {VIDIOC_PREPARE_BUF},");
        Console.WriteLine("}");
    }

    internal static int IOC(int dir, int type, int nr, int size)
            => (dir << ioc_dirshift) | (type << ioc_typeshift) | (nr << ioc_nrshift) | (size << ioc_sizeshift);

    internal static int IO(int type, int nr) => IOC(ioc_none, type, nr, 0);
    internal static int IOR<T>(int type, int nr) where T : unmanaged => IOC(ioc_read, type, nr, IOC_TYPECHECK<T>());
    internal static int IOW<T>(int type, int nr) where T : unmanaged => IOC(ioc_write, type, nr, IOC_TYPECHECK<T>());
    internal static int IOWR<T>(int type, int nr) where T : unmanaged => IOC(ioc_read | ioc_write, type, nr, IOC_TYPECHECK<T>());
    internal static unsafe int IOC_TYPECHECK<T>() where T : unmanaged => sizeof(T);
}
