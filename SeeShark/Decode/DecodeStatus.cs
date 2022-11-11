// Copyright (c) Speykious
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 3-Clause License. See LICENSE for details.

namespace SeeShark.Decode;

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
