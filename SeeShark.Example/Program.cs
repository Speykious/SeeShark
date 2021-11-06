using FFmpeg.AutoGen;
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
            ReadFrames(cameraDevice, outputFilename);
        }

        private static unsafe void ReadFrames(string url, string outputFilename)
        {
            using var decoder = new CameraStreamDecoder("v4l2", url, AVHWDeviceType.AV_HWDEVICE_TYPE_NONE);

            Console.WriteLine($"Codec name: {decoder.CodecName}");

            var info = decoder.GetContextInfo();
            foreach (var field in info)
                Console.WriteLine($"{field.Key} = {field.Value}");

            var srcPixelFormat = decoder.PixelFormat;
            var dstPixelFormat = AVPixelFormat.AV_PIX_FMT_RGB24;
            var width = decoder.FrameWidth;
            var height = decoder.FrameHeight;
            
            using var vfc = new FrameConverter(
                width, height, srcPixelFormat,
                width, height, dstPixelFormat
            );
            
            var outputStream = File.Create(outputFilename);

            for (int frameCount = 1; decoder.TryDecodeNextFrame(out var frame); frameCount++)
            {
                var cFrame = vfc.Convert(frame);
                var span0 = new ReadOnlySpan<byte>(cFrame.data[0], cFrame.linesize[0] * cFrame.height);
                
                // Only write one frame in the file.
                outputStream.Seek(0, SeekOrigin.Begin);
                outputStream.Write(span0);

                Console.WriteLine($"Read {frameCount} frames");
            }
        }
    }
}