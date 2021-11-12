// Copyright (c) The Vignette Authors
// This file is part of SeeShark.
// SeeShark is licensed under LGPL v3. See LICENSE.LESSER.md for details.

namespace SeeShark
{
    public interface IAdjustableCamera : ICamera
    {
        public double Saturation { get; set; }

        public double Contrast { get; set; }

        public double Exposure { get; set; }

        public double Gain { get; set; }

        public double Hue { get; set; }

        public double Focus { get; set; }

        public double AutoExposure { get; set; }

        public double AutoFocus { get; set; }

        public bool SupportsSetting(AdjustableCameraSetting setting);
    }
}
