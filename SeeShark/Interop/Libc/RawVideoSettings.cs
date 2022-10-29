// Copyright (c) The Vignette Authors
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 3-Clause License. See LICENSE for details.

using System;

namespace SeeShark.Interop.Libc
{
    /// <summary>
    /// videodev2.h Request Definition
    /// </summary>
    internal static class RawVideoSettings
    {
        public static readonly int VIDIOC_QUERYCAP = Ioctl.IOR('V', 0, typeof(v4l2_capability));
        public static readonly int VIDIOC_ENUM_FMT = Ioctl.IOWR('V', 2, typeof(v4l2_fmtdesc));
        public static readonly int VIDIOC_G_FMT = Ioctl.IOWR('V', 4, typeof(v4l2_format));
        public static readonly int VIDIOC_S_FMT = Ioctl.IOWR('V', 5, typeof(v4l2_format));
        public static readonly int VIDIOC_REQBUFS = Ioctl.IOWR('V', 8, typeof(v4l2_requestbuffers));
        public static readonly int VIDIOC_QUERYBUF = Ioctl.IOWR('V', 9, typeof(v4l2_buffer));
        public static readonly int VIDIOC_OVERLAY = Ioctl.IOW('V', 14, typeof(int));
        public static readonly int VIDIOC_QBUF = Ioctl.IOWR('V', 15, typeof(v4l2_buffer));
        public static readonly int VIDIOC_DQBUF = Ioctl.IOWR('V', 17, typeof(v4l2_buffer));
        public static readonly int VIDIOC_STREAMON = Ioctl.IOW('V', 18, typeof(int));
        public static readonly int VIDIOC_STREAMOFF = Ioctl.IOW('V', 19, typeof(int));
        public static readonly int VIDIOC_G_PARM = Ioctl.IOWR('V', 21, typeof(v4l2_streamparm));
        public static readonly int VIDIOC_S_PARM = Ioctl.IOWR('V', 22, typeof(v4l2_streamparm));
        public static readonly int VIDIOC_G_CTRL = Ioctl.IOWR('V', 27, typeof(v4l2_control));
        public static readonly int VIDIOC_S_CTRL = Ioctl.IOWR('V', 28, typeof(v4l2_control));
        public static readonly int VIDIOC_QUERYCTRL = Ioctl.IOWR('V', 36, typeof(v4l2_queryctrl));
        public static readonly int VIDIOC_G_INPUT = Ioctl.IOR('V', 38, typeof(int));
        public static readonly int VIDIOC_S_INPUT = Ioctl.IOWR('V', 39, typeof(int));
        public static readonly int VIDIOC_G_OUTPUT = Ioctl.IOR('V', 46, typeof(int));
        public static readonly int VIDIOC_S_OUTPUT = Ioctl.IOWR('V', 47, typeof(int));
        public static readonly int VIDIOC_CROPCAP = Ioctl.IOWR('V', 58, typeof(v4l2_cropcap));
        public static readonly int VIDIOC_G_CROP = Ioctl.IOWR('V', 59, typeof(v4l2_crop));
        public static readonly int VIDIOC_S_CROP = Ioctl.IOW('V', 60, typeof(v4l2_crop));
        public static readonly int VIDIOC_TRY_FMT = Ioctl.IOWR('V', 64, typeof(v4l2_format));
        public static readonly int VIDIOC_G_PRIORITY = Ioctl.IOR('V', 67, typeof(uint));
        public static readonly int VIDIOC_S_PRIORITY = Ioctl.IOW('V', 68, typeof(uint));
        public static readonly int VIDIOC_ENUM_FRAMESIZES = Ioctl.IOWR('V', 74, typeof(v4l2_frmsizeenum));
        public static readonly int VIDIOC_ENUM_FRAMEINTERVALS = Ioctl.IOWR('V', 75, typeof(v4l2_frmivalenum));
        public static readonly int VIDIOC_PREPARE_BUF = Ioctl.IOWR('V', 93, typeof(v4l2_buffer));

