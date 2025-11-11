// Copyright (c) Speykious
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 2-Clause License. See LICENSE for details.

using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace SeeShark.Interop.MacOS;

[SupportedOSPlatform("Macos")]
internal static class CoreMedia
{
    private const string lib_coremedia = "/System/Library/Frameworks/CoreMedia.framework/CoreMedia";

    internal static nint CoreMediaHandle;

    static CoreMedia()
    {
        CoreMediaHandle = DL.dlopen(lib_coremedia, DL.RTLD_NOW);
    }

    [DllImport(lib_coremedia)]
    internal static extern CVBufferRef CMSampleBufferGetImageBuffer(CMSampleBufferRef sbuf);
}

[SupportedOSPlatform("Macos")]
internal struct CMSampleBufferRef : INSObject
{
    private nint id;

    internal CMSampleBufferRef(nint id)
    {
        this.id = id;
    }

    public nint ID => id;
}

// See /Applications/Xcode.app/Contents/Developer/Platforms/MacOSX.platform/Developer/SDKs/MacOSX.sdk/System/Library/Frameworks/CoreMedia.framework/Versions/Current/Headers/CMTime.h
[SupportedOSPlatform("Macos")]
[StructLayout(LayoutKind.Sequential)]
internal struct CMTime
{
    /// <summary>
    /// The value of the CMTime. value/timescale = seconds
    /// </summary>
    public long Value;
    /// <summary>
    /// The timescale of the CMTime. value/timescale = seconds.
    /// </summary>
    public int Timescale;
    /// <summary>
    /// The flags, eg. kCMTimeFlags_Valid, kCMTimeFlags_PositiveInfinity, etc.
    /// </summary>
    public CMTimeFlags Flags;
    /// <summary>
    /// Differentiates between equal timestamps that are actually different because of looping, multi-item sequencing, etc.
	/// Will be used during comparison: greater epochs happen after lesser ones.
	/// Additions/subtraction is only possible within a single epoch, however, since epoch length may be unknown/variable
    /// </summary>
    public long Epoch;
}

[SupportedOSPlatform("Macos")]
[Flags]
internal enum CMTimeFlags : uint
{
    VALID = 0x0,
    HAS_BEEN_ROUNDED = 0x1,
    POSITIVE_INFINITY = 0x2,
    NEGATIVE_INFINITY = 0x4,
    INDEFINITE = 0x8,
}
