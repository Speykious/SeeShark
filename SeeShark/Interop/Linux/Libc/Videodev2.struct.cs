// Copyright (c) Speykious
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 2-Clause License. See LICENSE for details.

using System.Runtime.InteropServices;

namespace SeeShark.Interop.Linux.Libc;

#pragma warning disable IDE1006
[StructLayout(LayoutKind.Sequential)]
internal unsafe struct v4l2_capability
{
    internal fixed byte driver[16];
    internal fixed byte card[32];
    internal fixed byte bus_info[32];
    internal uint version;
    internal uint capabilities;
    internal uint device_caps;
    internal fixed uint reserved[3];
};

internal enum v4l2_ctrl_type : uint
{
    V4L2_CTRL_TYPE_INTEGER = 1,
    V4L2_CTRL_TYPE_BOOLEAN = 2,
    V4L2_CTRL_TYPE_MENU = 3,
    V4L2_CTRL_TYPE_BUTTON = 4,
    V4L2_CTRL_TYPE_INTEGER64 = 5,
    V4L2_CTRL_TYPE_CTRL_CLASS = 6,
    V4L2_CTRL_TYPE_STRING = 7,
    V4L2_CTRL_TYPE_BITMASK = 8,
    V4L2_CTRL_TYPE_INTEGER_MENU = 9,
    V4L2_CTRL_COMPOUND_TYPES = 0x0100,
    V4L2_CTRL_TYPE_U8 = 0x0100,
    V4L2_CTRL_TYPE_U16 = 0x0101,
    V4L2_CTRL_TYPE_U32 = 0x0102,
};

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct v4l2_queryctrl
{
    internal uint id;
    internal v4l2_ctrl_type type;
    internal fixed byte name[32];
    internal int minimum;
    internal int maximum;
    internal int step;
    internal int default_value;
    internal uint flags;
    internal fixed uint reserved[2];
};

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct v4l2_control
{
    internal uint id;
    internal int value;
}

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct v4l2_fmtdesc
{
    internal uint index;
    internal v4l2_buf_type type;
    internal uint flags;
    internal fixed byte description[32];
    internal V4l2InputFormat pixelformat;
    internal uint mbus_code;
    internal fixed uint reserved[3];
}

internal enum v4l2_buf_type : uint
{
    V4L2_BUF_TYPE_VIDEO_CAPTURE = 1,
    V4L2_BUF_TYPE_VIDEO_OUTPUT = 2,
    V4L2_BUF_TYPE_VIDEO_OVERLAY = 3,
    V4L2_BUF_TYPE_VBI_CAPTURE = 4,
    V4L2_BUF_TYPE_VBI_OUTPUT = 5,
    V4L2_BUF_TYPE_SLICED_VBI_CAPTURE = 6,
    V4L2_BUF_TYPE_SLICED_VBI_OUTPUT = 7,
    V4L2_BUF_TYPE_VIDEO_OUTPUT_OVERLAY = 8,
    V4L2_BUF_TYPE_VIDEO_CAPTURE_MPLANE = 9,
    V4L2_BUF_TYPE_VIDEO_OUTPUT_MPLANE = 10,
    V4L2_BUF_TYPE_SDR_CAPTURE = 11,
    V4L2_BUF_TYPE_SDR_OUTPUT = 12,
    V4L2_BUF_TYPE_META_CAPTURE = 13,
    V4L2_BUF_TYPE_META_OUTPUT = 14,
    V4L2_BUF_TYPE_PRIVATE = 0x80,
}

internal enum v4l2_field : uint
{
    V4L2_FIELD_ANY = 0,
    V4L2_FIELD_NONE = 1,
    V4L2_FIELD_TOP = 2,
    V4L2_FIELD_BOTTOM = 3,
    V4L2_FIELD_INTERLACED = 4,
    V4L2_FIELD_SEQ_TB = 5,
    V4L2_FIELD_SEQ_BT = 6,
    V4L2_FIELD_ALTERNATE = 7,
    V4L2_FIELD_INTERLACED_TB = 8,
    V4L2_FIELD_INTERLACED_BT = 9,
}

