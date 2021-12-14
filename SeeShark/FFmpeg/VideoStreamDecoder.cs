// Copyright (c) The Vignette Authors
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 3-Clause License. See LICENSE for details.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using FFmpeg.AutoGen;
using static SeeShark.FFmpeg.FFmpegManager;

namespace SeeShark.FFmpeg
{
    /// <summary>
    /// Decodes a video stream. <br/>
    /// Based on https://github.com/Ruslan-B/FFmpeg.AutoGen/blob/master/FFmpeg.AutoGen.Example/VideoStreamDecoder.cs.
    /// </summary>
    public unsafe class VideoStreamDecoder : Disposable
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

        public VideoStreamDecoder(string url, AVInputFormat* inputFormat = null)
        {
            SetupFFmpeg();

            FormatContext = ffmpeg.avformat_alloc_context();
            FormatContext->flags = ffmpeg.AVFMT_FLAG_NONBLOCK;

            var formatContext = FormatContext;
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

        public DecodeStatus TryDecodeNextFrame(out Frame nextFrame)
        {
            int eagain = ffmpeg.AVERROR(ffmpeg.EAGAIN);
            int error;

            do
            {
                #region Read frame
                // Manually wait for a new frame instead of letting it block
                ffmpeg.av_packet_unref(Packet);
                error = ffmpeg.av_read_frame(FormatContext, Packet);

                if (error < 0)
                {
                    nextFrame = Frame;
                    GC.Collect();
                    Thread.Sleep(1);

                    return error == eagain
                        ? DecodeStatus.NoFrameAvailable
                        : DecodeStatus.EndOfStream;
                }

                error.ThrowExceptionIfError();
                #endregion

                #region Decode packet
                if (Packet->stream_index != StreamIndex)
                    throw new InvalidOperationException("Packet does not belong to the decoder's video stream");

                ffmpeg.avcodec_send_packet(CodecContext, Packet).ThrowExceptionIfError();

                Frame.Unref();
                error = Frame.Receive(CodecContext);
                #endregion
            }
            while (error == eagain);
            error.ThrowExceptionIfError();

            nextFrame = Frame;
            GC.Collect();
            return DecodeStatus.NewFrame;
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

        protected override void DisposeManaged()
        {
            Frame.Dispose();
        }

        protected override void FreeUnmanaged()
        {
            ffmpeg.avcodec_close(CodecContext);

            var formatContext = FormatContext;
            ffmpeg.avformat_close_input(&formatContext);

            var packet = Packet;
            ffmpeg.av_packet_free(&packet);
        }
    }
}
