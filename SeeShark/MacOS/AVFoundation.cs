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
                            ImageFormat? maybeImageFormat = CVPixelFormatTypeIntoImageFormat(pixelFormatType);
                            if (maybeImageFormat is not ImageFormat imageFormat)
                                continue;

                            formats.Add(new VideoFormat
                            {
                                VideoSize = ((uint)maxDimension.Width, (uint)maxDimension.Height),
                                Framerate = new FramerateRatio
                                {
                                    Numerator = (uint)frameRateRange.MaxFrameRate,
                                    Denominator = 1,
                                },
                                ImageFormat = imageFormat,
                            });
                        }
                    }
                }
            }
        }

        return formats;
    }

    internal static ImageFormat? CVPixelFormatTypeIntoImageFormat(CVPixelFormatType pixelFormat)
    {
        return pixelFormat switch
        {
            CVPixelFormatType.k_24RGB => ImageFormat.Rgb24,
            CVPixelFormatType.k_32ARGB => ImageFormat.Argb,

            _ => ImageFormatTag.FindRawPixelFormat((int)pixelFormat),
        };
    }

    internal static CVPixelFormatType? ImageFormatIntoCVPixelFormatType(ImageFormat imageFormat)
    {
        return imageFormat switch
        {

#pragma warning disable format
            // YUV planar
            ImageFormat.Yuv420P       => CVPixelFormatType.k_420YpCbCr8Planar,
            // ImageFormat.Yuv422P       => CVPixelFormatType.k_422YpCbCr8, // TODO: not accurate?
            // ImageFormat.Yuv444P       => CVPixelFormatType.k_444YpCbCr8,
            // ImageFormat.Yuv410P       => YUV410,
            // ImageFormat.Yuv411P       => YUV411P,

            // YUYV et. al.
            // ImageFormat.Yuyv422       => YUYV,
            ImageFormat.Uyvy422       => CVPixelFormatType.k_444YpCbCr8,
            // ImageFormat.Yvyu422       => YVYU,

            // RGBA et. al.
            // ImageFormat.Gray8         => ,
            ImageFormat.Pal8          => CVPixelFormatType.k_8Indexed,

            ImageFormat.Rgb24         => CVPixelFormatType.k_24RGB,
            ImageFormat.Bgr24         => CVPixelFormatType.k_24BGR,

            ImageFormat.Argb          => CVPixelFormatType.k_32ARGB,
            ImageFormat.Rgba          => CVPixelFormatType.k_32RGBA,
            ImageFormat.Abgr          => CVPixelFormatType.k_32ABGR,
            ImageFormat.Bgra          => CVPixelFormatType.k_32BGRA,

            // ImageFormat.Xrgb          => CVPixelFormatType.k_32XRGB,
            // ImageFormat.Rgbx          => CVPixelFormatType.k_32RGBX,
            // ImageFormat.Xbgr          => CVPixelFormatType.k_32XBGR,
            // ImageFormat.Bgrx          => CVPixelFormatType.k_32BGRX,
#pragma warning restore format

            _ => null,
        };
    }

    internal static ImageFormat CVPixelFormatTypeIntoImageFormat_OrThrowIfUnsupported(CVPixelFormatType pixelFormat)
    {
        ImageFormat? imageFormat = CVPixelFormatTypeIntoImageFormat(pixelFormat);
        if (imageFormat is ImageFormat format)
            return format;
        else
            throw new ImageFormatNotSupportedException($"SeeShark does not support MacOS pixel format {pixelFormat}");
    }

    internal static CVPixelFormatType ImageFormatIntoCVPixelFormatType_OrThrowIfUnsupported(ImageFormat imageFormat)
    {
        CVPixelFormatType? pixelFormat = ImageFormatIntoCVPixelFormatType(imageFormat);
        if (pixelFormat is CVPixelFormatType format)
            return format;
        else
            throw new ImageFormatNotSupportedException($"SeeShark does not have a MacOS counterpart for image format {imageFormat}");
    }
}
