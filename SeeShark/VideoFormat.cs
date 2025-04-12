// Copyright (c) Speykious
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 2-Clause License. See LICENSE for details.

using System;
using System.Runtime.Versioning;
using SeeShark.Interop.Libc;

namespace SeeShark;

/// <summary>
/// Various video input configuration options.
/// We can indeed open a video input in different ways.
/// For example, you might want to open your camera at a different resolution or framerate.
/// </summary>
public struct VideoFormat
{
    /// <summary>
    /// The resolution of the video stream.
    /// </summary>
    /// <value>(width, height)</value>
    public (uint, uint) VideoSize { get; init; }

    /// <summary>
    /// Starting point of the capture. It's always (0, 0) for cameras.
    /// </summary>
    /// <value>(x, y)</value>
    public (int, int) VideoPosition { get; init; }

    /// <summary>
    /// To request a specific framerate for the video stream.
    /// </summary>
    /// <remarks>
    /// The underlying driver will change it back to a compatible framerate.
    /// </remarks>
    public FramerateRatio Framerate { get; init; }

    [SupportedOSPlatform("Linux")]
    internal V4l2InputFormat InputFormat { get; init; }

    /// <summary>
    /// Whether or not to draw the mouse cursor in display captures.
    /// </summary>
    public bool DrawMouse { get; init; }

    public override string ToString()
    {
        if (OperatingSystem.IsLinux())
            return $"size{VideoSize} pos{VideoPosition} {InputFormat} (mouse={DrawMouse}) | {Framerate}";
        else
            return $"size{VideoSize} pos{VideoPosition} (mouse={DrawMouse}) | {Framerate}";
    }
}
