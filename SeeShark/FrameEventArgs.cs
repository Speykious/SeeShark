// Copyright (c) The Vignette Authors
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 3-Clause License. See LICENSE for details.

using System;
using SeeShark.Decode;

namespace SeeShark
{
    /// <summary>
    /// Contains event data for a camera frame - in other words, just a <see cref="Frame"/>.
    /// </summary>
    public class FrameEventArgs : EventArgs
    {
        /// <summary>
        /// The frame sent from the camera.
        /// </summary>
        public Frame Frame { get; private set; }
        /// <summary>
        /// The decode status when sending that frame.
        /// </summary>
        public DecodeStatus Status { get; private set; }

        public FrameEventArgs(Frame frame, DecodeStatus status = DecodeStatus.NewFrame)
        {
            Frame = frame;
            Status = status;
        }
    }
}
