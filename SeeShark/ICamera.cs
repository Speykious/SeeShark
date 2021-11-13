// Copyright (c) The Vignette Authors
// This file is part of SeeShark.
// SeeShark is licensed under LGPL v3. See LICENSE.LESSER.md for details.

namespace SeeShark
{
    public interface ICamera : IDisposable
    {
        /// <summary>
        /// Information on this <see cref="ICamera"/>.
        /// </summary>
        public CameraInfo Info { get; }

        /// <summary>
        /// Whether the camera is sending frames.
        /// </summary>
        public bool IsActive { get; }

        /// <summary>
        /// Starts sending frames to the <see cref="OnNewFrame"/> event handler.
        /// </summary>
        public void Start();

        /// <summary>
        /// Stops sending frames to the <see cref="OnNewFrame"/> event handler.
        /// </summary>
        public void Stop();

        /// <summary>
        /// Fires whenever there's a new frame ready to be read.
        /// </summary>
        public event EventHandler<FrameEventArgs> OnNewFrame;

        /// <summary>
        /// Whether or not this <see cref="ICamera"/> is an <see cref="IAdjustableCamera"/>.
        /// </summary>
        public bool IsAdjustable => this is IAdjustableCamera;

        /// <summary>
        /// Whether or not this <see cref="ICamera"/> has been disposed yet.
        /// </summary>
        public bool IsDisposed { get; }
    }
}