internal enum v4l2_colorspace : uint
{
    V4L2_COLORSPACE_DEFAULT = 0,
    V4L2_COLORSPACE_SMPTE170M = 1,
    V4L2_COLORSPACE_SMPTE240M = 2,
    V4L2_COLORSPACE_REC709 = 3,
    V4L2_COLORSPACE_BT878 = 4,
    V4L2_COLORSPACE_470_SYSTEM_M = 5,
    V4L2_COLORSPACE_470_SYSTEM_BG = 6,
    V4L2_COLORSPACE_JPEG = 7,
    V4L2_COLORSPACE_SRGB = 8,
    V4L2_COLORSPACE_ADOBERGB = 9,
    V4L2_COLORSPACE_BT2020 = 10,
    V4L2_COLORSPACE_RAW = 11,
}

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct v4l2_pix_format
{
    internal uint width;
    internal uint height;
    internal V4l2InputFormat pixelformat;
    internal v4l2_field field;
    internal uint bytesperline;
    internal uint sizeimage;
    internal v4l2_colorspace colorspace;
    internal uint priv;
}

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct v4l2_rect
{
    internal int left;
    internal int top;
    internal uint width;
    internal uint height;
}

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct v4l2_clip
{
    internal v4l2_rect c;
    internal v4l2_clip* next;
}

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct v4l2_window
{
    internal v4l2_rect w;
    internal v4l2_field field;
    internal uint chromakey;
    internal v4l2_clip* clips;
    internal uint clipcount;
    internal void* bitmap;
    internal byte global_alpha;
}

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct v4l2_vbi_format
{
    internal uint sampling_rate;
    internal uint offset;
    internal uint samples_per_line;
    internal uint sample_format;
    internal fixed int start[2];
    internal fixed uint count[2];
    internal uint flags;
    internal fixed uint reserved[2];
}

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct v4l2_sliced_vbi_format
{
    internal uint service_set;
    internal fixed ushort service_lines[48];
    internal uint io_size;
    internal fixed uint reserved[2];
}

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct v4l2_sdr_format
{
    internal V4l2InputFormat pixelformat;
    internal uint buffersize;
    internal fixed byte reserved[24];
}

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct v4l2_meta_format
{
    internal uint dataformat;
    internal uint buffersize;
}

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct v4l2_format
{
    internal v4l2_buf_type type;

    [StructLayout(LayoutKind.Explicit)]
    internal unsafe struct fmt_union
    {
        [FieldOffset(0)]
        internal v4l2_pix_format pix;
        [FieldOffset(0)]
        internal v4l2_window win;
        [FieldOffset(0)]
        internal v4l2_vbi_format vbi;
        [FieldOffset(0)]
        internal v4l2_sliced_vbi_format sliced;
        [FieldOffset(0)]
        internal v4l2_sdr_format sdr;
        [FieldOffset(0)]
        internal v4l2_meta_format meta;
        [FieldOffset(0)]
        internal fixed byte raw_data[200];
    }
    internal fmt_union fmt;
}

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct v4l2_captureparm
{
    internal uint capability;
    internal uint capturemode;
    internal v4l2_fract timeperframe;
    internal uint extendedmode;
    internal uint readbuffers;
    internal fixed uint reserved[4];
}

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct v4l2_outputparm
{
    internal uint capability;
    internal uint outputmode;
    internal v4l2_fract timeperframe;
    internal uint extendedmode;
    internal uint writebuffers;
    internal fixed uint reserved[4];
}

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct v4l2_streamparm
{
    internal v4l2_buf_type type;

    [StructLayout(LayoutKind.Explicit)]
    internal unsafe struct parm_union
    {
        [FieldOffset(0)]
        internal v4l2_captureparm capture;
        [FieldOffset(0)]
        internal v4l2_outputparm output;
        [FieldOffset(0)]
        internal fixed byte raw[200];
    }
    internal parm_union parm;
}

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct v4l2_fract
{
    internal uint numerator;
    internal uint denominator;
}

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct v4l2_cropcap
{
    internal v4l2_buf_type type;
    internal v4l2_rect bounds;
    internal v4l2_rect defrect;
    internal v4l2_fract pixelaspect;
}

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct v4l2_crop
{
    internal v4l2_buf_type type;
    internal v4l2_rect c;
}

