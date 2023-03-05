// Copyright (c) Speykious
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 3-Clause License. See LICENSE for details.

using System;
using System.Runtime.InteropServices;
using SeeShark.Utils;

namespace SeeShark.Device;

/// <summary>
/// Manages your camera devices. Is able to enumerate them and create new <see cref="Camera"/>s.
/// It can also watch for available devices, and fire up <see cref="OnNewDevice"/> and
/// <see cref="OnLostDevice"/> events when it happens.
/// </summary>
public sealed unsafe class XPlatformVideoDeviceManager : VideoDeviceManager<VideoDeviceInfo, VideoDevice>
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
    public XPlatformVideoDeviceManager(DeviceInputFormat? inputFormat = null) : base(inputFormat ?? DefaultInputFormat)
    {
    }

    public override VideoDevice GetDevice(VideoDeviceInfo info, VideoInputOptions? options = null) =>
        new(info, InputFormat, options);

    /// <summary>
    /// Enumerates available devices.
    /// </summary>
    protected override VideoDeviceInfo[] EnumerateDevices()
    {
        // FFmpeg doesn't implement avdevice_list_input_sources() for the DShow input format yet.
        // See first SeeShark issue: https://github.com/vignetteapp/SeeShark/issues/1

        // Supported formats won't use the default method to allow better video input options handling.
        switch (InputFormat)
        {
            case DeviceInputFormat.DShow:
                return DShowUtils.EnumerateDevices();
            case DeviceInputFormat.V4l2:
                VideoDeviceInfo[] devices = base.EnumerateDevices();
                V4l2Utils.FillDeviceOptions(devices);
                return devices;
            default:
                return base.EnumerateDevices();
        }
    }
}
