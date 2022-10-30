// Copyright (c) The Vignette Authors
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 3-Clause License. See LICENSE for details.

using System;
using System.Diagnostics;
using SeeShark.Decode;
using SeeShark.Device;
using static SeeShark.FFmpeg.FFmpegManager;

namespace SeeShark.Example.Stats;

class Program
{
    static Camera? karen;
    static CameraManager? manager;

    static void Main(string[] args)
    {
        // Casually displaying "Oof :(" when exiting the program with force.
        Console.CancelKeyPress += (object? _sender, ConsoleCancelEventArgs e) =>
        {
            Console.Error.WriteLine("\n\n");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine("Oof :(");
            Console.ResetColor();
            Dispose();
        };

        // You can add your own path for FFmpeg libraries here!
        SetupFFmpeg(
            AppDomain.CurrentDomain.BaseDirectory,
            "/usr/lib",
            "/usr/lib64"
        );

        manager = new CameraManager();

        string devicePath;
        if (args.Length < 1)
        {
            /// Select an available camera device.
            /// <see cref="CameraManager.Devices"/> only gets filled when the camera manager is instanciated,
            /// since it is not watching devices by default.
            while (true)
            {
                Console.WriteLine("\nDevices available:");
                for (int i = 0; i < manager.Devices.Count; i++)
                    Console.WriteLine($"| #{i}: {manager.Devices[i]}");

                Console.Write("\nChoose a camera by index: ");
                Console.Out.Flush();
                if (int.TryParse(Console.ReadLine(), out int index) && index < manager.Devices.Count && index >= 0)
                {
                    devicePath = manager.Devices[index].Path;
                    break;
                }
            }
        }
        else
        {
            devicePath = args[0];
        }

        /// You can get a <see cref="Camera"/> from either a string
        /// representing the device path, or a <see cref="CameraInfo">.

        // Unfortunately, she saw the manager
        karen = manager.GetDevice(devicePath);

        /// Attach our <see cref="OnNewFrame"/> method to the camera's frame event handler,
        /// so that we can process every coming frame the way we want.
        karen.OnFrame += OnFrameEventHandler;

        Console.WriteLine($"Camera chosen: {karen.Info}");
        Console.WriteLine("Press Space or P to play/pause the camera.");
        Console.WriteLine("Press Enter or Q or Escape to exit the program.");

        // I could have written a simple `while (true)`, but then I used a `switch` statement.
        // If only C# had labelled loops like Rust :(
        for (var loop = true; loop;)
        {
            Console.WriteLine("\x1b[2K\rCamera is {0}", karen.IsPlaying ? "Playing" : "Paused");
            var cki = Console.ReadKey(true);
            switch (cki.Key)
            {
                case ConsoleKey.P:
                case ConsoleKey.Spacebar:
                    if (karen.IsPlaying)
                        karen.StopCapture();
                    else
                        karen.StartCapture();
                    Console.CursorVisible = !karen.IsPlaying;
                    break;

                case ConsoleKey.Q:
                case ConsoleKey.Enter:
                case ConsoleKey.Escape:
                    Console.CursorVisible = true;
                    loop = false;
                    break;
            }
        }

        Dispose();

        // Unless you filmed a shark with your camera, no.
        Console.WriteLine("\n\nDid you SeeShark? :)");
    }

    static long frameCount = 0;
    static double fps = 0;
    static double minFps = double.PositiveInfinity;
    static double maxFps = double.NegativeInfinity;


    /// <summary>
    /// Stopwatch used to measure the FPS.
    /// </summary>
    static readonly Stopwatch watch = new Stopwatch();

    /// <summary>
    /// Our custom frame event callback.
    /// Each time it is triggered, it will display some simple stats in the console.
    /// </summary>
    public static void OnFrameEventHandler(object? _sender, FrameEventArgs e)
    {
        // Don't redraw the frame if it's not new.
        if (e.Status != DecodeStatus.NewFrame)
            return;

        var frame = e.Frame;

        #region Stats
        if (frameCount == 0)
        {
            watch.Start();
        }
        else
        {
            fps = TimeSpan.TicksPerSecond * 100 / (double)watch.ElapsedTicks;
            minFps = fps < minFps ? fps : minFps;
            maxFps = fps > maxFps ? fps : maxFps;
            Console.WriteLine($"\x1b[2K\r{frame.Width}x{frame.Height} @ {fps:#.##} fps [{minFps} - {maxFps}]");
            watch.Restart();
        }
        #endregion

        frameCount++;
    }

    /// <summary>
    /// Dispose our <see cref="IDisposable"/> objects.
    /// </summary>
    public static void Dispose()
    {
        karen?.StopCapture();
        karen?.Dispose();
        manager?.Dispose();
    }
}
