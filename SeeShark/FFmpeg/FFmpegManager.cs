// Copyright (c) The Vignette Authors
// This file is part of SeeShark.
// SeeShark is licensed under LGPL v3. See LICENSE.LESSER.md for details.

using System.Runtime.InteropServices;
using FFmpeg.AutoGen;

namespace SeeShark.FFmpeg
{
    public static class FFmpegManager
    {
        /// <summary>
        /// Whether FFmpeg has been setup.
        /// </summary>
        public static bool IsFFmpegSetup { get; private set; } = false;

        /// <summary>
        /// Informative version string. It is usually the actual release version number or a git commit description. It has no fixed format and can change any time. It should never be parsed by code.
        /// <br/>
        /// Note: fetching this value will setup FFmpeg if it hasn't been done before.
        /// </summary>
        public static string FFmpegVersion
        {
            get
            {
                SetupFFmpeg();
                return ffmpeg.av_version_info();
            }
        }

        /// <summary>
        /// Root path for loading FFmpeg libraries.
        /// </summary>
        public static string FFmpegRootPath
        {
            get => ffmpeg.RootPath;
            private set => ffmpeg.RootPath = value;
        }

        private static av_log_set_callback_callback? logCallback;

        /// <summary>
        /// Setup FFmpeg: root path and logging.
        /// <br/>
        /// It will only setup FFmpeg once. Any non-first call will do nothing.
        /// <br/>
        /// SeeShark is designed such that this method is called whenever it is
        /// necessary to have FFmpeg ready.
        /// <br/>
        /// However, if you want to, you can still call it at the beginning of
        /// your program to customize your FFmpeg setup.
        /// </summary>
        /// <param name="rootPath">Root path for loading FFmpeg libraries.</param>
        /// <param name="logLevel">Log level for FFmpeg.</param>
        /// <param name="logColor">Color of the FFmpeg logs.</param>
        public static void SetupFFmpeg(string rootPath = "/usr/lib", FFmpegLogLevel logLevel = FFmpegLogLevel.Panic, ConsoleColor logColor = ConsoleColor.Yellow)
        {
            if (IsFFmpegSetup)
                return;

            FFmpegRootPath = rootPath;
            SetupFFmpegLogging(logLevel, logColor);
            ffmpeg.avdevice_register_all();

            IsFFmpegSetup = true;
        }

        internal static unsafe void SetupFFmpegLogging(FFmpegLogLevel logLevel, ConsoleColor logColor)
        {
            ffmpeg.av_log_set_level((int)logLevel);

            // Do not convert to local function!
            logCallback = (p0, level, format, vl) =>
            {
                if (level > ffmpeg.av_log_get_level())
                    return;

                var lineSize = 1024;
                var lineBuffer = stackalloc byte[lineSize];
                var printPrefix = 1;

                ffmpeg.av_log_format_line(p0, level, format, vl, lineBuffer, lineSize, &printPrefix);
                var line = Marshal.PtrToStringAnsi((IntPtr)lineBuffer);

                // TODO: maybe make it possible to log this in any stream?
                Console.ForegroundColor = logColor;
                Console.Write(line);
                Console.ResetColor();
            };

            ffmpeg.av_log_set_callback(logCallback);
        }
    }
}
