// Copyright (c) Speykious
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 2-Clause License. See LICENSE for details.

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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

    public AVCaptureDevice()
    {
        id = ObjC.objc_msgSend_id(classPtr.ID, ObjC.Sel_alloc);
        ObjC.objc_msgSend(id, ObjC.Sel_init);
    }

    public nint ID => id;

    private static OClass classPtr = ObjC.GetClass(nameof(AVCaptureDevice));

    private static Selector sel_devices = ObjC.sel_registerName("devices");
    private static Selector sel_defaultDeviceWithMediaType = ObjC.sel_registerName("defaultDeviceWithMediaType:");
    private static Selector sel_deviceWithUniqueID = ObjC.sel_registerName("deviceWithUniqueID:");
    private static Selector sel_authorizationStatusForMediaType = ObjC.sel_registerName("authorizationStatusForMediaType:");
    private static Selector sel_requestAccessForMediaType = ObjC.sel_registerName("requestAccessForMediaType:completionHandler:");

    private static Selector sel_uniqueID = ObjC.sel_registerName("uniqueID");
    private static Selector sel_localizedName = ObjC.sel_registerName("localizedName");
    private static Selector sel_hasMediaType = ObjC.sel_registerName("hasMediaType:");
    private static Selector sel_formats = ObjC.sel_registerName("formats");
    private static Selector sel_activeFormat = ObjC.sel_registerName("activeFormat");
    private static Selector sel_setActiveFormat = ObjC.sel_registerName("setActiveFormat:");
    private static Selector sel_lockForConfiguration = ObjC.sel_registerName("lockForConfiguration:");
    private static Selector sel_unlockForConfiguration = ObjC.sel_registerName("unlockForConfiguration");

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

    public static event EventHandler<bool>? VideoAccessPermissionEventHandler;

    private static unsafe ObjC.BlockDescriptor* rvaDescriptor;
    private static unsafe ObjC.BlockLiteral* rvaBlock;

    internal static void RequestVideoAccess()
    {
        unsafe
        {
            if (rvaDescriptor == null)
            {
                rvaDescriptor = (ObjC.BlockDescriptor*)NativeMemory.Alloc((nuint)sizeof(ObjC.BlockDescriptor));
                rvaDescriptor->Reserved = 0;
                rvaDescriptor->Size = (ulong)sizeof(ObjC.BlockLiteral);
                rvaDescriptor->CopyHelper = 0;
                rvaDescriptor->DisposeHelper = 0;
                rvaDescriptor->Signature = Marshal.StringToHGlobalAnsi("vB");
            }

            if (rvaBlock == null)
            {
                rvaBlock = (ObjC.BlockLiteral*)NativeMemory.Alloc((nuint)sizeof(ObjC.BlockLiteral));
                rvaBlock->Isa = ObjC.NSConcreteGlobalBlock;
                rvaBlock->Flags = ObjC.BlockFlags.HAS_SIGNATURE;
                rvaBlock->Reserved = 0;
                rvaBlock->Invoke = (nint)(delegate* unmanaged[Cdecl]<nint, byte, void>)&requestAccessForMediaType_completionHandler;
                rvaBlock->Descriptor = (nint)rvaDescriptor;
            }

            ObjC.objc_msgSend(classPtr.ID, sel_requestAccessForMediaType, AV_MEDIA_TYPE_VIDEO.ID, (nint)rvaBlock);
        }
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static unsafe void requestAccessForMediaType_completionHandler(nint _blockPtr, byte permissionGranted)
    {
        VideoAccessPermissionEventHandler?.Invoke(null, permissionGranted > 0);
    }

    internal readonly NSString UniqueID => new NSString(ObjC.objc_msgSend_id(id, sel_uniqueID));
    internal readonly NSString LocalizedName => new NSString(ObjC.objc_msgSend_id(id, sel_localizedName));
    internal readonly NSArray Formats => new NSArray(ObjC.objc_msgSend_id(id, sel_formats));

    internal AVCaptureDeviceFormat ActiveFormat
    {
        get => new AVCaptureDeviceFormat(ObjC.objc_msgSend_id(id, sel_activeFormat));
        set => ObjC.objc_msgSend(id, sel_setActiveFormat, value.ID);
    }

    internal bool HasMediaType(NSString mediaType) => ObjC.objc_msgSend_bool(id, sel_hasMediaType, mediaType.ID);

    internal bool LockForConfiguration() => ObjC.objc_msgSend_bool(id, sel_lockForConfiguration, 0);
    internal void UnlockForConfiguration() => ObjC.objc_msgSend(id, sel_unlockForConfiguration);
}

internal enum AVAuthorizationStatus : int
{
    NotDetermined = 0,
    Restricted = 1,
    Denied = 2,
    Authorized = 3,
}
