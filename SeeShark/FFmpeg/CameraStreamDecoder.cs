// Copyright (c) The Vignette Authors
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 3-Clause License. See LICENSE for details.

using FFmpeg.AutoGen;

namespace SeeShark.FFmpeg
{
    /// <summary>
    /// Decodes a camera stream.
    /// </summary>
    /// <remarks>It is actually just a <see cref="VideoStreamDecoder"/> with a specific input format in disguise.</remarks>
    public sealed unsafe class CameraStreamDecoder : VideoStreamDecoder
    {
        public CameraStreamDecoder(string url, DeviceInputFormat inputFormat)
        : base(url, ffmpeg.av_find_input_format(inputFormat.ToString()))
        {
        }
    }
}
