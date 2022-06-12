// Copyright (c) The Vignette Authors
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 3-Clause License. See LICENSE for details.

namespace SeeShark.Interop.Libc
{
    /// <summary>
    /// videodev2.h Request Definition
    /// </summary>
    internal class RawVideoSettings
    {
        public static int VIDIOC_QUERYCAP = Ioctl.IOR('V', 0, typeof(v4l2_capability));
        public static int VIDIOC_ENUM_FMT = Ioctl.IOWR('V', 2, typeof(v4l2_fmtdesc));
        public static int VIDIOC_G_FMT = Ioctl.IOWR('V', 4, typeof(v4l2_format));
        public static int VIDIOC_S_FMT = Ioctl.IOWR('V', 5, typeof(v4l2_format));
        public static int VIDIOC_REQBUFS = Ioctl.IOWR('V', 8, typeof(v4l2_requestbuffers));
        public static int VIDIOC_QUERYBUF = Ioctl.IOWR('V', 9, typeof(v4l2_buffer));
        public static int VIDIOC_OVERLAY = Ioctl.IOW('V', 14, typeof(int));
        public static int VIDIOC_QBUF = Ioctl.IOWR('V', 15, typeof(v4l2_buffer));
        public static int VIDIOC_DQBUF = Ioctl.IOWR('V', 17, typeof(v4l2_buffer));
        public static int VIDIOC_STREAMON = Ioctl.IOW('V', 18, typeof(int));
        public static int VIDIOC_STREAMOFF = Ioctl.IOW('V', 19, typeof(int));
        public static int VIDIOC_G_CTRL = Ioctl.IOWR('V', 27, typeof(v4l2_control));
        public static int VIDIOC_S_CTRL = Ioctl.IOWR('V', 28, typeof(v4l2_control));
        public static int VIDIOC_QUERYCTRL = Ioctl.IOWR('V', 36, typeof(v4l2_queryctrl));
        public static int VIDIOC_G_INPUT = Ioctl.IOR('V', 38, typeof(int));
        public static int VIDIOC_S_INPUT = Ioctl.IOWR('V', 39, typeof(int));
        public static int VIDIOC_G_OUTPUT = Ioctl.IOR('V', 46, typeof(int));
        public static int VIDIOC_S_OUTPUT = Ioctl.IOWR('V', 47, typeof(int));
        public static int VIDIOC_CROPCAP = Ioctl.IOWR('V', 58, typeof(v4l2_cropcap));
        public static int VIDIOC_G_CROP = Ioctl.IOWR('V', 59, typeof(v4l2_crop));
        public static int VIDIOC_S_CROP = Ioctl.IOW('V', 60, typeof(v4l2_crop));
        public static int VIDIOC_TRY_FMT = Ioctl.IOWR('V', 64, typeof(v4l2_format));
        public static int VIDIOC_G_PRIORITY = Ioctl.IOR('V', 67, typeof(uint));
        public static int VIDIOC_S_PRIORITY = Ioctl.IOW('V', 68, typeof(uint));
        public static int VIDIOC_ENUM_FRAMESIZES = Ioctl.IOWR('V', 74, typeof(v4l2_frmsizeenum));
        public static int VIDIOC_PREPARE_BUF = Ioctl.IOWR('V', 93, typeof(v4l2_buffer));
    }
}
