// Copyright (c) The Vignette Authors
// This file is part of SeeShark.
// SeeShark is licensed under LGPL v3. See LICENSE.LESSER.md for details.

using static SeeShark.FFmpeg.FFmpegManager;

namespace SeeShark.Example
{
    class Program
    {
        static FileStream? outputStream;

        static void Main(string[] args)
        {
            Console.CancelKeyPress += (object? _sender, ConsoleCancelEventArgs e) =>
            {
                Console.Error.WriteLine("\n\n");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine("Oof :(");
                Console.ResetColor();
            };

            if (args.Length < 2)
            {
                Console.WriteLine("Usage: dotnet run <camera-device> <output-file>");
                return;
            }

            var devicePath = args[0];
            outputStream = File.Create(args[1]);

            Console.WriteLine($"Current directory: {Environment.CurrentDirectory}");
            Console.WriteLine("Running in {0}-bit mode.", Environment.Is64BitProcess ? "64" : "32");
            Console.WriteLine($"FFmpeg version info: {FFmpegVersion}");

            Console.WriteLine("Creating camera manager...");
            var manager = new CameraManager();

            Console.WriteLine("\nDevices available:");
            foreach (var device in manager.Devices)
                Console.WriteLine($"| {device.Path} ({device.Name})");

            Console.WriteLine("\nCreating camera...");
            var karen = manager.GetCamera(devicePath);
            karen.NewFrameHandler += OnNewFrame;

            Console.WriteLine("Start the decoding process...");
            karen.Play();

            Console.WriteLine("Press Space or P to play/pause the camera.");
            Console.WriteLine("Press Enter or Q or Escape to exit the program.");

            var loop = true;
            while (loop)
            {
                Console.WriteLine("\x1b[2K\rCamera is {0}", karen.IsPlaying ? "Playing" : "Paused");
                var cki = Console.ReadKey(true);
                switch (cki.Key)
                {
                    case ConsoleKey.P:
                    case ConsoleKey.Spacebar:
                        if (karen.IsPlaying)
                            karen.Pause();
                        else
                            karen.Play();
                        break;

                    case ConsoleKey.Q:
                    case ConsoleKey.Enter:
                    case ConsoleKey.Escape:
                        karen.Pause();
                        loop = false;
                        break;
                }
            }

            Console.WriteLine("\n\nDid you SeeShark? :)");
        }

        static ulong frameCount = 0;
        public static void OnNewFrame(object? _sender, FrameEventArgs e)
        {
            var frame = e.Frame;
            Console.Write($"\x1b[2K\rFrame #{frameCount} | {frame.Width}x{frame.Height} (format {frame.PixelFormat})");

            outputStream?.Seek(0, SeekOrigin.Begin);
            outputStream?.Write(frame.RawData);

            frameCount++;
        }
    }
}
