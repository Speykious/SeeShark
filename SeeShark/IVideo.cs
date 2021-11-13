// Copyright (c) The Vignette Authors
// This file is part of SeeShark.
// SeeShark is licensed under LGPL v3. See LICENSE.LESSER.md for details.

namespace SeeShark
{
    public interface IVideo
    {
        /// <summary>
        /// Whether it is sending frames.
        /// </summary>
        public bool IsPlaying { get; }

        /// <summary>
        /// Starts sending frames to the <see cref="OnNewFrame"/> event handler.
        /// </summary>
        public void Play();

        /// <summary>
        /// Stops sending frames to the <see cref="OnNewFrame"/> event handler.
        /// </summary>
        public void Pause();

        /// <summary>
        /// Fires whenever there's a new frame ready to be read.
        /// </summary>
        public event EventHandler<FrameEventArgs> OnNewFrame;

        /// <summary>
        /// Whether or not this <see cref="IVideo"/> has been disposed yet.
        /// </summary>
        public bool IsDisposed { get; }
    }
}
