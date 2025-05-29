// Copyright (c) Speykious
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 2-Clause License. See LICENSE for details.

using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using SeeShark.Interop.MacOS;

namespace SeeShark.Camera;

[SupportedOSPlatform("MacOS")]
public sealed class MacOSCameraDevice : CameraDevice
{
    private readonly AVCaptureDevice captureDevice;

    private FrameQueue frameDataQueue;
    private GCHandle frameDataQueueHandle;
    private AVCaptureSession session;
    private AVCaptureVideoDataOutputSampleBufferDelegate sampleBufferDelegate;

    internal MacOSCameraDevice(AVCaptureDevice device, VideoFormatOptions options)
    {
        // TODO: go through device formats and choose most suited one according to options
        AVCaptureDeviceFormat deviceFormat = new AVCaptureDeviceFormat(device.Formats.ObjectAtIndex(1));
        device.LockForConfiguration();
        device.ActiveFormat = deviceFormat;
        device.UnlockForConfiguration();

        captureDevice = device;
        frameDataQueue = new FrameQueue(16);
        frameDataQueueHandle = GCHandle.Alloc(frameDataQueue, GCHandleType.Pinned);

        session = new AVCaptureSession();

        Console.Error.WriteLine($"Session preset (before): {session.SessionPreset.ToUTF8String()}");
        session.BeginConfiguration();
        {
            Console.Error.WriteLine($"Can set PRESET_HIGH:           {session.CanSetSessionPreset(AVCaptureSession.AV_CAPTURE_SESSION_PRESET_HIGH)}");
            Console.Error.WriteLine($"Can set PRESET_MEDIUM:         {session.CanSetSessionPreset(AVCaptureSession.AV_CAPTURE_SESSION_PRESET_MEDIUM)}");
            Console.Error.WriteLine($"Can set PRESET_LOW:            {session.CanSetSessionPreset(AVCaptureSession.AV_CAPTURE_SESSION_PRESET_LOW)}");
            Console.Error.WriteLine($"Can set PRESET_PHOTO:          {session.CanSetSessionPreset(AVCaptureSession.AV_CAPTURE_SESSION_PRESET_PHOTO)}");
            Console.Error.WriteLine($"Can set PRESET_INPUT_PRIORITY: {session.CanSetSessionPreset(AVCaptureSession.AV_CAPTURE_SESSION_PRESET_INPUT_PRIORITY)}");
            Console.Error.WriteLine($"Can set PRESET960X540:         {session.CanSetSessionPreset(AVCaptureSession.AV_CAPTURE_SESSION_PRESET960X540)}");
            Console.Error.WriteLine($"Can set PRESET1280X720:        {session.CanSetSessionPreset(AVCaptureSession.AV_CAPTURE_SESSION_PRESET1280X720)}");
            Console.Error.WriteLine($"Can set PRESET1920X1080:       {session.CanSetSessionPreset(AVCaptureSession.AV_CAPTURE_SESSION_PRESET1920X1080)}");
            Console.Error.WriteLine($"Can set PRESET3840X2160:       {session.CanSetSessionPreset(AVCaptureSession.AV_CAPTURE_SESSION_PRESET3840X2160)}");
            Console.Error.WriteLine($"Can set PRESET320X240:         {session.CanSetSessionPreset(AVCaptureSession.AV_CAPTURE_SESSION_PRESET320X240)}");
            Console.Error.WriteLine($"Can set PRESET640X480:         {session.CanSetSessionPreset(AVCaptureSession.AV_CAPTURE_SESSION_PRESET640X480)}");
            Console.Error.WriteLine($"Can set PRESETI_FRAME960X540:  {session.CanSetSessionPreset(AVCaptureSession.AV_CAPTURE_SESSION_PRESETI_FRAME960X540)}");
            Console.Error.WriteLine($"Can set PRESETI_FRAME1280X720: {session.CanSetSessionPreset(AVCaptureSession.AV_CAPTURE_SESSION_PRESETI_FRAME1280X720)}");
        }
        // session.SessionPreset = AVCaptureSession.AV_CAPTURE_SESSION_PRESET_MEDIUM;
        session.CommitConfiguration();
        Console.Error.WriteLine($"Session preset (after): {session.SessionPreset.ToUTF8String()}");

        AVAuthorizationStatus auth = AVCaptureDevice.AuthorizationStatusForMediaType(AVCaptureDevice.AV_MEDIA_TYPE_VIDEO);
        if (auth == AVAuthorizationStatus.Denied)
            throw new UnauthorizedAccessException("Access to camera is denied");

        AVCaptureDeviceInput deviceInput = AVCaptureDeviceInput.DeviceInputWithDevice(captureDevice);

        if (session.CanAddInput(deviceInput))
            session.AddInput(deviceInput);

        AVCaptureVideoDataOutput deviceOutput = new();

        Console.Error.WriteLine("Pixel format types available:");
        foreach (nint obj in deviceOutput.AvailableVideoCVPixelFormatTypes.ToArray())
        {
            NSNumber number = new(obj);
            Console.Error.WriteLine($"  {(CVPixelFormatType)number.UIntValue()}");
        }

        Console.Error.WriteLine("[SeeShark] Warning: framerate settings are currently ignored on MacOS");

        sampleBufferDelegate = new()
        {
            FrameDataQueueHandle = frameDataQueueHandle,
            CaptureDevice = captureDevice,
        };

        nint queue = ObjC.dispatch_queue_create("seeshark.deviceOutputQueue", 0);
        deviceOutput.SetSampleBufferDelegate(sampleBufferDelegate, queue);

        Console.Error.WriteLine($"Does auto-resize (before): {deviceOutput.AutomaticallyConfiguresOutputBufferDimensions()}");
        deviceOutput.SetAutomaticallyConfiguresOutputBufferDimensions(false);
        Console.Error.WriteLine($"Does auto-resize (after): {deviceOutput.AutomaticallyConfiguresOutputBufferDimensions()}");

        NSDictionary dict = videoFormatOptionsToSettingsDict(options);
        dict.DebugPrint();

        if (session.CanAddOutput(deviceOutput))
            session.AddOutput(deviceOutput);

        CMVideoDimensions maxDimension = new NSValue<CMVideoDimensions>(deviceFormat.SupportedMaxPhotoDimensions.ObjectAtIndex(0)).GetValue();
        AVFrameRateRange framerateRange = new AVFrameRateRange(deviceFormat.VideoSupportedFrameRateRanges.ObjectAtIndex(0));

        // According to Apple documentation:
        // "To receive samples in a default uncompressed format, set this value to nil. Then you can query this value to receive a dictionary of the settings the session uses."
        // https://developer.apple.com/documentation/avfoundation/avcapturevideodataoutput/videosettings?language=objc
        deviceOutput.VideoSettings = new NSDictionary(0);
        dict = deviceOutput.VideoSettings;
        dict.DebugPrint();

        nint objWidth = dict.ObjectForKey("Width");
        nint objHeight = dict.ObjectForKey("Height");

        uint width = objWidth == 0 ? (uint)maxDimension.Width : new NSNumber(objWidth).UIntValue();
        uint height = objHeight == 0 ? (uint)maxDimension.Height : new NSNumber(objHeight).UIntValue();

        NSNumber pixelFormatNumber = new(deviceOutput.VideoSettings.ObjectForKey("PixelFormatType"));
        CVPixelFormatType pixelFormat = (CVPixelFormatType)pixelFormatNumber.UIntValue();

        CurrentFormat = new VideoFormat
        {
            VideoSize = (width, height),
            Framerate = new FramerateRatio
            {
                Numerator = (uint)framerateRange.MaxFrameRate,
                Denominator = 1,
            },
            ImageFormat = new ImageFormat((uint)pixelFormat)
        };
    }

