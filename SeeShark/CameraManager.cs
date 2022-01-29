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
        public static DeviceInputFormat DefaultInputFormat
        {
            get
            {
                return OperatingSystem.IsWindows() ? DeviceInputFormat.DShow
                    : OperatingSystem.IsLinux() ? DeviceInputFormat.V4l2
                    : OperatingSystem.IsMacOS() ? DeviceInputFormat.AVFoundation
                    : throw new NotSupportedException(
                        $"Cannot find adequate camera input format for RID '{RuntimeInformation.RuntimeIdentifier}'.");
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
        public CameraManager(DeviceInputFormat? inputFormat = null) : base(inputFormat ?? DefaultInputFormat)
        {
        }

        public override Camera GetDevice(CameraInfo info) => new Camera(info, InputFormat);

        /// <summary>
        /// Enumerates available devices.
        /// </summary>
        protected override CameraInfo[] EnumerateDevices()
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
                    devices[i] = new CameraInfo
                    {
                        Name = dsDevice.Name,
                        Path = $"video={dsDevice.Name}",
                    };
                }
                return devices;
            }
            else
            {
                return base.EnumerateDevices();
            }
        }
    }
}
