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
using SeeShark.Interop.MacOS;
using SeeShark.Linux;
using SeeShark.MacOS;

public class Whatever
{
    public static unsafe void Main(string[] args)
    {
        if (OperatingSystem.IsLinux())
        {
            Console.Error.WriteLine("Getting available cameras");
            List<CameraPath> availableCameras = V4l2.AvailableCameras();
            CameraPath cameraPath = availableCameras[0];

            Console.Error.WriteLine($"\nAvailable video formats for {cameraPath}:");
            foreach (VideoFormat format in V4l2.AvailableFormats(cameraPath))
                Console.Error.WriteLine($"- {format}");

            Console.Error.WriteLine($"\nOpening {cameraPath}");
            V4l2.Camera camera = V4l2.OpenCamera(cameraPath);

            Console.Error.WriteLine("Start capture");
            V4l2.StartCapture(camera);

            Console.Error.WriteLine("Capturing frames...");
            Frame frame = new Frame();
            for (int i = 0; i < 100; i++)
            {
                V4l2.ReadFrame(camera, ref frame);
                Console.Error.WriteLine($"Frame: {frame}");

                Thread.Sleep(50);
            }
            Console.Error.WriteLine("ENOUGH");

            Console.Error.WriteLine("Stop capture");
            V4l2.StopCapture(camera);
        }
        else if (OperatingSystem.IsWindows())
        {
            Console.Error.WriteLine($"Nothing on Windows yet");
        }
        else if (OperatingSystem.IsMacOS())
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

            {
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
        }
    }
}

[SupportedOSPlatform("MacOS")]
internal class MyAVCaptureVideoDataOutputSampleBufferDelegate : IAVCaptureVideoDataOutputSampleBufferDelegate
{
    private bool wasPixelFormatDescribed = false;

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

        if (!wasPixelFormatDescribed)
            Console.Error.WriteLine($"Pixel format description: {pixelFormat} | {CVPixelFormatTypeMethods.Describe(pixelFormat)}\n");

        wasPixelFormatDescribed = true;
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
