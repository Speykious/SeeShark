// Copyright (c) The Vignette Authors
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 3-Clause License. See LICENSE for details.

using System;
using System.Threading;
using SeeShark.Decode;

namespace SeeShark.Device;

public class VideoDevice : Disposable
{
    private Thread? decodingThread;
    private readonly VideoStreamDecoder decoder;

    public VideoDeviceInfo Info { get; }
    public bool IsPlaying { get; private set; }

    public event EventHandler<FrameEventArgs>? OnFrame;
    public event EventHandler<DecodeStatus>? OnEndOfStream;

    public VideoDevice(VideoDeviceInfo info, DeviceInputFormat inputFormat, VideoInputOptions? options = null)
    {
        Info = info;
        decoder = new VideoStreamDecoder(info.Path, inputFormat, options?.ToAVDictOptions(inputFormat));
    }

    protected void DecodeLoop()
    {
        DecodeStatus status;
        while ((status = TryGetFrame(out var frame)) != DecodeStatus.EndOfStream)
        {
            OnFrame?.Invoke(this, new FrameEventArgs(frame, status));

            if (!IsPlaying)
                break;
        }

        // End of stream happened
        OnEndOfStream?.Invoke(this, status);
        IsPlaying = false;
    }

    /// <summary>
    /// Decodes the next frame from the stream.
    /// </summary>
    /// <remarks>
    /// This operation is blocking.
    /// If you want a synchronous non-blocking solution, use <see cref="TryGetFrame" />.
    /// If you want an asynchronous solution, use the <see cref="OnFrame" /> event instead
    /// and toggle capture with <see cref="StartCapture" /> and <see cref="StopCapture" />.
    /// </remarks>
    /// <returns>The decoded frame.</returns>
    public Frame GetFrame()
    {
        while (true)
        {
            switch (TryGetFrame(out var frame))
            {
                case DecodeStatus.NewFrame:
                    return frame;
                case DecodeStatus.EndOfStream:
                    throw new InvalidOperationException("End of stream");
            }
        }
    }

    /// <summary>
    /// Tries to decode the next frame from the stream.
    /// </summary>
    /// <remarks>
    /// This operation is non-blocking.
    /// If you want a synchronous blocking solution, use <see cref="GetFrame" />.
    /// If you want an asynchronous solution, use the <see cref="OnFrame" /> event instead
    /// and toggle capture with <see cref="StartCapture" /> and <see cref="StopCapture" />.
    /// </remarks>
    public DecodeStatus TryGetFrame(out Frame frame)
    {
        DecodeStatus status = decoder.TryDecodeNextFrame(out frame);

        // Wait 1/4 of configured FPS only if no frame is available.
        // This circumvents possible camera buffering issues.
        // Some cameras have adaptive FPS, so the previous solution isn't adapted.
        // See https://github.com/vignetteapp/SeeShark/issues/29

        // (RIP big brain move to avoid overloading the CPU...)

        // The decoder frame rate is just a guess from FFmpeg, so when there is no guess, we go to
        // a fallback value (60fps) to avoid dividing by zero.
        int den = decoder.Framerate.den == 0 ? 1 : decoder.Framerate.den;
        int num = decoder.Framerate.num == 0 ? 60 : decoder.Framerate.num;

        if (status == DecodeStatus.NoFrameAvailable)
            Thread.Sleep(1000 * den / (num * 4));

        return status;
    }

    public void StopCapture()
    {
        if (!IsPlaying)
            return;

        IsPlaying = false;
        decodingThread?.Join();
    }

    public void StartCapture()
    {
        if (IsPlaying)
            return;

        IsPlaying = true;
        decodingThread = new Thread(DecodeLoop);
        decodingThread.Start();
    }

    protected override void DisposeManaged()
    {
        StopCapture();
        decoder.Dispose();
    }
}
