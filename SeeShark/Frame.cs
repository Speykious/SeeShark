// Copyright (c) The Vignette Authors
// This file is part of SeeShark.
// SeeShark is licensed under LGPL v3. See LICENSE.LESSER.md for details.

using FFmpeg.AutoGen;

namespace SeeShark
{
    public sealed unsafe class Frame : IDisposable
    {
        internal readonly AVFrame* AVFrame;

        public int Width => AVFrame->width;
        public int Height => AVFrame->height;
        public int WidthStep => AVFrame->linesize[0];
        public PixelFormat PixelFormat => (PixelFormat)AVFrame->format;
        public ReadOnlySpan<byte> RawData => new ReadOnlySpan<byte>(AVFrame->data[0], WidthStep * Height);

        // This constructor is internal because the user of the library
        // is not supposed to deal with any actual FFmpeg type.
        internal Frame()
        {
            AVFrame = ffmpeg.av_frame_alloc();
        }

        internal Frame(AVFrame* avFrame)
        {
            AVFrame = avFrame;
        }

        public void Dispose()
        {
            var frame = AVFrame;
            ffmpeg.av_frame_free(&frame);
        }

        internal int HardwareAccelCopyTo(Frame hwFrame) => ffmpeg.av_hwframe_transfer_data(hwFrame.AVFrame, AVFrame, 0);

        internal void Unref() => ffmpeg.av_frame_unref(AVFrame);

        internal int Receive(AVCodecContext* codecContext) => ffmpeg.avcodec_receive_frame(codecContext, AVFrame);
    }
}
