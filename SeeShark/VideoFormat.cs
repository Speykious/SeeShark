// Copyright (c) Speykious
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 2-Clause License. See LICENSE for details.

namespace SeeShark;

/// <summary>
/// Various video input configuration options.
/// We can indeed open a video input in different ways.
/// For example, you might want to open your camera at a different resolution or framerate.
/// </summary>
public readonly struct VideoFormat
{
    /// <summary>
    /// The resolution of the video stream.
    /// </summary>
    /// <value><c>(width, height)</c></value>
    public (uint, uint) VideoSize { get; init; }

    /// <summary>
    /// Starting point of the capture. It's always <c>(0, 0)</c> for cameras.
    /// </summary>
    /// <value><c>(x, y)</c></value>
    public (int, int) VideoPosition { get; init; }

    /// <summary>
    /// The Frames Per Second of the video stream.
    /// </summary>
    public FramerateRatio Framerate { get; init; }

    /// <summary>
    /// The image format of the video stream.
    /// Can be a raw pixel format like <c>ARGB</c> of <c>YUV422</c>, or a compressed image format like <c>MJPEG</c> for instance.
    /// </summary>
    public ImageFormat ImageFormat { get; init; }

    /// <summary>
    /// Whether or not to draw the mouse cursor in display captures.
    /// </summary>
    public bool DrawMouse { get; init; }

    public override string ToString() => $"size{VideoSize} pos{VideoPosition} {ImageFormat} (mouse={DrawMouse}) | {Framerate}";
}
