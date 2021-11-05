using System.Drawing;
using System.Runtime.InteropServices;
using FFmpeg.AutoGen;

namespace SeeShark
{
    /// <summary>
    /// Decodes a camera stream. <br/>
    /// Based on https://github.com/Ruslan-B/FFmpeg.AutoGen/blob/master/FFmpeg.AutoGen.Example/VideoStreamDecoder.cs.
    /// </summary>
    public sealed unsafe class CameraStreamDecoder : IDisposable
    {
        private readonly AVCodecContext* pCodecContext;
        private readonly AVFormatContext* pFormatContext;
        private readonly AVFrame* pFrame;
        private readonly AVPacket* pPacket;
        private readonly AVFrame* receivedFrame;
        private readonly AVInputFormat* inputFormat;
        private readonly int streamIndex;

        public CameraStreamDecoder(string formatShortName, string url, AVHWDeviceType HWDeviceType = AVHWDeviceType.AV_HWDEVICE_TYPE_NONE)
        {
            ffmpeg.avdevice_register_all();
            inputFormat = ffmpeg.av_find_input_format(formatShortName);
            this.pFormatContext = ffmpeg.avformat_alloc_context();
            receivedFrame = ffmpeg.av_frame_alloc();
            var pFormatContext = this.pFormatContext;
            ffmpeg.avformat_open_input(&pFormatContext, url, inputFormat, null).ThrowExceptionIfError();
            ffmpeg.avformat_find_stream_info(pFormatContext, null).ThrowExceptionIfError();
            AVCodec* codec = null;
            streamIndex = ffmpeg
                .av_find_best_stream(pFormatContext, AVMediaType.AVMEDIA_TYPE_VIDEO, -1, -1, &codec, 0)
                .ThrowExceptionIfError();
            pCodecContext = ffmpeg.avcodec_alloc_context3(codec);
            if (HWDeviceType != AVHWDeviceType.AV_HWDEVICE_TYPE_NONE)
                ffmpeg.av_hwdevice_ctx_create(&pCodecContext->hw_device_ctx, HWDeviceType, null, null, 0)
                    .ThrowExceptionIfError();
            ffmpeg.avcodec_parameters_to_context(pCodecContext, pFormatContext->streams[streamIndex]->codecpar)
                .ThrowExceptionIfError();
            ffmpeg.avcodec_open2(pCodecContext, codec, null).ThrowExceptionIfError();

            CodecName = ffmpeg.avcodec_get_name(codec->id);
            FrameSize = new Size(pCodecContext->width, pCodecContext->height);
            PixelFormat = pCodecContext->pix_fmt;

            pPacket = ffmpeg.av_packet_alloc();
            pFrame = ffmpeg.av_frame_alloc();
        }

        public string CodecName { get; }
        public Size FrameSize { get; }
        public AVPixelFormat PixelFormat { get; }

        public void Dispose()
        {
            var pFrame = this.pFrame;
            ffmpeg.av_frame_free(&pFrame);

            var pPacket = this.pPacket;
            ffmpeg.av_packet_free(&pPacket);

            ffmpeg.avcodec_close(pCodecContext);
            var pFormatContext = this.pFormatContext;
            ffmpeg.avformat_close_input(&pFormatContext);
        }

        public bool TryDecodeNextFrame(out AVFrame frame)
        {
            ffmpeg.av_frame_unref(pFrame);
            ffmpeg.av_frame_unref(receivedFrame);
            int error;

            do
            {
                try
                {
                    do
                    {
                        ffmpeg.av_packet_unref(pPacket);
                        error = ffmpeg.av_read_frame(pFormatContext, pPacket);

                        if (error == ffmpeg.AVERROR_EOF)
                        {
                            frame = *pFrame;
                            return false;
                        }

                        error.ThrowExceptionIfError();
                    } while (pPacket->stream_index != streamIndex);

                    ffmpeg.avcodec_send_packet(pCodecContext, pPacket).ThrowExceptionIfError();
                }
                finally
                {
                    ffmpeg.av_packet_unref(pPacket);
                }

                error = ffmpeg.avcodec_receive_frame(pCodecContext, pFrame);
            } while (error == ffmpeg.AVERROR(ffmpeg.EAGAIN));

            error.ThrowExceptionIfError();

            if (pCodecContext->hw_device_ctx != null)
            {
                ffmpeg.av_hwframe_transfer_data(receivedFrame, pFrame, 0).ThrowExceptionIfError();
                frame = *receivedFrame;
            }
            else
                frame = *pFrame;

            return true;
        }

        public IReadOnlyDictionary<string, string> GetContextInfo()
        {
            AVDictionaryEntry* tag = null;
            var result = new Dictionary<string, string>();

            while ((tag = ffmpeg.av_dict_get(pFormatContext->metadata, "", tag, ffmpeg.AV_DICT_IGNORE_SUFFIX)) != null)
            {
                var key = Marshal.PtrToStringAnsi((IntPtr)tag->key);
                var value = Marshal.PtrToStringAnsi((IntPtr)tag->value);

                if (key != null && value != null)
                    result.Add(key, value);
            }

            return result;
        }
    }
}