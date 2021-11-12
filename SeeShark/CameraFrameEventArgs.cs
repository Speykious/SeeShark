// Copyright (c) The Vignette Authors
// This file is part of SeeShark.
// SeeShark is licensed under LGPL v3. See LICENSE.LESSER.md for details.

namespace SeeShark
{
    public class CameraFrameEventArgs : EventArgs
    {
        public Frame Frame { get; private set; }

        public CameraFrameEventArgs(Frame frame)
        {
            Frame = frame;
        }
    }
}
