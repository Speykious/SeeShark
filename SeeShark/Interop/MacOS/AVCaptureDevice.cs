// Copyright (c) Speykious
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 2-Clause License. See LICENSE for details.

using System.Runtime.Versioning;

namespace SeeShark.Interop.MacOS;

[SupportedOSPlatform("Macos")]
internal struct AVCaptureDevice : INSObject
{
    private nint id;

    internal AVCaptureDevice(nint id)
    {
        this.id = id;
    }

    public nint ID => id;

    private static OClass classPtr = ObjC.GetClass(nameof(AVCaptureDevice));

    private static Selector sel_devices = ObjC.sel_registerName("devices");
    private static Selector sel_defaultDeviceWithMediaType = ObjC.sel_registerName("defaultDeviceWithMediaType:");
    private static Selector sel_deviceWithUniqueID = ObjC.sel_registerName("deviceWithUniqueID:");

    private static Selector sel_uniqueID = ObjC.sel_registerName("uniqueID");
    private static Selector sel_localizedName = ObjC.sel_registerName("localizedName");
    private static Selector sel_hasMediaType = ObjC.sel_registerName("hasMediaType:");
    private static Selector sel_authorizationStatusForMediaType = ObjC.sel_registerName("authorizationStatusForMediaType:");
    private static Selector sel_requestAccessForMediaType = ObjC.sel_registerName("requestAccessForMediaType:completionHandler:");

    internal static NSString AV_MEDIA_TYPE_VIDEO = DL.GetConstant<NSString>(ObjC.AVFoundationHandle, "AVMediaTypeVideo");

    // TODO: use AVCaptureDeviceDiscoverySession instead?
    // https://developer.apple.com/documentation/avfoundation/avcapturedevicediscoverysession?language=objc
    internal static NSArray Devices => new NSArray(ObjC.objc_msgSend_id(classPtr.ID, sel_devices));

    internal static AVCaptureDevice? DefaultDeviceWithMediaType(NSString mediaType)
    {
        nint deviceID = ObjC.objc_msgSend_id(classPtr.ID, sel_defaultDeviceWithMediaType, mediaType.ID);
        if (deviceID == 0)
            return null;

        return new AVCaptureDevice(deviceID);
    }

    internal static AVCaptureDevice? DeviceWithUniqueID(string deviceUniqueID)
    {
        nint deviceID = ObjC.objc_msgSend_id(classPtr.ID, sel_deviceWithUniqueID, NSString.FromUTF8String(deviceUniqueID).ID);
        if (deviceID == 0)
            return null;

        return new AVCaptureDevice(deviceID);
    }

    internal static AVAuthorizationStatus AuthorizationStatusForMediaType(NSString mediaType) =>
        (AVAuthorizationStatus)ObjC.objc_msgSend_int(classPtr.ID, sel_authorizationStatusForMediaType, mediaType.ID);

    internal readonly NSString UniqueID => new NSString(ObjC.objc_msgSend_id(id, sel_uniqueID));
    internal readonly NSString LocalizedName => new NSString(ObjC.objc_msgSend_id(id, sel_localizedName));

    internal bool HasMediaType(NSString mediaType) => ObjC.objc_msgSend_bool(id, sel_hasMediaType, mediaType.ID);
}

internal enum AVAuthorizationStatus : int
{
    NotDetermined = 0,
    Restricted = 1,
    Denied = 2,
    Authorized = 3,
}
