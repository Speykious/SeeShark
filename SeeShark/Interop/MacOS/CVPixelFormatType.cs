// Copyright (c) Speykious
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 2-Clause License. See LICENSE for details.

using System.Runtime.Versioning;

namespace SeeShark.Interop.MacOS;

// All pixel format definitions can be found at the following path:
// /Applications/Xcode.app/Contents/Developer/Platforms/MacOSX.platform/Developer/SDKs/MacOSX.sdk/System/Library/Frameworks/CoreVideo.framework/Versions/A/Headers/CVPixelBuffer.h

[SupportedOSPlatform("Macos")]
internal enum CVPixelFormatType
{
#pragma warning disable format
    k_1Monochrome                         = 0x1,
    k_2Indexed                            = 0x2,
    k_4Indexed                            = 0x4,
    k_8Indexed                            = 0x8,
    k_1IndexedGray_WhiteIsZero            = 0x21,
    k_2IndexedGray_WhiteIsZero            = 0x22,
    k_4IndexedGray_WhiteIsZero            = 0x24,
    k_8IndexedGray_WhiteIsZero            = 0x28,
    k_16BE555                             = 0x10,
    k_16LE555                             = 0x4C353535, // 'L555'
    k_16LE5551                            = 0x35353531, // '5551'
    k_16BE565                             = 0x42353635, // 'B565'
    k_16LE565                             = 0x4C353635, // 'L565'
    k_24RGB                               = 0x18,
    k_24BGR                               = 0x32344247, // '24BG'
    k_32ARGB                              = 0x20,
    k_32BGRA                              = 0x42475241, // 'BGRA'
    k_32ABGR                              = 0x41424752, // 'ABGR'
    k_32RGBA                              = 0x52474241, // 'RGBA'
    k_64ARGB                              = 0x62363461, // 'b64a'
    k_64RGBALE                            = 0x6C363472, // 'l64r'
    k_48RGB                               = 0x62343872, // 'b48r'
    k_32AlphaGray                         = 0x62333261, // 'b32a'
    k_16Gray                              = 0x62313667, // 'b16g'
    k_30RGB                               = 0x5231306B, // 'R10k'
    k_30RGB_r210                          = 0x72323130, // 'r210'
    k_422YpCbCr8                          = 0x32767579, // '2vuy'
    k_4444YpCbCrA8                        = 0x76343038, // 'v408'
    k_4444YpCbCrA8R                       = 0x72343038, // 'r408'
    k_4444AYpCbCr8                        = 0x79343038, // 'y408'
    k_4444AYpCbCr16                       = 0x79343136, // 'y416'
    k_4444AYpCbCrFloat                    = 0x7234666C, // 'r4fl'
    k_444YpCbCr8                          = 0x76333038, // 'v308'
    k_422YpCbCr16                         = 0x76323136, // 'v216'
    k_422YpCbCr10                         = 0x76323130, // 'v210'
    k_444YpCbCr10                         = 0x76343130, // 'v410'
    k_420YpCbCr8Planar                    = 0x79343230, // 'y420'
    k_420YpCbCr8PlanarFullRange           = 0x66343230, // 'f420'
    k_422YpCbCr_4A_8BiPlanar              = 0x61327679, // 'a2vy'
    k_420YpCbCr8BiPlanarVideoRange        = 0x34323076, // '420v'
    k_420YpCbCr8BiPlanarFullRange         = 0x34323066, // '420f'
    k_422YpCbCr8BiPlanarVideoRange        = 0x34323276, // '422v'
    k_422YpCbCr8BiPlanarFullRange         = 0x34323266, // '422f'
    k_444YpCbCr8BiPlanarVideoRange        = 0x34343476, // '444v'
    k_444YpCbCr8BiPlanarFullRange         = 0x34343466, // '444f'
    k_422YpCbCr8_yuvs                     = 0x79757673, // 'yuvs'
    k_422YpCbCr8FullRange                 = 0x79757666, // 'yuvf'
    k_OneComponent8                       = 0x4C303038, // 'L008'
    k_TwoComponent8                       = 0x32433038, // '2C08'
    k_30RGBLEPackedWideGamut              = 0x77333072, // 'w30r'
    k_ARGB2101010LEPacked                 = 0x6C313072, // 'l10r'
    k_40ARGBLEWideGamut                   = 0x77343061, // 'w40a'
    k_40ARGBLEWideGamutPremultiplied      = 0x7734306D, // 'w40m'
    k_OneComponent10                      = 0x4C303130, // 'L010'
    k_OneComponent12                      = 0x4C303132, // 'L012'
    k_OneComponent16                      = 0x4C303136, // 'L016'
    k_TwoComponent16                      = 0x32433136, // '2C16'
    k_OneComponent16Half                  = 0x4C303068, // 'L00h'
    k_OneComponent32Float                 = 0x4C303066, // 'L00f'
    k_TwoComponent16Half                  = 0x32433068, // '2C0h'
    k_TwoComponent32Float                 = 0x32433066, // '2C0f'
    k_64RGBAHalf                          = 0x52476841, // 'RGhA'
    k_128RGBAFloat                        = 0x52476641, // 'RGfA'
    k_14Bayer_GRBG                        = 0x67726234, // 'grb4'
    k_14Bayer_RGGB                        = 0x72676734, // 'rgg4'
    k_14Bayer_BGGR                        = 0x62676734, // 'bgg4'
    k_14Bayer_GBRG                        = 0x67627234, // 'gbr4'
    k_DisparityFloat16                    = 0x68646973, // 'hdis'
    k_DisparityFloat32                    = 0x66646973, // 'fdis'
    k_DepthFloat16                        = 0x68646570, // 'hdep'
    k_DepthFloat32                        = 0x66646570, // 'fdep'
    k_420YpCbCr10BiPlanarVideoRange       = 0x78343230, // 'x420'
    k_422YpCbCr10BiPlanarVideoRange       = 0x78343232, // 'x422'
    k_444YpCbCr10BiPlanarVideoRange       = 0x78343434, // 'x444'
    k_420YpCbCr10BiPlanarFullRange        = 0x78663230, // 'xf20'
    k_422YpCbCr10BiPlanarFullRange        = 0x78663232, // 'xf22'
    k_444YpCbCr10BiPlanarFullRange        = 0x78663434, // 'xf44'
    k_420YpCbCr8VideoRange_8A_TriPlanar   = 0x76306138, // 'v0a8'
    k_16VersatileBayer                    = 0x62703136, // 'bp16'
    k_64RGBA_DownscaledProResRAW          = 0x62703634, // 'bp64'
    k_422YpCbCr16BiPlanarVideoRange       = 0x73763232, // 'sv22'
    k_444YpCbCr16BiPlanarVideoRange       = 0x73763434, // 'sv44'
    k_444YpCbCr16VideoRange_16A_TriPlanar = 0x73346173, // 's4as'

