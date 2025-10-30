// Copyright (c) Speykious
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 2-Clause License. See LICENSE for details.

using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

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

    internal static NSString AVVideoScalingModeKey => NSString.FromUTF8String("AVVideoScalingModeKey");
    internal static NSString AVVideoScalingModeResizeAspect => NSString.FromUTF8String("AVVideoScalingModeResizeAspect");

    internal static NSString KCvPixelBufferWidthKey => DL.GetConstant<NSString>(CoreVideoHandle, "kCVPixelBufferWidthKey");
    internal static NSString KCvPixelBufferHeightKey => DL.GetConstant<NSString>(CoreVideoHandle, "kCVPixelBufferHeightKey");
    internal static NSString KCvPixelBufferPixelFormatTypeKey => DL.GetConstant<NSString>(CoreVideoHandle, "kCVPixelBufferPixelFormatTypeKey");

    [DllImport(lib_corevideo)]
    internal static extern NSDictionary CVBufferCopyAttachments(CVBufferRef buffer, CVAttachmentMode attachmentMode);

    [DllImport(lib_corevideo)]
    internal static extern nuint CVPixelBufferGetWidth(CVBufferRef pixelBuffer);
    [DllImport(lib_corevideo)]
    internal static extern nuint CVPixelBufferGetHeight(CVBufferRef pixelBuffer);
    [DllImport(lib_corevideo)]
    internal static extern nuint CVPixelBufferGetBytesPerRow(CVBufferRef pixelBuffer);
    [DllImport(lib_corevideo)]
    internal static extern CVPixelFormatType CVPixelBufferGetPixelFormatType(CVBufferRef pixelBuffer);
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
