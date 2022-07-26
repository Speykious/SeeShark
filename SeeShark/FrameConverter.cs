// Copyright (c) The Vignette Authors
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 3-Clause License. See LICENSE for details.

using System;
using System.Runtime.InteropServices;
using FFmpeg.AutoGen;

namespace SeeShark
{
    /// <summary>
    /// Converts a frame into another pixel format and/or resizes it.
    /// </summary>
    public sealed unsafe class FrameConverter : Disposable
    {
        private readonly IntPtr convertedFrameBufferPtr;
        private readonly SwsContext* convertContext;

        public readonly Frame DstFrame;
        public readonly int SrcWidth;
        public readonly int SrcHeight;
        public readonly int DstWidth;
        public readonly int DstHeight;
        public readonly PixelFormat SrcPixelFormat;
        public readonly PixelFormat DstPixelFormat;

        public FrameConverter(Frame frame, PixelFormat pixelFormat)
        : this(frame, frame.Width, frame.Height, pixelFormat)
        {
        }

        public FrameConverter(int width, int height,
            PixelFormat srcPixelFormat, PixelFormat dstPixelFormat)
        : this(width, height, srcPixelFormat, width, height, dstPixelFormat)
        {
        }

        public FrameConverter(Frame frame, int width, int height)
        : this(frame, width, height, frame.PixelFormat)
        {
        }

        public FrameConverter(int srcWidth, int srcHeight,
            PixelFormat srcPixelFormat, int dstWidth, int dstHeight)
        : this(srcWidth, srcHeight, srcPixelFormat, dstWidth, dstHeight, srcPixelFormat)
        {
        }

        public FrameConverter(Frame frame, int width, int height, PixelFormat pixelFormat)
        : this(frame.Width, frame.Height, frame.PixelFormat, width, height, pixelFormat)
        {
        }

        public FrameConverter(
            int srcWidth, int srcHeight, PixelFormat srcPixelFormat,
            int dstWidth, int dstHeight, PixelFormat dstPixelFormat)
        {
            if (srcWidth == 0 || srcHeight == 0 || dstWidth == 0 || dstHeight == 0)
                throw new ArgumentException("Source/Destination's Width/Height cannot be zero");

            SrcWidth = srcWidth;
            SrcHeight = srcHeight;
            DstWidth = dstWidth;
            DstHeight = dstHeight;
            SrcPixelFormat = srcPixelFormat;
            DstPixelFormat = dstPixelFormat;

            var srcPF = (AVPixelFormat)srcPixelFormat.RecycleDeprecated();
            var dstPF = (AVPixelFormat)dstPixelFormat.RecycleDeprecated();

            convertContext = ffmpeg.sws_getContext(
                srcWidth, srcHeight, srcPF,
                dstWidth, dstHeight, dstPF,
                ffmpeg.SWS_FAST_BILINEAR,
                null, null, null);

            if (convertContext == null)
                throw new ApplicationException("Could not initialize the conversion context.");

            var convertedFrameBufferSize = ffmpeg.av_image_get_buffer_size(dstPF, dstWidth, dstHeight, 1);
            convertedFrameBufferPtr = Marshal.AllocHGlobal(convertedFrameBufferSize);

            var dstData = new byte_ptrArray4();
            var dstLinesize = new int_array4();
            ffmpeg.av_image_fill_arrays(ref dstData, ref dstLinesize,
                (byte*)convertedFrameBufferPtr, dstPF, dstWidth, dstHeight, 1);

            var dstFrame = ffmpeg.av_frame_alloc();
            dstFrame->width = DstWidth;
            dstFrame->height = DstHeight;
            dstFrame->data.UpdateFrom(dstData);
            dstFrame->linesize.UpdateFrom(dstLinesize);
            dstFrame->format = (int)dstPF;
            DstFrame = new Frame(dstFrame);
        }

        public Frame Convert(Frame srcFrame)
        {
            var srcAVFrame = srcFrame.AVFrame;
            var dstAVFrame = DstFrame.AVFrame;
            ffmpeg.sws_scale(convertContext,
                srcAVFrame->data, srcAVFrame->linesize, 0, srcAVFrame->height,
                dstAVFrame->data, dstAVFrame->linesize);

            return DstFrame;
        }

        protected override void DisposeManaged()
        {
            DstFrame.Dispose();
        }

        protected override void DisposeUnmanaged()
        {
            // Constructor initialization can fail at some points,
            // so we need to null check everything.
            // See https://github.com/vignetteapp/SeeShark/issues/27

            if (convertedFrameBufferPtr != IntPtr.Zero)
                Marshal.FreeHGlobal(convertedFrameBufferPtr);

            if (convertContext != null)
                ffmpeg.sws_freeContext(convertContext);
        }
    }
}
