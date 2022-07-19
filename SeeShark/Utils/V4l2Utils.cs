// Copyright (c) The Vignette Authors
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 3-Clause License. See LICENSE for details.

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using FFmpeg.AutoGen;
using SeeShark.Device;
using SeeShark.Interop.Libc;

namespace SeeShark.Utils
{
    internal static class V4l2Utils
    {
        public static void FillDeviceOptions(CameraInfo[] devices)
        {
            foreach (CameraInfo device in devices)
                device.AvailableVideoInputOptions = getAvailableOptions(device).ToArray();
        }

        /// <summary>
        /// Get available video input options of a V4l2 device.
        /// Inspired from https://github.com/ZhangGaoxing/v4l2.net
        /// </summary>
        private unsafe static List<VideoInputOptions> getAvailableOptions(CameraInfo device)
        {
            List<VideoInputOptions> options = new List<VideoInputOptions>();

            int deviceFd = Libc.open(device.Path, FileOpenFlags.O_RDWR);
            if (deviceFd < 0)
                throw new IOException($"Error {Marshal.GetLastWin32Error()}: Can not open video device {device}");

            v4l2_fmtdesc fmtdesc = new v4l2_fmtdesc
            {
                index = 0,
                type = v4l2_buf_type.V4L2_BUF_TYPE_VIDEO_CAPTURE
            };

            List<V4l2InputFormat> supportedInputFormats = new List<V4l2InputFormat>();
            while (v4l2Struct(deviceFd, VideoSettings.VIDIOC_ENUM_FMT, ref fmtdesc) != -1)
            {
                supportedInputFormats.Add(fmtdesc.pixelformat);
                fmtdesc.index++;
            }

            foreach (V4l2InputFormat inputFormat in supportedInputFormats)
            {
                v4l2_frmsizeenum frmsize = new v4l2_frmsizeenum
                {
                    index = 0,
                    pixel_format = inputFormat
                };

                while (v4l2Struct(deviceFd, VideoSettings.VIDIOC_ENUM_FRAMESIZES, ref frmsize) != -1)
                {
                    if (frmsize.type == v4l2_frmsizetype.V4L2_FRMSIZE_TYPE_DISCRETE)
                    {
                        fillFrameIntervalOptions(options, deviceFd, frmsize.pixel_format, frmsize.discrete.width, frmsize.discrete.height);
                    }
                    else
                    {
                        for (uint width = frmsize.stepwise.min_width; width < frmsize.stepwise.max_width; width += frmsize.stepwise.step_width)
                        {
                            for (uint height = frmsize.stepwise.min_height; height < frmsize.stepwise.max_height; height += frmsize.stepwise.step_height)
                                fillFrameIntervalOptions(options, deviceFd, frmsize.pixel_format, width, height);
                        }
                    }
                    frmsize.index++;
                }
            }

            Libc.close(deviceFd);
            return options;
        }

        private static void fillFrameIntervalOptions(List<VideoInputOptions> options, int deviceFd, V4l2InputFormat pixelFormat, uint width, uint height)
        {
            v4l2_frmivalenum frmival = new v4l2_frmivalenum
            {
                index = 0,
                pixel_format = pixelFormat,
                width = width,
                height = height,
            };

            while (v4l2Struct(deviceFd, VideoSettings.VIDIOC_ENUM_FRAMEINTERVALS, ref frmival) != -1)
            {
                if (frmival.type == v4l2_frmivaltype.V4L2_FRMIVAL_TYPE_DISCRETE)
                {
                    options.Add(new VideoInputOptions
                    {
                        InputFormat = pixelFormat.ToString(),
                        VideoSize = ((int)width, (int)height),
                        Framerate = new AVRational
                        {
                            num = (int)frmival.discrete.denominator,
                            den = (int)frmival.discrete.numerator,
                        },
                    });
                }
                frmival.index++;
            }
        }

        /// <summary>
        /// Get and set v4l2 struct.
        /// </summary>
        /// <typeparam name="T">V4L2 struct</typeparam>
        /// <param name="request">V4L2 request value</param>
        /// <param name="struct">The struct need to be read or set</param>
        /// <returns>The ioctl result</returns>
        private static int v4l2Struct<T>(int deviceFd, VideoSettings request, ref T @struct)
            where T : struct
        {
            IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(@struct));
            Marshal.StructureToPtr(@struct, ptr, true);

            int result = Libc.ioctl(deviceFd, (int)request, ptr);
            @struct = Marshal.PtrToStructure<T>(ptr);

            return result;
        }
    }
}
