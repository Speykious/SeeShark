// Copyright (c) The Vignette Authors
// This file is part of SeeShark.
// SeeShark is licensed under LGPL v3. See LICENSE.LESSER.md for details.

namespace SeeShark
{
    /// <summary>
    /// Adjustable setting for a camera.
    /// </summary>
    public enum AdjustableCameraSetting
    {
        /// <summary>
        /// Intensity of a color, expressed as the degree to which it differs from white.
        /// </summary>
        Saturation,
        /// <summary>
        /// Differences in color and tone that contribute to the visual effect of the frame.
        /// </summary>
        Contrast,
        /// <summary>
        /// Quantity of light reaching a photographic film, as determined by shutter speed and lens aperture.
        /// </summary>
        Exposure,
        /// <summary>
        /// Factor by which power or voltage is increased in an amplifier or other electronic device, usually expressed as a logarithm.
        /// </summary>
        Gain,
        /// <summary>
        /// Attribute of a color by virtue of which it is discernible as red, green, etc., and which is dependent on its dominant wavelength and independent of intensity or lightness.
        /// </summary>
        Hue,
        /// <summary>
        /// Center of interest or activity.
        /// </summary>
        Focus,
        /// <summary>
        /// Whether it sets the exposure automatically.
        /// </summary>
        AutoExposure,
        /// <summary>
        /// Whether it sets the focus automatically.
        /// </summary>
        AutoFocus
    }
}
