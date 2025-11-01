// Copyright (c) Speykious
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 2-Clause License. See LICENSE for details.

using System.Diagnostics;
using SeeShark;
using SeeShark.Camera;

Console.WriteLine("Getting available cameras");
List<CameraPath> availableCameras = CameraDevice.Available();
foreach (CameraPath availableCamera in availableCameras)
    Console.WriteLine($"- {availableCamera}");

CameraPath cameraPath = availableCameras[1];

Console.WriteLine($"\nAvailable video formats for {cameraPath}:");
List<VideoFormat> availableFormats = CameraDevice.AvailableFormats(cameraPath);
foreach (VideoFormat format in availableFormats)
    Console.WriteLine($"- {format}");

Console.WriteLine($"\nOpening {cameraPath}");
CameraDevice camera = CameraDevice.Open(cameraPath, new VideoFormatOptions
{
    VideoSize = (1280, 720),
    ImageFormat = ImageFormat.MJPG,
    Framerate = new FramerateRatio
    {
        Numerator = 30,
        Denominator = 1,
    },
});

(uint width, uint height) = camera.CurrentFormat.VideoSize;
ImageFormat imageFormat = camera.CurrentFormat.ImageFormat;

ImageFormatInfo imageFormatInfo = ImageFormatInfo.FromImageFormat(imageFormat);
string inputArgs = imageFormatInfo.IsRaw
    ? $"-f rawvideo -video_size {width}x{height} -pixel_format {imageFormatInfo.FFmpegInputFormat}"
    : $"-f {imageFormatInfo.FFmpegInputFormat}";

ProcessStartInfo ffmpegStartInfo = new ProcessStartInfo()
{
    RedirectStandardInput = true,
    UseShellExecute = false,
    FileName = "ffmpeg",
    Arguments = $"-hide_banner {inputArgs} -i - -pix_fmt yuv420p -y camera-feed.mp4"
};

Stream rawPixelsStream;
Process? ffmpegProcess = Process.Start(ffmpegStartInfo);
if (ffmpegProcess is not null)
{
    rawPixelsStream = ffmpegProcess.StandardInput.BaseStream;
}
else
{
    Console.WriteLine("[SeeShark] Could not find ffmpeg executable, outputting raw frames to file instead");
    rawPixelsStream = File.OpenWrite($"camera-feed.{width}x{height}.{imageFormat}.raw");
}

Console.WriteLine("Start capture");
camera.StartCapture();

Console.WriteLine("Capturing frames...");
Frame frame = new Frame();
for (int i = 0; i < 120; i++)
{
    camera.ReadFrame(ref frame);
    Console.WriteLine($"Frame: {frame}");
    rawPixelsStream.Write(frame.Data, 0, frame.Data.Length);
}
Console.WriteLine("ENOUGH");

Console.WriteLine("Stop capture");
camera.StopCapture();

rawPixelsStream.Close();
if (ffmpegProcess is not null)
    ffmpegProcess.WaitForExit();
else
    rawPixelsStream.Dispose();
