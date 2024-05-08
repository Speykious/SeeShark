// Copyright (c) Speykious
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 2-Clause License. See LICENSE for details.

namespace SeeShark;

/// <summary>
/// Framerate expressed as a positive rational number.
/// </summary>
public struct FramerateRatio
{
    /// <summary>
    /// Frames per second
    /// </summary>
    public double Value => Denominator == 0 ? 0 : (double)Numerator / Denominator;

    /// <summary>
    /// Seconds per frame
    /// </summary>
    public double Interval => Numerator == 0 ? 0 : (double)Denominator / Numerator;

    public uint Numerator;
    public uint Denominator;

    public override string ToString() => $"{Value} fps";
}
