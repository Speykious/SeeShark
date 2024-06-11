// Copyright (c) Speykious
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 2-Clause License. See LICENSE for details.

namespace SeeShark;

using System;
using System.Collections.Generic;
using System.Threading;
using SeeShark.Interop.MacOS;
using SeeShark.Linux;

public class Whatever
{
    public static unsafe void Main(string[] args)
    {
        if (OperatingSystem.IsLinux())
        {
            Console.WriteLine("Getting available cameras");
            List<CameraPath> availableCameras = V4l2.AvailableCameras();
            CameraPath cameraPath = availableCameras[0];

            Console.WriteLine($"\nAvailable video formats for {cameraPath}:");
            foreach (VideoFormat format in V4l2.AvailableFormats(cameraPath))
                Console.WriteLine($"- {format}");

            Console.WriteLine($"\nOpening {cameraPath}");
            V4l2.Camera camera = V4l2.OpenCamera(cameraPath);

            Console.WriteLine("Start capture");
            V4l2.StartCapture(camera);

            Console.WriteLine("Capturing frames...");
            Frame frame = new Frame();
            for (int i = 0; i < 100; i++)
            {
                V4l2.ReadFrame(camera, ref frame);
                Console.WriteLine($"Frame: {frame}");

                Thread.Sleep(50);
            }
            Console.WriteLine("ENOUGH");

            Console.WriteLine("Stop capture");
            V4l2.StopCapture(camera);
        }
        else if (OperatingSystem.IsWindows())
        {
            Console.WriteLine($"Nothing on Windows yet");
        }
        else if (OperatingSystem.IsMacOS())
        {
            Console.WriteLine($"AVFoundation handle: {ObjC.AVFoundationHandle}");

            AVAuthorizationStatus auth = AVCaptureDevice.AuthorizationStatusForMediaType(AVCaptureDevice.AV_MEDIA_TYPE_VIDEO);
            Console.WriteLine($"Current permissions: {auth}");

            {
                nint[] devices = AVCaptureDevice.Devices.ToArray();

                Console.WriteLine($"Got devices");
                foreach (nint deviceID in devices)
                {
                    AVCaptureDevice device = new AVCaptureDevice(deviceID);

                    Console.WriteLine($"\n- {device.LocalizedName.ToUTF8String()}");
                    Console.WriteLine($"    Unique ID:    {device.UniqueID.ToUTF8String()}");
                    Console.WriteLine($"    Video device: {device.HasMediaType(AVCaptureDevice.AV_MEDIA_TYPE_VIDEO)}");
                }
            }

            Console.WriteLine($"\nMedia type: {AVCaptureDevice.AV_MEDIA_TYPE_VIDEO.ToUTF8String()}");
            Console.WriteLine("Getting default device");
            AVCaptureDevice? maybeDefaultDevice = AVCaptureDevice.DefaultDeviceWithMediaType(AVCaptureDevice.AV_MEDIA_TYPE_VIDEO);

            if (maybeDefaultDevice is AVCaptureDevice defaultDevice)
            {
                Console.WriteLine("Got default device");

                AVCaptureDeviceInput deviceInput = AVCaptureDeviceInput.DeviceInputWithDevice(defaultDevice);
                Console.WriteLine("Got device input");

                AVCaptureSession session = new AVCaptureSession();
                Console.WriteLine($"Allocated session");

                if (session.CanAddInput(deviceInput))
                {
                    Console.WriteLine("Input can be added");

                    session.AddInput(deviceInput);
                    Console.WriteLine("Input was added");
                }
                else
                {
                    Console.WriteLine("Input cannot be added");
                }

                AVCaptureVideoDataOutput deviceOutput = new();
                Console.WriteLine("Got device output");

                nint queue = ObjC.dispatch_queue_create("seeshark.deviceOutputQueue", 0);
                deviceOutput.SetSampleBufferDelegate(new MyAVCaptureVideoDataOutputSampleBufferDelegate(), queue);
                Console.WriteLine("Buffer delegate was set");

                if (session.CanAddOutput(deviceOutput))
                {
                    Console.WriteLine("Output can be added");

                    session.AddOutput(deviceOutput);
                    Console.WriteLine("Output was added");
                }
                else
                {
                    Console.WriteLine("Output cannot be added");
                }

                Console.WriteLine("Start running...");
                session.StartRunning();
                Console.WriteLine("Started running");

                Thread.Sleep(5_000);

                Console.WriteLine("We ran!");
                session.StopRunning();
            }
            else
            {
                Console.WriteLine("there's no default device :(");
            }
        }
    }
}

internal class MyAVCaptureVideoDataOutputSampleBufferDelegate : IAVCaptureVideoDataOutputSampleBufferDelegate
{
    public void CaptureOutputSambleBuffer(IAVCaptureOutput output, nint sampleBuffer, nint connection)
    {
        Console.WriteLine("Captured a sample buffer");
    }

    public void CaptureDiscardedSampleBuffer(IAVCaptureOutput output, nint sampleBuffer, nint connection)
    {
        Console.WriteLine("Capturedn't a sample buffer");
    }
}
