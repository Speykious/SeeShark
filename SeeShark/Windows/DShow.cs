// Copyright (c) Speykious
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 2-Clause License. See LICENSE for details.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using DirectShowLib;

namespace SeeShark.Windows;

internal static class DShow
{
    /// <summary>
    /// Type of compression for a bitmap image.
    /// See https://docs.microsoft.com/en-us/openspecs/windows_protocols/ms-wmf/4e588f70-bd92-4a6f-b77f-35d0feaf7a57
    /// </summary>
    private enum BitmapCompression : int
    {
        Rgb = 0x00,
        Rle8 = 0x01,
        Rle4 = 0x02,
        Bitfields = 0x03,
        Jpeg = 0x04,
        Png = 0x05,
        Cmyk = 0x0B,
        Cmykrle8 = 0x0C,
        Cmykrle4 = 0x0D,
    }

    internal static List<CameraPath> AvailableCameras()
    {
        DsDevice[] dsDevices = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);

        List<CameraPath> devices = [];
        for (int i = 0; i < dsDevices.Length; i++)
        {
            DsDevice dsDevice = dsDevices[i];
            devices.Add(new CameraPath
            {
                Name = dsDevice.Name,
                Path = dsDevice.DevicePath,
            });
        }
        return devices;
    }

    /// <summary>
    /// Get available video input options of a DirectShow device.
    /// Inspired from https://github.com/eldhosekpaul18/WebCam-Capture-Opencvsharp/blob/master/Camera%20Configuration/Camera.cs
    /// </summary>
    internal static List<VideoFormat> GetAvailableFormats(DsDevice dsDevice)
    {
        List<VideoFormat> options = new List<VideoFormat>();

        try
        {
            uint bitCount = 0;

            IFilterGraph2 filterGraph = (IFilterGraph2)new FilterGraph();
            filterGraph.AddSourceFilterForMoniker(dsDevice.Mon, null, dsDevice.Name, out IBaseFilter sourceFilter);
            IPin rawPin = DsFindPin.ByCategory(sourceFilter, PinCategory.Capture, 0);

            VideoInfoHeader v = new VideoInfoHeader();
            rawPin.EnumMediaTypes(out IEnumMediaTypes mediaTypeEnum);

            AMMediaType[] mediaTypes = new AMMediaType[1];
            IntPtr fetched = IntPtr.Zero;
            mediaTypeEnum.Next(1, mediaTypes, fetched);

            for (; mediaTypes[0] != null; mediaTypeEnum.Next(1, mediaTypes, fetched))
            {
                Marshal.PtrToStructure(mediaTypes[0].formatPtr, v);

                if (v.BmiHeader.Size != 0 && v.BmiHeader.BitCount != 0)
                {
                    if (v.BmiHeader.BitCount > bitCount)
                        bitCount = (uint)v.BmiHeader.BitCount;

                    // Part of code inspired from dshow_get_format_info in dshow.c
                    // https://github.com/FFmpeg/FFmpeg/blob/a64e250680fbc7296eff714b81b54b1c0e2d185f/libavdevice/dshow.c#L692-L759
                    ImageFormat? maybeImageFormat = dshowImageFormat(v.BmiHeader.Compression, bitCount);

                    if (maybeImageFormat is null)
                    {
                        // https://learn.microsoft.com/en-us/windows/win32/directshow/h-264-video-types:
                        if (mediaTypes[0].subType.Equals(MediaSubType.Video.H264)
                            || mediaTypes[0].subType.Equals(MediaSubType.Video.h264)
                            || mediaTypes[0].subType.Equals(MediaSubType.Video.X264)
                            || mediaTypes[0].subType.Equals(MediaSubType.Video.x264)
                            || mediaTypes[0].subType.Equals(MediaSubType.Video.Avc1)
                            || mediaTypes[0].subType.Equals(MediaSubType.Video.avc1))
                        {
                            // SeeShark doesn't support H264 input
                        }
                        else if (Equals(mediaTypes[0].subType, MediaSubType.MJPG))
                        {
                            maybeImageFormat = ImageFormat.Mjpeg;
                        }
                        else
                        {
                            // TODO: remove? maybe? idk
                            Console.Error.WriteLine($"Warning: could not handle media type {mediaTypes[0].subType}");
                        }
                    }

                    if (maybeImageFormat is ImageFormat imageFormat)
                    {
                        VideoFormat format = new VideoFormat
                        {
                            VideoSize = ((uint)v.BmiHeader.Width, (uint)v.BmiHeader.Height),
                            ImageFormat = imageFormat,
                            // https://docs.microsoft.com/en-us/windows/win32/directshow/configure-the-video-output-format
                            // "frames per second = 10,000,000 / frame duration"
                            Framerate = new FramerateRatio
                            {
                                Numerator = 10_000_000,
                                Denominator = (uint)v.AvgTimePerFrame,
                            },
                        };

                        options.Add(format);
                    }
                }
            }
        }
        catch (Exception)
        {
        }

        return options;
    }

    /// <summary>
    /// Ported from libavdevice/dshow.c - dshow_pixfmt.
    /// See https://github.com/FFmpeg/FFmpeg/blob/a64e250680fbc7296eff714b81b54b1c0e2d185f/libavdevice/dshow.c#L59-L80
    /// </summary>
    private static ImageFormat? dshowImageFormat(int compression, uint bitCount)
    {
        if (compression == (int)BitmapCompression.Bitfields || compression == (int)BitmapCompression.Rgb)
        {
            // Caution: There's something going on with BE vs LE pixel formats that I don't fully understand.
            // I'm using little endian variants of the missing pixel formats until I find a better solution.
            // https://github.com/FFmpeg/FFmpeg/blob/a64e250680fbc7296eff714b81b54b1c0e2d185f/libavutil/pixfmt.h#L373-L377

            // 1-8 are untested
            switch (bitCount)
            {
                case 1:
                    return ImageFormat.Monowhite;
                case 4:
                    return ImageFormat.Rgb4;
                case 8:
                    return ImageFormat.Rgb8;
                case 16:
                    // This pixel format was originally RGB555.
                    // https://github.com/FFmpeg/FFmpeg/blob/a64e250680fbc7296eff714b81b54b1c0e2d185f/libavutil/pixfmt.h#L394
                    return ImageFormat.Rgb555Le;
                case 24:
                    return ImageFormat.Bgr24;
                case 32:
                    // This pixel format was originally 0RGB32.
                    // https://github.com/FFmpeg/FFmpeg/blob/a64e250680fbc7296eff714b81b54b1c0e2d185f/libavutil/pixfmt.h#L383
                    return ImageFormat.Bgrx;
            }
        }

        // All others
        return ImageFormatTag.FindRawPixelFormat(compression);
    }
}