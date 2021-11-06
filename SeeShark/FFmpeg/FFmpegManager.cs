using System.Runtime.InteropServices;
using FFmpeg.AutoGen;

namespace SeeShark.FFmpeg
{
    public static class FFmpegManager
    {
        public static bool IsFFmpegSetup { get; private set; } = false;

        public static string FFmpegVersion
        {
            get
            {
                SetupFFmpeg();
                return ffmpeg.av_version_info();
            }
        }
        public static string FFmpegRootPath
        {
            get => ffmpeg.RootPath;
            private set => ffmpeg.RootPath = value;
        }

        public static void SetupFFmpeg(string rootPath = "/usr/lib", FFmpegLogLevel logLevel = FFmpegLogLevel.Panic, ConsoleColor logColor = ConsoleColor.Yellow)
        {
            if (IsFFmpegSetup)
                return;

            FFmpegRootPath = rootPath;
            SetupFFmpegLogging(logLevel, logColor);

            IsFFmpegSetup = true;
        }

        internal static unsafe void SetupFFmpegLogging(FFmpegLogLevel logLevel, ConsoleColor logColor)
        {
            ffmpeg.av_log_set_level((int)logLevel);

            // do not convert to local function
            av_log_set_callback_callback logCallback = (p0, level, format, vl) =>
            {
                if (level > ffmpeg.av_log_get_level()) return;

                var lineSize = 1024;
                var lineBuffer = stackalloc byte[lineSize];
                var printPrefix = 1;
                ffmpeg.av_log_format_line(p0, level, format, vl, lineBuffer, lineSize, &printPrefix);
                var line = Marshal.PtrToStringAnsi((IntPtr)lineBuffer);
                Console.ForegroundColor = logColor;
                Console.Write(line);
                Console.ResetColor();
            };

            ffmpeg.av_log_set_callback(logCallback);
        }
    }
}