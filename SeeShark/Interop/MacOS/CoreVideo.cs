// Copyright (c) Speykious
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 2-Clause License. See LICENSE for details.

using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;

namespace SeeShark.Interop.MacOS;

[SupportedOSPlatform("Macos")]
internal static class CoreVideo
{
    private const string lib_corevideo = "/System/Library/Frameworks/CoreVideo.framework/CoreVideo";

    internal static nint CoreVideoHandle;

    static CoreVideo()
    {
        CoreVideoHandle = DL.dlopen(lib_corevideo, DL.RTLD_NOW);
    }

    [DllImport(lib_corevideo)]
    internal static extern NSDictionary CVBufferCopyAttachments(CVBufferRef buffer, CVAttachmentMode attachmentMode);

    [DllImport(lib_corevideo)]
    internal static extern nuint CVPixelBufferGetWidth(CVBufferRef pixelBuffer);
    [DllImport(lib_corevideo)]
    internal static extern nuint CVPixelBufferGetHeight(CVBufferRef pixelBuffer);
    [DllImport(lib_corevideo)]
    internal static extern FourCharCode CVPixelBufferGetPixelFormatType(CVBufferRef pixelBuffer);
    [DllImport(lib_corevideo)]
    internal static extern nuint CVPixelBufferGetDataSize(CVBufferRef pixelBuffer);
    [DllImport(lib_corevideo)]
    internal static extern nint CVPixelBufferGetBaseAddress(CVBufferRef pixelBuffer);
    [DllImport(lib_corevideo)]
    internal static extern bool CVPixelBufferIsPlanar(CVBufferRef pixelBuffer);
    [DllImport(lib_corevideo)]
    internal static extern int CVPixelBufferLockBaseAddress(CVBufferRef pixelBuffer, CVPixelBufferLockFlags lockFlags);
    [DllImport(lib_corevideo)]
    internal static extern int CVPixelBufferUnlockBaseAddress(CVBufferRef pixelBuffer, CVPixelBufferLockFlags unlockFlags);
}

[Flags]
internal enum CVPixelBufferLockFlags : uint
{
    None = 0,
    ReadOnly = 1,
}

internal struct FourCharCode
{
    private uint code;

    public override string ToString()
    {
        byte a = (byte)((code >> 0) & 0xff);
        byte b = (byte)((code >> 8) & 0xff);
        byte c = (byte)((code >> 16) & 0xff);
        byte d = (byte)((code >> 24) & 0xff);
        return Encoding.ASCII.GetString([a, b, c, d]);
    }
}

internal enum CVAttachmentMode : uint
{
    /// <summary>
    /// Indicates to not propagate the attachment.
    /// </summary>
    ShouldNotPropagate = 0,

    /// <summary>
    /// Indicates to copy the attachment.
    /// </summary>
    ShouldPropagate = 1,
}

[SupportedOSPlatform("Macos")]
internal struct CVBufferRef : INSObject
{
    private nint id;

    internal CVBufferRef(nint id)
    {
        this.id = id;
    }

    public nint ID => id;
}
