# SeeShark

> Simple C# camera library. First release coming soon!

[![Discord](https://img.shields.io/discord/871618277258960896?color=7289DA&label=%20&logo=discord&logoColor=white)](https://discord.gg/Tz96ZdKjSA)

When you SeeShark, you C#!

SeeShark is a simple cross-platform .NET library for handling camera inputs on Linux, Windows and MacOS.

Using FFmpeg, it allows you to enumerate camera devices and decode raw frames in 199 different pixel formats (because that's how powerful FFmpeg is!).

Features include:
- zero-copy
- memory-safe
- access to raw data
- conversion of a frame from a pixel format to another
- scaling frames
- managing camera devices

Features **don't** include:
- saving a frame to some image file format
- recording a camera stream to a video file
- manage audio devices

***

## Example code

```cs
using System;
using System.Threading;
using SeeShark;

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

![ASCII output of OBS virtual camera, feat. Bad Apple!!](https://media3.giphy.com/media/6GmAOVBsU1wVvVORRG/giphy.gif)

***

## Contribute

You can request a feature or fix a bug by reporting an issue.

If you feel like fixing a bug or implementing a feature, you can fork this repository and make a pull request at any time!

You can also join our discord server where we talk about our different projects.
This one has a particular **#Project SeeShark** thread that can be found under the **#main** channel.

## License

This library is licensed under the BSD 3-Clause License.
See [LICENSE](LICENSE) for details.
