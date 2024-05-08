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
        Console.WriteLine("Getting available cameras");
        List<CameraInfo> cameraInfos = V4l2.AvailableCameras();

        Console.WriteLine($"Opening {cameraInfos[0]}");
        V4l2.Camera camera = V4l2.OpenCamera(cameraInfos[0]);

        Console.WriteLine("Start capture");
        V4l2.StartCapture(camera);

        Console.Write("Waiting..");
        for (int i = 0; i < 5; i++)
        {
            Console.Write(".");
            Thread.Sleep(1000);
        }
        Console.WriteLine(" ENOUGH");

        Console.WriteLine("Stop capture");
        V4l2.StopCapture(camera);
    }
}
