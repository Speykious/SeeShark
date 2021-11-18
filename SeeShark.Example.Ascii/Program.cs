// Copyright (c) The Vignette Authors
// This file is part of SeeShark.
// SeeShark is licensed under LGPL v3. See LICENSE.LESSER.md for details.

using System;
using System.Diagnostics;
using System.Text;
using static SeeShark.FFmpeg.FFmpegManager;

namespace SeeShark.Example.Ascii
{
    class Program
    {
        static Camera? karen;

        static FrameConverter? converter;
        static void Main(string[] args)
        {
            Console.CancelKeyPress += (object? _sender, ConsoleCancelEventArgs e) =>
            {
                Console.Error.WriteLine("\n\n");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine("Oof :(");
                Console.ResetColor();
                karen?.StopCapture();
                karen?.Dispose();
                converter?.Dispose();
            };

            Console.WriteLine($"Current directory: {Environment.CurrentDirectory}");
            Console.WriteLine("Running in {0}-bit mode.", Environment.Is64BitProcess ? "64" : "32");
            Console.WriteLine($"FFmpeg version info: {FFmpegVersion}");

            var manager = new CameraManager();

            string devicePath;
            if (args.Length < 1)
            {
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

            karen = manager.GetCamera(devicePath);
            karen.NewFrameHandler += OnNewFrame;

            Console.WriteLine($"Camera chosen: {karen.Info}");
            Console.WriteLine("Press Space or P to play/pause the camera.");
            Console.WriteLine("Press Enter or Q or Escape to exit the program.");

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
                        break;

                    case ConsoleKey.Q:
                    case ConsoleKey.Enter:
                    case ConsoleKey.Escape:
                        loop = false;
                        break;
                }
            }

            karen.StopCapture();
            karen.Dispose();
            converter?.Dispose();
            Console.WriteLine("\n\nDid you SeeShark? :)");
        }

        static long frameCount = 0;
        public static void OnNewFrame(object? _sender, FrameEventArgs e)
        {
            var frame = e.Frame;
            if (converter == null || Console.WindowWidth != converter.SrcWidth ||
                Console.WindowHeight != converter.SrcHeight)
            {
                converter?.Dispose();
                converter = new FrameConverter(frame, Console.WindowWidth, Console.WindowHeight, PixelFormat.Gray8);
            }
            else if (e.Status != FFmpeg.DecodeStatus.NewFrame)
            {
                return;
            }

            Frame cFrame = converter.Convert(frame);
            DrawAsciiFrame(cFrame);
            DisplayTitle(frameCount, cFrame);

            frameCount++;
        }

        static readonly StringBuilder builder = new StringBuilder();
        static readonly char[] asciiPixels = " `'.,-~:;<>\"^=+*!?|\\/(){}[]#&$@".ToCharArray();
        public static void DrawAsciiFrame(Frame frame)
        {
            builder.Clear();
            Console.SetCursorPosition(0, 0);
            int length = frame.Width * frame.Height;
            for (int i = 0; i < length; i++)
                builder.Append(asciiPixels[RangeMap(frame.RawData[i], 0, 255, 0, asciiPixels.Length - 1)]);

            Console.Write(builder.ToString());
            Console.Out.Flush();
        }

        static readonly Stopwatch watch = new Stopwatch();
        public static void DisplayTitle(long frameCount, Frame outputFrame)
        {
            if (frameCount == 0)
            {
                watch.Start();
            }
            else if (frameCount % 10 == 0)
            {
                var fps = 1000f / watch.ElapsedMilliseconds;
                Console.Title = $"{outputFrame.Width}x{outputFrame.Height}@{fps:#.##}fps";
                watch.Restart();
            }
        }

        public static int RangeMap(int x, int in_min, int in_max, int out_min, int out_max)
        => (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
    }
}