        public static void PrintConstants()
        {
            Console.WriteLine($"    internal enum VideoSettings : int");
            Console.WriteLine($"    {{");
            Console.WriteLine($"        {nameof(VIDIOC_QUERYCAP)} = {VIDIOC_QUERYCAP},");
            Console.WriteLine($"        {nameof(VIDIOC_ENUM_FMT)} = {VIDIOC_ENUM_FMT},");
            Console.WriteLine($"        {nameof(VIDIOC_G_FMT)} = {VIDIOC_G_FMT},");
            Console.WriteLine($"        {nameof(VIDIOC_S_FMT)} = {VIDIOC_S_FMT},");
            Console.WriteLine($"        {nameof(VIDIOC_REQBUFS)} = {VIDIOC_REQBUFS},");
            Console.WriteLine($"        {nameof(VIDIOC_QUERYBUF)} = {VIDIOC_QUERYBUF},");
            Console.WriteLine($"        {nameof(VIDIOC_OVERLAY)} = {VIDIOC_OVERLAY},");
            Console.WriteLine($"        {nameof(VIDIOC_QBUF)} = {VIDIOC_QBUF},");
            Console.WriteLine($"        {nameof(VIDIOC_DQBUF)} = {VIDIOC_DQBUF},");
            Console.WriteLine($"        {nameof(VIDIOC_STREAMON)} = {VIDIOC_STREAMON},");
            Console.WriteLine($"        {nameof(VIDIOC_STREAMOFF)} = {VIDIOC_STREAMOFF},");
            Console.WriteLine($"        {nameof(VIDIOC_G_PARM)} = {VIDIOC_G_PARM},");
            Console.WriteLine($"        {nameof(VIDIOC_S_PARM)} = {VIDIOC_S_PARM},");
            Console.WriteLine($"        {nameof(VIDIOC_G_CTRL)} = {VIDIOC_G_CTRL},");
            Console.WriteLine($"        {nameof(VIDIOC_S_CTRL)} = {VIDIOC_S_CTRL},");
            Console.WriteLine($"        {nameof(VIDIOC_QUERYCTRL)} = {VIDIOC_QUERYCTRL},");
            Console.WriteLine($"        {nameof(VIDIOC_G_INPUT)} = {VIDIOC_G_INPUT},");
            Console.WriteLine($"        {nameof(VIDIOC_S_INPUT)} = {VIDIOC_S_INPUT},");
            Console.WriteLine($"        {nameof(VIDIOC_G_OUTPUT)} = {VIDIOC_G_OUTPUT},");
            Console.WriteLine($"        {nameof(VIDIOC_S_OUTPUT)} = {VIDIOC_S_OUTPUT},");
            Console.WriteLine($"        {nameof(VIDIOC_CROPCAP)} = {VIDIOC_CROPCAP},");
            Console.WriteLine($"        {nameof(VIDIOC_G_CROP)} = {VIDIOC_G_CROP},");
            Console.WriteLine($"        {nameof(VIDIOC_S_CROP)} = {VIDIOC_S_CROP},");
            Console.WriteLine($"        {nameof(VIDIOC_TRY_FMT)} = {VIDIOC_TRY_FMT},");
            Console.WriteLine($"        {nameof(VIDIOC_G_PRIORITY)} = {VIDIOC_G_PRIORITY},");
            Console.WriteLine($"        {nameof(VIDIOC_S_PRIORITY)} = {VIDIOC_S_PRIORITY},");
            Console.WriteLine($"        {nameof(VIDIOC_ENUM_FRAMESIZES)} = {VIDIOC_ENUM_FRAMESIZES},");
            Console.WriteLine($"        {nameof(VIDIOC_ENUM_FRAMEINTERVALS)} = {VIDIOC_ENUM_FRAMEINTERVALS},");
            Console.WriteLine($"        {nameof(VIDIOC_PREPARE_BUF)} = {VIDIOC_PREPARE_BUF},");
            Console.WriteLine($"    }}");
        }
    }
}
