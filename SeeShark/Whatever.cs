// Copyright (c) Speykious
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 2-Clause License. See LICENSE for details.

namespace SeeShark;

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Threading;
using SeeShark.Camera;
using SeeShark.Interop.MacOS;
using SeeShark.MacOS;

public class Whatever
{
    public static unsafe void Main(string[] args)
    {
        if (OperatingSystem.IsMacOS())
        {
            Console.Error.WriteLine($"AVFoundation handle: {ObjC.AVFoundationHandle}");
            Console.Error.WriteLine($"CoreMedia    handle: {CoreMedia.CoreMediaHandle}");
            Console.Error.WriteLine($"CoreVideo    handle: {CoreVideo.CoreVideoHandle}");

            AVAuthorizationStatus auth = AVCaptureDevice.AuthorizationStatusForMediaType(AVCaptureDevice.AV_MEDIA_TYPE_VIDEO);
            Console.Error.WriteLine($"Current permissions: {auth}");

            {
                foreach (CameraPath cameraPath in AVFoundation.AvailableCameras())
                {
                    Console.Error.WriteLine($"\n- {cameraPath.Name} ({cameraPath.Path})");
                    foreach (VideoFormat availableFormat in AVFoundation.AvailableFormats(cameraPath))
                        Console.Error.WriteLine($"  - {availableFormat}");
                }
            }

            Console.Error.WriteLine($"\nMedia type: {AVCaptureDevice.AV_MEDIA_TYPE_VIDEO.ToUTF8String()}");

            Console.Error.WriteLine("Getting default device");
            AVCaptureDevice? maybeDefaultDevice = AVCaptureDevice.DefaultDeviceWithMediaType(AVCaptureDevice.AV_MEDIA_TYPE_VIDEO);

            if (maybeDefaultDevice is AVCaptureDevice defaultDevice)
            {
                AVCaptureDeviceInput deviceInput = AVCaptureDeviceInput.DeviceInputWithDevice(defaultDevice);
                AVCaptureSession session = new AVCaptureSession();

                if (session.CanAddInput(deviceInput))
                    session.AddInput(deviceInput);

                AVCaptureVideoDataOutput deviceOutput = new();

                nint queue = ObjC.dispatch_queue_create("seeshark.deviceOutputQueue", 0);
                deviceOutput.SetSampleBufferDelegate(new MyAVCaptureVideoDataOutputSampleBufferDelegate(), queue);

                if (session.CanAddOutput(deviceOutput))
                    session.AddOutput(deviceOutput);

                Console.Error.WriteLine("Start running...");
                session.StartRunning();
                Console.Error.WriteLine("Started running");

                Thread.Sleep(5_000);

                Console.Error.WriteLine("We ran!");
                session.StopRunning();
            }
            else
            {
                Console.Error.WriteLine("There's no default device :(");
            }
        }
        else
        {
            Console.Error.WriteLine("Getting available cameras");
            List<CameraPath> availableCameras = CameraDevice.Available();
            CameraPath cameraPath = availableCameras[0];

            Console.Error.WriteLine($"\nAvailable video formats for {cameraPath}:");
            foreach (VideoFormat format in CameraDevice.AvailableFormats(cameraPath))
                Console.Error.WriteLine($"- {format}");

            Console.Error.WriteLine($"\nOpening {cameraPath}");
            CameraDevice camera = CameraDevice.Open(cameraPath);

            (uint width, uint height) = camera.CurrentFormat.VideoSize;
            ImageFormat imageFormat = camera.CurrentFormat.ImageFormat;
            using FileStream rawPixelsFile = File.OpenWrite($"camera-feed.{width}x{height}.{imageFormat}.raw");

            Console.Error.WriteLine("Start capture");
            camera.StartCapture();

            Console.Error.WriteLine("Capturing frames...");
            Frame frame = new Frame();
            for (int i = 0; i < 100; i++)
            {
                camera.ReadFrame(ref frame);
                Console.Error.WriteLine($"Frame: {frame}");
                rawPixelsFile.Write(frame.Data, 0, frame.Data.Length);

                Thread.Sleep(50);
            }
            Console.Error.WriteLine("ENOUGH");

            Console.Error.WriteLine("Stop capture");
            camera.StopCapture();
        }
    }
}

[SupportedOSPlatform("MacOS")]
internal class MyAVCaptureVideoDataOutputSampleBufferDelegate : IAVCaptureVideoDataOutputSampleBufferDelegate
{
    public void CaptureOutputSambleBuffer(IAVCaptureOutput output, CMSampleBufferRef sampleBuffer, nint connection)
    {
        CVBufferRef buffer = CoreMedia.CMSampleBufferGetImageBuffer(sampleBuffer);
        NSDictionary dict = CoreVideo.CVBufferCopyAttachments(buffer, CVAttachmentMode.ShouldPropagate);
        uint count = (uint)dict.Count();
        nuint width = CoreVideo.CVPixelBufferGetWidth(buffer);
        nuint height = CoreVideo.CVPixelBufferGetHeight(buffer);
        CVPixelFormatType pixelFormat = CoreVideo.CVPixelBufferGetPixelFormatType(buffer);

        nuint dataSize = CoreVideo.CVPixelBufferGetDataSize(buffer);
        bool isPlanar = CoreVideo.CVPixelBufferIsPlanar(buffer);

        byte[] pixelBuffer = new byte[dataSize];

        CoreVideo.CVPixelBufferLockBaseAddress(buffer, CVPixelBufferLockFlags.ReadOnly);
        nint baseAddress = CoreVideo.CVPixelBufferGetBaseAddress(buffer);
        Marshal.Copy(baseAddress, pixelBuffer, 0, (int)dataSize);
        CoreVideo.CVPixelBufferUnlockBaseAddress(buffer, CVPixelBufferLockFlags.ReadOnly);

        Console.Error.WriteLine($"Captured a sample buffer ({count} attachments, {width}x{height}, pxfmt {pixelFormat}, ds {pixelBuffer.Length}, planar={isPlanar})");

        // Write this raw pixel buffer to a file
        using (FileStream rawPixels = File.Open($"camera-feed.{width}x{height}.{pixelFormat}.raw", FileMode.Append))
            rawPixels.Write(pixelBuffer, 0, pixelBuffer.Length);
    }

    public void CaptureDiscardedSampleBuffer(IAVCaptureOutput output, CMSampleBufferRef sampleBuffer, nint connection)
    {
        Console.Error.WriteLine("Capturedn't a sample buffer");
    }
}
