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
    /// Manages your video devices. Is able to enumerate them and create new <see cref="Camera"/>s.
    /// It can also watch for available devices, and fire up <see cref="OnNewDevice"/> and
    /// <see cref="OnLostDevice"/> events when it happens.
    /// </summary>
    public abstract unsafe class VideoDeviceManager : Disposable
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
        public event Action<VideoDeviceInfo>? OnNewDevice;

        /// <summary>
        /// Invoked when a video device has been disconnected.
        /// </summary>
        public event Action<VideoDeviceInfo>? OnLostDevice;

        /// <summary>
        /// Enumerates available devices.
        /// </summary>
        private VideoDeviceInfo[] enumerateDevices()
        {
            // FFmpeg doesn't implement avdevice_list_input_sources() for the DShow input format yet.
            // See first SeeShark issue: https://github.com/vignetteapp/SeeShark/issues/1
            if (InputFormat == DeviceInputFormat.DShow)
            {
                var dsDevices = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
                var devices = new VideoDeviceInfo[dsDevices.Length];
                for (int i = 0; i < dsDevices.Length; i++)
                {
                    var dsDevice = dsDevices[i];
                    devices[i] = new VideoDeviceInfo(dsDevice.Name, $"video={dsDevice.Name}");
                }
                return devices;
            }
            else
            {
                AVDeviceInfoList* avDeviceInfoList = null;
                ffmpeg.avdevice_list_input_sources(AvInputFormat, null, null, &avDeviceInfoList).ThrowExceptionIfError();
                int nDevices = avDeviceInfoList->nb_devices;
                var avDevices = avDeviceInfoList->devices;

                var devices = new VideoDeviceInfo[nDevices];
                for (int i = 0; i < nDevices; i++)
                {
                    var avDevice = avDevices[i];
                    var name = Marshal.PtrToStringAnsi((IntPtr)avDevice->device_description);
                    var path = Marshal.PtrToStringAnsi((IntPtr)avDevice->device_name);

                    if (path == null)
                        throw new InvalidOperationException($"Device at index {i} doesn't have a path!");

                    devices[i] = new VideoDeviceInfo(name, path);
                }

                ffmpeg.avdevice_free_list_devices(&avDeviceInfoList);
                return devices;
            }
        }


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

        public Camera GetCamera(VideoDeviceInfo info) => new Camera(info, InputFormat);
        public Camera GetCamera(int index = 0) => GetCamera(Devices[index]);
        public Camera GetCamera(string path) => GetCamera(Devices.First((ci) => ci.Path == path));

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
        public void SyncDevices()
        {
            var newDevices = enumerateDevices().ToImmutableList();

            if (Devices.SequenceEqual(newDevices))
                return;

            foreach (var device in newDevices.Except(Devices))
                OnNewDevice?.Invoke(device);

            foreach (var device in Devices.Except(newDevices))
                OnLostDevice?.Invoke(device);

            Devices = newDevices;
        }

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
