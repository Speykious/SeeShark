// Copyright (c) Speykious
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 2-Clause License. See LICENSE for details.

using System;
using System.Runtime.InteropServices;

namespace SeeShark;

/// <summary>
/// A frame queue implemented as a circular buffer byte arrays.
/// It needs to be pinnable by the GC so that it can be sent across FFI boundaries,
/// especially on callback API backends like AVFoundation on MacOS.
/// So it uses an unmanaged array to store the frames.
/// </summary>
internal class FrameQueue : IDisposable
{
    /// <summary>
    /// Unmanaged buffer of frames.
    /// </summary>
    private unsafe RawFrame* rawFrames;

    [StructLayout(LayoutKind.Sequential)]
    private unsafe struct RawFrame
    {
        public byte* Ptr;
        public int Length;

        public uint Width;
        public uint Height;
        public ImageFormat ImageFormat;
    }

    /// <summary>
    /// Head of the frame buffer, as a frame index.
    /// </summary>
    private int head;
    /// <summary>
    /// Tail of the frame buffer, as a frame index.
    /// </summary>
    private int tail;

    public int MaxCount { get; init; }

    public int Count { get; private set; }

    public FrameQueue(int maxCount)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxCount);

        MaxCount = maxCount;
        Count = 0;

        head = 0;
        tail = 0;

        unsafe
        {
            rawFrames = (RawFrame*)NativeMemory.Alloc((nuint)(maxCount * sizeof(RawFrame)));
        }
    }

    /// <summary>
    /// Push a frame into this queue.
    /// </summary>
    /// <param name="data">The data of the frame. Its size must be the same as <see cref="FrameSize"/>.</param>
    /// <returns>Whether it overwrote the oldest frame in the queue.</returns>
    public bool EnqueueFrame(Frame frame)
    {
        bool hasOverwritten = false;

        if (Count == MaxCount)
        {
            // Queue is full, erase one frame (move head forward)
            tryDequeueFrame(false);
            hasOverwritten = true;
        }

        unsafe
        {
            // Copy frame data
            byte* rawBuffer = (byte*)NativeMemory.Alloc((nuint)frame.Data.Length);
            Marshal.Copy(frame.Data, 0, (nint)rawBuffer, frame.Data.Length);

            rawFrames[tail] = new RawFrame
            {
                Ptr = rawBuffer,
                Length = frame.Data.Length,

                Width = frame.Width,
                Height = frame.Height,
                ImageFormat = frame.ImageFormat,
            };
        }

        // Move tail forward
        tail = (tail + 1) % MaxCount;

        Count++;

        return hasOverwritten;
    }

    /// <summary>
    /// Pop a frame out of this queue.
    /// </summary>
    /// <returns>The frame to dequeue, or <c>null</c> if the queue is empty.</returns>
    public Frame? TryDequeueFrame() => tryDequeueFrame(true);

    /// <summary>
    /// Pop a frame out of this queue.
    /// </summary>
    /// <returns>The frame to dequeue, or <c>null</c> if the queue is empty.</returns>
    private Frame? tryDequeueFrame(bool getData)
    {
        if (Count == 0)
            return null;

        Frame? frame = null;

        unsafe
        {
            // Paste frame data
            RawFrame fb = rawFrames[head];

            if (getData)
            {
                frame = new Frame()
                {
                    Data = new byte[fb.Length],
                    Width = fb.Width,
                    Height = fb.Height,
                    ImageFormat = fb.ImageFormat,
                };

                Marshal.Copy((nint)fb.Ptr, frame.Data, 0, fb.Length);
            }

            NativeMemory.Free(fb.Ptr);
            fb.Ptr = null;
        }

        // Erase one frame (move head forward)
        head = (head + 1) % MaxCount;
        Count--;

        return frame;
    }

    public bool IsEmpty() => Count == 0;

    ~FrameQueue() => dispose();

    public void Dispose()
    {
        dispose();
        GC.SuppressFinalize(this);
    }

    private void dispose()
    {
        unsafe
        {
            if (rawFrames != null)
            {
                NativeMemory.Free(rawFrames);
                rawFrames = null;
            }
        }
    }
}
