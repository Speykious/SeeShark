// Copyright (c) The Vignette Authors
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 3-Clause License. See LICENSE for details.

using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using SeeShark.Decode;
using SeeShark.Device;
using static SeeShark.FFmpeg.FFmpegManager;

namespace SeeShark.Example.Ascii
{
    class Program
    {
        static Camera? karen;
        static CameraManager? manager;
        static FrameConverter? converter;

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
                FFmpeg.FFmpegLogLevel.Info,
                ConsoleColor.Yellow,
                AppDomain.CurrentDomain.BaseDirectory,
                "/usr/lib",
                "/usr/lib64"
            );

            Console.WriteLine($"Current directory: {Environment.CurrentDirectory}");
            Console.WriteLine("Running in {0}-bit mode.", Environment.Is64BitProcess ? "64" : "32");
            Console.WriteLine($"FFmpeg version info: {FFmpegVersion}");

            manager = new CameraManager();

            CameraInfo device;
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
                        device = manager.Devices[index];
                        break;
                    }
                }
            }
            else
            {
                device = manager.Devices.First((ci) => ci.Path == args[0]);
            }

            /// Select video input options for the given device path.
            VideoInputOptions? vios = null;
            if (device.AvailableVideoInputOptions != null)
            {
                while (true)
                {
                    Console.WriteLine("\nVideo input options available:");
                    for (int i = 0; i < device.AvailableVideoInputOptions.Length; i++)
                        Console.WriteLine($"| #{i}: {device.AvailableVideoInputOptions[i]}");

                    Console.Write("\nChoose an input option by index: ");
                    Console.Out.Flush();
                    if (int.TryParse(Console.ReadLine(), out int index) && index < device.AvailableVideoInputOptions.Length && index >= 0)
                    {
                        vios = device.AvailableVideoInputOptions[index];
                        break;
                    }
                }
            }

            /// You can get a <see cref="Camera"/> from either a string
            /// representing the device path, or a <see cref="CameraInfo">.

            // Unfortunately, she saw the manager
            karen = manager.GetDevice(device, vios);

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

        /// <summary>
        /// Our custom frame event callback.
        /// Each time it is triggered, it will draw a new ASCII frame on the screen
        /// and update the terminal window title.
        /// </summary>
        public static void OnFrameEventHandler(object? _sender, FrameEventArgs e)
        {
            // Don't redraw the frame if it's not new, unless it's resized.
            if (e.Status != DecodeStatus.NewFrame)
                return;

            var frame = e.Frame;
            if (converter == null || Console.WindowWidth != converter.SrcWidth ||
                Console.WindowHeight != converter.SrcHeight)
            {
                // We can't just override the FrameConverter's DstWidth and DstHeight, due to how FFmpeg works.
                // We have to dispose the previous one and instanciate a new one with the new window size.
                converter?.Dispose();
                converter = new FrameConverter(frame, Console.WindowWidth, Console.WindowHeight, PixelFormat.Gray8);
            }

            // Resize the frame to the size of the terminal window, then draw it in ASCII.
            Frame cFrame = converter.Convert(frame);
            DrawAsciiFrame(cFrame);
            DisplayTitle(frameCount, cFrame.Width, cFrame.Height);

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
            converter?.Dispose();
        }

        static readonly StringBuilder builder = new StringBuilder();
        static readonly char[] asciiPixels = " `'.,-~:;<>\"^=+*!?|\\/(){}[]#&$@".ToCharArray();

        /// <summary>
        /// Draw a frame in ASCII art.
        /// </summary>
        /// <remarks>
        /// In this particular example we know that the frame has the Gray8 pixel format
        /// and that it has been resized to have the exact size of the terminal window.
        /// </remarks>
        /// <param name="frame">Frame containing raw Gray8 pixel data.</param>
        public static void DrawAsciiFrame(Frame frame)
        {
            // We don't call Console.Clear() here because it actually adds stutter.
            // Go ahead and try this example in Alacritty to see how smooth it is!
            builder.Clear();
            Console.SetCursorPosition(0, 0);
            int length = frame.Width * frame.Height;

            // Since we know that the frame has the exact size of the terminal window,
            // we have no need to add any newline characters. Thus we can just go through
            // the entire byte array to build the ASCII converted string.
            for (int i = 0; i < length; i++)
                builder.Append(asciiPixels[RangeMap(frame.RawData[i], 0, 255, 0, asciiPixels.Length - 1)]);

            Console.Write(builder.ToString());
            Console.Out.Flush();
        }

        /// <summary>
        /// Stopwatch used to measure the FPS.
        /// </summary>
        static readonly Stopwatch watch = new Stopwatch();

        /// <summary>
        /// Updates the title of the terminal window to display width, height and FPS information.
        /// </summary>
        /// <param name="frameCount">Number of frames decoded so far.</param>
        /// <param name="width">Current terminal window width.</param>
        /// <param name="height">Current terminal window height.</param>
        public static void DisplayTitle(long frameCount, int width, int height)
        {
            if (frameCount == 0)
            {
                watch.Start();
            }
            else if (frameCount % 10 == 0)
            {
                var fps = 1000f / watch.ElapsedMilliseconds;
                Console.Title = $"{width}x{height}@{fps:#.##}fps";
                watch.Restart();
            }
        }

        public static int RangeMap(int x, int in_min, int in_max, int out_min, int out_max)
        => (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
    }
}
