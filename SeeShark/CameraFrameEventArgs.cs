// Copyright (c) The Vignette Authors
// This file is part of SeeShark.
// SeeShark is licensed under LGPL v3. See LICENSE.LESSER.md for details.

namespace SeeShark
{
    /// <summary>
    /// Contains event data for a camera frame - in other words, just a <see cref="Frame"/>.
    /// </summary>
    public class CameraFrameEventArgs : EventArgs
    {
        /// <summary>
        /// The frame sent from the camera.
        /// </summary>
        public Frame Frame { get; private set; }

        public CameraFrameEventArgs(Frame frame)
        {
            Frame = frame;
        }
    }
}
