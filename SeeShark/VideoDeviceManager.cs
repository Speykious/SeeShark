// Copyright (c) The Vignette Authors
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 3-Clause License. See LICENSE for details.

using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using FFmpeg.AutoGen;
using SeeShark.FFmpeg;

namespace SeeShark
{
    /// <summary>
    /// Manages your video devices. Is able to enumerate them and create new <see cref="T"/>s.
    /// It can also watch for available devices, and fire up <see cref="OnNewDevice"/> and
    /// <see cref="OnLostDevice"/> events when it happens.
    /// </summary>
    public abstract unsafe class VideoDeviceManager<TDeviceInfo, T> : Disposable where T : VideoDevice where TDeviceInfo : VideoDeviceInfo
    {
        protected AVInputFormat* AvInputFormat;
        protected AVFormatContext* AvFormatContext;
        protected Timer DeviceWatcher;

        /// <summary>
        /// Whether this <see cref="VideoDeviceManager"/> is watching for devices.
        /// </summary>
        public bool IsWatching { get; protected set; }

        /// <summary>
        /// Input format used by this <see cref="VideoDeviceManager"/> to watch devices.
        /// </summary>
        public DeviceInputFormat InputFormat { get; protected set; }

        /// <summary>
        /// List of all the available video devices.
        /// </summary>
        public ImmutableList<TDeviceInfo> Devices { get; protected set; } = ImmutableList<TDeviceInfo>.Empty;

        /// <summary>
        /// Invoked when a video device has been connected.
        /// </summary>
        public abstract event Action<TDeviceInfo>? OnNewDevice;

        /// <summary>
        /// Invoked when a video device has been disconnected.
        /// </summary>
        public abstract event Action<TDeviceInfo>? OnLostDevice;

        public abstract T GetDevice(TDeviceInfo info);
        public T GetDevice(int index = 0) => GetDevice(Devices[index]);
        public T GetDevice(string path) => GetDevice(Devices.First((ci) => ci.Path == path));

        /// <summary>
        /// Starts watching for available devices.
        /// </summary>
        public void StartWatching()
        {
            DeviceWatcher.Change(TimeSpan.Zero, TimeSpan.FromSeconds(1));
            IsWatching = true;
        }

        /// <summary>
        /// Stops watching for available devices.
        /// </summary>
        public void StopWatching()
        {
            DeviceWatcher.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
            IsWatching = false;
        }

        /// <summary>
        /// Looks for available devices and triggers <see cref="OnNewDevice"/> and <see cref="OnLostDevice"/> events.
        /// </summary>
        public abstract void SyncDevices();


        protected override void DisposeManaged()
        {
            DeviceWatcher.Dispose();
        }

        protected override void DisposeUnmanaged()
        {
            ffmpeg.avformat_free_context(AvFormatContext);
        }
    }
}
