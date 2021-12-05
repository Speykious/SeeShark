// Copyright (c) The Vignette Authors
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 3-Clause License. See LICENSE for details.

using System;
using System.Threading;
using SeeShark.FFmpeg;

namespace SeeShark
{
    public class Camera : IDisposable
    {
        private Thread? decodingThread;
        private readonly CameraStreamDecoder decoder;

        public CameraInfo Info { get; }
        public bool IsPlaying { get; private set; }
        public bool IsDisposed { get; private set; }

        public event EventHandler<FrameEventArgs>? OnFrame;

        public Camera(CameraInfo info, DeviceInputFormat inputFormat)
        {
            Info = info;
            decoder = new CameraStreamDecoder(info.Path, inputFormat);
        }

        protected void DecodeLoop()
        {
            DecodeStatus status;
            while ((status = decoder.TryDecodeNextFrame(out var frame)) != DecodeStatus.EndOfStream)
            {
                OnFrame?.Invoke(this, new FrameEventArgs(frame, status));

                if (!IsPlaying)
                    break;
            }
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
                StopCapture();
                decoder.Dispose();
            }

            IsDisposed = true;
        }

        ~Camera()
        {
            Dispose(false);
        }
    }
}
