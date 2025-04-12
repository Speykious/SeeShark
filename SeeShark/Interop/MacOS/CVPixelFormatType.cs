// Copyright (c) Speykious
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 2-Clause License. See LICENSE for details.

namespace SeeShark.Interop.MacOS;

internal enum CVPixelFormatType
{
#pragma warning disable format
    k_1Monochrome                         = 1,
    k_2Indexed                            = 2,
    k_4Indexed                            = 4,
    k_8Indexed                            = 8,
    k_1IndexedGray_WhiteIsZero            = 33,
    k_2IndexedGray_WhiteIsZero            = 34,
    k_4IndexedGray_WhiteIsZero            = 36,
    k_8IndexedGray_WhiteIsZero            = 40,
    k_16BE555                             = 16,
    k_16LE555                             = 0x4C353535, // 'L555'
    k_16LE5551                            = 0x35353531, // '5551'
    k_16BE565                             = 0x42353635, // 'B565'
    k_16LE565                             = 0x4C353635, // 'L565'
    k_24RGB                               = 24,
    k_24BGR                               = 0x32344247, // '24BG'
    k_32ARGB                              = 32,
    k_32BGRA                              = 0x42475241, // 'BGRA'
    k_32ABGR                              = 0x41424752, // 'ABGR'
    k_32RGBA                              = 0x52474241, // 'RGBA'
    k_64ARGB                              = 0x62363461, // 'b64a'
    k_48RGB                               = 0x62343872, // 'b48r'
    k_32AlphaGray                         = 0x62333261, // 'b32a'
    k_16Gray                              = 0x62313667, // 'b16g'
    k_30RGB                               = 0x5231306B, // 'R10k'
    k_422YpCbCr8                          = 0x32767579, // '2vuy'
    k_4444YpCbCrA8                        = 0x76343038, // 'v408'
    k_4444YpCbCrA8R                       = 0x72343038, // 'r408'
    k_4444AYpCbCr8                        = 0x79343038, // 'y408'
    k_4444AYpCbCr16                       = 0x79343136, // 'y416'
    k_444YpCbCr8                          = 0x76333038, // 'v308'
    k_422YpCbCr16                         = 0x76323136, // 'v216'
    k_422YpCbCr10                         = 0x76323130, // 'v210'
    k_444YpCbCr10                         = 0x76343130, // 'v410'
    k_420YpCbCr8Planar                    = 0x79343230, // 'y420'
    k_420YpCbCr8PlanarFullRange           = 0x66343230, // 'f420'
    k_422YpCbCr_4A_8BiPlanar              = 0x61327679, // 'a2vy'
    k_420YpCbCr8BiPlanarVideoRange        = 0x34323076, // '420v'
    k_420YpCbCr8BiPlanarFullRange         = 0x34323066, // '420f'
    k_422YpCbCr8_yuvs                     = 0x79757673, // 'yuvs'
    k_422YpCbCr8FullRange                 = 0x79757666, // 'yuvf'
    k_OneComponent8                       = 0x4C303038, // 'L008'
    k_TwoComponent8                       = 0x32433038, // '2C08'
    k_OneComponent16Half                  = 0x4C303068, // 'L00h'
    k_OneComponent32Float                 = 0x4C303066, // 'L00f'
    k_TwoComponent16Half                  = 0x32433068, // '2C0h'
    k_TwoComponent32Float                 = 0x32433066, // '2C0f'
    k_64RGBAHalf                          = 0x52476841, // 'RGhA'
    k_128RGBAFloat                        = 0x52476641, // 'RGfA'
    k_14Bayer_BGGR                        = 0x62676734, // 'bgg4'
    k_14Bayer_GBRG                        = 0x67627234, // 'gbr4'
    k_14Bayer_GRBG                        = 0x67726234, // 'grb4'
    k_14Bayer_RGGB                        = 0x72676734, // 'rgg4'
    k_30RGBLEPackedWideGamut              = 0x77333072, // 'w30r'
    k_ARGB2101010LEPacked                 = 0x6C313072, // 'l10r'
    k_420YpCbCr10BiPlanarFullRange        = 0x78663230, // 'xf20'
    k_420YpCbCr10BiPlanarVideoRange       = 0x78343230, // 'x420'
    k_422YpCbCr10BiPlanarFullRange        = 0x78663232, // 'xf22'
    k_422YpCbCr10BiPlanarVideoRange       = 0x78343232, // 'x422'
    k_444YpCbCr10BiPlanarFullRange        = 0x78663434, // 'xf44'
    k_444YpCbCr10BiPlanarVideoRange       = 0x78343434, // 'x444'
    k_DepthFloat16                        = 0x68646570, // 'hdep'
    k_DepthFloat32                        = 0x66646570, // 'fdep'
    k_DisparityFloat16                    = 0x68646973, // 'hdis'
    k_DisparityFloat32                    = 0x66646973, // 'fdis'
    k_16VersatileBayer                    = 0x62703136, // 'bp16'
    k_40ARGBLEWideGamut                   = 0x77343061, // 'w40a'
    k_40ARGBLEWideGamutPremultiplied      = 0x7734306D, // 'w40m'
    k_420YpCbCr8VideoRange_8A_TriPlanar   = 0x76306138, // 'v0a8'
    k_422YpCbCr16BiPlanarVideoRange       = 0x73763232, // 'sv22'
    k_422YpCbCr8BiPlanarFullRange         = 0x34323266, // '422f'
    k_422YpCbCr8BiPlanarVideoRange        = 0x34323276, // '422v'
    k_4444AYpCbCrFloat                    = 0x7234666C, // 'r4fl'
    k_444YpCbCr16BiPlanarVideoRange       = 0x73763434, // 'sv44'
    k_444YpCbCr16VideoRange_16A_TriPlanar = 0x73346173, // 's4as'
    k_444YpCbCr8BiPlanarFullRange         = 0x34343466, // '444f'
    k_444YpCbCr8BiPlanarVideoRange        = 0x34343476, // '444v'
    k_64RGBALE                            = 0x6C363472, // 'l64r'
    k_64RGBA_DownscaledProResRAW          = 0x62703634, // 'bp64'
    k_OneComponent10                      = 0x4C303130, // 'L010'
    k_OneComponent12                      = 0x4C303132, // 'L012'
    k_OneComponent16                      = 0x4C303136, // 'L016'
    k_TwoComponent16                      = 0x32433136, // '2C16'
#pragma warning restore format
}
