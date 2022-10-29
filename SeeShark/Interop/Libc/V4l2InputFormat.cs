// Copyright (c) The Vignette Authors
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 3-Clause License. See LICENSE for details.

namespace SeeShark.Interop.Libc
{
    /// <summary>
    /// The pixel format or codec of a video device.
    /// </summary>
    internal enum V4l2InputFormat : uint
    {
        /// <summary>
        /// RGB332
        /// </summary>
        RGB332 = 826427218,

        /// <summary>
        /// RGB444
        /// </summary>
        RGB444 = 875836498,

        /// <summary>
        /// ARGB444
        /// </summary>
        ARGB444 = 842093121,

        /// <summary>
        /// XRGB444
        /// </summary>
        XRGB444 = 842093144,

        /// <summary>
        /// RGBA444
        /// </summary>
        RGBA444 = 842088786,

        /// <summary>
        /// RGBX444
        /// </summary>
        RGBX444 = 842094674,

        /// <summary>
        /// ABGR444
        /// </summary>
        ABGR444 = 842089025,

        /// <summary>
        /// XBGR444
        /// </summary>
        XBGR444 = 842089048,

        /// <summary>
        /// BGRA444
        /// </summary>
        BGRA444 = 842088775,

        /// <summary>
        /// BGRX444
        /// </summary>
        BGRX444 = 842094658,

        /// <summary>
        /// RGB555
        /// </summary>
        RGB555 = 1329743698,

        /// <summary>
        /// ARGB555
        /// </summary>
        ARGB555 = 892424769,

        /// <summary>
        /// XRGB555
        /// </summary>
        XRGB555 = 892424792,

        /// <summary>
        /// RGBA555
        /// </summary>
        RGBA555 = 892420434,

        /// <summary>
        /// RGBX555
        /// </summary>
        RGBX555 = 892426322,

        /// <summary>
        /// ABGR555
        /// </summary>
        ABGR555 = 892420673,

        /// <summary>
        /// XBGR555
        /// </summary>
        XBGR555 = 892420696,

        /// <summary>
        /// BGRA555
        /// </summary>
        BGRA555 = 892420418,

        /// <summary>
        /// BGRX555
        /// </summary>
        BGRX555 = 892426306,

        /// <summary>
        /// RGB565
        /// </summary>
        RGB565 = 1346520914,

        /// <summary>
        /// RGB555X
        /// </summary>
        RGB555X = 1363298130,

        /// <summary>
        /// ARGB555X
        /// </summary>
        ARGB555X = 3039908417,

        /// <summary>
        /// XRGB555X
        /// </summary>
        XRGB555X = 3039908440,

        /// <summary>
        /// RGB565X
        /// </summary>
        RGB565X = 1380075346,

        /// <summary>
        /// BGR666
        /// </summary>
        BGR666 = 1213351746,

        /// <summary>
        /// BGR24
        /// </summary>
        BGR24 = 861030210,

        /// <summary>
        /// RGB24
        /// </summary>
        RGB24 = 859981650,

        /// <summary>
        /// BGR32
        /// </summary>
        BGR32 = 877807426,

        /// <summary>
        /// ABGR32
        /// </summary>
        ABGR32 = 875713089,

        /// <summary>
        /// XBGR32
        /// </summary>
        XBGR32 = 875713112,

        /// <summary>
        /// BGRA32
        /// </summary>
        BGRA32 = 875708754,

        /// <summary>
        /// BGRX32
        /// </summary>
        BGRX32 = 875714642,

        /// <summary>
        /// RGB32
        /// </summary>
        RGB32 = 876758866,

        /// <summary>
        /// RGBA32
        /// </summary>
        RGBA32 = 875708993,

        /// <summary>
        /// RGBX32
        /// </summary>
        RGBX32 = 875709016,

        /// <summary>
        /// ARGB32
        /// </summary>
        ARGB32 = 875708738,

        /// <summary>
        /// XRGB32
        /// </summary>
        XRGB32 = 875714626,

        /// <summary>
        /// GREY
        /// </summary>
        GREY = 1497715271,

