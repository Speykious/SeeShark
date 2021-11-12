// Copyright (c) The Vignette Authors
// This file is part of SeeShark.
// SeeShark is licensed under LGPL v3. See LICENSE.LESSER.md for details.

namespace SeeShark
{
    public interface IAdjustableCamera : ICamera
    {
        /// <summary>
        /// Intensity of a color, expressed as the degree to which it differs from white.
        /// </summary>
        public double Saturation { get; set; }
        /// <summary>
        /// Differences in color and tone that contribute to the visual effect of the frame.
        /// </summary>
        public double Contrast { get; set; }
        /// <summary>
        /// Quantity of light reaching a photographic film, as determined by shutter speed and lens aperture.
        /// </summary>
        public double Exposure { get; set; }
        /// <summary>
        /// Factor by which power or voltage is increased in an amplifier or other electronic device, usually expressed as a logarithm.
        /// </summary>
        public double Gain { get; set; }
        /// <summary>
        /// Attribute of a color by virtue of which it is discernible as red, green, etc., and which is dependent on its dominant wavelength and independent of intensity or lightness.
        /// </summary>
        public double Hue { get; set; }
        /// <summary>
        /// Center of interest or activity.
        /// </summary>
        public double Focus { get; set; }
        /// <summary>
        /// Whether it sets the exposure automatically.
        /// </summary>
        public double AutoExposure { get; set; }
        /// <summary>
        /// Whether it sets the focus automatically.
        /// </summary>
        public double AutoFocus { get; set; }

        /// <summary>
        /// Whether this <see cref="IAdjustableCamera"/> supports a specific adjustable camera setting.
        /// </summary>
        /// <param name="setting">Adjustable camera setting.</param>
        public bool SupportsSetting(AdjustableCameraSetting setting);
    }
}
