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
    private static bool hasPermission = false;
    private static bool askedPermission = false;

    private static void onPermissionRequestCompleted(object? _sender, bool permissionGranted)
    {
        if (permissionGranted)
            Console.Error.WriteLine("Permission has been granted!");
        else
            Console.Error.WriteLine("Permission has not been granted! (wat)");
        hasPermission = permissionGranted;
        askedPermission = true;
    }

    public static unsafe void Main(string[] args)
    {
        Console.Error.WriteLine("Getting available cameras");
        List<CameraPath> availableCameras = CameraDevice.Available();
        CameraPath cameraPath = availableCameras[0];

        Console.Error.WriteLine($"\nAvailable video formats for {cameraPath}:");
        List<VideoFormat> availableFormats = CameraDevice.AvailableFormats(cameraPath);
        foreach (VideoFormat format in availableFormats)
            Console.Error.WriteLine($"- {format}");

        Console.Error.WriteLine($"\nOpening {cameraPath}");
        CameraDevice camera = CameraDevice.Open(cameraPath, new VideoFormatOptions
        {
            VideoSize = (640, 480),
            // ImageFormat = new ImageFormat(0x20),
        });

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

            Thread.Sleep(10);
        }
        Console.Error.WriteLine("ENOUGH");

        Console.Error.WriteLine("Stop capture");
        camera.StopCapture();
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
