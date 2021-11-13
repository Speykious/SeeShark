// Copyright (c) The Vignette Authors
// This file is part of SeeShark.
// SeeShark is licensed under LGPL v3. See LICENSE.LESSER.md for details.

namespace SeeShark
{
    /// <summary>
    /// Various information about the camera device.
    /// </summary>
    public struct CameraInfo : IEquatable<CameraInfo>
    {
        /// <summary>
        /// Name of the camera. When it doesn't have a specific name, it is equal to its <see cref="Path"/>.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Path of the camera device. It can be anything from a file on the system (on Linux for instance) or a UUID (on Windows for example).
        /// </summary>
        public string Path { get; set; }

        internal CameraInfo(string name, string path)
        {
            Name = name;
            Path = path;
        }

        public bool Equals(CameraInfo other) => Path == other.Path;
        public override bool Equals(object? obj) => obj is CameraInfo info && Equals(info);
        public override int GetHashCode() => Path.GetHashCode();

        public static bool operator ==(CameraInfo left, CameraInfo right) => left.Equals(right);
        public static bool operator !=(CameraInfo left, CameraInfo right) => !(left == right);

        public override string? ToString() => $"{Name} ({Path})";
    }
}
