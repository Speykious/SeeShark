// Copyright (c) Speykious
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 2-Clause License. See LICENSE for details.

using System;
using System.Diagnostics;
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
        // TODO: Go through device formats and choose most suited one according to options
        AVCaptureDeviceFormat deviceFormat = new AVCaptureDeviceFormat(device.Formats.ObjectAtIndex(1));
        device.LockForConfiguration();
        device.ActiveFormat = deviceFormat;
        device.UnlockForConfiguration();

        captureDevice = device;
        frameDataQueue = new FrameQueue(16);
        frameDataQueueHandle = GCHandle.Alloc(frameDataQueue, GCHandleType.Pinned);

        session = new AVCaptureSession();

        // // TODO: Consider setting a session preset if default video options are given.
        // session.BeginConfiguration();
        // {
        //     Console.Error.WriteLine($"Can set PRESET_HIGH:           {session.CanSetSessionPreset(AVCaptureSession.AV_CAPTURE_SESSION_PRESET_HIGH)}");
        //     Console.Error.WriteLine($"Can set PRESET_MEDIUM:         {session.CanSetSessionPreset(AVCaptureSession.AV_CAPTURE_SESSION_PRESET_MEDIUM)}");
        //     Console.Error.WriteLine($"Can set PRESET_LOW:            {session.CanSetSessionPreset(AVCaptureSession.AV_CAPTURE_SESSION_PRESET_LOW)}");
        //     Console.Error.WriteLine($"Can set PRESET_PHOTO:          {session.CanSetSessionPreset(AVCaptureSession.AV_CAPTURE_SESSION_PRESET_PHOTO)}");
        //     Console.Error.WriteLine($"Can set PRESET_INPUT_PRIORITY: {session.CanSetSessionPreset(AVCaptureSession.AV_CAPTURE_SESSION_PRESET_INPUT_PRIORITY)}");
        //     Console.Error.WriteLine($"Can set PRESET960X540:         {session.CanSetSessionPreset(AVCaptureSession.AV_CAPTURE_SESSION_PRESET960X540)}");
        //     Console.Error.WriteLine($"Can set PRESET1280X720:        {session.CanSetSessionPreset(AVCaptureSession.AV_CAPTURE_SESSION_PRESET1280X720)}");
        //     Console.Error.WriteLine($"Can set PRESET1920X1080:       {session.CanSetSessionPreset(AVCaptureSession.AV_CAPTURE_SESSION_PRESET1920X1080)}");
        //     Console.Error.WriteLine($"Can set PRESET3840X2160:       {session.CanSetSessionPreset(AVCaptureSession.AV_CAPTURE_SESSION_PRESET3840X2160)}");
        //     Console.Error.WriteLine($"Can set PRESET320X240:         {session.CanSetSessionPreset(AVCaptureSession.AV_CAPTURE_SESSION_PRESET320X240)}");
        //     Console.Error.WriteLine($"Can set PRESET640X480:         {session.CanSetSessionPreset(AVCaptureSession.AV_CAPTURE_SESSION_PRESET640X480)}");
        //     Console.Error.WriteLine($"Can set PRESETI_FRAME960X540:  {session.CanSetSessionPreset(AVCaptureSession.AV_CAPTURE_SESSION_PRESETI_FRAME960X540)}");
        //     Console.Error.WriteLine($"Can set PRESETI_FRAME1280X720: {session.CanSetSessionPreset(AVCaptureSession.AV_CAPTURE_SESSION_PRESETI_FRAME1280X720)}");
        // }
        // session.SessionPreset = AVCaptureSession.AV_CAPTURE_SESSION_PRESET_MEDIUM;
        // session.CommitConfiguration();

        AVAuthorizationStatus auth = AVCaptureDevice.AuthorizationStatusForMediaType(AVCaptureDevice.AV_MEDIA_TYPE_VIDEO);
        if (auth == AVAuthorizationStatus.Denied)
            throw new UnauthorizedAccessException("Access to camera is denied");

        AVCaptureDeviceInput deviceInput = AVCaptureDeviceInput.DeviceInputWithDevice(captureDevice);

        if (session.CanAddInput(deviceInput))
            session.AddInput(deviceInput);

        AVCaptureVideoDataOutput deviceOutput = new();

        // TODO: A camera supports several pixel formats for the same dimensions.
        // Might need to do something more clever about enumerating the available video formats.

        // TODO: Remove these console prints
        Console.Error.WriteLine("Pixel format types available:");
        foreach (nint obj in deviceOutput.AvailableVideoCVPixelFormatTypes.ToArray())
        {
            NSNumber number = new(obj);
            Console.Error.WriteLine($"  {(CVPixelFormatType)number.UIntValue()}");
        }

        // TODO: Think about how I should do logging
        Console.Error.WriteLine("[SeeShark] Warning: framerate settings are currently ignored on MacOS");

        sampleBufferDelegate = new()
        {
            FrameDataQueueHandle = frameDataQueueHandle,
            CaptureDevice = captureDevice,
        };

        nint queue = ObjC.dispatch_queue_create("seeshark.deviceOutputQueue", 0);
        deviceOutput.SetSampleBufferDelegate(sampleBufferDelegate, queue);

        deviceOutput.SetAutomaticallyConfiguresOutputBufferDimensions(false);
        Debug.Assert(!deviceOutput.AutomaticallyConfiguresOutputBufferDimensions(), "Cannot disable automatic configuration of output buffer dimensions in device output");

        if (session.CanAddOutput(deviceOutput))
            session.AddOutput(deviceOutput);

        // Set video settings

        // NOTE: While MacOS lists a few set dimensions as available formats, it supports arbitrary dimensions
        // in its video settings and will resize the output accordingly.
        NSDictionary dict = videoFormatOptionsToSettingsDict(options);
        deviceOutput.VideoSettings = dict;

        // // According to Apple documentation:
        // // "To receive samples in a default uncompressed format, set this value to nil. Then you can query this value to receive a dictionary of the settings the session uses."
        // // https://developer.apple.com/documentation/avfoundation/avcapturevideodataoutput/videosettings?language=objc
        // deviceOutput.VideoSettings = new NSDictionary(0);

        dict = deviceOutput.VideoSettings;

        // Determine current video format from applied video settings

        CMVideoDimensions maxDimension = new NSValue<CMVideoDimensions>(deviceFormat.SupportedMaxPhotoDimensions.ObjectAtIndex(0)).GetValue();
        AVFrameRateRange framerateRange = new AVFrameRateRange(deviceFormat.VideoSupportedFrameRateRanges.ObjectAtIndex(0));

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
        int maxLength = 4;
        int count = 0;

        nint[] keys = new nint[maxLength];
        nint[] objects = new nint[maxLength];

        // TODO: Should this video scaling mode be configurable by the user?
        keys[count] = CoreVideo.AVVideoScalingModeKey.ID;
        objects[count++] = CoreVideo.AVVideoScalingModeResizeAspect.ID;

        if (options.ImageFormat is ImageFormat imageFormat)
        {
            keys[count] = CoreVideo.KCvPixelBufferPixelFormatTypeKey.ID;
            objects[count++] = NSNumber.UInt(imageFormat.FourCC).ID;
        }

        if (options.VideoSize is (uint width, uint height))
        {
            keys[count] = CoreVideo.KCvPixelBufferWidthKey.ID;
            objects[count++] = NSNumber.UInt(width).ID;

            keys[count] = CoreVideo.KCvPixelBufferHeightKey.ID;
            objects[count++] = NSNumber.UInt(height).ID;
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
        // NOTE: The sample buffers we get have a stride corresponding to a width that is always a multiple of 16 pixels.
        // So to correctly copy the buffer over, we need to compare the actual stride with the expected stride and then
        // do the copying row by row if needed.

        CVBufferRef buffer = CoreMedia.CMSampleBufferGetImageBuffer(sampleBuffer);
        nuint width = CoreVideo.CVPixelBufferGetWidth(buffer);
        nuint height = CoreVideo.CVPixelBufferGetHeight(buffer);
        nuint stride = CoreVideo.CVPixelBufferGetBytesPerRow(buffer);
        CVPixelFormatType pixelFormat = CoreVideo.CVPixelBufferGetPixelFormatType(buffer);

        // TODO: Test what happens when an assert fails inside of a delegate
        // (It gets passed to native code, so let's make sure it's still ok)

        int sampleBitSize = CVPixelFormatTypeMethods.BitSize(pixelFormat);
        Debug.Assert(sampleBitSize > 0, $"Unsupported pixel format ({pixelFormat}): untested, bit size unknown or non-trivial/non-raw format");
        Debug.Assert(sampleBitSize % 8 == 0, $"Unsupported pixel format ({pixelFormat}): untested, bit size is not a multiple of 8");

        nuint expectedStride = width * (nuint)(sampleBitSize / 8);
        nuint bufferSize = CoreVideo.CVPixelBufferGetDataSize(buffer);
        Debug.Assert(stride * height == bufferSize, "Pixel buffer size, stride and height are somehow inconsistent");
        Debug.Assert(expectedStride <= stride, "Pixel buffer's actual stride is smaller than expected stride");

        byte[] pixelBuffer = new byte[expectedStride * height];

        CoreVideo.CVPixelBufferLockBaseAddress(buffer, CVPixelBufferLockFlags.ReadOnly);
        nint baseAddress = CoreVideo.CVPixelBufferGetBaseAddress(buffer);
        if (stride == expectedStride)
        {
            // Expected stride same as buffer stride; copy buffer in one go
            Marshal.Copy(baseAddress, pixelBuffer, 0, pixelBuffer.Length);
        }
        else
        {
            // Expected stride different from buffer stride; copy row by row
            for (nuint y = 0; y < height; y++)
                Marshal.Copy(baseAddress + (nint)(y * stride), pixelBuffer, (int)(y * expectedStride), (int)expectedStride);
        }
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