        /// <summary>
        /// Y4
        /// </summary>
        Y4 = 540291161,

        /// <summary>
        /// Y6
        /// </summary>
        Y6 = 540422233,

        /// <summary>
        /// Y10
        /// </summary>
        Y10 = 540029273,

        /// <summary>
        /// Y12
        /// </summary>
        Y12 = 540160345,

        /// <summary>
        /// Y16
        /// </summary>
        Y16 = 540422489,

        /// <summary>
        /// Y16_BE
        /// </summary>
        Y16_BE = 2687906137,

        /// <summary>
        /// Y10BPACK
        /// </summary>
        Y10BPACK = 1110454617,

        /// <summary>
        /// Y10P
        /// </summary>
        Y10P = 1345335641,

        /// <summary>
        /// PAL8
        /// </summary>
        PAL8 = 944521552,

        /// <summary>
        /// UV8
        /// </summary>
        UV8 = 540563029,

        /// <summary>
        /// YUYV
        /// </summary>
        YUYV = 1448695129,

        /// <summary>
        /// YYUV
        /// </summary>
        YYUV = 1448434009,

        /// <summary>
        /// YVYU
        /// </summary>
        YVYU = 1431918169,

        /// <summary>
        /// UYVY
        /// </summary>
        UYVY = 1498831189,

        /// <summary>
        /// VYUY
        /// </summary>
        VYUY = 1498765654,

        /// <summary>
        /// Y41P
        /// </summary>
        Y41P = 1345401945,

        /// <summary>
        /// YUV444
        /// </summary>
        YUV444 = 875836505,

        /// <summary>
        /// YUV555
        /// </summary>
        YUV555 = 1331058009,

        /// <summary>
        /// YUV565
        /// </summary>
        YUV565 = 1347835225,

        /// <summary>
        /// YUV32
        /// </summary>
        YUV32 = 878073177,

        /// <summary>
        /// AYUV32
        /// </summary>
        AYUV32 = 1448433985,

        /// <summary>
        /// XYUV32
        /// </summary>
        XYUV32 = 1448434008,

        /// <summary>
        /// VUYA32
        /// </summary>
        VUYA32 = 1096373590,

        /// <summary>
        /// VUYX32
        /// </summary>
        VUYX32 = 1482249558,

        /// <summary>
        /// HI240
        /// </summary>
        HI240 = 875710792,

        /// <summary>
        /// HM12
        /// </summary>
        HM12 = 842091848,

        /// <summary>
        /// M420
        /// </summary>
        M420 = 808596557,

        /// <summary>
        /// NV12
        /// </summary>
        NV12 = 842094158,

        /// <summary>
        /// NV21
        /// </summary>
        NV21 = 825382478,

        /// <summary>
        /// NV16
        /// </summary>
        NV16 = 909203022,

        /// <summary>
        /// NV61
        /// </summary>
        NV61 = 825644622,

        /// <summary>
        /// NV24
        /// </summary>
        NV24 = 875714126,

        /// <summary>
        /// NV42
        /// </summary>
        NV42 = 842290766,

        /// <summary>
        /// NV12M
        /// </summary>
        NV12M = 842091854,

        /// <summary>
        /// NV21M
        /// </summary>
        NV21M = 825380174,

        /// <summary>
        /// NV16M
        /// </summary>
        NV16M = 909200718,

        /// <summary>
        /// NV61M
        /// </summary>
        NV61M = 825642318,

        /// <summary>
        /// NV12MT
        /// </summary>
        NV12MT = 842091860,

        /// <summary>
        /// NV12MT_16X16
        /// </summary>
        NV12MT_16X16 = 842091862,

        /// <summary>
        /// YUV410
        /// </summary>
        YUV410 = 961959257,

        /// <summary>
        /// YVU410
        /// </summary>
        YVU410 = 961893977,

        /// <summary>
        /// YUV411P
        /// </summary>
        YUV411P = 1345401140,

        /// <summary>
        /// YUV420
        /// </summary>
        YUV420 = 842093913,

        /// <summary>
        /// YVU420
        /// </summary>
        YVU420 = 842094169,

