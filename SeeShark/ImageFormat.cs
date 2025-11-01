// Copyright (c) Speykious
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 2-Clause License. See LICENSE for details.

using System;

namespace SeeShark;

/// <summary>
/// Image formats supported by SeeShark.
/// They are represented with a FourCC code.
/// </summary>
public enum ImageFormat : uint
{
    // RGB
    BGR_24 = 0x33524742, // 'BGR3'
    RGB_24 = 0x33424752, // 'RGB3'
    ARGB_32 = 0x34324142, // 'BA24'
    BGRA_32 = 0x34324152, // 'RA24'
    ABGR_32 = 0x34325241, // 'AR24'
    RGBA_32 = 0x34324241, // 'AB24'

    // YUV
    YUYV = 0x56595559, // 'YUYV'
    YVYU = 0x55595659, // 'YVYU'
    UYVY = 0x59565955, // 'UYVY'
    Y41P = 0x50313459, // 'Y41P'
    YUV_444 = 0x34343459, // 'Y444'

    // Compressed
    MJPG = 0x47504a4d, // 'MJPG'
}

public readonly struct ImageFormatInfo
{
    /// <summary>
    /// The <see href="https://wiki.multimedia.cx/index.php/FourCC"/>FourCC</see> code of this image format.
    /// </summary>
    public readonly uint FourCC = 0x00000000;

    public readonly string Name;
    public readonly string? FFmpegInputFormat;
    public readonly bool IsRaw;
    public readonly string Description;

    internal ImageFormatInfo(uint fourCC, string name, string? ffmpegInputFormat, bool isRaw, string description)
    {
        FourCC = fourCC;
        Name = name;
        FFmpegInputFormat = ffmpegInputFormat;
        IsRaw = isRaw;
        Description = description;
    }

    public static ImageFormatInfo FromImageFormat(ImageFormat imageFormat)
    {
        return imageFormat switch
        {
#pragma warning disable format
            ImageFormat.BGR_24  => new ImageFormatInfo((uint)imageFormat, "BGR",    "bgr",     isRaw: true,  "BGR 24-bit"),
            ImageFormat.RGB_24  => new ImageFormatInfo((uint)imageFormat, "RGB",    "rgb",     isRaw: true,  "RGB 24-bit"),
            ImageFormat.ARGB_32 => new ImageFormatInfo((uint)imageFormat, "ARGB",   "argb",    isRaw: true,  "ARGB 32-bit"),
            ImageFormat.BGRA_32 => new ImageFormatInfo((uint)imageFormat, "BGRA",   "bgra",    isRaw: true,  "BGRA 32-bit"),
            ImageFormat.ABGR_32 => new ImageFormatInfo((uint)imageFormat, "ABGR",   "abgr",    isRaw: true,  "ABGR 32-bit"),
            ImageFormat.RGBA_32 => new ImageFormatInfo((uint)imageFormat, "RGBA",   "rgba",    isRaw: true,  "RGBA 32-bit"),

            ImageFormat.YUYV    => new ImageFormatInfo((uint)imageFormat, "YUYV",   "yuyv422", isRaw: true,  "Component Y'CbCr 8-bit 4:2:2, ordered Y'0 Cb Y'1 Cr"),
            ImageFormat.YVYU    => new ImageFormatInfo((uint)imageFormat, "YVYU",   "yvyu422", isRaw: true,  "Component Y'CbCr 8-bit 4:2:2, ordered Y'0 Cr Y'1 Cb"),
            ImageFormat.UYVY    => new ImageFormatInfo((uint)imageFormat, "UYVY",   "uyvy422", isRaw: true,  "Component Y'CbCr 8-bit 4:2:2, ordered Cb Y'0 Cr Y'1"),
            ImageFormat.Y41P    => new ImageFormatInfo((uint)imageFormat, "Y41P",   "yuv411p", isRaw: true,  "YUV 4:1:1"),
            ImageFormat.YUV_444 => new ImageFormatInfo((uint)imageFormat, "YUV444", "yuv444p", isRaw: true,  "YUV 4:4:4"),

            ImageFormat.MJPG    => new ImageFormatInfo((uint)imageFormat, "MJPG",   "mjpeg",   isRaw: false, "MJPEG compressed"),
#pragma warning restore format
            _ => throw new NotImplementedException(),
        };

    }
}
