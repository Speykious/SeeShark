// Copyright (c) Speykious
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 2-Clause License. See LICENSE for details.

using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;

namespace SeeShark.Interop.MacOS;

[SupportedOSPlatform("Macos")]
internal static class CoreVideo
{
    private const string lib_corevideo = "/System/Library/Frameworks/CoreVideo.framework/CoreVideo";

    internal static nint CoreVideoHandle;

    static CoreVideo()
    {
        CoreVideoHandle = DL.dlopen(lib_corevideo, DL.RTLD_NOW);
    }

    [DllImport(lib_corevideo)]
    internal static extern NSDictionary CVBufferCopyAttachments(CVBufferRef buffer, CVAttachmentMode attachmentMode);

    [DllImport(lib_corevideo)]
    internal static extern nuint CVPixelBufferGetWidth(CVBufferRef pixelBuffer);
    [DllImport(lib_corevideo)]
    internal static extern nuint CVPixelBufferGetHeight(CVBufferRef pixelBuffer);
    [DllImport(lib_corevideo)]
    internal static extern CVPixelFormatType CVPixelBufferGetPixelFormatType(CVBufferRef pixelBuffer);
    [DllImport(lib_corevideo)]
    internal static extern nuint CVPixelBufferGetDataSize(CVBufferRef pixelBuffer);
    [DllImport(lib_corevideo)]
    internal static extern nint CVPixelBufferGetBaseAddress(CVBufferRef pixelBuffer);
    [DllImport(lib_corevideo)]
    internal static extern bool CVPixelBufferIsPlanar(CVBufferRef pixelBuffer);
    [DllImport(lib_corevideo)]
    internal static extern int CVPixelBufferLockBaseAddress(CVBufferRef pixelBuffer, CVPixelBufferLockFlags lockFlags);
    [DllImport(lib_corevideo)]
    internal static extern int CVPixelBufferUnlockBaseAddress(CVBufferRef pixelBuffer, CVPixelBufferLockFlags unlockFlags);

