// Copyright (c) Speykious
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 2-Clause License. See LICENSE for details.

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SeeShark.Interop.MacOS;

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
    private static Selector sel_setSampleBufferDelegate = ObjC.sel_registerName("setSampleBufferDelegate:queue:");

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

    internal void SetSampleBufferDelegate(IAVCaptureVideoDataOutputSampleBufferDelegate sampleBufferDelegate, nint sampleBufferCallbackQueue)
    {
        nint cvdoDelegateInstance = ObjC.class_createInstance(cvdoDelegateClass, 0);

        managedDelegateDict.Add(id, new CVDOData
        {
            CVDOID = ID,
            SampleBufferCallbackQueue = sampleBufferCallbackQueue,
            CVDODelegate = sampleBufferDelegate,
            CVDODelegateInstance = cvdoDelegateInstance,
        });

        ObjC.objc_msgSend(id, sel_setSampleBufferDelegate, cvdoDelegateInstance, sampleBufferCallbackQueue);
    }

    private static OClass cvdoDelegateClass = createCVDODelegateClass();

    private static OClass createCVDODelegateClass()
    {
        OClass resultClass = ObjC.objc_allocateClassPair(ObjC.NSObjectClass, "SeeSharkCVDODelegate", 0);

        unsafe
        {
            Selector sel_captureOutputSambleBuffer = ObjC.sel_registerName("captureOutput:didOutputSampleBuffer:fromConnection:");
            bool successM1 = ObjC.class_addMethod(resultClass, sel_captureOutputSambleBuffer, &captureOutput_didOutputSampleBuffer_fromConnection, "v@:@@@");

            Selector sel_captureDiscardedSampleBuffer = ObjC.sel_registerName("captureOutput:didDropSampleBuffer:fromConnection:");
            bool successM2 = ObjC.class_addMethod(resultClass, sel_captureDiscardedSampleBuffer, &captureOutput_didDropSampleBuffer_fromConnection, "v@:@@@");

            nint cvdosbdProtocol = ObjC.objc_getProtocol("AVCaptureVideoDataOutputSampleBufferDelegate");
            bool successP = ObjC.class_addProtocol(resultClass, cvdosbdProtocol);

            Console.WriteLine($"CVDO method/protocol creation success: {successM1} {successM2} {successP}");
        }

        ObjC.objc_registerClassPair(resultClass);
        return resultClass;
    }

    private static Dictionary<nint, CVDOData> managedDelegateDict = [];

    private class CVDOData : IDisposable
    {
        internal required nint CVDOID;
        internal required nint SampleBufferCallbackQueue;
        internal required IAVCaptureVideoDataOutputSampleBufferDelegate CVDODelegate;
        internal required nint CVDODelegateInstance;

        ~CVDOData() => dispose();

        public void Dispose()
        {
            dispose();
            GC.SuppressFinalize(this);
        }

        private void dispose()
        {
            if (SampleBufferCallbackQueue != 0)
            {
                ObjC.dispatch_release(SampleBufferCallbackQueue);
                ObjC.class_destructInstance(CVDODelegateInstance);
                SampleBufferCallbackQueue = 0;
            }
        }
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static unsafe void captureOutput_didOutputSampleBuffer_fromConnection(nint self, Selector _cmd, nint output, nint sampleBuffer, nint connection)
    {
        if (managedDelegateDict[output].CVDODelegate is IAVCaptureVideoDataOutputSampleBufferDelegate managedDelegate)
            managedDelegate.CaptureOutputSambleBuffer(new AVCaptureVideoDataOutput(output), sampleBuffer, connection);
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static unsafe void captureOutput_didDropSampleBuffer_fromConnection(nint self, Selector _cmd, nint output, nint sampleBuffer, nint connection)
    {
        if (managedDelegateDict[output].CVDODelegate is IAVCaptureVideoDataOutputSampleBufferDelegate managedDelegate)
            managedDelegate.CaptureDiscardedSampleBuffer(new AVCaptureVideoDataOutput(output), sampleBuffer, connection);
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
