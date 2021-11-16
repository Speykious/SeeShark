# SeeShark

> Simple C# camera library.

When you SeeShark, you C#!

Currently Work In Progress.

## State of the library

The library is quite close to a first beta release!

## TODO

- [x] Abstract `CameraStreamDecoder` into a `VideoStreamDecoder`
- [x] Extract frame sending logic from `ICamera` into an `IVideo`
- [x] Implement a way to enumerate camera devices
- [x] Write a `Camera` class that implements `ICamera`
- [x] Review implementations of `IDisposable`
- [x] Make so that `CameraManager` can provide cameras
- [x] Implement platform-dependant helpers
- [x] Use `Camera` instead of `CameraStreamDecoder` in `SeeShark.Example`
- [ ] Make an ASCII art example *(yes, I will)*
- [ ] Document usage of the library

***

## License

![LGPL v3 logo - Free as in Freedom.](https://www.gnu.org/graphics/lgplv3-with-text-154x68.png)

This library is licensed under the [LGPL v3](LICENSE.LESSER.md), which is a set of additional permissions on top of the [GPL v3](LICENSE.md).
