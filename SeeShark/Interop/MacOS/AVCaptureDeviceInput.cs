// Copyright (c) Speykious
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 2-Clause License. See LICENSE for details.

using System.Runtime.Versioning;

namespace SeeShark.Interop.MacOS;

[SupportedOSPlatform("Macos")]
internal struct AVCaptureDeviceInput : IAVCaptureInput
{
    private nint id;

    internal AVCaptureDeviceInput(nint id)
    {
        this.id = id;
    }

    public nint ID => id;

    private static OClass classPtr = ObjC.GetClass(nameof(AVCaptureDeviceInput));

    private static Selector sel_deviceInputWithDevice = ObjC.sel_registerName("deviceInputWithDevice:error:");

    internal static AVCaptureDeviceInput DeviceInputWithDevice(AVCaptureDevice device) =>
        new AVCaptureDeviceInput(ObjC.objc_msgSend_id(classPtr.ID, sel_deviceInputWithDevice, device.ID, nint.Zero));
}