        /// <summary>
        /// YUV422P
        /// </summary>
        YUV422P = 1345466932,

        /// <summary>
        /// YUV420M
        /// </summary>
        YUV420M = 842091865,

        /// <summary>
        /// YVU420M
        /// </summary>
        YVU420M = 825380185,

        /// <summary>
        /// YUV422M
        /// </summary>
        YUV422M = 909200729,

        /// <summary>
        /// YVU422M
        /// </summary>
        YVU422M = 825642329,

        /// <summary>
        /// YUV444M
        /// </summary>
        YUV444M = 875711833,

        /// <summary>
        /// YVU444M
        /// </summary>
        YVU444M = 842288473,

        /// <summary>
        /// SBGGR8
        /// </summary>
        SBGGR8 = 825770306,

        /// <summary>
        /// SGBRG8
        /// </summary>
        SGBRG8 = 1196573255,

        /// <summary>
        /// SGRBG8
        /// </summary>
        SGRBG8 = 1195528775,

        /// <summary>
        /// SRGGB8
        /// </summary>
        SRGGB8 = 1111967570,

        /// <summary>
        /// SBGGR10
        /// </summary>
        SBGGR10 = 808535874,

        /// <summary>
        /// SGBRG10
        /// </summary>
        SGBRG10 = 808534599,

        /// <summary>
        /// SGRBG10
        /// </summary>
        SGRBG10 = 808534338,

        /// <summary>
        /// SRGGB10
        /// </summary>
        SRGGB10 = 808535890,

        /// <summary>
        /// SBGGR10P
        /// </summary>
        SBGGR10P = 1094795888,

        /// <summary>
        /// SGBRG10P
        /// </summary>
        SGBRG10P = 1094797168,

        /// <summary>
        /// SGRBG10P
        /// </summary>
        SGRBG10P = 1094805360,

        /// <summary>
        /// SRGGB10P
        /// </summary>
        SRGGB10P = 1094799984,

        /// <summary>
        /// SBGGR10ALAW8
        /// </summary>
        SBGGR10ALAW8 = 943800929,

        /// <summary>
        /// SGBRG10ALAW8
        /// </summary>
        SGBRG10ALAW8 = 943802209,

        /// <summary>
        /// SGRBG10ALAW8
        /// </summary>
        SGRBG10ALAW8 = 943810401,

        /// <summary>
        /// SRGGB10ALAW8
        /// </summary>
        SRGGB10ALAW8 = 943805025,

        /// <summary>
        /// SBGGR10DPCM8
        /// </summary>
        SBGGR10DPCM8 = 943800930,

        /// <summary>
        /// SGBRG10DPCM8
        /// </summary>
        SGBRG10DPCM8 = 943802210,

        /// <summary>
        /// SGRBG10DPCM8
        /// </summary>
        SGRBG10DPCM8 = 808535106,

        /// <summary>
        /// SRGGB10DPCM8
        /// </summary>
        SRGGB10DPCM8 = 943805026,

        /// <summary>
        /// SBGGR12
        /// </summary>
        SBGGR12 = 842090306,

        /// <summary>
        /// SGBRG12
        /// </summary>
        SGBRG12 = 842089031,

        /// <summary>
        /// SGRBG12
        /// </summary>
        SGRBG12 = 842088770,

        /// <summary>
        /// SRGGB12
        /// </summary>
        SRGGB12 = 842090322,

        /// <summary>
        /// SBGGR12P
        /// </summary>
        SBGGR12P = 1128481392,

        /// <summary>
        /// SGBRG12P
        /// </summary>
        SGBRG12P = 1128482672,

        /// <summary>
        /// SGRBG12P
        /// </summary>
        SGRBG12P = 1128490864,

        /// <summary>
        /// SRGGB12P
        /// </summary>
        SRGGB12P = 1128485488,

        /// <summary>
        /// SBGGR14P
        /// </summary>
        SBGGR14P = 1162166896,

        /// <summary>
        /// SGBRG14P
        /// </summary>
        SGBRG14P = 1162168176,

