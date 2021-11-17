// Copyright (c) The Vignette Authors
// This file is part of SeeShark.
// SeeShark is licensed under LGPL v3. See LICENSE.LESSER.md for details.

using System;
using System.Diagnostics;
using System.Text;
using static SeeShark.FFmpeg.FFmpegManager;

namespace SeeShark.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.CancelKeyPress += (object? _sender, ConsoleCancelEventArgs e) =>
            {
                Console.Error.WriteLine("\n\n");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine("Oof :(");
                Console.ResetColor();
            };

            if (args.Length < 1)
            {
                Console.WriteLine("Usage: dotnet run <camera-device>");
                return;
            }

            var devicePath = args[0];

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
                        karen.Dispose();
                        loop = false;
                        converter?.Dispose();
                        break;
                }
            }

            Console.WriteLine("\n\nDid you SeeShark? :)");
        }

        static uint frameCount = 0;
        private static FrameConverter? converter;
        private static Stopwatch watch = new Stopwatch();
        private static StringBuilder builder = new StringBuilder();
        public static void OnNewFrame(object? _sender, FrameEventArgs e)
        {
            var frame = e.Frame;
            if (converter == null || Console.WindowWidth != converter.SrcWidth ||
                Console.WindowHeight != converter.SrcHeight)
            {
                converter?.Dispose();
                converter = new FrameConverter(frame.Width, frame.Height, frame.PixelFormat,
                    Console.WindowWidth, Console.WindowHeight, PixelFormat.Gray8);
            }

            Frame outputFrame = converter.Convert(frame);

            char[] chars = "`'.,-~:;\"^=+*rcvuoeasnmwzxiygjlfthkqpdb!?ILOAEBCDFGHJKMNPRSTUVYZWQX(){}[]|\\/&$@#"
                .ToCharArray();

            Console.SetCursorPosition(0, 0);
            for (int y = 0; y < outputFrame.Height; y++)
            {
                for (int x = 0; x < outputFrame.Width; x++)
                {
                    builder.Append(chars[map(outputFrame.RawData[y * outputFrame.Width + x], 0, 255, 0, chars.Length - 1)]);
                }
            }
            Console.Write(builder.ToString());
            Console.Out.Flush();
            builder.Clear();

            if (frameCount == 10)
            {
                Console.Title = "FPS: " + frameCount / (watch.ElapsedMilliseconds / 1000f);
                frameCount = 0;
                watch.Restart();
            }
            else if (frameCount == 0)
            {
                watch.Start();
            }
            frameCount++;
        }

        static int map(int x, int in_min, int in_max, int out_min, int out_max) {
            return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
        }
    }
}
