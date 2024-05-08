// Copyright (c) Speykious
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 2-Clause License. See LICENSE for details.

using System.Runtime.Versioning;
using SeeShark.Interop.Libc;

namespace SeeShark;

/// <summary>
/// Various video format configuration options.
/// We can indeed open a video input in different ways.
/// For example, you might want to open your camera at a different resolution or framerate.
/// </summary>
public class VideoFormatOptions
{
    /// <summary>
    /// To request a specific resolution of the video stream.
    /// </summary>
    /// <remarks>
    /// The underlying driver will change it back to a compatible resolution.
    /// </remarks>
    /// <value>(width, height)</value>
    public (uint, uint)? VideoSize { get; set; }

    /// <summary>
    /// To request the capture to start from a specific point
    /// </summary>
    /// <value>(x, y)</value>
    public (int, int)? VideoPosition { get; set; }

    /// <summary>
    /// To request a specific framerate for the video stream.
    /// </summary>
    /// <remarks>
    /// The underlying driver will change it back to a compatible framerate.
    /// </remarks>
    public FramerateRatio? Framerate { get; set; }

    [SupportedOSPlatform("Linux")]
    public V4l2InputFormat? InputFormat { get; set; }

    /// <summary>
    /// Whether or not to draw the mouse cursor in display captures.
    /// </summary>
    public bool DrawMouse { get; set; } = true;
}
