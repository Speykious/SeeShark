// Copyright (c) The Vignette Authors
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 3-Clause License. See LICENSE for details.

using System;

namespace SeeShark.Device
{
    /// <summary>
    /// Various information about the camera device.
    /// </summary>
    public class VideoDeviceInfo : IEquatable<VideoDeviceInfo>
    {
        /// <summary>
        /// Name of the camera. Can be null.
        /// </summary>
        public string? Name { get; internal set; }
        /// <summary>
        /// Path of the camera device. It can be anything from a file on the system (on Linux for instance) or a UUID (on Windows for example).
        /// </summary>
        public string Path { get; internal set; } = "";
        /// <summary>
        /// Available sets of video input options for this device.
        /// </summary>
        public VideoInputOptions[]? AvailableVideoInputOptions { get; internal set; }

        public bool Equals(VideoDeviceInfo? other) => Path == other?.Path;
        public override bool Equals(object? obj) => obj is VideoDeviceInfo info && Equals(info);
        public override int GetHashCode() => Path.GetHashCode();

        public static bool operator ==(VideoDeviceInfo left, VideoDeviceInfo right) => left.Equals(right);
        public static bool operator !=(VideoDeviceInfo left, VideoDeviceInfo right) => !(left == right);

        public override string? ToString() => Name == null ? Path : $"{Name} ({Path})";
    }
}
