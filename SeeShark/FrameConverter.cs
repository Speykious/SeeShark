// Copyright (c) The Vignette Authors
// This file is part of SeeShark.
// SeeShark is licensed under LGPL v3. See LICENSE.LESSER.md for details.

using System.Runtime.InteropServices;
using FFmpeg.AutoGen;

namespace SeeShark
{
    public sealed unsafe class FrameConverter : IDisposable
    {
        private readonly IntPtr convertedFrameBufferPtr;
        private readonly SwsContext* convertContext;
        private readonly AVFrame* dstFrame;

        public readonly int SrcWidth;
        public readonly int SrcHeight;
        public readonly int DstWidth;
        public readonly int DstHeight;
        public readonly PixelFormat SrcPixelFormat;
        public readonly PixelFormat DstPixelFormat;

        public FrameConverter(int width, int height,
            PixelFormat srcPixelFormat, PixelFormat dstPixelFormat)
        : this(width, height, srcPixelFormat, width, height, dstPixelFormat)
        {
        }

        public FrameConverter(
            int srcWidth, int srcHeight, PixelFormat srcPixelFormat,
            int dstWidth, int dstHeight, PixelFormat dstPixelFormat)
        {
            SrcWidth = srcWidth;
            SrcHeight = srcHeight;
            DstWidth = dstWidth;
            DstHeight = dstHeight;
            SrcPixelFormat = srcPixelFormat;
            DstPixelFormat = dstPixelFormat;

            var srcPF = (AVPixelFormat)srcPixelFormat;
            var dstPF = (AVPixelFormat)dstPixelFormat;

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

            dstFrame = ffmpeg.av_frame_alloc();
            dstFrame->width = DstWidth;
            dstFrame->height = DstHeight;
            dstFrame->data.UpdateFrom(dstData);
            dstFrame->linesize.UpdateFrom(dstLinesize);
        }

        public void Dispose()
        {
            Marshal.FreeHGlobal(convertedFrameBufferPtr);
            ffmpeg.sws_freeContext(convertContext);

            var dstFrame = this.dstFrame;
            ffmpeg.av_frame_free(&dstFrame);
        }

        public AVFrame Convert(AVFrame srcFrame)
        {
            ffmpeg.sws_scale(convertContext,
                srcFrame.data, srcFrame.linesize, 0, srcFrame.height,
                dstFrame->data, dstFrame->linesize);

            return *dstFrame;
        }
    }
}

