# SeeShark

> Simple C# camera library.

When you SeeShark, you C#!

Currently Work In Progress.

## State of the library

the `CameraStreamDecoder` class is now operational! Now a bunch of abstractions and encapsulation needs to be done for the library to be usable at all.

A few have been made, especially some `enum` types that are going to be used outside of the library.
A very subtle memory leak has been fixed and we ca now focus on abstractions again.

## TODO

- [x] Abstract `CameraStreamDecoder` into a `VideoStreamDecoder`
- [x] Extract frame sending logic from `ICamera` into an `IVideo`
- [ ] Implement a way to enumerate camera devices
- [ ] Write a `Camera` class that implements `ICamera` so that it is used instead of `CameraStreamDecoder`

***

## License

![LGPL v3 logo - Free as in Freedom.](https://www.gnu.org/graphics/lgplv3-with-text-154x68.png)

This library is licensed under the [LGPL v3](LICENSE.LESSER.md), which is a set of additional permissions on top of the [GPL v3](LICENSE.md).
