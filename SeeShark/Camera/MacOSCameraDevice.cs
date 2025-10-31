// Copyright (c) Speykious
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 2-Clause License. See LICENSE for details.

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
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

    internal MacOSCameraDevice(CameraPath cameraInfo, VideoFormatOptions options)
    {
        // Make sure we have the permission to access the camera
        AVAuthorizationStatus auth = AVCaptureDevice.AuthorizationStatusForMediaType(AVCaptureDevice.AV_MEDIA_TYPE_VIDEO);
        if (auth == AVAuthorizationStatus.Denied)
            throw new UnauthorizedAccessException("Access to camera is denied");

        AVCaptureDevice? maybeCaptureDevice = AVCaptureDevice.DeviceWithUniqueID(cameraInfo.Path);
        if (maybeCaptureDevice is not AVCaptureDevice device)
            throw new CameraDeviceIOException(cameraInfo, "Cannot open camera");

        if (options.VideoSize is not null || options.Framerate is not null)
        {
            if (!device.LockForConfiguration())
                throw new CameraDeviceIOException(cameraInfo, "Cannot lock camera for configuration");

            try
            {
                // Go through device formats and choose most suited one according to options
                if (options.VideoSize is (uint width, uint height))
                {
                    AVCaptureDeviceFormat[] deviceFormats = device.Formats.ToTypedArray<AVCaptureDeviceFormat>(id => new(id));

                    // Compare minimum side lengths, get closest dimension
                    AVCaptureDeviceFormat selectedDeviceFormat = deviceFormats[0];
                    {
                        int minDelta = int.MaxValue;
                        foreach (AVCaptureDeviceFormat deviceFormat in deviceFormats)
                        {
                            // TODO: What about other supported max photo dimensions?
                            CMVideoDimensions maxDimension = new NSValue<CMVideoDimensions>(deviceFormat.SupportedMaxPhotoDimensions.ObjectAtIndex(0)).GetValue();

                            int delta = Math.Abs(height <= width
                                ? (int)height - maxDimension.Height
                                : (int)width - maxDimension.Width);

                            if (delta < minDelta)
                            {
                                minDelta = delta;
                                selectedDeviceFormat = deviceFormat;
                            }
                        }
                    }

                    device.ActiveFormat = selectedDeviceFormat;
                }

                if (options.Framerate is FramerateRatio framerateRatio)
                {
                    double framerate = framerateRatio.Value;

                    // Check framerate against supported ranges ourselves to avoid crashing.
                    // See https://developer.apple.com/documentation/avfoundation/avcapturedevice/activevideominframeduration?language=objc
                    //     https://developer.apple.com/documentation/avfoundation/avcapturedevice/activevideomaxframeduration?language=objc

                    AVFrameRateRange[] frameRateRanges = device.ActiveFormat.VideoSupportedFrameRateRanges.ToTypedArray<AVFrameRateRange>(id => new(id));

                    bool supported = false;
                    foreach (AVFrameRateRange frameRateRange in frameRateRanges)
                    {
                        if (frameRateRange.MinFrameRate <= framerate && framerate <= frameRateRange.MaxFrameRate)
                            supported = true;
                    }

                    if (!supported)
                    {
                        StringBuilder errorMessage = new StringBuilder();
                        errorMessage.AppendLine($"This device does not support a framerate of {framerate:0.##} Hz.");
                        errorMessage.Append("Supported framerate ranges: ");

                        bool started = false;
                        foreach (AVFrameRateRange frameRateRange in frameRateRanges)
                        {
                            if (started)
                                errorMessage.Append(", ");
                            else
                                started = true;

                            errorMessage.Append($"{frameRateRange.MinFrameRate:0.##}..{frameRateRange.MaxFrameRate:0.##}");
                        }

                        throw new Exception(errorMessage.ToString());
                    }

                    CMTime frameDuration = new()
                    {
                        Value = framerateRatio.Denominator,
                        Timescale = (int)framerateRatio.Numerator,
                        Flags = CMTimeFlags.HAS_BEEN_ROUNDED,
                        Epoch = 0,
                    };

                    // TODO: Consider whether it would be useful to be able to let the user specify a framerate range.
                    device.ActiveVideoMinFrameDuration = frameDuration;
                    device.ActiveVideoMaxFrameDuration = frameDuration;
                }
            }
            finally
            {
                device.UnlockForConfiguration();
            }
        }

        captureDevice = device;
        frameDataQueue = new FrameQueue(16);
        frameDataQueueHandle = GCHandle.Alloc(frameDataQueue, GCHandleType.Pinned);

        session = new AVCaptureSession();

        // Set a session preset if no video dimension options are given
        if (options.VideoSize is null)
        {
            session.BeginConfiguration();
            {
                if (session.CanSetSessionPreset(AVCaptureSession.AV_CAPTURE_SESSION_PRESET_HIGH))
                    session.SessionPreset = AVCaptureSession.AV_CAPTURE_SESSION_PRESET_HIGH;
                else if (session.CanSetSessionPreset(AVCaptureSession.AV_CAPTURE_SESSION_PRESET_MEDIUM))
                    session.SessionPreset = AVCaptureSession.AV_CAPTURE_SESSION_PRESET_MEDIUM;
                else if (session.CanSetSessionPreset(AVCaptureSession.AV_CAPTURE_SESSION_PRESET_LOW))
                    session.SessionPreset = AVCaptureSession.AV_CAPTURE_SESSION_PRESET_LOW;

                // TODO: Is it worth considering any other presets?
            }
            session.CommitConfiguration();
        }

        AVCaptureDeviceInput deviceInput = AVCaptureDeviceInput.DeviceInputWithDevice(captureDevice);
        if (session.CanAddInput(deviceInput))
            session.AddInput(deviceInput);

        AVCaptureVideoDataOutput deviceOutput = new();
        if (session.CanAddOutput(deviceOutput))
            session.AddOutput(deviceOutput);

        // TODO: Think about how I should do logging

        deviceOutput.SetAutomaticallyConfiguresOutputBufferDimensions(false);
        Debug.Assert(!deviceOutput.AutomaticallyConfiguresOutputBufferDimensions(), "Cannot disable automatic configuration of output buffer dimensions in device output");

        // Set video settings

        // NOTE: While MacOS lists a few set dimensions as available formats, it supports arbitrary dimensions
        // in its video settings and will resize the output accordingly.
        deviceOutput.VideoSettings = videoFormatOptionsToSettingsDict(options);

        // // According to Apple documentation:
        // // "To receive samples in a default uncompressed format, set this value to nil. Then you can query this value to receive a dictionary of the settings the session uses."
        // // https://developer.apple.com/documentation/avfoundation/avcapturevideodataoutput/videosettings?language=objc
        // deviceOutput.VideoSettings = new NSDictionary(0);

        // Determine current video format from applied video settings
        {
            NSDictionary dict = deviceOutput.VideoSettings;
            CMTime maxFrameDuration = device.ActiveVideoMaxFrameDuration;

            CMVideoDimensions maxDimension = new NSValue<CMVideoDimensions>(device.ActiveFormat.SupportedMaxPhotoDimensions.ObjectAtIndex(0)).GetValue();

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
                    Numerator = (uint)maxFrameDuration.Timescale,
                    Denominator = (uint)maxFrameDuration.Value,
                },
                ImageFormat = new ImageFormat((uint)pixelFormat)
            };
        }

        sampleBufferDelegate = new()
        {
            FrameDataQueueHandle = frameDataQueueHandle,
            CaptureDevice = captureDevice,
        };

        nint queue = ObjC.dispatch_queue_create("seeshark.deviceOutputQueue", 0);
        deviceOutput.SetSampleBufferDelegate(sampleBufferDelegate, queue);
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

        // NOTE: Consider making the video scaling mode configurable by the user in the future.
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

        int sampleBitSize = CVPixelFormatTypeMethods.BitSize(pixelFormat);
        assertFrame(sampleBitSize > 0, $"Unsupported pixel format ({pixelFormat}): untested, bit size unknown or non-trivial/non-raw format");
        assertFrame(sampleBitSize % 8 == 0, $"Unsupported pixel format ({pixelFormat}): untested, bit size is not a multiple of 8");

        nuint expectedStride = width * (nuint)(sampleBitSize / 8);
        nuint bufferSize = CoreVideo.CVPixelBufferGetDataSize(buffer);
        assertFrame(stride * height == bufferSize, "Pixel buffer size, stride and height are somehow inconsistent");
        assertFrame(expectedStride <= stride, "Pixel buffer's actual stride is smaller than expected stride");

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
            // Expected stride different from buffer stride; copy row by row while ignoring the padding at the end
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

    private static void assertFrame(bool condition, string message)
    {
        if (!condition)
            throw new CameraDeviceInvalidFrameException(message);
    }
}
