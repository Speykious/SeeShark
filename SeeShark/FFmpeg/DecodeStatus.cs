// Copyright (c) The Vignette Authors
// This file is part of SeeShark.
// SeeShark is licensed under LGPL v3. See LICENSE.LESSER.md for details.

namespace SeeShark.FFmpeg
{
    /// <summary>
    /// Describes the decoding status of a given <see cref="VideoStreamDecoder"/>
    /// after calling its <see cref="VideoStreamDecoder.TryDecodeNextFrame"/> method.
    /// </summary>
    public enum DecodeStatus
    {
        /// <summary>
        /// A new frame has been returned.
        /// </summary>
        NewFrame,
        /// <summary>
        /// No new frame is available at the moment.
        /// The given <see cref="VideoStreamDecoder"/> is expected to try decoding a new frame again.
        /// </summary>
        NoFrameAvailable,
        /// <summary>
        /// Decoder reached the end of the stream.
        /// The given <see cref="VideoStreamDecoder"/> is expected to stop decoding.
        /// </summary>
        EndOfStream,
    }
}
