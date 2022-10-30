// Copyright (c) The Vignette Authors
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 3-Clause License. See LICENSE for details.

using System.Collections.Generic;
using FFmpeg.AutoGen;
using SeeShark.Device;

namespace SeeShark;

/// <summary>
/// Options to give to a <see cref="VideoStreamDecoder" /> for it to feed them to FFmpeg when opening a video input stream.
/// We can indeed open an input in different ways. For example, you might want to open your camera at a different resolution, or change the input format.
/// </summary>
/// <remarks>
/// Some examples of input options are:
/// <list type="bullet">
/// <item>https://ffmpeg.org/ffmpeg-devices.html#video4linux2_002c-v4l2</item>
/// <item>https://ffmpeg.org/ffmpeg-devices.html#dshow</item>
/// <item>https://ffmpeg.org/ffmpeg-devices.html#avfoundation</item>
/// </list>
/// </remarks>
public class VideoInputOptions
{
    /// <summary>
    /// To request a specific resolution of the video stream.
    /// </summary>
    /// <remarks>
    /// The underlying driver will change it back to a compatible resolution.
    /// </remarks>
    /// <value>(width, height)</value>
    public (int, int)? VideoSize { get; set; }

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
    public AVRational? Framerate { get; set; }
    /// <summary>
    /// To request a specific input format for the video stream.
    /// If the video stream is raw, it is the name of its pixel format, otherwise it is the name of its codec.
    /// </summary>
    public string? InputFormat { get; set; }
    /// <summary>
    /// Used on Windows only - tells us if the video stream is raw or not.
    /// If the video stream is raw, it is a pixel format, otherwise it is a codec.
    /// </summary>
    public bool IsRaw { get; set; }

    /// <summary>
    /// Whether or not to draw the mouse cursor in display captures.
    /// </summary>
    public bool DrawMouse { get; set; } = true;

    /// <summary>
    /// Combines all properties into a dictionary of options that FFmpeg can use.
    /// </summary>
    public virtual IDictionary<string, string> ToAVDictOptions(DeviceInputFormat deviceFormat)
    {
        Dictionary<string, string> dict = new();

        if (VideoSize != null)
        {
            (int width, int height) = VideoSize.Value;
            dict.Add("video_size", $"{width}x{height}");
        }

        if (Framerate != null)
            dict.Add("framerate", $"{Framerate.Value.num}/{Framerate.Value.den}");

        if (InputFormat != null)
        {
            string key = "input_format";
            if (deviceFormat == DeviceInputFormat.DShow)
                key = IsRaw ? "pixel_format" : "vcodec";

            // I have no idea why there is an inconsistency but here we are...
            string inputFormat = InputFormat switch
            {
                "YUYV" => "yuv422p",
                "YUV420" => "yuv420p",
                _ => InputFormat.ToLower(),
            };
            dict.Add(key, inputFormat);
        }

        if (VideoPosition != null)
        {
            switch (deviceFormat)
            {
                case DeviceInputFormat.X11Grab:
                    dict.Add("grab_x", VideoPosition.Value.Item1.ToString());
                    dict.Add("grab_y", VideoPosition.Value.Item2.ToString());
                    break;
                case DeviceInputFormat.GdiGrab:
                    dict.Add("offset_x", VideoPosition.Value.Item1.ToString());
                    dict.Add("offset_y", VideoPosition.Value.Item2.ToString());
                    break;

            }
        }

        switch (deviceFormat)
        {
            case DeviceInputFormat.X11Grab:
            case DeviceInputFormat.GdiGrab:
                dict.Add("draw_mouse", DrawMouse ? "1" : "0");
                break;
            case DeviceInputFormat.AVFoundation:
                dict.Add("capture_cursor", DrawMouse ? "1" : "0");
                break;
        }

        return dict;
    }

    public override string ToString()
    {
        string s = $"{InputFormat} {VideoSize}";
        if (Framerate != null)
        {
            double fps = ffmpeg.av_q2d(Framerate.Value);
            s += $" - {fps:0.000} fps";
        }
        return s;
    }
}