        /// <summary>
        /// SGRBG14P
        /// </summary>
        SGRBG14P = 1162176368,

        /// <summary>
        /// SRGGB14P
        /// </summary>
        SRGGB14P = 1162170992,

        /// <summary>
        /// SBGGR16
        /// </summary>
        SBGGR16 = 844257602,

        /// <summary>
        /// SGBRG16
        /// </summary>
        SGBRG16 = 909197895,

        /// <summary>
        /// SGRBG16
        /// </summary>
        SGRBG16 = 909201991,

        /// <summary>
        /// SRGGB16
        /// </summary>
        SRGGB16 = 909199186,

        /// <summary>
        /// HSV24
        /// </summary>
        HSV24 = 861295432,

        /// <summary>
        /// HSV32
        /// </summary>
        HSV32 = 878072648,

        /// <summary>
        /// MJPEG
        /// </summary>
        MJPEG = 1196444237,

        /// <summary>
        /// JPEG
        /// </summary>
        JPEG = 1195724874,

        /// <summary>
        /// DV
        /// </summary>
        DV = 1685288548,

        /// <summary>
        /// MPEG
        /// </summary>
        MPEG = 1195724877,

        /// <summary>
        /// H264
        /// </summary>
        H264 = 875967048,

        /// <summary>
        /// H264_NO_SC
        /// </summary>
        H264_NO_SC = 826496577,

        /// <summary>
        /// H264_MVC
        /// </summary>
        H264_MVC = 875967053,

        /// <summary>
        /// H263
        /// </summary>
        H263 = 859189832,

        /// <summary>
        /// MPEG1
        /// </summary>
        MPEG1 = 826757197,

        /// <summary>
        /// MPEG2
        /// </summary>
        MPEG2 = 843534413,

        /// <summary>
        /// MPEG2_SLICE
        /// </summary>
        MPEG2_SLICE = 1395803981,

        /// <summary>
        /// MPEG4
        /// </summary>
        MPEG4 = 877088845,

        /// <summary>
        /// XVID
        /// </summary>
        XVID = 1145656920,

        /// <summary>
        /// VC1_ANNEX_G
        /// </summary>
        VC1_ANNEX_G = 1194410838,

        /// <summary>
        /// VC1_ANNEX_L
        /// </summary>
        VC1_ANNEX_L = 1278296918,

        /// <summary>
        /// VP8
        /// </summary>
        VP8 = 808996950,

        /// <summary>
        /// VP9
        /// </summary>
        VP9 = 809062486,

        /// <summary>
        /// HEVC
        /// </summary>
        HEVC = 1129727304,

        /// <summary>
        /// FWHT
        /// </summary>
        FWHT = 1414027078,

        /// <summary>
        /// FWHT_STATELESS
        /// </summary>
        FWHT_STATELESS = 1213679187,

        /// <summary>
        /// CPIA1
        /// </summary>
        CPIA1 = 1095323715,

        /// <summary>
        /// WNVA
        /// </summary>
        WNVA = 1096175191,

        /// <summary>
        /// SN9C10X
        /// </summary>
        SN9C10X = 808532307,

        /// <summary>
        /// SN9C20X_I420
        /// </summary>
        SN9C20X_I420 = 808597843,

        /// <summary>
        /// PWC1
        /// </summary>
        PWC1 = 826496848,

        /// <summary>
        /// PWC2
        /// </summary>
        PWC2 = 843274064,

        /// <summary>
        /// ET61X251
        /// </summary>
        ET61X251 = 892483141,

        /// <summary>
        /// SPCA501
        /// </summary>
        SPCA501 = 825242963,

        /// <summary>
        /// SPCA505
        /// </summary>
        SPCA505 = 892351827,

        /// <summary>
        /// SPCA508
        /// </summary>
        SPCA508 = 942683475,

        /// <summary>
        /// SPCA561
        /// </summary>
        SPCA561 = 825636179,

        /// <summary>
        /// PAC207
        /// </summary>
        PAC207 = 925905488,

        /// <summary>
        /// MR97310A
        /// </summary>
        MR97310A = 808530765,

