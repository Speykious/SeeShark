// Copyright (c) The Vignette Authors
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 3-Clause License. See LICENSE for details.

using System;
using FFmpeg.AutoGen;

namespace SeeShark
{
    public sealed unsafe class Frame : Disposable
    {
        internal readonly AVFrame* AVFrame;

        /// <summary>
        /// Width of the frame in pixels.
        /// </summary>
        public int Width => AVFrame->width;
        /// <summary>
        /// Height of the frame in pixels.
        /// </summary>
        public int Height => AVFrame->height;
        /// <summary>
        /// Line size of the frame in pixels - is equal to its width multiplied by its pixel stride,
        /// that which is determined by its <see cref="PixelFormat"/>.
        /// </summary>
        public int WidthStep => AVFrame->linesize[0];
        /// <summary>
        /// Pixel format of the frame.
        /// </summary>
        /// <returns></returns>
        public PixelFormat PixelFormat => (PixelFormat)AVFrame->format;
        /// <summary>
        /// Raw data of the frame in bytes.
        /// </summary>
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

        /// <summary>
        /// Copies data to a hardware accelerated frame.
        /// </summary>
        /// <remarks>The frame it copies data to must have an AVHWFramesContext attached.</remarks>
        /// <param name="hwFrame">Hardware accelerated frame.</param>
        /// <returns>0 on success, a negative AVERROR error code on failure.</returns>
        internal int HardwareAccelCopyTo(Frame hwFrame) => ffmpeg.av_hwframe_transfer_data(hwFrame.AVFrame, AVFrame, 0);

        /// <summary>
        /// Unreference all the buffers referenced by frame and reset the frame fields.
        /// </summary>
        internal void Unref() => ffmpeg.av_frame_unref(AVFrame);

        /// <summary>
        /// Return decoded output data from a decoder.
        /// </summary>
        /// <returns>
        /// 0: success, a frame was returned <br/>
        /// AVERROR(EAGAIN): output is not available in this state - user must try to send new input <br/>
        /// AVERROR_EOF: the decoder has been fully flushed, and there will be no more output frames <br/>
        /// AVERROR(EINVAL): codec not opened, or it is an encoder <br/>
        /// AVERROR_INPUT_CHANGED: current decoded frame has changed parameters with respect to first decoded frame.
        /// Applicable when flag AV_CODEC_FLAG_DROPCHANGED is set. <br/>
        /// Other negative values: legitimate decoding errors
        /// </returns>
        internal int Receive(AVCodecContext* codecContext) => ffmpeg.avcodec_receive_frame(codecContext, AVFrame);

        protected override void DisposeManaged()
        {
        }

        protected override void DisposeUnmanaged()
        {
            var frame = AVFrame;
            ffmpeg.av_frame_free(&frame);
        }
    }
}
