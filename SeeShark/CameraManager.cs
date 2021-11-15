// Copyright (c) The Vignette Authors
// This file is part of SeeShark.
// SeeShark is licensed under LGPL v3. See LICENSE.LESSER.md for details.

using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using FFmpeg.AutoGen;
using SeeShark.FFmpeg;
using static SeeShark.FFmpeg.FFmpegManager;

namespace SeeShark
{
    /// <summary>
    /// Manages your camera devices. Is able to enumerate them and create new <see cref="Camera"/>s.
    /// It can also watch for available devices, and fire up <see cref="OnNewDevice"/> and
    /// <see cref="OnLostDevice"/> events when it happens.
    /// </summary>
    public unsafe class CameraManager : IDisposable
    {
        private readonly AVInputFormat* avInputFormat;
        private readonly AVFormatContext* avFormatContext;
        private readonly Timer deviceWatcher;

        /// <summary>
        /// Whether this <see cref="CameraManager"/> is watching for devices.
        /// </summary>
        public bool IsWatching { get; private set; }

        /// <summary>
        /// Input format used by this <see cref="CameraManager"/> to watch devices.
        /// </summary>
        public readonly DeviceInputFormat InputFormat;

        /// <summary>
        /// List of all the available camera devices.
        /// </summary>
        public ImmutableList<CameraInfo> Devices = ImmutableList<CameraInfo>.Empty;

        /// <summary>
        /// Invoked when a camera device has been connected.
        /// </summary>
        public event Action<CameraInfo>? OnNewDevice;

        /// <summary>
        /// Invoked when a camera device has been disconnected.
        /// </summary>
        public event Action<CameraInfo>? OnLostDevice;

        /// <summary>
        /// Whether this <see cref="CameraManager"/> has been disposed yet.
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Enumerates available devices.
        /// </summary>
        protected CameraInfo[] EnumerateDevices()
        {
            AVDeviceInfoList* avDeviceInfoList = null;
            ffmpeg.avdevice_list_input_sources(avInputFormat, null, null, &avDeviceInfoList).ThrowExceptionIfError();
            int nDevices = avDeviceInfoList->nb_devices;
            var avDevices = avDeviceInfoList->devices;

            var devices = new CameraInfo[nDevices];
            for (int i = 0; i < nDevices; i++)
            {
                var avDevice = avDevices[i];
                var name = Marshal.PtrToStringAnsi((IntPtr)avDevice->device_description);
                var path = Marshal.PtrToStringAnsi((IntPtr)avDevice->device_name);

                if (path == null)
                    throw new InvalidOperationException($"Device at index {i} doesn't have a path!");

                if (name == null)
                    name = path;

                devices[i] = new CameraInfo(name, path);
            }

            ffmpeg.avdevice_free_list_devices(&avDeviceInfoList);
            return devices;
        }

        /// <summary>
        /// Creates a new <see cref="CameraManager"/>.
        /// </summary>
        /// <remarks>
        /// Upon creation, it will call <see cref="SyncCameraDevices"/> once, but won't be in a watching state.
        /// </remarks>
        public CameraManager(DeviceInputFormat inputFormat)
        {
            SetupFFmpeg();
            InputFormat = inputFormat;
            avInputFormat = ffmpeg.av_find_input_format(InputFormat.ToString());
            avFormatContext = ffmpeg.avformat_alloc_context();

            SyncCameraDevices();
            deviceWatcher = new Timer(
                (object? _state) => SyncCameraDevices(),
                null, Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan
            );

            IsWatching = false;
        }

        /// <summary>
        /// Starts watching for available devices.
        /// </summary>
        public void StartWatching()
        {
            deviceWatcher.Change(TimeSpan.Zero, TimeSpan.FromSeconds(1));
            IsWatching = true;
        }

        /// <summary>
        /// Stops watching for available devices.
        /// </summary>
        public void StopWatching()
        {
            deviceWatcher.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
            IsWatching = false;
        }

        /// <summary>
        /// Looks for available devices and triggers <see cref="OnNewDevice"/> and <see cref="OnLostDevice"/> events.
        /// </summary>
        public void SyncCameraDevices()
        {
            var newDevices = EnumerateDevices().ToImmutableList();

            if (Devices.SequenceEqual(newDevices))
                return;

            foreach (var device in newDevices.Except(Devices))
                OnNewDevice?.Invoke(device);

            foreach (var device in Devices.Except(newDevices))
                OnLostDevice?.Invoke(device);

            Devices = newDevices;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed)
                return;

            if (disposing)
                ffmpeg.avformat_free_context(avFormatContext);

            IsDisposed = true;
        }

        ~CameraManager()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
