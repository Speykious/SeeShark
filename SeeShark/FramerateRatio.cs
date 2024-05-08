// Copyright (c) Speykious
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 3-Clause License. See LICENSE for details.

namespace SeeShark;

/// <summary>
/// Framerate expressed as a positive rational number.
/// </summary>
public struct FramerateRatio
{
    public double Value => Denominator == 0 ? 0 : (double)Numerator / Denominator;

    public uint Numerator;
    public uint Denominator;

    public override string ToString() => $"{Value} fps";
}
