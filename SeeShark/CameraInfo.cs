// Copyright (c) The Vignette Authors
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 3-Clause License. See LICENSE for details.

using System;

namespace SeeShark
{
    /// <summary>
    /// Various information about the camera device.
    /// </summary>
    public struct CameraInfo : IEquatable<CameraInfo>
    {
        /// <summary>
        /// Name of the camera. Can be null.
        /// </summary>
        public readonly string? Name;
        /// <summary>
        /// Path of the camera device. It can be anything from a file on the system (on Linux for instance) or a UUID (on Windows for example).
        /// </summary>
        public readonly string Path;

        internal CameraInfo(string path)
        {
            Name = null;
            Path = path;
        }

        internal CameraInfo(string? name, string path)
        {
            Name = name;
            Path = path;
        }

        public bool Equals(CameraInfo other) => Path == other.Path;
        public override bool Equals(object? obj) => obj is CameraInfo info && Equals(info);
        public override int GetHashCode() => Path.GetHashCode();

        public static bool operator ==(CameraInfo left, CameraInfo right) => left.Equals(right);
        public static bool operator !=(CameraInfo left, CameraInfo right) => !(left == right);

        public override string? ToString() => Name == null ? Path : $"{Name} ({Path})";
    }
}