    #region Capture
    public override void StartCapture()
    {
        session.StartRunning();
    }

    public override void StopCapture()
    {
        session.StopRunning();
    }

    public override bool TryReadFrame(ref Frame frame)
    {
        Frame? maybeFrame;
        lock (frameDataQueue)
            maybeFrame = frameDataQueue.TryDequeueFrame();

        if (maybeFrame == null)
            return false;

        frame.Data = maybeFrame.Data;
        frame.Width = maybeFrame.Width;
        frame.Height = maybeFrame.Height;
        frame.ImageFormat = maybeFrame.ImageFormat;

        return true;
    }
    #endregion

    private NSDictionary videoFormatOptionsToSettingsDict(VideoFormatOptions options)
    {
        int maxLength = 3;
        int count = 0;

        nint[] keys = new nint[maxLength];
        nint[] objects = new nint[maxLength];

        if (options.VideoSize is (uint width, uint height))
        {
            keys[count] = CoreVideo.KCvPixelBufferWidthKey.ID;
            objects[count++] = NSNumber.UInt(width).ID;

            keys[count] = CoreVideo.KCvPixelBufferHeightKey.ID;
            objects[count++] = NSNumber.UInt(height).ID;
        }

        if (options.ImageFormat is ImageFormat imageFormat)
        {
            keys[count] = CoreVideo.KCvPixelBufferPixelFormatTypeKey.ID;
            objects[count++] = NSNumber.UInt(imageFormat.FourCC).ID;
        }

        NSArray keysArray = NSArray.WithObjects(keys, count);
        NSArray objectsArray = NSArray.WithObjects(objects, count);
        return NSDictionary.DictionaryWithObjectsAndKeys(objectsArray, keysArray);
    }
}

