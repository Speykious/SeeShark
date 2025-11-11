# SeeShark

> Simple C# camera and display library.

***

> [!IMPORTANT]
> SeeShark is currently being [rewritten from scratch](https://github.com/Speykious/SeeShark/pull/51) without using FFmpeg.
> It will become a library with zero third-party dependencies and rely solely on ones readily available on your OS.

> [!NOTE]
> SeeShark now has a [Discord server](https://discord.gg/Ev4ezRuq74) so that I can provide more direct support to the best of my availability.

***

When you SeeShark, you C#!

SeeShark is a simple cross-platform .NET library for handling camera and screen display inputs on Linux and Windows.

Using FFmpeg, it allows you to enumerate camera and display devices, and decode raw frames in 206 different pixel formats (because that's how powerful FFmpeg is!).

Features include:
- Control framerate, resolution and input format.
- Notify the application if devices get connected/disconnected.
- Conversion of a frame from a pixel format to another.
- Scaling frames.
- Access to raw pixel data.

Features **don't** include:
- Saving a frame as an image (here's a [wiki page on how to do it](https://github.com/Speykious/SeeShark/wiki/Saving-images) using ImageSharp).
- Recording a video stream to a video file.
- Managing audio devices.

## Cross-platform support

SeeShark has been confirmed to work on Linux and Windows.

Unfortunately, it doesn't work on MacOS. Long story short, FFmpeg 5 doesn't implement device enumeration for MacOS, and SeeShark doesn't implement a custom function to do that.

## FFmpeg

SeeShark 4 depends on FFmpeg 5. You can get binaries here:
- Windows: [ffmpeg 5.1.2 builds](https://github.com/GyanD/codexffmpeg/releases/tag/5.1.2) (`ffmpeg-5.1.2-full_build-shared.zip`)
- Linux: *last time I tried, it was a disaster and I had to download several dependencies of `libav*` myself. Let me know if you can find an easy solution.*

The important elements are the `libav*` DLLs/shared libraries:
|          | Windows           | Linux               |
| -------- | ----------------- | ------------------- |
| avcodec  | `avcodec-59.dll`  | `libavcodec.so.59`  |
| avdevice | `avdevice-59.dll` | `libavdevice.so.59` |
| avformat | `avformat-59.dll` | `libavformat.so.59` |
| swscale  | `swscale-6.dll`   | `libswscale.so.6`   |

You can ignore executables like `ffmpeg` or `ffplay` as they are not used by SeeShark.

If you get an error message about a module's dependency potentially missing, you might need to specify a `LD_LIBRARY_PATH` environment variable:
```sh
LD_LIBRARY_PATH=/path/to/libs:$LD_LIBRARY_PATH
```

***

## Example code

```cs
using System;
using System.Threading;
using SeeShark;
using SeeShark.FFmpeg;
using SeeShark.Device;

namespace YourProgram;

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
        camera.OnFrame += frameEventHandler;

        // Start decoding frames asynchronously
        camera.StartCapture();

        // Just wait a bit
        Thread.Sleep(TimeSpan.FromSeconds(10));

        // Stop decoding frames
        camera.StopCapture();
    }

    // Create a callback for decoded camera frames
    private static void frameEventHandler(object? _sender, FrameEventArgs e)
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
```

You can also look at our overcommented [`SeeShark.Example.Ascii`](./SeeShark.Example.Ascii/) program which displays your camera input with ASCII characters.

See demo of the example below.

[![ASCII output of OBS virtual camera, feat. Bad Apple!!](https://user-images.githubusercontent.com/34704796/146024429-6b3d9188-5fd9-4463-8014-b2a33071c29e.gif)](https://i.imgur.com/YnW5Nn2.gif)

***

## Contribute

You can request a feature or fix a bug by reporting an issue.

If you feel like fixing a bug or implementing a feature, you can fork this repository and make a pull request at any time!

## Vignette

This library was previously hosted on https://github.com/vignetteapp/SeeShark. It was first made to be used in Vignette's vtuber application. Now, it is its own self-contained library!

## License

This library is licensed under the BSD 3-Clause License.
See [LICENSE](LICENSE) for details.
