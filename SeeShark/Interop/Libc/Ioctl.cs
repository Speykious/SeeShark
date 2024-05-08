// Copyright (c) Speykious
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 3-Clause License. See LICENSE for details.

namespace SeeShark.Interop.Libc;

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

    public static readonly int VIDIOC_QUERYCAP = IOR<v4l2_capability>('V', 0);
    public static readonly int VIDIOC_ENUM_FMT = IOWR<v4l2_fmtdesc>('V', 2);
    public static readonly int VIDIOC_G_FMT = IOWR<v4l2_format>('V', 4);
    public static readonly int VIDIOC_S_FMT = IOWR<v4l2_format>('V', 5);
    public static readonly int VIDIOC_REQBUFS = IOWR<v4l2_requestbuffers>('V', 8);
    public static readonly int VIDIOC_QUERYBUF = IOWR<v4l2_buffer>('V', 9);
    public static readonly int VIDIOC_OVERLAY = IOW<int>('V', 14);
    public static readonly int VIDIOC_QBUF = IOWR<v4l2_buffer>('V', 15);
    public static readonly int VIDIOC_DQBUF = IOWR<v4l2_buffer>('V', 17);
    public static readonly int VIDIOC_STREAMON = IOW<int>('V', 18);
    public static readonly int VIDIOC_STREAMOFF = IOW<int>('V', 19);
    public static readonly int VIDIOC_G_PARM = IOWR<v4l2_streamparm>('V', 21);
    public static readonly int VIDIOC_S_PARM = IOWR<v4l2_streamparm>('V', 22);
    public static readonly int VIDIOC_G_CTRL = IOWR<v4l2_control>('V', 27);
    public static readonly int VIDIOC_S_CTRL = IOWR<v4l2_control>('V', 28);
    public static readonly int VIDIOC_QUERYCTRL = IOWR<v4l2_queryctrl>('V', 36);
    public static readonly int VIDIOC_G_INPUT = IOR<int>('V', 38);
    public static readonly int VIDIOC_S_INPUT = IOWR<int>('V', 39);
    public static readonly int VIDIOC_G_OUTPUT = IOR<int>('V', 46);
    public static readonly int VIDIOC_S_OUTPUT = IOWR<int>('V', 47);
    public static readonly int VIDIOC_CROPCAP = IOWR<v4l2_cropcap>('V', 58);
    public static readonly int VIDIOC_G_CROP = IOWR<v4l2_crop>('V', 59);
    public static readonly int VIDIOC_S_CROP = IOW<v4l2_crop>('V', 60);
    public static readonly int VIDIOC_TRY_FMT = IOWR<v4l2_format>('V', 64);
    public static readonly int VIDIOC_G_PRIORITY = IOR<uint>('V', 67);
    public static readonly int VIDIOC_S_PRIORITY = IOW<uint>('V', 68);
    public static readonly int VIDIOC_ENUM_FRAMESIZES = IOWR<v4l2_frmsizeenum>('V', 74);
    public static readonly int VIDIOC_ENUM_FRAMEINTERVALS = IOWR<v4l2_frmivalenum>('V', 75);
    public static readonly int VIDIOC_PREPARE_BUF = IOWR<v4l2_buffer>('V', 93);

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
        ENUM_FRAMESIZES = -1070311862,
        ENUM_FRAMEINTERVALS = -1069787573,
        PREPARE_BUF = -1067952547,
    }

    internal static int IOC(int dir, int type, int nr, int size)
            => (dir << ioc_dirshift) | (type << ioc_typeshift) | (nr << ioc_nrshift) | (size << ioc_sizeshift);

    internal static int IO(int type, int nr) => IOC(ioc_none, type, nr, 0);
    internal static int IOR<T>(int type, int nr) where T : unmanaged => IOC(ioc_read, type, nr, IOC_TYPECHECK<T>());
    internal static int IOW<T>(int type, int nr) where T : unmanaged => IOC(ioc_write, type, nr, IOC_TYPECHECK<T>());
    internal static int IOWR<T>(int type, int nr) where T : unmanaged => IOC(ioc_read | ioc_write, type, nr, IOC_TYPECHECK<T>());
    internal static unsafe int IOC_TYPECHECK<T>() where T : unmanaged => sizeof(T);
}
