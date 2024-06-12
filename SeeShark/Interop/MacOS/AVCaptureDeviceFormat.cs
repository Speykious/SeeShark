// Copyright (c) Speykious
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 2-Clause License. See LICENSE for details.

using System.Runtime.Versioning;

namespace SeeShark.Interop.MacOS;

[SupportedOSPlatform("Macos")]
internal struct AVCaptureDeviceFormat : INSObject
{
    private nint id;

    internal AVCaptureDeviceFormat(nint id)
    {
        this.id = id;
    }

    public nint ID => id;

    private static Selector sel_mediaType = ObjC.sel_registerName("mediaType");
    private static Selector sel_videoSupportedFrameRateRanges = ObjC.sel_registerName("videoSupportedFrameRateRanges");
    private static Selector sel_supportedMaxPhotoDimensions = ObjC.sel_registerName("supportedMaxPhotoDimensions");

    internal NSString MediaType => new NSString(ObjC.objc_msgSend_id(id, sel_mediaType));
    internal NSArray VideoSupportedFrameRateRanges => new NSArray(ObjC.objc_msgSend_id(id, sel_videoSupportedFrameRateRanges));
    internal NSArray SupportedMaxPhotoDimensions => new NSArray(ObjC.objc_msgSend_id(id, sel_supportedMaxPhotoDimensions));
}

[SupportedOSPlatform("Macos")]
internal struct AVFrameRateRange : INSObject
{
    private nint id;

    internal AVFrameRateRange(nint id)
    {
        this.id = id;
    }

    public nint ID => id;

    private static Selector sel_maxFrameRate = ObjC.sel_registerName("maxFrameRate");
    private static Selector sel_minFrameRate = ObjC.sel_registerName("minFrameRate");

    internal readonly double MaxFrameRate => ObjC.objc_msgSend_double(id, sel_maxFrameRate);
    internal readonly double MinFrameRate => ObjC.objc_msgSend_double(id, sel_minFrameRate);
}

[SupportedOSPlatform("Macos")]
internal struct CMVideoDimensions
{
    public int Width;
    public int Height;
}
