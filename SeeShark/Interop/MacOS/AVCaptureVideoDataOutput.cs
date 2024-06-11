// Copyright (c) Speykious
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 2-Clause License. See LICENSE for details.

using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace SeeShark.Interop.MacOS;

[SupportedOSPlatform("Macos")]
internal struct AVCaptureVideoDataOutput : IAVCaptureOutput
{
    private nint id;

    internal AVCaptureVideoDataOutput(nint id)
    {
        this.id = id;
    }

    public AVCaptureVideoDataOutput()
    {
        id = ObjC.objc_msgSend_id(classPtr.ID, ObjC.Sel_alloc);
        ObjC.objc_msgSend(id, ObjC.Sel_init);
    }

    public nint ID => id;

    private static OClass classPtr = ObjC.GetClass(nameof(AVCaptureVideoDataOutput));

    private static Selector sel_availableVideoCodecTypes = ObjC.sel_registerName("availableVideoCodecTypes");
    private static Selector sel_sampleBufferDelegate = ObjC.sel_registerName("sampleBufferDelegate");
    private static Selector sel_setSampleBufferDelegate = ObjC.sel_registerName("setSampleBufferDelegate:queue:");

    private static Selector sel_captureOutputSambleBuffer = ObjC.sel_registerName("captureOutput:didOutputSampleBuffer:fromConnection:");
    private static Selector sel_captureDiscardedSampleBuffer = ObjC.sel_registerName("captureOutput:didDropSampleBuffer:fromConnection:");

    internal static NSString AV_VIDEO_CODEC_TYPE_H264 = DL.GetConstant<NSString>(ObjC.AVFoundationHandle, "AVVideoCodecTypeH264");
    internal static NSString AV_VIDEO_CODEC_TYPE_HEVC = DL.GetConstant<NSString>(ObjC.AVFoundationHandle, "AVVideoCodecTypeHEVC");
    internal static NSString AV_VIDEO_CODEC_TYPE_HEVC_WITH_ALPHA = DL.GetConstant<NSString>(ObjC.AVFoundationHandle, "AVVideoCodecTypeHEVCWithAlpha");
    internal static NSString AV_VIDEO_CODEC_TYPE_JPEG = DL.GetConstant<NSString>(ObjC.AVFoundationHandle, "AVVideoCodecTypeJPEG");
    internal static NSString AV_VIDEO_CODEC_TYPE_APPLE_PRO_RES422 = DL.GetConstant<NSString>(ObjC.AVFoundationHandle, "AVVideoCodecTypeAppleProRes422");
    internal static NSString AV_VIDEO_CODEC_TYPE_APPLE_PRO_RES422LT = DL.GetConstant<NSString>(ObjC.AVFoundationHandle, "AVVideoCodecTypeAppleProRes422LT");
    internal static NSString AV_VIDEO_CODEC_TYPE_APPLE_PRO_RES422HQ = DL.GetConstant<NSString>(ObjC.AVFoundationHandle, "AVVideoCodecTypeAppleProRes422HQ");
    internal static NSString AV_VIDEO_CODEC_TYPE_APPLE_PRO_RES422_PROXY = DL.GetConstant<NSString>(ObjC.AVFoundationHandle, "AVVideoCodecTypeAppleProRes422Proxy");
    internal static NSString AV_VIDEO_CODEC_TYPE_APPLE_PRO_RES4444 = DL.GetConstant<NSString>(ObjC.AVFoundationHandle, "AVVideoCodecTypeAppleProRes4444");

    internal readonly NSArray AvailableVideoCodecTypes => new NSArray(ObjC.objc_msgSend_id(id, sel_availableVideoCodecTypes));

    private nint getSampleBufferDelegate() => ObjC.objc_msgSend_id(id, sel_sampleBufferDelegate);

