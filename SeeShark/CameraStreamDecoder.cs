using System.Runtime.InteropServices;
using FFmpeg.AutoGen;
using static SeeShark.FFmpeg.FFmpegManager;

namespace SeeShark
{
    /// <summary>
    /// Decodes a camera stream. <br/>
    /// Based on https://github.com/Ruslan-B/FFmpeg.AutoGen/blob/master/FFmpeg.AutoGen.Example/VideoStreamDecoder.cs.
    /// </summary>
    public sealed unsafe class CameraStreamDecoder : IDisposable
    {
        private readonly AVCodecContext* codecContext;
        private readonly AVFormatContext* formatContext;
        private readonly AVFrame* frame;
        private readonly AVFrame* hwFrame;
        private readonly AVPacket* packet;
        private readonly int streamIndex;

        public readonly string CodecName;
        public readonly int FrameWidth;
        public readonly int FrameHeight;
        public readonly PixelFormat PixelFormat;

        public CameraStreamDecoder(string formatShortName, string url, HardwareAccelDevice hwAccelDevice)
        {
            SetupFFmpeg();

            var inputFormat = ffmpeg.av_find_input_format(formatShortName);
            this.formatContext = ffmpeg.avformat_alloc_context();
            var formatContext = this.formatContext;
            ffmpeg.avformat_open_input(&formatContext, url, inputFormat, null).ThrowExceptionIfError();
            ffmpeg.avformat_find_stream_info(formatContext, null).ThrowExceptionIfError();
            AVCodec* codec = null;
            streamIndex = ffmpeg
                .av_find_best_stream(formatContext, AVMediaType.AVMEDIA_TYPE_VIDEO, -1, -1, &codec, 0)
                .ThrowExceptionIfError();
            codecContext = ffmpeg.avcodec_alloc_context3(codec);
            if (hwAccelDevice != HardwareAccelDevice.None)
                ffmpeg.av_hwdevice_ctx_create(&codecContext->hw_device_ctx, (AVHWDeviceType)hwAccelDevice, null, null, 0)
                    .ThrowExceptionIfError();
            ffmpeg.avcodec_parameters_to_context(codecContext, formatContext->streams[streamIndex]->codecpar)
                .ThrowExceptionIfError();
            ffmpeg.avcodec_open2(codecContext, codec, null).ThrowExceptionIfError();

            CodecName = ffmpeg.avcodec_get_name(codec->id);
            FrameWidth = codecContext->width;
            FrameHeight = codecContext->height;
            PixelFormat = (PixelFormat)codecContext->pix_fmt;

            packet = ffmpeg.av_packet_alloc();
            frame = ffmpeg.av_frame_alloc();
            hwFrame = ffmpeg.av_frame_alloc();
        }

        public void Dispose()
        {
            ffmpeg.avcodec_close(codecContext);

            var formatContext = this.formatContext;
            ffmpeg.avformat_close_input(&formatContext);

            var frame = this.frame;
            ffmpeg.av_frame_free(&frame);

            var hwFrame = this.hwFrame;
            ffmpeg.av_frame_free(&hwFrame);

            var packet = this.packet;
            ffmpeg.av_packet_free(&packet);
        }

        public bool TryDecodeNextFrame(out AVFrame nextFrame)
        {
            ffmpeg.av_frame_unref(frame);
            ffmpeg.av_frame_unref(hwFrame);
            int error;

            do
            {
                try
                {
                    do
                    {
                        ffmpeg.av_packet_unref(packet);
                        error = ffmpeg.av_read_frame(formatContext, packet);

                        if (error == ffmpeg.AVERROR_EOF)
                        {
                            nextFrame = *frame;
                            return false;
                        }

                        error.ThrowExceptionIfError();
                    } while (packet->stream_index != streamIndex);

                    ffmpeg.avcodec_send_packet(codecContext, packet).ThrowExceptionIfError();
                }
                finally
                {
                    ffmpeg.av_packet_unref(packet);
                }

                error = ffmpeg.avcodec_receive_frame(codecContext, frame);
            } while (error == ffmpeg.AVERROR(ffmpeg.EAGAIN));

            error.ThrowExceptionIfError();

            if (codecContext->hw_device_ctx != null)
            {
                ffmpeg.av_hwframe_transfer_data(hwFrame, frame, 0).ThrowExceptionIfError();
                nextFrame = *hwFrame;
            }
            else
                nextFrame = *frame;

            return true;
        }

        public IReadOnlyDictionary<string, string> GetContextInfo()
        {
            AVDictionaryEntry* tag = null;
            var result = new Dictionary<string, string>();

            while ((tag = ffmpeg.av_dict_get(formatContext->metadata, "", tag, ffmpeg.AV_DICT_IGNORE_SUFFIX)) != null)
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