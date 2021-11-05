using System.Runtime.InteropServices;
using FFmpeg.AutoGen;

namespace SeeShark
{
    public sealed unsafe class FrameConverter : IDisposable
    {
        private readonly IntPtr convertedFrameBufferPtr;
        private readonly int dstWidth;
        private readonly int dstHeight;
        private readonly byte_ptrArray4 dstData;
        private readonly int_array4 dstLinesize;
        private readonly SwsContext* pConvertContext;

        public FrameConverter(int srcWidth, int srcHeight, AVPixelFormat srcPixelFormat,
            int dstWidth, int dstHeight, AVPixelFormat dstPixelFormat)
        {
            this.dstWidth = dstWidth;
            this.dstHeight = dstHeight;

            pConvertContext = ffmpeg.sws_getContext(
                srcWidth,
                srcHeight,
                srcPixelFormat,
                dstWidth,
                dstHeight,
                dstPixelFormat,
                ffmpeg.SWS_FAST_BILINEAR,
                null,
                null,
                null);
            if (pConvertContext == null)
                throw new ApplicationException("Could not initialize the conversion context.");

            var convertedFrameBufferSize = ffmpeg.av_image_get_buffer_size(dstPixelFormat,
                dstWidth,
                dstHeight,
                1);
            convertedFrameBufferPtr = Marshal.AllocHGlobal(convertedFrameBufferSize);
            dstData = new byte_ptrArray4();
            dstLinesize = new int_array4();

            ffmpeg.av_image_fill_arrays(ref dstData,
                ref dstLinesize,
                (byte*) convertedFrameBufferPtr,
                dstPixelFormat,
                dstWidth,
                dstHeight,
                1);
        }

        public void Dispose()
        {
            Marshal.FreeHGlobal(convertedFrameBufferPtr);
            ffmpeg.sws_freeContext(pConvertContext);
        }

        public AVFrame Convert(AVFrame sourceFrame)
        {
            ffmpeg.sws_scale(pConvertContext,
                sourceFrame.data,
                sourceFrame.linesize,
                0,
                sourceFrame.height,
                dstData,
                dstLinesize);

            var data = new byte_ptrArray8();
            data.UpdateFrom(dstData);
            var linesize = new int_array8();
            linesize.UpdateFrom(dstLinesize);

            return new AVFrame
            {
                data = data,
                linesize = linesize,
                width = dstWidth,
                height = dstHeight
            };
        }
    }
}