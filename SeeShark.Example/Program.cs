// Copyright (c) The Vignette Authors
// This file is part of SeeShark.
// SeeShark is licensed under LGPL v3. See LICENSE.LESSER.md for details.

using static SeeShark.FFmpeg.FFmpegManager;

namespace SeeShark.Example
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: dotnet run <camera-device> <output-file>");
                return;
            }

            var cameraDevice = args[0];
            var outputFilename = args[1];

            Console.WriteLine("Current directory: " + Environment.CurrentDirectory);
            Console.WriteLine("Running in {0}-bit mode.", Environment.Is64BitProcess ? "64" : "32");
            Console.WriteLine($"FFmpeg version info: {FFmpegVersion}");

            Console.WriteLine("Decoding...");
            readFrames(cameraDevice, outputFilename);
        }

        private static void readFrames(string url, string outputFilename)
        {
            using var dec = new CameraStreamDecoder("v4l2", url, HardwareAccelDevice.None);

            Console.WriteLine($"Codec name: {dec.CodecName}");

            var info = dec.GetContextInfo();
            foreach (var field in info)
                Console.WriteLine($"{field.Key} = {field.Value}");

            using var vfc = new FrameConverter(
                dec.FrameWidth, dec.FrameHeight,
                dec.PixelFormat, PixelFormat.Rgb24
            );

            var outputStream = File.Create(outputFilename);

            for (int frameCount = 1; dec.TryDecodeNextFrame(out var frame); frameCount++)
            {
                var cFrame = vfc.Convert(frame);

                // Only write one frame in the file.
                outputStream.Seek(0, SeekOrigin.Begin);
                outputStream.Write(cFrame.RawData);

                Console.WriteLine($"Read {frameCount} frames");
            }
        }
    }
}
