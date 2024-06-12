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

    internal static unsafe List<VideoFormat> AvailableFormats(CameraPath path)
    {
        List<VideoFormat> formats = new List<VideoFormat>();

        if (AVCaptureDevice.DeviceWithUniqueID(path.Path) is AVCaptureDevice device)
        {
            nint[] deviceFormats = device.Formats.ToArray();
            foreach (nint deviceFormatID in deviceFormats)
            {
                AVCaptureDeviceFormat deviceFormat = new AVCaptureDeviceFormat(deviceFormatID);

                nint[] maxDimensions = deviceFormat.SupportedMaxPhotoDimensions.ToArray();
                nint[] framerateRanges = deviceFormat.VideoSupportedFrameRateRanges.ToArray();

                foreach (nint maxDimensionID in maxDimensions)
                {
                    NSValue<CMVideoDimensions> maxDimensionValue = new NSValue<CMVideoDimensions>(maxDimensionID);
                    CMVideoDimensions maxDimension = maxDimensionValue.GetValue();

                    foreach (nint framerateRangeID in framerateRanges)
                    {
                        AVFrameRateRange framerateRange = new AVFrameRateRange(framerateRangeID);

                        formats.Add(new VideoFormat
                        {
                            VideoSize = ((uint)maxDimension.Width, (uint)maxDimension.Height),
                            Framerate = new FramerateRatio
                            {
                                Numerator = (uint)framerateRange.MaxFrameRate,
                                Denominator = 1,
                            },
                        });
                    }
                }
            }
        }

        return formats;
    }
}