    internal void SetSampleBufferDelegate(IAVCaptureVideoDataOutputSampleBufferDelegate sampleBufferDelegate, nint sampleBufferCallbackQueue)
    {
        nint prevDelegateInstance = getSampleBufferDelegate();
        if (prevDelegateInstance != 0)
        {
            GCHandle delegateHandle = getManagedSampleBufferDelegate(prevDelegateInstance);
            if (delegateHandle.IsAllocated)
                delegateHandle.Free();
        }

        nint cvdoDelegateInstance = ObjC.class_createInstance(cvdoDelegateClass, 0);
        setManagedSampleBufferDelegate(cvdoDelegateInstance, sampleBufferDelegate);

        ObjC.objc_msgSend(id, sel_setSampleBufferDelegate, cvdoDelegateInstance, sampleBufferCallbackQueue);
    }

    private static readonly OClass cvdoDelegateClass = createCVDODelegateClass();
    private static readonly nint ivar_CVDO_MSBD = ObjC.class_getInstanceVariable(cvdoDelegateClass, "managedSampleBufferDelegate");

    private static OClass createCVDODelegateClass()
    {
        OClass resultClass = ObjC.objc_allocateClassPair(ObjC.NSObjectClass, "SeeSharkCVDODelegate", 0);

        unsafe
        {
            ObjC.class_addIvar(resultClass, "managedSampleBufferDelegate", (nuint)sizeof(GCHandle), (byte)BitOperations.Log2((nuint)sizeof(nint)), "i");

            ObjC.class_addMethod(resultClass, sel_captureOutputSambleBuffer, &captureOutput_didOutputSampleBuffer_fromConnection, "v@:@@@");
            ObjC.class_addMethod(resultClass, sel_captureDiscardedSampleBuffer, &captureOutput_didDropSampleBuffer_fromConnection, "v@:@@@");

            nint cvdosbdProtocol = ObjC.objc_getProtocol("AVCaptureVideoDataOutputSampleBufferDelegate");
            ObjC.class_addProtocol(resultClass, cvdosbdProtocol);
        }

        ObjC.objc_registerClassPair(resultClass);
        return resultClass;
    }

    private static GCHandle getManagedSampleBufferDelegate(nint cvdoDelegateInstance)
    {
        unsafe
        {
            GCHandle* ivarDelegateHandlePtr = (GCHandle*)(cvdoDelegateInstance + ObjC.ivar_getOffset(ivar_CVDO_MSBD));
            return *ivarDelegateHandlePtr;
        }
    }

    private static void setManagedSampleBufferDelegate(nint cvdoDelegateInstance, IAVCaptureVideoDataOutputSampleBufferDelegate sampleBufferDelegate)
    {
        unsafe
        {
            GCHandle* ivarDelegateHandlePtr = (GCHandle*)(cvdoDelegateInstance + ObjC.ivar_getOffset(ivar_CVDO_MSBD));
            *ivarDelegateHandlePtr = GCHandle.Alloc(sampleBufferDelegate, GCHandleType.Pinned);
        }
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static unsafe void captureOutput_didOutputSampleBuffer_fromConnection(nint self, Selector _cmd, nint captureOutput, nint sampleBuffer, nint connection)
    {
        GCHandle delegateHandle = getManagedSampleBufferDelegate(self);
        if (delegateHandle.Target is IAVCaptureVideoDataOutputSampleBufferDelegate managedDelegate)
            managedDelegate.CaptureOutputSambleBuffer(new AVCaptureVideoDataOutput(captureOutput), sampleBuffer, connection);
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static unsafe void captureOutput_didDropSampleBuffer_fromConnection(nint self, Selector _cmd, nint cvdoDelegateOutput, nint sampleBuffer, nint connection)
    {
        GCHandle delegateHandle = getManagedSampleBufferDelegate(self);
        if (delegateHandle.Target is IAVCaptureVideoDataOutputSampleBufferDelegate managedDelegate)
            managedDelegate.CaptureDiscardedSampleBuffer(new AVCaptureVideoDataOutput(cvdoDelegateOutput), sampleBuffer, connection);
    }
}

internal interface IAVCaptureVideoDataOutputSampleBufferDelegate
{
    /// <summary>
    /// Notifies the delegate that a new video frame was written.
    /// </summary>
    void CaptureOutputSambleBuffer(IAVCaptureOutput output, nint sampleBuffer, nint connection);

    /// <summary>
    /// Notifies the delegate that a video frame was discarded.
    /// </summary>
    void CaptureDiscardedSampleBuffer(IAVCaptureOutput output, nint sampleBuffer, nint connection);
}
