using FFmpeg.AutoGen;

namespace SeeShark.FFmpeg
{
    public enum FFmpegLogLevel : int
    {
        Quiet = ffmpeg.AV_LOG_QUIET,
        Panic = ffmpeg.AV_LOG_PANIC,
        SkipRepeated = ffmpeg.AV_LOG_SKIP_REPEATED,
        PrintLevel = ffmpeg.AV_LOG_PRINT_LEVEL,
        Fatal = ffmpeg.AV_LOG_FATAL,
        Error = ffmpeg.AV_LOG_ERROR,
        Warning = ffmpeg.AV_LOG_WARNING,
        Info = ffmpeg.AV_LOG_INFO,
        Verbose = ffmpeg.AV_LOG_VERBOSE,
        Debug = ffmpeg.AV_LOG_DEBUG,
        Trace = ffmpeg.AV_LOG_TRACE,
        MaxOffset = ffmpeg.AV_LOG_MAX_OFFSET,
    }
}