    internal static string DescribePixelFormat(CVPixelFormatType pixelFormat)
    {
        switch (pixelFormat)
        {
            #pragma warning disable format
            case CVPixelFormatType.k_1Monochrome:                                  return "1 bit indexed";
            case CVPixelFormatType.k_2Indexed:                                     return "2 bit indexed";
            case CVPixelFormatType.k_4Indexed:                                     return "4 bit indexed";
            case CVPixelFormatType.k_8Indexed:                                     return "8 bit indexed";
            case CVPixelFormatType.k_1IndexedGray_WhiteIsZero:                     return "1 bit indexed gray, white is zero";
            case CVPixelFormatType.k_2IndexedGray_WhiteIsZero:                     return "2 bit indexed gray, white is zero";
            case CVPixelFormatType.k_4IndexedGray_WhiteIsZero:                     return "4 bit indexed gray, white is zero";
            case CVPixelFormatType.k_8IndexedGray_WhiteIsZero:                     return "8 bit indexed gray, white is zero";
            case CVPixelFormatType.k_16BE555:                                      return "16 bit BE RGB 555";
            case CVPixelFormatType.k_16LE555:                                      return "[L555] 16 bit LE RGB 555";
            case CVPixelFormatType.k_16LE5551:                                     return "[5551] 16 bit LE RGB 5551";
            case CVPixelFormatType.k_16BE565:                                      return "[B565] 16 bit BE RGB 565";
            case CVPixelFormatType.k_16LE565:                                      return "[L565] 16 bit LE RGB 565";
            case CVPixelFormatType.k_24RGB:                                        return "24 bit RGB";
            case CVPixelFormatType.k_24BGR:                                        return "24 bit BGR";
            case CVPixelFormatType.k_32ARGB:                                       return "32 bit ARGB";
            case CVPixelFormatType.k_32BGRA:                                       return "[BGRA] 32 bit BGRA";
            case CVPixelFormatType.k_32ABGR:                                       return "[ABGR] 32 bit ABGR";
            case CVPixelFormatType.k_32RGBA:                                       return "[RGBA] 32 bit RGBA";
            case CVPixelFormatType.k_64ARGB:                                       return "[b64a] 64 bit ARGB, 16-bit big-endian samples";
            case CVPixelFormatType.k_64RGBALE:                                     return "[l64r] 64 bit RGBA, 16-bit little-endian full-range (0-65535) samples";
            case CVPixelFormatType.k_48RGB:                                        return "[b48r] 48 bit RGB, 16-bit big-endian samples";
            case CVPixelFormatType.k_32AlphaGray:                                  return "[b32a] 32 bit AlphaGray, 16-bit big-endian samples, black is zero";
            case CVPixelFormatType.k_16Gray:                                       return "[b16g] 16 bit Grayscale, 16-bit big-endian samples, black is zero";
            case CVPixelFormatType.k_30RGB:                                        return "[R10k] 30 bit RGB, 10-bit big-endian samples, 2 unused padding bits (at least significant end).";
            case CVPixelFormatType.k_30RGB_r210:                                   return "[r210] 30 bit RGB, 10-bit big-endian samples, 2 unused padding bits (at most significant end), video-range (64-940).";
            case CVPixelFormatType.k_422YpCbCr8:                                   return "[2vuy] Component Y'CbCr 8-bit 4:2:2, ordered Cb Y'0 Cr Y'1";
            case CVPixelFormatType.k_4444YpCbCrA8:                                 return "[v408] Component Y'CbCrA 8-bit 4:4:4:4, ordered Cb Y' Cr A";
            case CVPixelFormatType.k_4444YpCbCrA8R:                                return "[r408] Component Y'CbCrA 8-bit 4:4:4:4, rendering format. full range alpha, zero biased YUV, ordered A Y' Cb Cr";
            case CVPixelFormatType.k_4444AYpCbCr8:                                 return "[y408] Component Y'CbCrA 8-bit 4:4:4:4, ordered A Y' Cb Cr, full range alpha, video range Y'CbCr.";
            case CVPixelFormatType.k_4444AYpCbCr16:                                return "[y416] Component Y'CbCrA 16-bit 4:4:4:4, ordered A Y' Cb Cr, full range alpha, video range Y'CbCr, 16-bit little-endian samples.";
            case CVPixelFormatType.k_4444AYpCbCrFloat:                             return "[r4fl] Component AY'CbCr single precision floating-point 4:4:4:4";
            case CVPixelFormatType.k_444YpCbCr8:                                   return "[v308] Component Y'CbCr 8-bit 4:4:4, ordered Cr Y' Cb, video range Y'CbCr";
            case CVPixelFormatType.k_422YpCbCr16:                                  return "[v216] Component Y'CbCr 10,12,14,16-bit 4:2:2";
            case CVPixelFormatType.k_422YpCbCr10:                                  return "[v210] Component Y'CbCr 10-bit 4:2:2";
            case CVPixelFormatType.k_444YpCbCr10:                                  return "[v410] Component Y'CbCr 10-bit 4:4:4";
            case CVPixelFormatType.k_420YpCbCr8Planar:                             return "[y420] Planar Component Y'CbCr 8-bit 4:2:0.  baseAddr points to a big-endian CVPlanarPixelBufferInfo_YCbCrPlanar struct";
            case CVPixelFormatType.k_420YpCbCr8PlanarFullRange:                    return "[f420] Planar Component Y'CbCr 8-bit 4:2:0, full range.  baseAddr points to a big-endian CVPlanarPixelBufferInfo_YCbCrPlanar struct";
            case CVPixelFormatType.k_422YpCbCr_4A_8BiPlanar:                       return "[a2vy] First plane: Video-range Component Y'CbCr 8-bit 4:2:2, ordered Cb Y'0 Cr Y'1; second plane: alpha 8-bit 0-255";
            case CVPixelFormatType.k_420YpCbCr8BiPlanarVideoRange:                 return "[420v] Bi-Planar Component Y'CbCr 8-bit 4:2:0, video-range (luma=[16,235] chroma=[16,240]).  baseAddr points to a big-endian CVPlanarPixelBufferInfo_YCbCrBiPlanar struct";
            case CVPixelFormatType.k_420YpCbCr8BiPlanarFullRange:                  return "[420f] Bi-Planar Component Y'CbCr 8-bit 4:2:0, full-range (luma=[0,255] chroma=[1,255]).  baseAddr points to a big-endian CVPlanarPixelBufferInfo_YCbCrBiPlanar struct";
            case CVPixelFormatType.k_422YpCbCr8BiPlanarVideoRange:                 return "[422v] Bi-Planar Component Y'CbCr 8-bit 4:2:2, video-range (luma=[16,235] chroma=[16,240]).  baseAddr points to a big-endian CVPlanarPixelBufferInfo_YCbCrBiPlanar struct";
            case CVPixelFormatType.k_422YpCbCr8BiPlanarFullRange:                  return "[422f] Bi-Planar Component Y'CbCr 8-bit 4:2:2, full-range (luma=[0,255] chroma=[1,255]).  baseAddr points to a big-endian CVPlanarPixelBufferInfo_YCbCrBiPlanar struct";
            case CVPixelFormatType.k_444YpCbCr8BiPlanarVideoRange:                 return "[444v] Bi-Planar Component Y'CbCr 8-bit 4:4:4, video-range (luma=[16,235] chroma=[16,240]).  baseAddr points to a big-endian CVPlanarPixelBufferInfo_YCbCrBiPlanar struct";
            case CVPixelFormatType.k_444YpCbCr8BiPlanarFullRange:                  return "[444f] Bi-Planar Component Y'CbCr 8-bit 4:4:4, full-range (luma=[0,255] chroma=[1,255]).  baseAddr points to a big-endian CVPlanarPixelBufferInfo_YCbCrBiPlanar struct";
            case CVPixelFormatType.k_422YpCbCr8_yuvs:                              return "[yuvs] Component Y'CbCr 8-bit 4:2:2, ordered Y'0 Cb Y'1 Cr";
            case CVPixelFormatType.k_422YpCbCr8FullRange:                          return "[yuvf] Component Y'CbCr 8-bit 4:2:2, full range, ordered Y'0 Cb Y'1 Cr";
            case CVPixelFormatType.k_OneComponent8:                                return "[L008] 8 bit one component, black is zero";
            case CVPixelFormatType.k_TwoComponent8:                                return "[2C08] 8 bit two component, black is zero";
            case CVPixelFormatType.k_30RGBLEPackedWideGamut:                       return "[w30r] little-endian RGB101010, 2 MSB are ignored, wide-gamut (384-895)";
            case CVPixelFormatType.k_ARGB2101010LEPacked:                          return "[l10r] little-endian ARGB2101010 full-range ARGB";
            case CVPixelFormatType.k_40ARGBLEWideGamut:                            return "[w40a] little-endian ARGB10101010, each 10 bits in the MSBs of 16bits, wide-gamut (384-895, including alpha)";
            case CVPixelFormatType.k_40ARGBLEWideGamutPremultiplied:               return "[w40m] little-endian ARGB10101010, each 10 bits in the MSBs of 16bits, wide-gamut (384-895, including alpha). Alpha premultiplied";
            case CVPixelFormatType.k_OneComponent10:                               return "[L010] 10 bit little-endian one component, stored as 10 MSBs of 16 bits, black is zero";
            case CVPixelFormatType.k_OneComponent12:                               return "[L012] 12 bit little-endian one component, stored as 12 MSBs of 16 bits, black is zero";
            case CVPixelFormatType.k_OneComponent16:                               return "[L016] 16 bit little-endian one component, black is zero";
            case CVPixelFormatType.k_TwoComponent16:                               return "[2C16] 16 bit little-endian two component, black is zero";
            case CVPixelFormatType.k_OneComponent16Half:                           return "[L00h] 16 bit one component IEEE half-precision float, 16-bit little-endian samples";
            case CVPixelFormatType.k_OneComponent32Float:                          return "[L00f] 32 bit one component IEEE float, 32-bit little-endian samples";
            case CVPixelFormatType.k_TwoComponent16Half:                           return "[2C0h] 16 bit two component IEEE half-precision float, 16-bit little-endian samples";
            case CVPixelFormatType.k_TwoComponent32Float:                          return "[2C0f] 32 bit two component IEEE float, 32-bit little-endian samples";
            case CVPixelFormatType.k_64RGBAHalf:                                   return "[RGhA] 64 bit RGBA IEEE half-precision float, 16-bit little-endian samples";
            case CVPixelFormatType.k_128RGBAFloat:                                 return "[RGfA] 128 bit RGBA IEEE float, 32-bit little-endian samples";
            case CVPixelFormatType.k_14Bayer_GRBG:                                 return "[grb4] Bayer 14-bit Little-Endian, packed in 16-bits, ordered G R G R... alternating with B G B G...";
            case CVPixelFormatType.k_14Bayer_RGGB:                                 return "[rgg4] Bayer 14-bit Little-Endian, packed in 16-bits, ordered R G R G... alternating with G B G B...";
            case CVPixelFormatType.k_14Bayer_BGGR:                                 return "[bgg4] Bayer 14-bit Little-Endian, packed in 16-bits, ordered B G B G... alternating with G R G R...";
            case CVPixelFormatType.k_14Bayer_GBRG:                                 return "[gbr4] Bayer 14-bit Little-Endian, packed in 16-bits, ordered G B G B... alternating with R G R G...";
            case CVPixelFormatType.k_DisparityFloat16:                             return "[hdis] IEEE754-2008 binary16 (half float), describing the normalized shift when comparing two images. Units are 1/meters: ( pixelShift / (pixelFocalLength * baselineInMeters) )";
            case CVPixelFormatType.k_DisparityFloat32:                             return "[fdis] IEEE754-2008 binary32 float, describing the normalized shift when comparing two images. Units are 1/meters: ( pixelShift / (pixelFocalLength * baselineInMeters) )";
            case CVPixelFormatType.k_DepthFloat16:                                 return "[hdep] IEEE754-2008 binary16 (half float), describing the depth (distance to an object) in meters";
            case CVPixelFormatType.k_DepthFloat32:                                 return "[fdep] IEEE754-2008 binary32 float, describing the depth (distance to an object) in meters";
            case CVPixelFormatType.k_420YpCbCr10BiPlanarVideoRange:                return "[x420] 2 plane YCbCr10 4:2:0, each 10 bits in the MSBs of 16bits, video-range (luma=[64,940] chroma=[64,960])";
            case CVPixelFormatType.k_422YpCbCr10BiPlanarVideoRange:                return "[x422] 2 plane YCbCr10 4:2:2, each 10 bits in the MSBs of 16bits, video-range (luma=[64,940] chroma=[64,960])";
            case CVPixelFormatType.k_444YpCbCr10BiPlanarVideoRange:                return "[x444] 2 plane YCbCr10 4:4:4, each 10 bits in the MSBs of 16bits, video-range (luma=[64,940] chroma=[64,960])";
            case CVPixelFormatType.k_420YpCbCr10BiPlanarFullRange:                 return "[xf20] 2 plane YCbCr10 4:2:0, each 10 bits in the MSBs of 16bits, full-range (Y range 0-1023)";
            case CVPixelFormatType.k_422YpCbCr10BiPlanarFullRange:                 return "[xf22] 2 plane YCbCr10 4:2:2, each 10 bits in the MSBs of 16bits, full-range (Y range 0-1023)";
            case CVPixelFormatType.k_444YpCbCr10BiPlanarFullRange:                 return "[xf44] 2 plane YCbCr10 4:4:4, each 10 bits in the MSBs of 16bits, full-range (Y range 0-1023)";
            case CVPixelFormatType.k_420YpCbCr8VideoRange_8A_TriPlanar:            return "[v0a8] first and second planes as per 420YpCbCr8BiPlanarVideoRange (420v), alpha 8 bits in third plane full-range.  No CVPlanarPixelBufferInfo struct.";
            case CVPixelFormatType.k_16VersatileBayer:                             return "[bp16] Single plane Bayer 16-bit little-endian sensor element (\"sensel\") samples from full-size decoding of ProRes RAW images; Bayer pattern (sensel ordering) and other raw conversion information is described via buffer attachments";
            case CVPixelFormatType.k_64RGBA_DownscaledProResRAW:                   return "[bp64] Single plane 64-bit RGBA (16-bit little-endian samples) from downscaled decoding of ProRes RAW images; components--which may not be co-sited with one another--are sensel values and require raw conversion, information for which is described via buffer attachments";
            case CVPixelFormatType.k_422YpCbCr16BiPlanarVideoRange:                return "[sv22] 2 plane YCbCr16 4:2:2, video-range (luma=[4096,60160] chroma=[4096,61440])";
            case CVPixelFormatType.k_444YpCbCr16BiPlanarVideoRange:                return "[sv44] 2 plane YCbCr16 4:4:4, video-range (luma=[4096,60160] chroma=[4096,61440])";
            case CVPixelFormatType.k_444YpCbCr16VideoRange_16A_TriPlanar:          return "[s4as] 3 plane video-range YCbCr16 4:4:4 with 16-bit full-range alpha (luma=[4096,60160] chroma=[4096,61440] alpha=[0,65535]).  No CVPlanarPixelBufferInfo struct.";
            case CVPixelFormatType.k_Lossless_32BGRA:                              return "[&BGA] Lossless-compressed form of k_32BGRA.";
            case CVPixelFormatType.k_Lossless_64RGBAHalf:                          return "[&RhA] Lossless-compressed form of k_64RGBAHalf.  No CVPlanarPixelBufferInfo struct.";
            case CVPixelFormatType.k_Lossless_420YpCbCr8BiPlanarVideoRange:        return "[&8v0] Lossless-compressed form of k_420YpCbCr8BiPlanarVideoRange. No CVPlanarPixelBufferInfo struct.";
            case CVPixelFormatType.k_Lossless_420YpCbCr8BiPlanarFullRange:         return "[&8f0] Lossless-compressed form of k_420YpCbCr8BiPlanarFullRange. No CVPlanarPixelBufferInfo struct.";
            case CVPixelFormatType.k_Lossless_420YpCbCr10PackedBiPlanarVideoRange: return "[&xv0] Lossless-compressed-packed form of k_420YpCbCr10BiPlanarVideoRange. No CVPlanarPixelBufferInfo struct. Format is compressed-packed with no padding bits between pixels.";
            case CVPixelFormatType.k_Lossless_422YpCbCr10PackedBiPlanarVideoRange: return "[&xv2] Lossless-compressed form of k_422YpCbCr10BiPlanarVideoRange. No CVPlanarPixelBufferInfo struct. Format is compressed-packed with no padding bits between pixels.";
            case CVPixelFormatType.k_Lossless_420YpCbCr10PackedBiPlanarFullRange:  return "[&xf0] Lossless-compressed form of k_420YpCbCr10BiPlanarFullRange. No CVPlanarPixelBufferInfo struct. Format is compressed-packed with no padding bits between pixels.";
            case CVPixelFormatType.k_Lossy_32BGRA:                                 return "[-BGA] Lossy-compressed form of k_32BGRA. No CVPlanarPixelBufferInfo struct.";
            case CVPixelFormatType.k_Lossy_420YpCbCr8BiPlanarVideoRange:           return "[-8v0] Lossy-compressed form of k_420YpCbCr8BiPlanarVideoRange. No CVPlanarPixelBufferInfo struct.";
            case CVPixelFormatType.k_Lossy_420YpCbCr8BiPlanarFullRange:            return "[-8f0] Lossy-compressed form of k_420YpCbCr8BiPlanarFullRange. No CVPlanarPixelBufferInfo struct.";
            case CVPixelFormatType.k_Lossy_420YpCbCr10PackedBiPlanarVideoRange:    return "[-xv0] Lossy-compressed form of k_420YpCbCr10BiPlanarVideoRange. No CVPlanarPixelBufferInfo struct. Format is compressed-packed with no padding bits between pixels.";
            case CVPixelFormatType.k_Lossy_422YpCbCr10PackedBiPlanarVideoRange:    return "[-xv2] Lossy-compressed form of k_422YpCbCr10BiPlanarVideoRange. No CVPlanarPixelBufferInfo struct. Format is compressed-packed with no padding bits between pixels.";
            #pragma warning restore format
        }

        return "???";
    }
}

[Flags]
internal enum CVPixelBufferLockFlags : uint
{
    None = 0,
    ReadOnly = 1,
}

internal enum CVAttachmentMode : uint
{
    /// <summary>
    /// Indicates to not propagate the attachment.
    /// </summary>
    ShouldNotPropagate = 0,

    /// <summary>
    /// Indicates to copy the attachment.
    /// </summary>
    ShouldPropagate = 1,
}

[SupportedOSPlatform("Macos")]
internal struct CVBufferRef : INSObject
{
    private nint id;

    internal CVBufferRef(nint id)
    {
        this.id = id;
    }

    public nint ID => id;
}
