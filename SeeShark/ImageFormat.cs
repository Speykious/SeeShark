// Copyright (c) Speykious
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 2-Clause License. See LICENSE for details.

using System;
using SeeShark.Interop.Libc;
using SeeShark.Interop.MacOS;

namespace SeeShark;

public struct ImageFormat
{
    /// <summary>
    /// The <see href="https://wiki.multimedia.cx/index.php/FourCC"/>FourCC</see> code of this image format.
    /// </summary>
    public readonly uint FourCC = 0x00000000;

    internal ImageFormat(uint fourCC)
    {
        FourCC = fourCC;
    }

    public static uint GetFourCC(char a, char b, char c, char d) => a | ((uint)b << 8) | ((uint)c << 16) | ((uint)d << 24);
    public static uint GetFourCC_BE(char a, char b, char c, char d) => GetFourCC(a, b, c, d) | ((uint)1 << 31);

    public static ImageFormat FromFourCC(char a, char b, char c, char d)
    {
        return new ImageFormat(GetFourCC(a, b, c, d));
    }

    public static ImageFormat FromFourCC_BE(char a, char b, char c, char d)
    {
        return new ImageFormat(GetFourCC_BE(a, b, c, d));
    }

    public override string ToString()
    {
        if (OperatingSystem.IsLinux())
            return $"{(V4l2InputFormat)FourCC}";
        else if (OperatingSystem.IsMacOS())
            return $"{(CVPixelFormatType)FourCC}";
        else
            throw new NotImplementedException();
    }
}
