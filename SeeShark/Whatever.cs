// Copyright (c) Speykious
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 3-Clause License. See LICENSE for details.

namespace SeeShark;

using System;
using System.Collections.Generic;
using System.Threading;
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
            Console.WriteLine($"Nothing on MacOS yet");
        }
    }
}
