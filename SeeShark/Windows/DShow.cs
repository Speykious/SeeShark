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
        foreach (DsDevice dsDevice in dsDevices)
        {
            devices.Add(new CameraPath
            {
                Name = dsDevice.Name,
                Path = dsDevice.DevicePath,
            });
        }
        return devices;
    }

    internal static DsDevice? FindCameraDevice(CameraPath cameraPath)
    {
        // TODO: ...Is there really no function to find a device by its device path? This is ridiculous
        foreach (DsDevice dsDevice in DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice))
        {
            if (dsDevice.DevicePath == cameraPath.Path)
                return dsDevice;
        }

        return null;
    }

    internal static List<VideoFormat> AvailableFormats(CameraPath cameraPath)
    {
        if (FindCameraDevice(cameraPath) is DsDevice device)
            return AvailableFormats(device);
        else
            return [];
    }

    /// <summary>
    /// Get available video input options of a DirectShow device.
    /// Inspired from https://github.com/eldhosekpaul18/WebCam-Capture-Opencvsharp/blob/master/Camera%20Configuration/Camera.cs
    /// </summary>
    internal static List<VideoFormat> AvailableFormats(DsDevice dsDevice)
    {
        List<VideoFormat> options = [];

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
                    ImageFormat? maybeImageFormat = DShowImageFormat(v.BmiHeader.Compression, bitCount);

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
                            // TODO: logging, perhaps
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

    internal static int ConnectFilters(IGraphBuilder filterGraphBuilder, IPin outputPin, IBaseFilter downstreamFilter)
    {
        FindUnconnectedPin(downstreamFilter, PinDirection.Input, out IPin? inputPin);
        if (inputPin is IPin pin)
            return filterGraphBuilder.Connect(outputPin, pin);

        return 0;
    }

    // Return the first unconnected input pin or output pin.
    internal static void FindUnconnectedPin(IBaseFilter filterGraph, PinDirection pinDirection, out IPin? unconnectedPin)
    {
        unconnectedPin = null;

        Marshal.ThrowExceptionForHR(filterGraph.EnumPins(out IEnumPins enumPins));

        IPin[] pinHolder = new IPin[1];
        while (enumPins.Next(1, pinHolder, 0) == 0)
        {
            Marshal.ThrowExceptionForHR(MatchPin(pinHolder[0], pinDirection, false, out bool found));

            if (found)
            {
                unconnectedPin = pinHolder[0];
                break;
            }
        }
    }

    // Match a pin by pin direction and connection state.
    internal static int MatchPin(IPin pin, PinDirection direction, bool shouldBeConnected, out bool result)
    {
        result = false;

        bool matches = false;

        int hr = IsPinConnected(pin, out bool isConnected);
        if (hr == 0)
        {
            if (isConnected == shouldBeConnected)
                hr = IsPinDirection(pin, direction, out matches);
        }

        if (hr == 0)
            result = matches;

        return hr;
    }

    // Query whether a pin has a specified direction (input / output)
    internal static int IsPinDirection(IPin pin, PinDirection dir, out bool result)
    {
        int hr = pin.QueryDirection(out PinDirection pinDir);
        result = (hr == 0) && (pinDir == dir);
        return hr;
    }

    private const int vfw_e_not_connected = unchecked((int)0x80040209);

    internal static int IsPinConnected(IPin pin, out bool result)
    {
        result = false;

        int hr = pin.ConnectedTo(out IPin _tempPin);
        if (hr == 0)
            result = true;
        else if (hr == vfw_e_not_connected)
            hr = 0; // The pin is not connected. This is not an error for our purposes.

        return hr;
    }

    /// <summary>
    /// Ported from libavdevice/dshow.c - dshow_pixfmt.
    /// See https://github.com/FFmpeg/FFmpeg/blob/a64e250680fbc7296eff714b81b54b1c0e2d185f/libavdevice/dshow.c#L59-L80
    /// </summary>
    internal static ImageFormat? DShowImageFormat(int compression, uint bitCount)
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

    internal static string AMMediaTypeToReadableString(AMMediaType mediaType)
    {
#pragma warning disable format
        string majorType;
             if (mediaType.majorType == MediaType.Null)            majorType = "Null";
        else if (mediaType.majorType == MediaType.Video)           majorType = "Video";
        else if (mediaType.majorType == MediaType.Video)           majorType = "Video";
        else if (mediaType.majorType == MediaType.Interleaved)     majorType = "Interleaved";
        else if (mediaType.majorType == MediaType.Audio)           majorType = "Audio";
        else if (mediaType.majorType == MediaType.Subtitle)        majorType = "Subtitle";
        else if (mediaType.majorType == MediaType.FileSourceAsync) majorType = "FileSourceAsync";
        else if (mediaType.majorType == MediaType.Texts)           majorType = "Texts";
        else if (mediaType.majorType == MediaType.Stream)          majorType = "Stream";
        else if (mediaType.majorType == MediaType.VBI)             majorType = "VBI";
        else if (mediaType.majorType == MediaType.Midi)            majorType = "Midi";
        else if (mediaType.majorType == MediaType.File)            majorType = "File";
        else if (mediaType.majorType == MediaType.ScriptCommand)   majorType = "ScriptCommand";
        else if (mediaType.majorType == MediaType.AuxLine21Data)   majorType = "AuxLine21Data";
        else if (mediaType.majorType == MediaType.Timecode)        majorType = "Timecode";
        else if (mediaType.majorType == MediaType.LMRT)            majorType = "LMRT";
        else if (mediaType.majorType == MediaType.URLStream)       majorType = "URLStream";
        else if (mediaType.majorType == MediaType.AnalogVideo)     majorType = "AnalogVideo";
        else if (mediaType.majorType == MediaType.AnalogAudio)     majorType = "AnalogAudio";
        else if (mediaType.majorType == MediaType.Mpeg2Sections)   majorType = "Mpeg2Sections";
        else if (mediaType.majorType == MediaType.DTVCCData)       majorType = "DTVCCData";
        else if (mediaType.majorType == MediaType.MSTVCaption)     majorType = "MSTVCaption";
        else if (mediaType.majorType == MediaType.AUXTeletextPage) majorType = "AUXTeletextPage";
        else if (mediaType.majorType == MediaType.CC_Container)    majorType = "CC_Container";
        else                                                       majorType = "????";

        string subType;
             if (mediaType.subType == MediaSubType.Audio.Aac)                     subType = "A:Aac";
        else if (mediaType.subType == MediaSubType.Audio.LatmAac)                 subType = "A:LatmAac";
        else if (mediaType.subType == MediaSubType.Audio.MpegLoas)                subType = "A:MpegLoas";
        else if (mediaType.subType == MediaSubType.Audio.AacAdts)                 subType = "A:AacAdts";
        else if (mediaType.subType == MediaSubType.Audio.MpegAdtsAac)             subType = "A:MpegAdtsAac";
        else if (mediaType.subType == MediaSubType.Audio.MpegRawAac)              subType = "A:MpegRawAac";
        else if (mediaType.subType == MediaSubType.Audio.NokiaMpegAdtsAac)        subType = "A:NokiaMpegAdtsAac";
        else if (mediaType.subType == MediaSubType.Audio.NokiaMpegRawAac)         subType = "A:NokiaMpegRawAac";
        else if (mediaType.subType == MediaSubType.Audio.VodafoneMpegAdtsAac)     subType = "A:VodafoneMpegAdtsAac";
        else if (mediaType.subType == MediaSubType.Audio.VodafoneMpegRawAac)      subType = "A:VodafoneMpegRawAac";
        else if (mediaType.subType == MediaSubType.Audio.MpegHeAac)               subType = "A:MpegHeAac";
        else if (mediaType.subType == MediaSubType.Audio.Als)                     subType = "A:Als";
        else if (mediaType.subType == MediaSubType.Audio.Mpeg4Audio)              subType = "A:Mpeg4Audio";
        else if (mediaType.subType == MediaSubType.Audio.Mpeg4AudioAdvanced)      subType = "A:Mpeg4AudioAdvanced";
        else if (mediaType.subType == MediaSubType.Audio.DolbyWaveAc3)            subType = "A:DolbyWaveAc3";
        else if (mediaType.subType == MediaSubType.Audio.DolbyAc3Spdif)           subType = "A:DolbyAc3Spdif";
        else if (mediaType.subType == MediaSubType.Audio.DolbyEac3)               subType = "A:DolbyEac3";
        else if (mediaType.subType == MediaSubType.Audio.DolbyArcsoftEac3)        subType = "A:DolbyArcsoftEac3";
        else if (mediaType.subType == MediaSubType.Audio.DolbyTrueHd)             subType = "A:DolbyTrueHd";
        else if (mediaType.subType == MediaSubType.Audio.DolbyArcsoftTrueHd)      subType = "A:DolbyArcsoftTrueHd";
        else if (mediaType.subType == MediaSubType.Audio.DolbyAc3)                subType = "A:DolbyAc3";
        else if (mediaType.subType == MediaSubType.Audio.WaveDts)                 subType = "A:WaveDts";
        else if (mediaType.subType == MediaSubType.Audio.Dts)                     subType = "A:Dts";
        else if (mediaType.subType == MediaSubType.Audio.DtsHd)                   subType = "A:DtsHd";
        else if (mediaType.subType == MediaSubType.Audio.Mpeg1Packet)             subType = "A:Mpeg1Packet";
        else if (mediaType.subType == MediaSubType.Audio.Mpeg1Payload)            subType = "A:Mpeg1Payload";
        else if (mediaType.subType == MediaSubType.Audio.Mpeg1AudioPayload)       subType = "A:Mpeg1AudioPayload";
        else if (mediaType.subType == MediaSubType.Audio.Mp3)                     subType = "A:Mp3";
        else if (mediaType.subType == MediaSubType.Audio.Mpeg2Audio)              subType = "A:Mpeg2Audio";
        else if (mediaType.subType == MediaSubType.Audio.Flac)                    subType = "A:Flac";
        else if (mediaType.subType == MediaSubType.Audio.FlacFramed)              subType = "A:FlacFramed";
        else if (mediaType.subType == MediaSubType.Audio.Vorbis2)                 subType = "A:Vorbis2";
        else if (mediaType.subType == MediaSubType.Audio.Tta1)                    subType = "A:Tta1";
        else if (mediaType.subType == MediaSubType.Audio.WavPack4)                subType = "A:WavPack4";
        else if (mediaType.subType == MediaSubType.Audio.Mlp)                     subType = "A:Mlp";
        else if (mediaType.subType == MediaSubType.Audio.Alac)                    subType = "A:Alac";
        else if (mediaType.subType == MediaSubType.Audio.Aes3)                    subType = "A:Aes3";
        else if (mediaType.subType == MediaSubType.Audio.DvdLpcmAudio)            subType = "A:DvdLpcmAudio";
        else if (mediaType.subType == MediaSubType.Audio.BdLpcmAudio)             subType = "A:BdLpcmAudio";
        else if (mediaType.subType == MediaSubType.Audio.HdmvLpcmAudio)           subType = "A:HdmvLpcmAudio";
        else if (mediaType.subType == MediaSubType.Audio.PcmNone)                 subType = "A:PcmNone";
        else if (mediaType.subType == MediaSubType.Audio.PcmRaw)                  subType = "A:PcmRaw";
        else if (mediaType.subType == MediaSubType.Audio.PcmTwos)                 subType = "A:PcmTwos";
        else if (mediaType.subType == MediaSubType.Audio.PcmSowt)                 subType = "A:PcmSowt";
        else if (mediaType.subType == MediaSubType.Audio.PcmIn24)                 subType = "A:PcmIn24";
        else if (mediaType.subType == MediaSubType.Audio.PcmIn32)                 subType = "A:PcmIn32";
        else if (mediaType.subType == MediaSubType.Audio.PcmFl32)                 subType = "A:PcmFl32";
        else if (mediaType.subType == MediaSubType.Audio.PcmFl64)                 subType = "A:PcmFl64";
        else if (mediaType.subType == MediaSubType.Audio.PcmIn24Le)               subType = "A:PcmIn24Le";
        else if (mediaType.subType == MediaSubType.Audio.PcmIn32Le)               subType = "A:PcmIn32Le";
        else if (mediaType.subType == MediaSubType.Audio.PcmFl32Le)               subType = "A:PcmFl32Le";
        else if (mediaType.subType == MediaSubType.Audio.PcmFl64Le)               subType = "A:PcmFl64Le";
        else if (mediaType.subType == MediaSubType.Audio.MsAudio)                 subType = "A:MsAudio";
        else if (mediaType.subType == MediaSubType.Audio.WmAudio2)                subType = "A:WmAudio2";
        else if (mediaType.subType == MediaSubType.Audio.WmAudio3)                subType = "A:WmAudio3";
        else if (mediaType.subType == MediaSubType.Audio.WmAudioLossless)         subType = "A:WmAudioLossless";
        else if (mediaType.subType == MediaSubType.Audio.WmaSpdif)                subType = "A:WmaSpdif";
        else if (mediaType.subType == MediaSubType.Audio.WmAudio4)                subType = "A:WmAudio4";
        else if (mediaType.subType == MediaSubType.Audio.Cook)                    subType = "A:Cook";
        else if (mediaType.subType == MediaSubType.Audio.Raac)                    subType = "A:Raac";
        else if (mediaType.subType == MediaSubType.Audio.Racp)                    subType = "A:Racp";
        else if (mediaType.subType == MediaSubType.Audio.Sipr)                    subType = "A:Sipr";
        else if (mediaType.subType == MediaSubType.Audio.SiprWave)                subType = "A:SiprWave";
        else if (mediaType.subType == MediaSubType.Audio.Dnet)                    subType = "A:Dnet";
        else if (mediaType.subType == MediaSubType.Audio.Ra28_8)                  subType = "A:Ra28_8";
        else if (mediaType.subType == MediaSubType.Audio.Ra14_4)                  subType = "A:Ra14_4";
        else if (mediaType.subType == MediaSubType.Audio.Ralf)                    subType = "A:Ralf";
        else if (mediaType.subType == MediaSubType.Audio.Dsdl)                    subType = "A:Dsdl";
        else if (mediaType.subType == MediaSubType.Audio.Dsdm)                    subType = "A:Dsdm";
        else if (mediaType.subType == MediaSubType.Audio.Dsd1)                    subType = "A:Dsd1";
        else if (mediaType.subType == MediaSubType.Audio.Dsd8)                    subType = "A:Dsd8";
        else if (mediaType.subType == MediaSubType.Audio.Pcm)                     subType = "A:Pcm";
        else if (mediaType.subType == MediaSubType.Audio.DrmAudio)                subType = "A:DrmAudio";
        else if (mediaType.subType == MediaSubType.Audio.IeeeFloat)               subType = "A:IeeeFloat";
        else if (mediaType.subType == MediaSubType.Audio.RawSport)                subType = "A:RawSport";
        else if (mediaType.subType == MediaSubType.Audio.SpdifTag241h)            subType = "A:SpdifTag241h";
        else if (mediaType.subType == MediaSubType.Audio.PcmAudioObsolete)        subType = "A:PcmAudioObsolete";
        else if (mediaType.subType == MediaSubType.Audio.Wave)                    subType = "A:Wave";
        else if (mediaType.subType == MediaSubType.Audio.Amr)                     subType = "A:Amr";
        else if (mediaType.subType == MediaSubType.Audio.FfmpegAudio)             subType = "A:FfmpegAudio";
        else if (mediaType.subType == MediaSubType.Audio.Speex)                   subType = "A:Speex";
        else if (mediaType.subType == MediaSubType.Audio.Opus)                    subType = "A:Opus";
        else if (mediaType.subType == MediaSubType.Audio.Samr)                    subType = "A:Samr";
        else if (mediaType.subType == MediaSubType.Audio.Nellymoser)              subType = "A:Nellymoser";
        else if (mediaType.subType == MediaSubType.Audio.Alaw)                    subType = "A:Alaw";
        else if (mediaType.subType == MediaSubType.Audio.Mulaw)                   subType = "A:Mulaw";
        else if (mediaType.subType == MediaSubType.Audio.MsGsm610)                subType = "A:MsGsm610";
        else if (mediaType.subType == MediaSubType.Audio.AdPmcMs)                 subType = "A:AdPmcMs";
        else if (mediaType.subType == MediaSubType.Audio.TrueSpeech)              subType = "A:TrueSpeech";
        else if (mediaType.subType == MediaSubType.Audio.Qdm2)                    subType = "A:Qdm2";
        else if (mediaType.subType == MediaSubType.Audio.Rt29)                    subType = "A:Rt29";
        else if (mediaType.subType == MediaSubType.Audio.Atrac3)                  subType = "A:Atrac3";
        else if (mediaType.subType == MediaSubType.Audio.Atrc)                    subType = "A:Atrc";
        else if (mediaType.subType == MediaSubType.Audio.Atrac3P)                 subType = "A:Atrac3P";
        else if (mediaType.subType == MediaSubType.Audio.Bink)                    subType = "A:Bink";
        else if (mediaType.subType == MediaSubType.Video.H264)                    subType = "V:H264";
        else if (mediaType.subType == MediaSubType.Video.h264)                    subType = "V:h264";
        else if (mediaType.subType == MediaSubType.Video.X264)                    subType = "V:X264";
        else if (mediaType.subType == MediaSubType.Video.x264)                    subType = "V:x264";
        else if (mediaType.subType == MediaSubType.Video.Avc1)                    subType = "V:Avc1";
        else if (mediaType.subType == MediaSubType.Video.avc1)                    subType = "V:avc1";
        else if (mediaType.subType == MediaSubType.Video.Ccv1)                    subType = "V:Ccv1";
        else if (mediaType.subType == MediaSubType.Video.H264Bis)                 subType = "V:H264Bis";
        else if (mediaType.subType == MediaSubType.Video.Amvc)                    subType = "V:Amvc";
        else if (mediaType.subType == MediaSubType.Video.Mvc1)                    subType = "V:Mvc1";
        else if (mediaType.subType == MediaSubType.Video.Hevc)                    subType = "V:Hevc";
        else if (mediaType.subType == MediaSubType.Video.Hvc1)                    subType = "V:Hvc1";
        else if (mediaType.subType == MediaSubType.Video.Hm10)                    subType = "V:Hm10";
        else if (mediaType.subType == MediaSubType.Video.Mpeg1Payload)            subType = "V:Mpeg1Payload";
        else if (mediaType.subType == MediaSubType.Video.Bink)                    subType = "V:Bink";
        else if (mediaType.subType == MediaSubType.Video.Div3)                    subType = "V:Div3";
        else if (mediaType.subType == MediaSubType.Video.div3)                    subType = "V:div3";
        else if (mediaType.subType == MediaSubType.Video.Dx50)                    subType = "V:Dx50";
        else if (mediaType.subType == MediaSubType.Video.dx50)                    subType = "V:dx50";
        else if (mediaType.subType == MediaSubType.Video.Divx)                    subType = "V:Divx";
        else if (mediaType.subType == MediaSubType.Video.divx)                    subType = "V:divx";
        else if (mediaType.subType == MediaSubType.Video.Dvsd)                    subType = "V:Dvsd";
        else if (mediaType.subType == MediaSubType.Video.Dvhd)                    subType = "V:Dvhd";
        else if (mediaType.subType == MediaSubType.Video.Dvsl)                    subType = "V:Dvsl";
        else if (mediaType.subType == MediaSubType.Video.Dvcp)                    subType = "V:Dvcp";
        else if (mediaType.subType == MediaSubType.Video.Flv1)                    subType = "V:Flv1";
        else if (mediaType.subType == MediaSubType.Video.flv1)                    subType = "V:flv1";
        else if (mediaType.subType == MediaSubType.Video.Flv4)                    subType = "V:Flv4";
        else if (mediaType.subType == MediaSubType.Video.flv4)                    subType = "V:flv4";
        else if (mediaType.subType == MediaSubType.Video.Fps1)                    subType = "V:Fps1";
        else if (mediaType.subType == MediaSubType.Null)                          subType = "Null";
        else if (mediaType.subType == MediaSubType.CLPL)                          subType = "CLPL";
        else if (mediaType.subType == MediaSubType.YUYV)                          subType = "YUYV";
        else if (mediaType.subType == MediaSubType.IYUV)                          subType = "IYUV";
        else if (mediaType.subType == MediaSubType.YVU9)                          subType = "YVU9";
        else if (mediaType.subType == MediaSubType.Y411)                          subType = "Y411";
        else if (mediaType.subType == MediaSubType.Y41P)                          subType = "Y41P";
        else if (mediaType.subType == MediaSubType.YUY2)                          subType = "YUY2";
        else if (mediaType.subType == MediaSubType.YVYU)                          subType = "YVYU";
        else if (mediaType.subType == MediaSubType.UYVY)                          subType = "UYVY";
        else if (mediaType.subType == MediaSubType.Y211)                          subType = "Y211";
        else if (mediaType.subType == MediaSubType.YV16)                          subType = "YV16";
        else if (mediaType.subType == MediaSubType.YV24)                          subType = "YV24";
        else if (mediaType.subType == MediaSubType.v210)                          subType = "v210";
        else if (mediaType.subType == MediaSubType.v216)                          subType = "v216";
        else if (mediaType.subType == MediaSubType.Y410)                          subType = "Y410";
        else if (mediaType.subType == MediaSubType.Y416)                          subType = "Y416";
        else if (mediaType.subType == MediaSubType.v410)                          subType = "v410";
        else if (mediaType.subType == MediaSubType.CLJR)                          subType = "CLJR";
        else if (mediaType.subType == MediaSubType.IF09)                          subType = "IF09";
        else if (mediaType.subType == MediaSubType.CPLA)                          subType = "CPLA";
        else if (mediaType.subType == MediaSubType.MJPG)                          subType = "MJPG";
        else if (mediaType.subType == MediaSubType.TVMJ)                          subType = "TVMJ";
        else if (mediaType.subType == MediaSubType.WAKE)                          subType = "WAKE";
        else if (mediaType.subType == MediaSubType.CFCC)                          subType = "CFCC";
        else if (mediaType.subType == MediaSubType.IJPG)                          subType = "IJPG";
        else if (mediaType.subType == MediaSubType.PLUM)                          subType = "PLUM";
        else if (mediaType.subType == MediaSubType.DVCS)                          subType = "DVCS";
        else if (mediaType.subType == MediaSubType.DVSD)                          subType = "DVSD";
        else if (mediaType.subType == MediaSubType.MDVF)                          subType = "MDVF";
        else if (mediaType.subType == MediaSubType.RGB1)                          subType = "RGB1";
        else if (mediaType.subType == MediaSubType.RGB4)                          subType = "RGB4";
        else if (mediaType.subType == MediaSubType.RGB8)                          subType = "RGB8";
        else if (mediaType.subType == MediaSubType.RGB565)                        subType = "RGB565";
        else if (mediaType.subType == MediaSubType.RGB555)                        subType = "RGB555";
        else if (mediaType.subType == MediaSubType.RGB24)                         subType = "RGB24";
        else if (mediaType.subType == MediaSubType.RGB32)                         subType = "RGB32";
        else if (mediaType.subType == MediaSubType.DXVA_H264_E)                   subType = "DXVA_H264_E";
        else if (mediaType.subType == MediaSubType.DXVA_H264_F)                   subType = "DXVA_H264_F";
        else if (mediaType.subType == MediaSubType.DXVA_VC1_A)                    subType = "DXVA_VC1_A";
        else if (mediaType.subType == MediaSubType.DXVA_VC1_B)                    subType = "DXVA_VC1_B";
        else if (mediaType.subType == MediaSubType.DXVA_VC1_C)                    subType = "DXVA_VC1_C";
        else if (mediaType.subType == MediaSubType.DXVA_VC1_D)                    subType = "DXVA_VC1_D";
        else if (mediaType.subType == MediaSubType.DXVA_MPEG2_A)                  subType = "DXVA_MPEG2_A";
        else if (mediaType.subType == MediaSubType.DXVA_MPEG2_B)                  subType = "DXVA_MPEG2_B";
        else if (mediaType.subType == MediaSubType.DXVA_MPEG2_C)                  subType = "DXVA_MPEG2_C";
        else if (mediaType.subType == MediaSubType.DXVA_MPEG2_D)                  subType = "DXVA_MPEG2_D";
        else if (mediaType.subType == MediaSubType.DXVA_WMV9_A)                   subType = "DXVA_WMV9_A";
        else if (mediaType.subType == MediaSubType.DXVA_WMV9_B)                   subType = "DXVA_WMV9_B";
        else if (mediaType.subType == MediaSubType.DXVA_WMV9_C)                   subType = "DXVA_WMV9_C";
        else if (mediaType.subType == MediaSubType.DXVA_WMV8_A)                   subType = "DXVA_WMV8_A";
        else if (mediaType.subType == MediaSubType.DXVA_WMV8_B)                   subType = "DXVA_WMV8_B";
        else if (mediaType.subType == MediaSubType.ARGB1555)                      subType = "ARGB1555";
        else if (mediaType.subType == MediaSubType.ARGB4444)                      subType = "ARGB4444";
        else if (mediaType.subType == MediaSubType.ARGB32)                        subType = "ARGB32";
        else if (mediaType.subType == MediaSubType.A2R10G10B10)                   subType = "A2R10G10B10";
        else if (mediaType.subType == MediaSubType.A2B10G10R10)                   subType = "A2B10G10R10";
        else if (mediaType.subType == MediaSubType.AYUV)                          subType = "AYUV";
        else if (mediaType.subType == MediaSubType.AI44)                          subType = "AI44";
        else if (mediaType.subType == MediaSubType.IA44)                          subType = "IA44";
        else if (mediaType.subType == MediaSubType.RGB32_D3D_DX7_RT)              subType = "RGB32_D3D_DX7_RT";
        else if (mediaType.subType == MediaSubType.RGB16_D3D_DX7_RT)              subType = "RGB16_D3D_DX7_RT";
        else if (mediaType.subType == MediaSubType.ARGB32_D3D_DX7_RT)             subType = "ARGB32_D3D_DX7_RT";
        else if (mediaType.subType == MediaSubType.ARGB4444_D3D_DX7_RT)           subType = "ARGB4444_D3D_DX7_RT";
        else if (mediaType.subType == MediaSubType.ARGB1555_D3D_DX7_RT)           subType = "ARGB1555_D3D_DX7_RT";
        else if (mediaType.subType == MediaSubType.RGB32_D3D_DX9_RT)              subType = "RGB32_D3D_DX9_RT";
        else if (mediaType.subType == MediaSubType.RGB16_D3D_DX9_RT)              subType = "RGB16_D3D_DX9_RT";
        else if (mediaType.subType == MediaSubType.ARGB32_D3D_DX9_RT)             subType = "ARGB32_D3D_DX9_RT";
        else if (mediaType.subType == MediaSubType.ARGB4444_D3D_DX9_RT)           subType = "ARGB4444_D3D_DX9_RT";
        else if (mediaType.subType == MediaSubType.ARGB1555_D3D_DX9_RT)           subType = "ARGB1555_D3D_DX9_RT";
        else if (mediaType.subType == MediaSubType.YV12)                          subType = "YV12";
        else if (mediaType.subType == MediaSubType.NV12)                          subType = "NV12";
        else if (mediaType.subType == MediaSubType.IMC1)                          subType = "IMC1";
        else if (mediaType.subType == MediaSubType.IMC2)                          subType = "IMC2";
        else if (mediaType.subType == MediaSubType.IMC3)                          subType = "IMC3";
        else if (mediaType.subType == MediaSubType.IMC4)                          subType = "IMC4";
        else if (mediaType.subType == MediaSubType.S340)                          subType = "S340";
        else if (mediaType.subType == MediaSubType.S342)                          subType = "S342";
        else if (mediaType.subType == MediaSubType.Overlay)                       subType = "Overlay";
        else if (mediaType.subType == MediaSubType.MPEG1SystemStream)             subType = "MPEG1SystemStream";
        else if (mediaType.subType == MediaSubType.MPEG1System)                   subType = "MPEG1System";
        else if (mediaType.subType == MediaSubType.MPEG1VideoCD)                  subType = "MPEG1VideoCD";
        else if (mediaType.subType == MediaSubType.MPEG1Video)                    subType = "MPEG1Video";
        else if (mediaType.subType == MediaSubType.MPEG1Audio)                    subType = "MPEG1Audio";
        else if (mediaType.subType == MediaSubType.Avi)                           subType = "Avi";
        else if (mediaType.subType == MediaSubType.Asf)                           subType = "Asf";
        else if (mediaType.subType == MediaSubType.QTMovie)                       subType = "QTMovie";
        else if (mediaType.subType == MediaSubType.QTRpza)                        subType = "QTRpza";
        else if (mediaType.subType == MediaSubType.QTSmc)                         subType = "QTSmc";
        else if (mediaType.subType == MediaSubType.QTRle)                         subType = "QTRle";
        else if (mediaType.subType == MediaSubType.QTJpeg)                        subType = "QTJpeg";
        else if (mediaType.subType == MediaSubType.AU)                            subType = "AU";
        else if (mediaType.subType == MediaSubType.AIFF)                          subType = "AIFF";
        else if (mediaType.subType == MediaSubType.dvhd)                          subType = "dvhd";
        else if (mediaType.subType == MediaSubType.dvsl)                          subType = "dvsl";
        else if (mediaType.subType == MediaSubType.dv25)                          subType = "dv25";
        else if (mediaType.subType == MediaSubType.dv50)                          subType = "dv50";
        else if (mediaType.subType == MediaSubType.dvh1)                          subType = "dvh1";
        else if (mediaType.subType == MediaSubType.Line21_BytePair)               subType = "Line21_BytePair";
        else if (mediaType.subType == MediaSubType.Line21_GOPPacket)              subType = "Line21_GOPPacket";
        else if (mediaType.subType == MediaSubType.Line21_VBIRawData)             subType = "Line21_VBIRawData";
        else if (mediaType.subType == MediaSubType.TELETEXT)                      subType = "TELETEXT";
        else if (mediaType.subType == MediaSubType.WSS)                           subType = "WSS";
        else if (mediaType.subType == MediaSubType.VPS)                           subType = "VPS";
        else if (mediaType.subType == MediaSubType.DssVideo)                      subType = "DssVideo";
        else if (mediaType.subType == MediaSubType.DssAudio)                      subType = "DssAudio";
        else if (mediaType.subType == MediaSubType.VPVideo)                       subType = "VPVideo";
        else if (mediaType.subType == MediaSubType.VPVBI)                         subType = "VPVBI";
        else if (mediaType.subType == MediaSubType.AnalogVideo_NTSC_M)            subType = "AnalogVideo_NTSC_M";
        else if (mediaType.subType == MediaSubType.AnalogVideo_PAL_B)             subType = "AnalogVideo_PAL_B";
        else if (mediaType.subType == MediaSubType.AnalogVideo_PAL_D)             subType = "AnalogVideo_PAL_D";
        else if (mediaType.subType == MediaSubType.AnalogVideo_PAL_G)             subType = "AnalogVideo_PAL_G";
        else if (mediaType.subType == MediaSubType.AnalogVideo_PAL_H)             subType = "AnalogVideo_PAL_H";
        else if (mediaType.subType == MediaSubType.AnalogVideo_PAL_I)             subType = "AnalogVideo_PAL_I";
        else if (mediaType.subType == MediaSubType.AnalogVideo_PAL_M)             subType = "AnalogVideo_PAL_M";
        else if (mediaType.subType == MediaSubType.AnalogVideo_PAL_N)             subType = "AnalogVideo_PAL_N";
        else if (mediaType.subType == MediaSubType.AnalogVideo_PAL_N_COMBO)       subType = "AnalogVideo_PAL_N_COMBO";
        else if (mediaType.subType == MediaSubType.AnalogVideo_SECAM_B)           subType = "AnalogVideo_SECAM_B";
        else if (mediaType.subType == MediaSubType.AnalogVideo_SECAM_D)           subType = "AnalogVideo_SECAM_D";
        else if (mediaType.subType == MediaSubType.AnalogVideo_SECAM_G)           subType = "AnalogVideo_SECAM_G";
        else if (mediaType.subType == MediaSubType.AnalogVideo_SECAM_H)           subType = "AnalogVideo_SECAM_H";
        else if (mediaType.subType == MediaSubType.AnalogVideo_SECAM_K)           subType = "AnalogVideo_SECAM_K";
        else if (mediaType.subType == MediaSubType.AnalogVideo_SECAM_K1)          subType = "AnalogVideo_SECAM_K1";
        else if (mediaType.subType == MediaSubType.AnalogVideo_SECAM_L)           subType = "AnalogVideo_SECAM_L";
        else if (mediaType.subType == MediaSubType.I420)                          subType = "I420";
        else if (mediaType.subType == MediaSubType.VideoImage)                    subType = "VideoImage";
        else if (mediaType.subType == MediaSubType.Mpeg2Video)                    subType = "Mpeg2Video";
        else if (mediaType.subType == MediaSubType.WebStream)                     subType = "WebStream";
        else if (mediaType.subType == MediaSubType.Mpeg2Audio)                    subType = "Mpeg2Audio";
        else if (mediaType.subType == MediaSubType.Mpeg2DvD)                      subType = "Mpeg2DvD";
        else if (mediaType.subType == MediaSubType.DvbSI)                         subType = "DvbSI";
        else if (mediaType.subType == MediaSubType.AtscSI)                        subType = "AtscSI";
        else if (mediaType.subType == MediaSubType.Mpeg2Data)                     subType = "Mpeg2Data";
        else if (mediaType.subType == MediaSubType.Mpeg2Program)                  subType = "Mpeg2Program";
        else if (mediaType.subType == MediaSubType.Mpeg2Transport)                subType = "Mpeg2Transport";
        else if (mediaType.subType == MediaSubType.Mpeg2TransportStride)          subType = "Mpeg2TransportStride";
        else if (mediaType.subType == MediaSubType.None)                          subType = "None";
        else if (mediaType.subType == MediaSubType.BdaMpeg2Transport)             subType = "BdaMpeg2Transport";
        else if (mediaType.subType == MediaSubType.VC1)                           subType = "VC1";
        else if (mediaType.subType == MediaSubType.CyberlinkVC1)                  subType = "CyberlinkVC1";
        else if (mediaType.subType == MediaSubType.XVID1)                         subType = "XVID1";
        else if (mediaType.subType == MediaSubType.XVID2)                         subType = "XVID2";
        else if (mediaType.subType == MediaSubType.NV24)                          subType = "NV24";
        else if (mediaType.subType == MediaSubType.Data708_608)                   subType = "Data708_608";
        else if (mediaType.subType == MediaSubType.DtvCcData)                     subType = "DtvCcData";
        else if (mediaType.subType == MediaSubType.LATMAACLAF)                    subType = "LATMAACLAF";
        else if (mediaType.subType == MediaSubType.DVB_Subtitles)                 subType = "DVB_Subtitles";
        else if (mediaType.subType == MediaSubType.ISDB_Captions)                 subType = "ISDB_Captions";
        else if (mediaType.subType == MediaSubType.ISDB_Superimpose)              subType = "ISDB_Superimpose";
        else if (mediaType.subType == MediaSubType.NV11)                          subType = "NV11";
        else if (mediaType.subType == MediaSubType.P208)                          subType = "P208";
        else if (mediaType.subType == MediaSubType.P210)                          subType = "P210";
        else if (mediaType.subType == MediaSubType.P216)                          subType = "P216";
        else if (mediaType.subType == MediaSubType.P010)                          subType = "P010";
        else if (mediaType.subType == MediaSubType.P016)                          subType = "P016";
        else if (mediaType.subType == MediaSubType.Y210)                          subType = "Y210";
        else if (mediaType.subType == MediaSubType.Y216)                          subType = "Y216";
        else if (mediaType.subType == MediaSubType.P408)                          subType = "P408";
        else if (mediaType.subType == MediaSubType.CC_Container)                  subType = "CC_Container";
        else if (mediaType.subType == MediaSubType.VBI)                           subType = "VBI";
        else if (mediaType.subType == MediaSubType.XDS)                           subType = "XDS";
        else if (mediaType.subType == MediaSubType.ETDTFilter_Tagged)             subType = "ETDTFilter_Tagged";
        else if (mediaType.subType == MediaSubType.CPFilters_Processed)           subType = "CPFilters_Processed";
        else if (mediaType.subType == MediaSubType.Utf8)                          subType = "Utf8";
        else if (mediaType.subType == MediaSubType.Ssa)                           subType = "Ssa";
        else if (mediaType.subType == MediaSubType.Ass)                           subType = "Ass";
        else if (mediaType.subType == MediaSubType.Ass2)                          subType = "Ass2";
        else if (mediaType.subType == MediaSubType.Ssf)                           subType = "Ssf";
        else if (mediaType.subType == MediaSubType.VobSub)                        subType = "VobSub";
        else if (mediaType.subType == MediaSubType.HdmvSub)                       subType = "HdmvSub";
        else if (mediaType.subType == MediaSubType.DvbSub)                        subType = "DvbSub";
        else                                                                      subType = "???";
#pragma warning restore format

        return $"{majorType}>>{subType}";
    }
}