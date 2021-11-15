// Copyright (c) The Vignette Authors
// This file is part of SeeShark.
// SeeShark is licensed under LGPL v3. See LICENSE.LESSER.md for details.

using System.Runtime.InteropServices;
using FFmpeg.AutoGen;
using SeeShark.FFmpeg;
using static SeeShark.FFmpeg.FFmpegManager;

namespace SeeShark.FFmpeg
{
    /// <summary>
    /// Decodes a video stream. <br/>
    /// Based on https://github.com/Ruslan-B/FFmpeg.AutoGen/blob/master/FFmpeg.AutoGen.Example/VideoStreamDecoder.cs.
    /// </summary>
    public unsafe class VideoStreamDecoder : IDisposable
    {
        protected readonly AVCodecContext* CodecContext;
        protected readonly AVFormatContext* FormatContext;
        protected readonly Frame Frame;
        protected readonly AVPacket* Packet;
        protected readonly int StreamIndex;

        public readonly string CodecName;
        public readonly int FrameWidth;
        public readonly int FrameHeight;
        public readonly PixelFormat PixelFormat;

        public bool IsDisposed { get; private set; }

        public VideoStreamDecoder(string url, AVInputFormat* inputFormat = null)
        {
            SetupFFmpeg();

            this.FormatContext = ffmpeg.avformat_alloc_context();
            var formatContext = this.FormatContext;
            ffmpeg.avformat_open_input(&formatContext, url, inputFormat, null).ThrowExceptionIfError();
            ffmpeg.avformat_find_stream_info(formatContext, null).ThrowExceptionIfError();

            AVCodec* codec = null;
            StreamIndex = ffmpeg
                .av_find_best_stream(formatContext, AVMediaType.AVMEDIA_TYPE_VIDEO, -1, -1, &codec, 0)
                .ThrowExceptionIfError();
            CodecContext = ffmpeg.avcodec_alloc_context3(codec);

            ffmpeg.avcodec_parameters_to_context(CodecContext, formatContext->streams[StreamIndex]->codecpar)
                .ThrowExceptionIfError();
            ffmpeg.avcodec_open2(CodecContext, codec, null).ThrowExceptionIfError();

            CodecName = ffmpeg.avcodec_get_name(codec->id);
            FrameWidth = CodecContext->width;
            FrameHeight = CodecContext->height;
            PixelFormat = (PixelFormat)CodecContext->pix_fmt;

            Packet = ffmpeg.av_packet_alloc();
            Frame = new Frame();
        }

        public bool TryDecodeNextFrame(out Frame nextFrame)
        {
            Frame.Unref();
            int error;

            do
            {
                try
                {
                    do
                    {
                        ffmpeg.av_packet_unref(Packet);
                        error = ffmpeg.av_read_frame(FormatContext, Packet);

                        if (error == ffmpeg.AVERROR_EOF)
                        {
                            nextFrame = Frame;
                            GC.Collect();
                            return false;
                        }

                        error.ThrowExceptionIfError();
                    } while (Packet->stream_index != StreamIndex);

                    ffmpeg.avcodec_send_packet(CodecContext, Packet).ThrowExceptionIfError();
                }
                finally
                {
                    ffmpeg.av_packet_unref(Packet);
                }

                error = Frame.Receive(CodecContext);
            } while (error == ffmpeg.AVERROR(ffmpeg.EAGAIN));

            error.ThrowExceptionIfError();

            nextFrame = Frame;
            GC.Collect();
            return true;
        }

        public IReadOnlyDictionary<string, string> GetContextInfo()
        {
            AVDictionaryEntry* tag = null;
            var result = new Dictionary<string, string>();

            while ((tag = ffmpeg.av_dict_get(FormatContext->metadata, "", tag, ffmpeg.AV_DICT_IGNORE_SUFFIX)) != null)
            {
                var key = Marshal.PtrToStringAnsi((IntPtr)tag->key);
                var value = Marshal.PtrToStringAnsi((IntPtr)tag->value);

                if (key != null && value != null)
                    result.Add(key, value);
            }

            return result;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed)
                return;

            if (disposing)
            {
                // Dispose managed resources
                Frame.Dispose();
            }

            ffmpeg.avcodec_close(CodecContext);

            var formatContext = FormatContext;
            ffmpeg.avformat_close_input(&formatContext);

            var packet = Packet;
            ffmpeg.av_packet_free(&packet);

            IsDisposed = true;
        }

        ~VideoStreamDecoder()
        {
            Dispose(false);
        }
    }
}