    #region Lossless-Compressed Pixel Formats
    /*
	    The following pixel formats can be used to reduce the memory bandwidth involved in large-scale pixel data flow, which can have benefits for battery life and thermal efficiency.
	    They work by dividing pixel buffers into fixed-width, fixed-height, fixed-byte-size blocks.
        Hardware units (video codecs, GPU, ISP, etc.) attempt to write a compressed encoding for each block using a lossless algorithm.
        If a block of pixels is successfully encoded using fewer bytes than the uncompressed pixel data, the hardware unit does not need to write as many bytes for that pixel block.
        If the encoding is unsuccessful, the uncompressed pixel data is written, filling the whole pixel block.
        Each compressed pixel buffer has a separate area of metadata recording the encoding choices for each pixel block.

	    Padding bits are eliminated, so for example, 10-bit-per-component lossless-compressed pixel buffers are slightly smaller than their uncompressed equivalents.
        For pixel formats with no padding, the lossless-compressed pixel buffers are slightly larger due to the metadata.

	    IMPORTANT CAVEATS:
	    Some devices do not support these pixel formats at all.
	    Before using one of these pixel formats, call CVIsCompressedPixelFormatAvailable() to check that it is available on the current device.
	    On different devices, the concrete details of these formats may be different.
	    On different devices, the degree and details of support by hardware units (video codecs, GPU, ISP, etc.) may be different.
	    Do not ship code that reads the contents of lossless-compressed pixel buffers directly with the CPU, or which saves or transfers it to other devices, as this code will break with future hardware.
	    The bandwidth benefits of these formats are generally outweighed by the cost of buffer copies to convert to uncompressed pixel formats,
        so if you find that you need to perform a buffer copy to covert for CPU usage, it's likely that you would have been better served by using the equivalent uncompressed pixel formats in the first place.
    */

