// Copyright (c) The Vignette Authors
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 3-Clause License. See LICENSE for details.

using System;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using DirectShowLib;
using FFmpeg.AutoGen;
using SeeShark.FFmpeg;
using static SeeShark.FFmpeg.FFmpegManager;

namespace SeeShark
{
    /// <summary>
    /// Manages your video devices. Is able to enumerate them and create new <see cref="T"/>s.
    /// It can also watch for available devices, and fire up <see cref="OnNewDevice"/> and
    /// <see cref="OnLostDevice"/> events when it happens.
    /// </summary>
    public abstract unsafe class VideoDeviceManager<T> : Disposable where T : VideoDevice
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
        public ImmutableList<VideoDeviceInfo> Devices { get; protected set; } = ImmutableList<VideoDeviceInfo>.Empty;

        /// <summary>
        /// Invoked when a video device has been connected.
        /// </summary>
        public abstract event Action<VideoDeviceInfo>? OnNewDevice;

        /// <summary>
        /// Invoked when a video device has been disconnected.
        /// </summary>
        public abstract event Action<VideoDeviceInfo>? OnLostDevice;


        /// <summary>
        /// Creates a new <see cref="VideoDeviceManager"/>.
        /// It will call <see cref="SyncDevices"/> once, but won't be in a watching state.
        /// </summary>
        /// <remarks>
        /// If you don't specify any input format, it will attempt to choose one suitable for your OS platform.
        /// </remarks>
        /// <param name="inputFormat">
        /// Input format used to enumerate devices and create video devices.
        /// </param>
        public VideoDeviceManager(DeviceInputFormat? inputFormat = null)
        {
            SetupFFmpeg();

            InputFormat = inputFormat ?? (
                RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? DeviceInputFormat.GdiGrab
                : RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? DeviceInputFormat.X11Grab
                : RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? DeviceInputFormat.AVFoundation
                : throw new NotSupportedException($"Cannot find adequate input format for RID '{RuntimeInformation.RuntimeIdentifier}'."));

            AvInputFormat = ffmpeg.av_find_input_format(InputFormat.ToString());
            AvFormatContext = ffmpeg.avformat_alloc_context();

            SyncDevices();
            DeviceWatcher = new Timer(
                (object? _state) => SyncDevices(),
                null, Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan
            );

            IsWatching = false;
        }

        public abstract T GetDevice(VideoDeviceInfo info);
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