internal enum v4l2_memory : uint
{
    V4L2_MEMORY_MMAP = 1,
    V4L2_MEMORY_USERPTR = 2,
    V4L2_MEMORY_OVERLAY = 3,
    V4L2_MEMORY_DMABUF = 4,
}

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct v4l2_requestbuffers
{
    internal uint count;
    internal v4l2_buf_type type;
    internal v4l2_memory memory;
    internal fixed uint reserved[2];
}

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct v4l2_timecode
{
    internal uint type;
    internal uint flags;
    internal byte frames;
    internal byte seconds;
    internal byte minutes;
    internal byte hours;
    internal fixed byte userbits[4];
}

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct v4l2_plane
{
    internal uint bytesused;
    internal uint length;

    [StructLayout(LayoutKind.Explicit)]
    internal struct m_union
    {
        [FieldOffset(0)]
        uint mem_offset;
        [FieldOffset(0)]
        ulong userptr;
        [FieldOffset(0)]
        int fd;
    }
    internal m_union m;

    uint data_offset;
    fixed uint reserved[11];
};

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct v4l2_buffer
{
    internal uint index;
    internal v4l2_buf_type type;
    internal uint bytesused;
    internal uint flags;
    internal v4l2_field field;
    internal timeval_t timestamp;

    internal v4l2_timecode timecode;
    internal uint sequence;
    internal v4l2_memory memory;

    [StructLayout(LayoutKind.Explicit)]
    internal struct m_union
    {
        [FieldOffset(0)]
        internal uint offset;
        [FieldOffset(0)]
        internal ulong userptr;
        [FieldOffset(0)]
        internal v4l2_plane* planes;
        [FieldOffset(0)]
        internal int fd;
    }
    internal m_union m;

    internal uint length;
    internal uint input;
    internal uint reserved;
}

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct v4l2_frmsizeenum
{
    internal uint index;
    internal V4l2InputFormat pixel_format;
    internal v4l2_frmsizetypes type;

    [StructLayout(LayoutKind.Explicit)]
    internal struct frame_size_union
    {
        [FieldOffset(0)]
        internal v4l2_frmsize_discrete discrete;
        [FieldOffset(0)]
        internal v4l2_frmsize_stepwise stepwise;
    }
    internal frame_size_union frame_size;

    internal fixed uint reserved[2];
}

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct v4l2_frmsize_discrete
{
    internal uint width;
    internal uint height;
}

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct v4l2_frmsize_stepwise
{
    internal uint min_width;
    internal uint max_width;
    internal uint step_width;
    internal uint min_height;
    internal uint max_height;
    internal uint step_height;
};

internal enum v4l2_frmsizetypes : uint
{
    V4L2_FRMSIZE_TYPE_DISCRETE = 1,
    V4L2_FRMSIZE_TYPE_CONTINUOUS = 2,
    V4L2_FRMSIZE_TYPE_STEPWISE = 3,
}

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct v4l2_frmivalenum
{
    internal uint index;
    internal V4l2InputFormat pixel_format;
    internal uint width;
    internal uint height;
    internal v4l2_frmivaltypes type;

    [StructLayout(LayoutKind.Explicit)]
    internal struct frame_interval_union
    {
        [FieldOffset(0)]
        internal v4l2_fract discrete;
        [FieldOffset(0)]
        internal v4l2_frmival_stepwise stepwise;
    }
    internal frame_interval_union frame_interval;

    internal fixed uint reserved[2];
}

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct v4l2_frmival_stepwise
{
    internal v4l2_fract min;
    internal v4l2_fract max;
    internal v4l2_fract step;
}

internal enum v4l2_frmivaltypes : uint
{
    V4L2_FRMIVAL_TYPE_DISCRETE = 1,
    V4L2_FRMIVAL_TYPE_CONTINUOUS = 2,
    V4L2_FRMIVAL_TYPE_STEPWISE = 3,
}
