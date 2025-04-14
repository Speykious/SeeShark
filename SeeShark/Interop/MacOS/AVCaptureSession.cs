// Copyright (c) Speykious
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 2-Clause License. See LICENSE for details.

using System.Runtime.Versioning;

namespace SeeShark.Interop.MacOS;

[SupportedOSPlatform("Macos")]
internal struct AVCaptureSession : INSObject
{
    private nint id;

    internal AVCaptureSession(nint id)
    {
        this.id = id;
    }

    public AVCaptureSession()
    {
        id = ObjC.objc_msgSend_id(classPtr.ID, ObjC.Sel_alloc);
        ObjC.objc_msgSend(id, ObjC.Sel_init);
    }

    public nint ID => id;

    private static OClass classPtr = ObjC.GetClass(nameof(AVCaptureSession));

    private static Selector sel_sessionPreset = ObjC.sel_registerName("sessionPreset");
    private static Selector sel_setSessionPreset = ObjC.sel_registerName("setSessionPreset");
    private static Selector sel_canSetSessionPreset = ObjC.sel_registerName("canSetSessionPreset:");
    private static Selector sel_canAddInput = ObjC.sel_registerName("canAddInput:");
    private static Selector sel_addInput = ObjC.sel_registerName("addInput:");
    private static Selector sel_removeInput = ObjC.sel_registerName("removeInput:");
    private static Selector sel_canAddOutput = ObjC.sel_registerName("canAddOutput:");
    private static Selector sel_addOutput = ObjC.sel_registerName("addOutput:");
    private static Selector sel_removeOutput = ObjC.sel_registerName("removeOutput:");
    private static Selector sel_startRunning = ObjC.sel_registerName("startRunning");
    private static Selector sel_stopRunning = ObjC.sel_registerName("stopRunning");

    internal static NSString AV_CAPTURE_SESSION_PRESET_HIGH = DL.GetConstant<NSString>(ObjC.AVFoundationHandle, "AVCaptureSessionPresetHigh");
    internal static NSString AV_CAPTURE_SESSION_PRESET_MEDIUM = DL.GetConstant<NSString>(ObjC.AVFoundationHandle, "AVCaptureSessionPresetMedium");
    internal static NSString AV_CAPTURE_SESSION_PRESET_LOW = DL.GetConstant<NSString>(ObjC.AVFoundationHandle, "AVCaptureSessionPresetLow");
    internal static NSString AV_CAPTURE_SESSION_PRESET_PHOTO = DL.GetConstant<NSString>(ObjC.AVFoundationHandle, "AVCaptureSessionPresetPhoto");
    internal static NSString AV_CAPTURE_SESSION_PRESET_INPUT_PRIORITY = DL.GetConstant<NSString>(ObjC.AVFoundationHandle, "AVCaptureSessionPresetInputPriority");
    internal static NSString AV_CAPTURE_SESSION_PRESET960X540 = DL.GetConstant<NSString>(ObjC.AVFoundationHandle, "AVCaptureSessionPreset960x540");
    internal static NSString AV_CAPTURE_SESSION_PRESET1280X720 = DL.GetConstant<NSString>(ObjC.AVFoundationHandle, "AVCaptureSessionPreset1280x720");
    internal static NSString AV_CAPTURE_SESSION_PRESET1920X1080 = DL.GetConstant<NSString>(ObjC.AVFoundationHandle, "AVCaptureSessionPreset1920x1080");
    internal static NSString AV_CAPTURE_SESSION_PRESET3840X2160 = DL.GetConstant<NSString>(ObjC.AVFoundationHandle, "AVCaptureSessionPreset3840x2160");
    internal static NSString AV_CAPTURE_SESSION_PRESET320X240 = DL.GetConstant<NSString>(ObjC.AVFoundationHandle, "AVCaptureSessionPreset320x240");
    internal static NSString AV_CAPTURE_SESSION_PRESET640X480 = DL.GetConstant<NSString>(ObjC.AVFoundationHandle, "AVCaptureSessionPreset640x480");
    internal static NSString AV_CAPTURE_SESSION_PRESETI_FRAME960X540 = DL.GetConstant<NSString>(ObjC.AVFoundationHandle, "AVCaptureSessionPresetiFrame960x540");
    internal static NSString AV_CAPTURE_SESSION_PRESETI_FRAME1280X720 = DL.GetConstant<NSString>(ObjC.AVFoundationHandle, "AVCaptureSessionPresetiFrame1280x720");
    internal static NSString AV_CAPTURE_SESSION_PRESET352X288 = DL.GetConstant<NSString>(ObjC.AVFoundationHandle, "AVCaptureSessionPreset352x288");

    internal NSString SessionPreset
    {
        get => new NSString(ObjC.objc_msgSend_id(ID, sel_sessionPreset));
        set => ObjC.objc_msgSend(ID, sel_setSessionPreset, value.ID);
    }

    internal bool CanSetSessionPreset(NSString preset) => ObjC.objc_msgSend_bool(id, sel_canSetSessionPreset, preset.ID);
    internal bool CanAddInput(IAVCaptureInput input) => ObjC.objc_msgSend_bool(id, sel_canAddInput, input.ID);
    internal void AddInput(IAVCaptureInput input) => ObjC.objc_msgSend(id, sel_addInput, input.ID);
    internal void RemoveInput(IAVCaptureInput input) => ObjC.objc_msgSend(id, sel_removeInput, input.ID);
    internal bool CanAddOutput(IAVCaptureOutput output) => ObjC.objc_msgSend_bool(id, sel_canAddOutput, output.ID);
    internal void AddOutput(IAVCaptureOutput output) => ObjC.objc_msgSend(id, sel_addOutput, output.ID);
    internal void RemoveOutput(IAVCaptureOutput output) => ObjC.objc_msgSend(id, sel_removeOutput, output.ID);

    internal void StartRunning() => ObjC.objc_msgSend(id, sel_startRunning);
    internal void StopRunning() => ObjC.objc_msgSend(id, sel_stopRunning);
}

internal interface IAVCaptureInput : INSObject { }

internal interface IAVCaptureOutput : INSObject { }
