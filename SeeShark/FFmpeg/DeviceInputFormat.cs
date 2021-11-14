// Copyright (c) The Vignette Authors
// This file is part of SeeShark.
// SeeShark is licensed under LGPL v3. See LICENSE.LESSER.md for details.

namespace SeeShark.FFmpeg
{
    /// <summary>
    /// Input format for camera devices.
    /// </summary>
    public enum DeviceInputFormat
    {
        #region Windows compatible
        /// <summary>
        /// dshow (DirectShow) input device, the preferred option for Windows users. <br/>
        /// See the wiki article about <see href="https://trac.ffmpeg.org/wiki/DirectShow">DirectShow</see>
        /// and the <see href="https://ffmpeg.org/ffmpeg-devices.html#dshow">dshow input device documentation</see> for more information.
        /// </summary>
        DShow,
        /// <summary>
        /// vfwcap input device. <br/>
        /// See the <see href="https://ffmpeg.org/ffmpeg-all.html#vfwcap">vfwcap input device documentation</see> for more information.
        /// </summary>
        /// <remarks>
        /// vfwcap is outdated. Use dshow instead if possible.
        /// See <see href="https://trac.ffmpeg.org/wiki/DirectShow">DirectShow</see> for more information.
        /// </remarks>
        VfWCap,
        #endregion

        #region Linux compatible
        /// <summary>
        /// Uses the video4linux2 (or simply v4l2) input device to capture live input such as from a webcam.
        /// See the <see href="https://ffmpeg.org/ffmpeg-all.html#video4linux2_002c-v4l2">v4l2 input device documentation</see> for more information.
        /// </summary>
        V4l2,
        #endregion

        #region MacOS compatible
        /// <summary>
        /// OS X users can use the <see href="https://ffmpeg.org/ffmpeg-devices.html#avfoundation">avfoundation</see>
        /// and <see href="https://ffmpeg.org/ffmpeg-devices.html#qtkit">qtkit</see> input devices for grabbing
        /// integrated iSight cameras as well as cameras connected via USB or FireWire. <br/>
        /// </summary>
        /// <remarks>
        /// AVFoundation is available on Mac OS X 10.7 (Lion) and later.
        /// Since then, Apple recommends AVFoundation for stream grabbing on OS X and iOS devices.
        /// </remarks>
        AVFoundation,
        /// <summary>
        /// OS X users can use the <see href="https://ffmpeg.org/ffmpeg-devices.html#avfoundation">avfoundation</see>
        /// and <see href="https://ffmpeg.org/ffmpeg-devices.html#qtkit">qtkit</see> input devices for grabbing
        /// integrated iSight cameras as well as cameras connected via USB or FireWire. <br/>
        /// </summary>
        /// <remarks>
        /// QTKit is available on Mac OS X 10.4 (Tiger) and later.
        /// QTKit has been marked deprecated since OS X 10.7 (Lion) and may not be available on future releases.
        /// </remarks>
        QTKit,
        #endregion
    }

    public static class DeviceInputFormatExtension
    {
        public static string ToString(this DeviceInputFormat deviceInputFormat)
        {
            return deviceInputFormat switch
            {
                DeviceInputFormat.DShow => "dshow",
                DeviceInputFormat.VfWCap => "vfwcap",
                DeviceInputFormat.V4l2 => "v4l2",
                DeviceInputFormat.AVFoundation => "avfoundation",
                DeviceInputFormat.QTKit => "qtkit",
                _ => throw new ArgumentException("Unknown device input format"),
            };
        }
    }
}
