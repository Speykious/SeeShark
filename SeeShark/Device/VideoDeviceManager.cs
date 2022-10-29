// Copyright (c) The Vignette Authors
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 3-Clause License. See LICENSE for details.

using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using FFmpeg.AutoGen;
using SeeShark.FFmpeg;
using static SeeShark.FFmpeg.FFmpegManager;

namespace SeeShark.Device
{
    /// <summary>
    /// Manages your video devices. Is able to enumerate them and create new <see cref="T"/>s.
    /// It can also watch for available devices, and fire up <see cref="OnNewDevice"/> and
    /// <see cref="OnLostDevice"/> events when it happens.
    /// </summary>
    public abstract unsafe class VideoDeviceManager<TDeviceInfo, T> : Disposable
        where T : VideoDevice
        where TDeviceInfo : VideoDeviceInfo, new()
    {
        protected readonly AVInputFormat* AvInputFormat;
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
        public event Action<TDeviceInfo>? OnNewDevice;

        /// <summary>
        /// Invoked when a video device has been disconnected.
        /// </summary>
        public event Action<TDeviceInfo>? OnLostDevice;

        protected VideoDeviceManager(DeviceInputFormat inputFormat)
        {
            SetupFFmpeg();

            InputFormat = inputFormat;
            AvInputFormat = ffmpeg.av_find_input_format(InputFormat.ToString());

            SyncDevices();
            DeviceWatcher = new Timer(
                (_state) => SyncDevices(),
                null, Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan
            );

            IsWatching = false;
        }

        public abstract T GetDevice(TDeviceInfo info, VideoInputOptions? options = null);
        public T GetDevice(int index = 0, VideoInputOptions? options = null) =>
            GetDevice(Devices[index], options);
        public T GetDevice(string path, VideoInputOptions? options = null) =>
            GetDevice(Devices.First((ci) => ci.Path == path), options);

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

        protected virtual TDeviceInfo[] EnumerateDevices()
        {
            AVDeviceInfoList* avDeviceInfoList = null;
            ffmpeg.avdevice_list_input_sources(AvInputFormat, null, null, &avDeviceInfoList).ThrowExceptionIfError();
            int nDevices = avDeviceInfoList->nb_devices;
            AVDeviceInfo** avDevices = avDeviceInfoList->devices;

            TDeviceInfo[] devices = new TDeviceInfo[nDevices];
            for (int i = 0; i < nDevices; i++)
            {
                AVDeviceInfo* avDevice = avDevices[i];
                string name = new string((sbyte*)avDevice->device_description);
                string path = new string((sbyte*)avDevice->device_name);

                if (path == null)
                    throw new InvalidOperationException($"Device at index {i} doesn't have a path!");

                devices[i] = new TDeviceInfo
                {
                    Name = name,
                    Path = path,
                };
            }

            ffmpeg.avdevice_free_list_devices(&avDeviceInfoList);
            return devices;
        }

        /// <summary>
        /// Looks for available devices and triggers <see cref="OnNewDevice"/> and <see cref="OnLostDevice"/> events.
        /// </summary>
        public void SyncDevices()
        {
            ImmutableList<TDeviceInfo> newDevices = EnumerateDevices().ToImmutableList();

            if (Devices.SequenceEqual(newDevices))
                return;

            foreach (TDeviceInfo device in newDevices.Except(Devices))
                OnNewDevice?.Invoke(device);

            foreach (TDeviceInfo device in Devices.Except(newDevices))
                OnLostDevice?.Invoke(device);

            Devices = newDevices;
        }

        protected override void DisposeManaged()
        {
            DeviceWatcher.Dispose();
        }
    }
}
