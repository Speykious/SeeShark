# SeeShark

> Simple C# camera and display library.

[![Discord](https://img.shields.io/discord/871618277258960896?color=7289DA&label=%20&logo=discord&logoColor=white)](https://discord.gg/Tz96ZdKjSA) ![NuGet](https://img.shields.io/nuget/v/SeeShark)

When you SeeShark, you C#!

SeeShark is a simple cross-platform .NET library for handling camera and screen display inputs on Linux, Windows and MacOS.

Using FFmpeg, it allows you to enumerate camera devices and decode raw frames in 199 different pixel formats (because that's how powerful FFmpeg is!).

Features include:
- Zero-copy.
- Memory-safe.
- Cross platform (Tested on Windows and Linux, might work on more platforms like MacOS).
- Managing camera and display devices.
- Control framerate, resolution and input format.
- Notifies the application if devices get connected/disconnected.
- Provides synchronous (method-driven) and asynchronous (event-driven) code flow.
- Supports 206 different pixel formats.
- Conversion of a frame from a pixel format to another.
- Scaling frames.
- Access to raw pixel data.

Features **don't** include:
- Saving a frame as an image (here's a [wiki page on how to do it](https://github.com/vignetteapp/SeeShark/wiki/Saving-images) using ImageSharp).
- Recording a video stream to a video file.
- Managing audio devices.

***

## Example code

```cs
using System;
using System.Threading;
using SeeShark;
using SeeShark.FFmpeg;

namespace YourProgram
{
    // This program will display camera frames info for 10 seconds.
    class Program
    {
        static void Main(string[] args)
        {
            // Create a CameraManager to manage camera devices
            using var manager = new CameraManager();

            // Get the first camera available
            using var camera = manager.GetCamera(0);

            // Attach your callback to the camera's frame event handler
            camera.OnFrame += FrameEventHandler;

            // Start decoding frames
            camera.StartCapture();

            // The camera decodes frames asynchronously.
            Thread.Sleep(TimeSpan.FromSeconds(10));

            // Stop decoding frames
            camera.StopCapture();
        }

        // Create a callback for decoded camera frames
        public static void FrameEventHandler(object? _sender, FrameEventArgs e)
        {
            // Only care about new frames
            if (e.Status != DecodeStatus.NewFrame)
                return;

            Frame frame = e.Frame;

            // Get information and raw data from a frame
            Console.WriteLine($"New frame ({frame.Width}x{frame.Height} | {frame.PixelFormat})");
            Console.WriteLine($"Length of raw data: {frame.RawData.Length} bytes");
        }
    }
}

```

You can also look at our overcommented [`SeeShark.Example.Ascii`](./SeeShark.Example.Ascii/) program which displays your camera input with ASCII characters.

See demo of the example below.

[![ASCII output of OBS virtual camera, feat. Bad Apple!!](https://user-images.githubusercontent.com/34704796/146024429-6b3d9188-5fd9-4463-8014-b2a33071c29e.gif)](https://i.imgur.com/YnW5Nn2.gif)

***

## Contribute

You can request a feature or fix a bug by reporting an issue.

If you feel like fixing a bug or implementing a feature, you can fork this repository and make a pull request at any time!

You can also join our discord server where we talk about our different projects.
This one has a particular **#Project SeeShark** thread that can be found under the **#main** channel.

## License

This library is licensed under the BSD 3-Clause License.
See [LICENSE](LICENSE) for details.
