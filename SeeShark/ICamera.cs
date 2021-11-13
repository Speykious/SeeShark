// Copyright (c) The Vignette Authors
// This file is part of SeeShark.
// SeeShark is licensed under LGPL v3. See LICENSE.LESSER.md for details.

namespace SeeShark
{
    public interface ICamera : IVideo, IDisposable
    {
        /// <summary>
        /// Information on this <see cref="ICamera"/>.
        /// </summary>
        public CameraInfo Info { get; }

        /// <summary>
        /// Whether or not this <see cref="ICamera"/> is an <see cref="IAdjustableCamera"/>.
        /// </summary>
        public bool IsAdjustable => this is IAdjustableCamera;
    }
}
