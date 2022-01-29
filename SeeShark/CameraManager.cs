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
    /// Manages your camera devices. Is able to enumerate them and create new <see cref="Camera"/>s.
    /// It can also watch for available devices, and fire up <see cref="OnNewDevice"/> and
    /// <see cref="OnLostDevice"/> events when it happens.
    /// </summary>
    public sealed unsafe class CameraManager : VideoDeviceManager<CameraInfo, Camera>
    {
        public override event Action<CameraInfo>? OnNewDevice;
        public override event Action<CameraInfo>? OnLostDevice;

        public override Camera GetDevice(CameraInfo info) => new Camera(info, InputFormat);

        /// <summary>
        /// Enumerates available devices.
        /// </summary>
        private CameraInfo[] enumerateDevices()
        {
            // FFmpeg doesn't implement avdevice_list_input_sources() for the DShow input format yet.
            // See first SeeShark issue: https://github.com/vignetteapp/SeeShark/issues/1
            if (InputFormat == DeviceInputFormat.DShow)
            {
                var dsDevices = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
                var devices = new CameraInfo[dsDevices.Length];
                for (int i = 0; i < dsDevices.Length; i++)
                {
                    var dsDevice = dsDevices[i];
                    devices[i] = new CameraInfo(dsDevice.Name, $"video={dsDevice.Name}");
                }
                return devices;
            }
            else
            {
                AVDeviceInfoList* avDeviceInfoList = null;
                ffmpeg.avdevice_list_input_sources(AvInputFormat, null, null, &avDeviceInfoList).ThrowExceptionIfError();
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

                    devices[i] = new CameraInfo(name, path);
                }

                ffmpeg.avdevice_free_list_devices(&avDeviceInfoList);
                return devices;
            }
        }


        /// <summary>
        /// Creates a new <see cref="CameraManager"/>.
        /// It will call <see cref="SyncCameraDevices"/> once, but won't be in a watching state.
        /// </summary>
        /// <remarks>
        /// If you don't specify any input format, it will attempt to choose one suitable for your OS platform.
        /// </remarks>
        /// <param name="inputFormat">
        /// Input format used to enumerate devices and create cameras.
        /// </param>
        public CameraManager(DeviceInputFormat? inputFormat = null)
        {
            SetupFFmpeg();

            InputFormat = inputFormat ?? (
                RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? DeviceInputFormat.DShow
                : RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? DeviceInputFormat.V4l2
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

        public override void SyncDevices()
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
    }
}
