// Copyright (c) Speykious
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
        if (status == DecodeStatus.NoFrameAvailable)
        {
            // Some cameras report 0 as the framerate, so we need to avoid division by 0.
            (int num, int den) = (decoder.Framerate.num, decoder.Framerate.den);
            int millis = num == 0 ? 1 : 1000 * den / (num * 4);
            Thread.Sleep(millis);
        }

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
