// Copyright (c) Speykious
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 2-Clause License. See LICENSE for details.

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
