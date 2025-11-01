// Copyright (c) Speykious
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 2-Clause License. See LICENSE for details.

using System;
using System.Collections.Generic;
using System.Runtime.Versioning;
using SeeShark.Interop.MacOS;

namespace SeeShark.MacOS;

[SupportedOSPlatform("Macos")]
internal static class AVFoundation
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
                    Name = device.LocalizedName.ToUTF8String(),
                    Path = device.UniqueID.ToUTF8String(),
                });
            }
        }

        return captureDevices;
    }

    internal static unsafe List<VideoFormat> AvailableFormats(CameraPath path)
    {
        // TODO: Might need to do something more clever about enumerating the available video formats.

        List<VideoFormat> formats = new List<VideoFormat>();

        if (AVCaptureDevice.DeviceWithUniqueID(path.Path) is AVCaptureDevice device)
        {
            // Behold, a *quadruple* foreach...

            AVCaptureVideoDataOutput deviceOutput = new();
            CVPixelFormatType[] pixelFormatTypes = deviceOutput.AvailableVideoCVPixelFormatTypes.ToTypedArray(id => (CVPixelFormatType)new NSNumber(id).UIntValue());
            AVCaptureDeviceFormat[] deviceFormats = device.Formats.ToTypedArray<AVCaptureDeviceFormat>(id => new(id));

            Console.Error.WriteLine("[SeeShark] Device formats available:");
            foreach (AVCaptureDeviceFormat deviceFormat in deviceFormats)
            {
                CMVideoDimensions[] maxDimensions = deviceFormat.SupportedMaxPhotoDimensions.ToTypedArray(id => new NSValue<CMVideoDimensions>(id).GetValue());
                AVFrameRateRange[] frameRateRanges = deviceFormat.VideoSupportedFrameRateRanges.ToTypedArray<AVFrameRateRange>(id => new(id));

                foreach (CMVideoDimensions maxDimension in maxDimensions)
                {
                    foreach (AVFrameRateRange frameRateRange in frameRateRanges)
                    {
                        foreach (CVPixelFormatType pixelFormatType in pixelFormatTypes)
                        {
                            // Ignore pixel formats we don't support (for example planar ones)
                            int bitSize = CVPixelFormatTypeMethods.BitSize(pixelFormatType);
                            if (bitSize <= 0 || bitSize % 8 != 0)
                                continue;

                            formats.Add(new VideoFormat
                            {
                                VideoSize = ((uint)maxDimension.Width, (uint)maxDimension.Height),
                                Framerate = new FramerateRatio
                                {
                                    Numerator = (uint)frameRateRange.MaxFrameRate,
                                    Denominator = 1,
                                },
                                ImageFormat = new ImageFormat((uint)pixelFormatType),
                            });
                        }
                    }
                }
            }
        }

        return formats;
    }
}
