# SeeShark

> Simple C# camera and display library.

When you SeeShark, you C#!

SeeShark is a simple cross-platform .NET library for handling camera and screen display inputs on Linux, Windows and MacOS.

⚠️ **Rewrite In Progress for v5.0** ⚠️

Features include:
- [ ] Cross platform
  - [x] Linux (V4l2)
  - [ ] Linux (X11)
  - [ ] Windows (DirectShow)
  - [ ] Windows (Gdi)
  - [x] MacOS (AVFoundation)
- [x] Managing camera devices
- [ ] Managing display devices
- [x] Control framerate, resolution and input format
- [ ] Notifies the application if devices get connected/disconnected
- [x] Provides synchronous (method-driven) control flow
- [ ] Provides asynchronous (event-driven) control flow
- [ ] Supports (???) different pixel formats
- [x] Access to raw pixel data

Features **don't** include:
- Saving a frame as an image (here's a [wiki page on how to do it](https://github.com/Speykious/SeeShark/wiki/Saving-images) using ImageSharp)
- Recording a video stream to a video file
- Managing audio devices

## Example code

> (Coming Soon™.)

## Vignette

This library was previously hosted on https://github.com/vignetteapp/SeeShark. It was first made to be used in Vignette's vtuber application. Now, it is its own self-contained library!

## License

This library is licensed under the BSD 3-Clause License.
See [LICENSE](LICENSE) for details.
