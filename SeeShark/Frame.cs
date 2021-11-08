// Copyright (c) The Vignette Authors
// This file is part of SeeShark.
// SeeShark is licensed under LGPL v3. See LICENSE.LESSER.md for details.

using FFmpeg.AutoGen;

namespace SeeShark
{
    public sealed unsafe class Frame : IDisposable
    {
        private readonly AVFrame* avFrame;

        public int Width => avFrame->width;
        public int Height => avFrame->height;
        public int WidthStep => avFrame->linesize[0];
        public PixelFormat PixelFormat => (PixelFormat)avFrame->format;
        public ReadOnlySpan<byte> RawData => new ReadOnlySpan<byte>(avFrame->data[0], WidthStep * Height);

        // This constructor is internal because the user of the library
        // is not supposed to deal with any actual FFmpeg type.
        internal Frame()
        {
            avFrame = ffmpeg.av_frame_alloc();
        }

        public void Dispose()
        {
            var frame = avFrame;
            ffmpeg.av_frame_free(&frame);
        }

        internal int HardwareAccelCopyTo(Frame hwFrame) => ffmpeg.av_hwframe_transfer_data(hwFrame.avFrame, avFrame, 0);

        internal void Unref() => ffmpeg.av_frame_unref(avFrame);

        internal int Receive(AVCodecContext* codecContext) => ffmpeg.avcodec_receive_frame(codecContext, avFrame);
    }
}