    k_Lossless_32BGRA                              = 0x26424741, // '&BGA'
    k_Lossless_64RGBAHalf                          = 0x26526841, // '&RhA'

	// Lossless-compressed Bi-planar YCbCr pixel format types
    k_Lossless_420YpCbCr8BiPlanarVideoRange        = 0x26387630, // '&8v0'
    k_Lossless_420YpCbCr8BiPlanarFullRange         = 0x26386630, // '&8f0'
    k_Lossless_420YpCbCr10PackedBiPlanarVideoRange = 0x26787630, // '&xv0'
    k_Lossless_422YpCbCr10PackedBiPlanarVideoRange = 0x26787632, // '&xv2'
    k_Lossless_420YpCbCr10PackedBiPlanarFullRange  = 0x26786630, // '&xf0'
    #endregion

    #region Lossy-Compressed Pixel Formats
    /*
        The following pixel formats can be used to reduce memory bandwidth and memory footprint involved in large-scale pixel data flow, which can have benefits for battery life and thermal efficiency.
        Similar to lossless pixel formats, they work by dividing pixel buffers into fixed-width, fixed-height, fixed-byte-size blocks.
        Pixel buffers allocated using lossy formats have reduced memory footprint than their lossless equivalents; this reduced footprint may or may not result in loss of quality depending on the content of the individual block.
        Hardware units (video codecs, GPU, ISP, etc.) attempt to write a compressed encoding for each block using either a lossless or lossy algorithm.
        If a block of pixels is successfully encoded within its pre-defined memory footprint, then the lossless alogrithm is applied; if the encoded block of pixels exceeds the pre-defined memory footprint then the lossy algorithm is applied.
        Each compressed pixel buffer has a separate area of metadata recording the encoding choices for each pixel block.

        IMPORTANT CAVEATS:
        Some devices do not support these pixel formats at all.
        Before using one of these pixel formats, call CVIsCompressedPixelFormatAvailable() to check that it is available on the current device.
        On different devices, the concrete details of these formats may be different.
        On different devices, the degree and details of support by hardware units (video codecs, GPU, ISP, etc.) may be different.
        Do not ship code that reads the contents of lossless-compressed pixel buffers directly with the CPU, or which saves or transfers it to other devices, as this code will break with future hardware.
        The bandwidth benefits of these formats are generally outweighed by the cost of buffer copies to convert to uncompressed pixel formats, so if you find that you need to perform a buffer copy to covert for CPU usage, it's likely that you would have been better served by using the equivalent uncompressed pixel formats in the first place.
    */

    k_Lossy_32BGRA                              = 0x2D424741, // '-BGA'
    k_Lossy_420YpCbCr8BiPlanarVideoRange        = 0x2D387630, // '-8v0'
    k_Lossy_420YpCbCr8BiPlanarFullRange         = 0x2D386630, // '-8f0'
    k_Lossy_420YpCbCr10PackedBiPlanarVideoRange = 0x2D787630, // '-xv0'
    k_Lossy_422YpCbCr10PackedBiPlanarVideoRange = 0x2D787632 // '-xv2'
    #endregion
#pragma warning restore format
}

[SupportedOSPlatform("Macos")]
internal static class CVPixelFormatTypeMethods
{
    internal static string Describe(CVPixelFormatType pixelFormat)
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