[SupportedOSPlatform("MacOS")]
internal class AVCaptureVideoDataOutputSampleBufferDelegate : IAVCaptureVideoDataOutputSampleBufferDelegate
{
    internal required GCHandle FrameDataQueueHandle { get; init; }
    internal required AVCaptureDevice CaptureDevice { get; init; }

    public void CaptureOutputSambleBuffer(IAVCaptureOutput output, CMSampleBufferRef sampleBuffer, nint connection)
    {
        CVBufferRef buffer = CoreMedia.CMSampleBufferGetImageBuffer(sampleBuffer);
        nuint width = CoreVideo.CVPixelBufferGetWidth(buffer);
        nuint height = CoreVideo.CVPixelBufferGetHeight(buffer);
        CVPixelFormatType pixelFormat = CoreVideo.CVPixelBufferGetPixelFormatType(buffer);
        Console.Error.WriteLine($"Captured buffer {width}x{height} ({pixelFormat})");

        nuint dataSize = CoreVideo.CVPixelBufferGetDataSize(buffer);

        byte[] pixelBuffer = new byte[dataSize];

        CoreVideo.CVPixelBufferLockBaseAddress(buffer, CVPixelBufferLockFlags.ReadOnly);
        nint baseAddress = CoreVideo.CVPixelBufferGetBaseAddress(buffer);
        Marshal.Copy(baseAddress, pixelBuffer, 0, (int)dataSize);
        CoreVideo.CVPixelBufferUnlockBaseAddress(buffer, CVPixelBufferLockFlags.ReadOnly);

        Frame frame = new Frame()
        {
            Data = pixelBuffer,
            Width = (uint)width,
            Height = (uint)height,
            ImageFormat = new ImageFormat((uint)pixelFormat),
        };

        if (FrameDataQueueHandle.Target is FrameQueue frameDataQueue)
        {
            lock (frameDataQueue)
                frameDataQueue.EnqueueFrame(frame);
        }
    }

    public void CaptureDiscardedSampleBuffer(IAVCaptureOutput output, CMSampleBufferRef sampleBuffer, nint connection)
    {
    }
}