        /// <summary>
        /// JL2005BCD
        /// </summary>
        JL2005BCD = 808602698,

        /// <summary>
        /// SN9C2028
        /// </summary>
        SN9C2028 = 1481527123,

        /// <summary>
        /// SQ905C
        /// </summary>
        SQ905C = 1127559225,

        /// <summary>
        /// PJPG
        /// </summary>
        PJPG = 1196444240,

        /// <summary>
        /// OV511
        /// </summary>
        OV511 = 825308495,

        /// <summary>
        /// OV518
        /// </summary>
        OV518 = 942749007,

        /// <summary>
        /// STV0680
        /// </summary>
        STV0680 = 808990291,

        /// <summary>
        /// TM6000
        /// </summary>
        TM6000 = 808865108,

        /// <summary>
        /// CIT_YYVYUY
        /// </summary>
        CIT_YYVYUY = 1448364355,

        /// <summary>
        /// KONICA420
        /// </summary>
        KONICA420 = 1229868875,

        /// <summary>
        /// JPGL
        /// </summary>
        JPGL = 1279742026,

        /// <summary>
        /// SE401
        /// </summary>
        SE401 = 825242707,

        /// <summary>
        /// S5C_UYVY_JPG
        /// </summary>
        S5C_UYVY_JPG = 1229141331,

        /// <summary>
        /// Y8I
        /// </summary>
        Y8I = 541669465,

        /// <summary>
        /// Y12I
        /// </summary>
        Y12I = 1228026201,

        /// <summary>
        /// Z16
        /// </summary>
        Z16 = 540422490,

        /// <summary>
        /// MT21C
        /// </summary>
        MT21C = 825381965,

        /// <summary>
        /// INZI
        /// </summary>
        INZI = 1230655049,

        /// <summary>
        /// SUNXI_TILED_NV12
        /// </summary>
        SUNXI_TILED_NV12 = 842093651,

        /// <summary>
        /// CNF4
        /// </summary>
        CNF4 = 877022787,

        /// <summary>
        /// IPU3_SBGGR10
        /// </summary>
        IPU3_SBGGR10 = 1647538281,

        /// <summary>
        /// IPU3_SGBRG10
        /// </summary>
        IPU3_SGBRG10 = 1731424361,

        /// <summary>
        /// IPU3_SGRBG10
        /// </summary>
        IPU3_SGRBG10 = 1194553449,

        /// <summary>
        /// IPU3_SRGGB10
        /// </summary>
        IPU3_SRGGB10 = 1915973737,

        /// <summary>
        /// CU8
        /// </summary>
        CU8 = 942691651,

        /// <summary>
        /// CU16LE
        /// </summary>
        CU16LE = 909202755,

        /// <summary>
        /// CS8
        /// </summary>
        CS8 = 942691139,

        /// <summary>
        /// CS14LE
        /// </summary>
        CS14LE = 875647811,

        /// <summary>
        /// RU12LE
        /// </summary>
        RU12LE = 842093906,

        /// <summary>
        /// PCU16BE
        /// </summary>
        PCU16BE = 909198160,

        /// <summary>
        /// PCU18BE
        /// </summary>
        PCU18BE = 942752592,

        /// <summary>
        /// PCU20BE
        /// </summary>
        PCU20BE = 808600400,

        /// <summary>
        /// DELTA_TD16
        /// </summary>
        DELTA_TD16 = 909198420,

        /// <summary>
        /// DELTA_TD08
        /// </summary>
        DELTA_TD08 = 942687316,

        /// <summary>
        /// TU16
        /// </summary>
        TU16 = 909202772,

        /// <summary>
        /// TU08
        /// </summary>
        TU08 = 942691668,

        /// <summary>
        /// VSP1_HGO
        /// </summary>
        VSP1_HGO = 1213223766,

        /// <summary>
        /// VSP1_HGT
        /// </summary>
        VSP1_HGT = 1414550358,

        /// <summary>
        /// UVC
        /// </summary>
        UVC = 1212372565,

        /// <summary>
        /// D4XX
        /// </summary>
        D4XX = 1482175556,
    }
}