    /// <summary>
    /// Bit size of the pixel format
    /// </summary>
    /// <returns>The size of a pixel in this pixel format in bits, -1 if unknown</returns>
    internal static int BitSize(CVPixelFormatType pixelFormat)
    {
        switch (pixelFormat)
        {
            #pragma warning disable format
            case CVPixelFormatType.k_1Monochrome:                                  return 1;
            case CVPixelFormatType.k_2Indexed:                                     return 2;
            case CVPixelFormatType.k_4Indexed:                                     return 4;
            case CVPixelFormatType.k_8Indexed:                                     return 8;
            case CVPixelFormatType.k_1IndexedGray_WhiteIsZero:                     return 1;
            case CVPixelFormatType.k_2IndexedGray_WhiteIsZero:                     return 2;
            case CVPixelFormatType.k_4IndexedGray_WhiteIsZero:                     return 4;
            case CVPixelFormatType.k_8IndexedGray_WhiteIsZero:                     return 8;
            case CVPixelFormatType.k_16BE555:                                      return 16;
            case CVPixelFormatType.k_16LE555:                                      return 16;
            case CVPixelFormatType.k_16LE5551:                                     return 16;
            case CVPixelFormatType.k_16BE565:                                      return 16;
            case CVPixelFormatType.k_16LE565:                                      return 16;
            case CVPixelFormatType.k_24RGB:                                        return 24;
            case CVPixelFormatType.k_24BGR:                                        return 24;
            case CVPixelFormatType.k_32ARGB:                                       return 32;
            case CVPixelFormatType.k_32BGRA:                                       return 32;
            case CVPixelFormatType.k_32ABGR:                                       return 32;
            case CVPixelFormatType.k_32RGBA:                                       return 32;
            case CVPixelFormatType.k_64ARGB:                                       return 64;
            case CVPixelFormatType.k_64RGBALE:                                     return 64;
            case CVPixelFormatType.k_48RGB:                                        return 48;
            case CVPixelFormatType.k_32AlphaGray:                                  return 32;
            case CVPixelFormatType.k_16Gray:                                       return 16;
            case CVPixelFormatType.k_30RGB:                                        return 32;
            case CVPixelFormatType.k_30RGB_r210:                                   return 32;
            case CVPixelFormatType.k_422YpCbCr8:                                   return 16;
            case CVPixelFormatType.k_4444YpCbCrA8:                                 return 8;
            case CVPixelFormatType.k_4444YpCbCrA8R:                                return 8;
            case CVPixelFormatType.k_4444AYpCbCr8:                                 return 8;
            case CVPixelFormatType.k_4444AYpCbCr16:                                return 16;
            case CVPixelFormatType.k_4444AYpCbCrFloat:                             return -1;
            case CVPixelFormatType.k_444YpCbCr8:                                   return 8;
            case CVPixelFormatType.k_422YpCbCr16:                                  return -1;
            case CVPixelFormatType.k_422YpCbCr10:                                  return 10;
            case CVPixelFormatType.k_444YpCbCr10:                                  return 10;
            case CVPixelFormatType.k_420YpCbCr8Planar:                             return -1;
            case CVPixelFormatType.k_420YpCbCr8PlanarFullRange:                    return -1;
            case CVPixelFormatType.k_422YpCbCr_4A_8BiPlanar:                       return -1;
            case CVPixelFormatType.k_420YpCbCr8BiPlanarVideoRange:                 return -1;
            case CVPixelFormatType.k_420YpCbCr8BiPlanarFullRange:                  return -1;
            case CVPixelFormatType.k_422YpCbCr8BiPlanarVideoRange:                 return -1;
            case CVPixelFormatType.k_422YpCbCr8BiPlanarFullRange:                  return -1;
            case CVPixelFormatType.k_444YpCbCr8BiPlanarVideoRange:                 return -1;
            case CVPixelFormatType.k_444YpCbCr8BiPlanarFullRange:                  return -1;
            case CVPixelFormatType.k_422YpCbCr8_yuvs:                              return 16;
            case CVPixelFormatType.k_422YpCbCr8FullRange:                          return 16;
            case CVPixelFormatType.k_OneComponent8:                                return 8;
            case CVPixelFormatType.k_TwoComponent8:                                return 8;
            case CVPixelFormatType.k_30RGBLEPackedWideGamut:                       return 30;
            case CVPixelFormatType.k_ARGB2101010LEPacked:                          return 32;
            case CVPixelFormatType.k_40ARGBLEWideGamut:                            return 64;
            case CVPixelFormatType.k_40ARGBLEWideGamutPremultiplied:               return 64;
            case CVPixelFormatType.k_OneComponent10:                               return 16;
            case CVPixelFormatType.k_OneComponent12:                               return 16;
            case CVPixelFormatType.k_OneComponent16:                               return 16;
            case CVPixelFormatType.k_TwoComponent16:                               return 16;
            case CVPixelFormatType.k_OneComponent16Half:                           return 16;
            case CVPixelFormatType.k_OneComponent32Float:                          return 32;
            case CVPixelFormatType.k_TwoComponent16Half:                           return 16;
            case CVPixelFormatType.k_TwoComponent32Float:                          return 32;
            case CVPixelFormatType.k_64RGBAHalf:                                   return 64;
            case CVPixelFormatType.k_128RGBAFloat:                                 return 128;
            case CVPixelFormatType.k_14Bayer_RGGB:                                 return 16;
            case CVPixelFormatType.k_14Bayer_GRBG:                                 return 16;
            case CVPixelFormatType.k_14Bayer_BGGR:                                 return 16;
            case CVPixelFormatType.k_14Bayer_GBRG:                                 return 16;
            case CVPixelFormatType.k_DisparityFloat16:                             return 16;
            case CVPixelFormatType.k_DisparityFloat32:                             return 32;
            case CVPixelFormatType.k_DepthFloat16:                                 return 16;
            case CVPixelFormatType.k_DepthFloat32:                                 return 32;
            case CVPixelFormatType.k_420YpCbCr10BiPlanarVideoRange:                return -1;
            case CVPixelFormatType.k_422YpCbCr10BiPlanarVideoRange:                return -1;
            case CVPixelFormatType.k_444YpCbCr10BiPlanarVideoRange:                return -1;
            case CVPixelFormatType.k_420YpCbCr10BiPlanarFullRange:                 return -1;
            case CVPixelFormatType.k_422YpCbCr10BiPlanarFullRange:                 return -1;
            case CVPixelFormatType.k_444YpCbCr10BiPlanarFullRange:                 return -1;
            case CVPixelFormatType.k_420YpCbCr8VideoRange_8A_TriPlanar:            return -1;
            case CVPixelFormatType.k_16VersatileBayer:                             return 16;
            case CVPixelFormatType.k_64RGBA_DownscaledProResRAW:                   return 64;
            case CVPixelFormatType.k_422YpCbCr16BiPlanarVideoRange:                return -1;
            case CVPixelFormatType.k_444YpCbCr16BiPlanarVideoRange:                return -1;
            case CVPixelFormatType.k_444YpCbCr16VideoRange_16A_TriPlanar:          return -1;
            case CVPixelFormatType.k_Lossless_32BGRA:                              return -1;
            case CVPixelFormatType.k_Lossless_64RGBAHalf:                          return -1;
            case CVPixelFormatType.k_Lossless_420YpCbCr8BiPlanarVideoRange:        return -1;
            case CVPixelFormatType.k_Lossless_420YpCbCr8BiPlanarFullRange:         return -1;
            case CVPixelFormatType.k_Lossless_420YpCbCr10PackedBiPlanarVideoRange: return -1;
            case CVPixelFormatType.k_Lossless_422YpCbCr10PackedBiPlanarVideoRange: return -1;
            case CVPixelFormatType.k_Lossless_420YpCbCr10PackedBiPlanarFullRange:  return -1;
            case CVPixelFormatType.k_Lossy_32BGRA:                                 return -1;
            case CVPixelFormatType.k_Lossy_420YpCbCr8BiPlanarVideoRange:           return -1;
            case CVPixelFormatType.k_Lossy_420YpCbCr8BiPlanarFullRange:            return -1;
            case CVPixelFormatType.k_Lossy_420YpCbCr10PackedBiPlanarVideoRange:    return -1;
            case CVPixelFormatType.k_Lossy_422YpCbCr10PackedBiPlanarVideoRange:    return -1;
            #pragma warning restore format
        }

        return -1;
    }
}
