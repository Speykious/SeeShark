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
            DecodeAllFramesToImages(cameraDevice, outputFilename);
        }

        private static unsafe void DecodeAllFramesToImages(string url, string outputFilename)
        {
            using var decoder = new CameraStreamDecoder("v4l2", url, AVHWDeviceType.AV_HWDEVICE_TYPE_NONE);

            Console.WriteLine($"codec name: {decoder.CodecName}");

            var info = decoder.GetContextInfo();
            info.ToList().ForEach(x => Console.WriteLine($"{x.Key} = {x.Value}"));

            var srcPixelFormat = decoder.PixelFormat;
            var dstPixelFormat = AVPixelFormat.AV_PIX_FMT_RGB24;
            var width = decoder.FrameWidth;
            var height = decoder.FrameHeight;
            using var vfc = new FrameConverter(
                width, height, srcPixelFormat,
                width, height, dstPixelFormat
            );

            var dstData = new byte_ptrArray4();
            var dstLineSizes = new int_array4();
            int bufferSize = ffmpeg.av_image_alloc(
                ref dstData, ref dstLineSizes,
                width, height, dstPixelFormat, 1).ThrowExceptionIfError();
            
            var outputStream = File.Create(outputFilename);

            var frameNumber = 0;
            while (decoder.TryDecodeNextFrame(out var frame))
            {
                var cFrame = vfc.Convert(frame);

                var span0 = new ReadOnlySpan<byte>(cFrame.data[0], bufferSize);
                outputStream.Write(span0);

                Console.WriteLine($"frame: {frameNumber}");
                frameNumber++;
            }
        }
    }
}