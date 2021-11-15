// Copyright (c) The Vignette Authors
// This file is part of SeeShark.
// SeeShark is licensed under LGPL v3. See LICENSE.LESSER.md for details.

using SeeShark.FFmpeg;

namespace SeeShark
{
    public class Camera : ICamera, IDisposable
    {
        private Thread? decodingThread;
        private readonly CameraStreamDecoder decoder;

        public CameraInfo Info { get; private set; }
        public bool IsPlaying { get; private set; }
        public bool IsDisposed { get; private set; }

        public event EventHandler<FrameEventArgs>? NewFrameHandler;

        public Camera(CameraInfo info, DeviceInputFormat inputFormat)
        {
            Info = info;
            decoder = new CameraStreamDecoder(info.Path, inputFormat);
        }

        protected void OnNewFrame(FrameEventArgs e) => NewFrameHandler?.Invoke(this, e);

        protected void DecodeLoop()
        {
            while (decoder.TryDecodeNextFrame(out var frame))
            {
                OnNewFrame(new FrameEventArgs(frame));

                if (!IsPlaying)
                    break;
            }
        }

        public void Pause()
        {
            if (!IsPlaying)
                return;

            IsPlaying = false;
            decodingThread?.Join();
        }

        public void Play()
        {
            if (IsPlaying)
                return;

            IsPlaying = true;
            decodingThread = new Thread(new ThreadStart(DecodeLoop));
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
                Pause();
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
