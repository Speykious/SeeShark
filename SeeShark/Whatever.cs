// Copyright (c) Speykious
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 2-Clause License. See LICENSE for details.

namespace SeeShark;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using SeeShark.Camera;
using SeeShark.Interop.MacOS;

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
        foreach (CameraPath availableCamera in availableCameras)
            Console.Error.WriteLine($"- {availableCamera}");

        CameraPath cameraPath = availableCameras[0];

        Console.Error.WriteLine($"\nAvailable video formats for {cameraPath}:");
        List<VideoFormat> availableFormats = CameraDevice.AvailableFormats(cameraPath);
        foreach (VideoFormat format in availableFormats)
            Console.Error.WriteLine($"- {format}");

        ImageFormat imageFormat = new ImageFormat((uint)CVPixelFormatType.k_32ARGB);
        string ffmpegPixelFormat = "argb";

        Console.Error.WriteLine($"\nOpening {cameraPath}");
        CameraDevice camera = CameraDevice.Open(cameraPath, new VideoFormatOptions
        {
            VideoSize = (1280, 720),
            ImageFormat = imageFormat,
            Framerate = new FramerateRatio
            {
                Numerator = 30,
                Denominator = 1,
            },
        });

        (uint width, uint height) = camera.CurrentFormat.VideoSize;
        imageFormat = camera.CurrentFormat.ImageFormat;

        Stream rawPixelsStream;

        ProcessStartInfo ffmpegStartInfo = new ProcessStartInfo()
        {
            RedirectStandardInput = true,
            UseShellExecute = false,
            FileName = "ffmpeg",
            Arguments = $"-hide_banner -f rawvideo -video_size {width}x{height} -pixel_format {ffmpegPixelFormat} -i - -pix_fmt yuv420p -y camera-feed.mp4"
        };

        Process? ffmpegProcess = Process.Start(ffmpegStartInfo);
        if (ffmpegProcess is not null)
        {
            rawPixelsStream = ffmpegProcess.StandardInput.BaseStream;
        }
        else
        {
            Console.Error.WriteLine("[SeeShark] Could not find ffmpeg executable, outputting raw frames to file instead");
            rawPixelsStream = File.OpenWrite($"camera-feed.{width}x{height}.{imageFormat}.raw");
        }

        Console.Error.WriteLine("Start capture");
        camera.StartCapture();

        Console.Error.WriteLine("Capturing frames...");
        Frame frame = new Frame();
        for (int i = 0; i < 120; i++)
        {
            camera.ReadFrame(ref frame);
            Console.Error.WriteLine($"Frame: {frame}");
            rawPixelsStream.Write(frame.Data, 0, frame.Data.Length);
        }
        Console.Error.WriteLine("ENOUGH");

        Console.Error.WriteLine("Stop capture");
        camera.StopCapture();

        rawPixelsStream.Close();
        if (ffmpegProcess is not null)
            ffmpegProcess.WaitForExit();
        else
            rawPixelsStream.Dispose();
    }
}
