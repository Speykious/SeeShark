// Copyright (c) The Vignette Authors
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 3-Clause License. See LICENSE for details.

using System.Collections.Generic;
using FFmpeg.AutoGen;

namespace SeeShark.Decode
{
    public class VideoInputOptions
    {
        public (int, int)? VideoSize { get; set; }
        public AVRational? Framerate { get; set; }
        public string? InputFormat { get; set; }

        public virtual IDictionary<string, string> ToAVDictOptions()
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
                dict.Add("input_format", InputFormat);

            return dict;
        }
    }
}
