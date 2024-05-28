// Copyright (c) Speykious
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 2-Clause License. See LICENSE for details.

using System.Collections.Generic;
using System.Runtime.Versioning;
using SeeShark.Interop.MacOS;

namespace SeeShark.MacOS;

[SupportedOSPlatform("Macos")]
public static class AVFoundation
{
    internal static unsafe List<CameraPath> AvailableCameras()
    {
        var captureDevices = new List<CameraPath>();

        NSArray devices = AVCaptureDevice.Devices;
        int count = (int)devices.Count;

        for (int i = 0; i < count; i++)
        {
            AVCaptureDevice device = new AVCaptureDevice(devices.ObjectAtIndex(i));
            if (device.HasMediaType(AVCaptureDevice.AV_MEDIA_TYPE_VIDEO))
            {
                captureDevices.Add(new CameraPath
                {
                    Path = device.UniqueID.ToUTF8String(),
                });
            }
        }

        return captureDevices;
    }
}
