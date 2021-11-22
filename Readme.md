# SeeShark

> Simple C# camera library.

When you SeeShark, you C#!

Currently Work In Progress.

## State of the library

We just fixed the most significant bug of the library, where `av_read_frame()` would block if you stop your virtual camera.

The library is thus in an MVP state!

We decided not to include any image saving or video stream recording feature for now.
The reason is that there are already other libraries that use FFmpeg and that can do that, using FFmpeg or something else.
This is honestly way enough abstraction to have to get a working camera stream.

Thus we can now focus on documentation, and then later on packaging.

## TODO

- [x] Abstract `CameraStreamDecoder` into a `VideoStreamDecoder`
- [x] Extract frame sending logic from `ICamera` into an `IVideo`
- [x] Implement a way to enumerate camera devices
- [x] Write a `Camera` class that implements `ICamera`
- [x] Review implementations of `IDisposable`
- [x] Make so that `CameraManager` can provide cameras
- [x] Implement platform-dependant helpers
- [x] Use `Camera` instead of `CameraStreamDecoder` in `SeeShark.Example`
- [x] Make an ASCII art example *(yes, I will)*
- [x] Fix significant bug where `av_read_frame()` can block
- [ ] Document usage of the library

***

## License

![LGPL v3 logo - Free as in Freedom.](https://www.gnu.org/graphics/lgplv3-with-text-154x68.png)

This library is licensed under the [LGPL v3](LICENSE.LESSER.md), which is a set of additional permissions on top of the [GPL v3](LICENSE.md).
