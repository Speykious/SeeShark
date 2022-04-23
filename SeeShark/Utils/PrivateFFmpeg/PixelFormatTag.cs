// Copyright (c) The Vignette Authors
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 3-Clause License. See LICENSE for details.

namespace SeeShark.Utils.PrivateFFmpeg
{
    /// <summary>
    /// https://github.com/FFmpeg/FFmpeg/blob/a64e250680fbc7296eff714b81b54b1c0e2d185f/libavcodec/raw.h#L32-L35
    /// </summary>
    internal class PixelFormatTag
    {
        public PixelFormat PixelFormat { get; set; }
        public int FourCC { get; set; }

        public PixelFormatTag(PixelFormat pixelFormat, int fourcc)
        {
            PixelFormat = pixelFormat;
            FourCC = fourcc;
        }

#pragma warning disable CS0618
        /// <summary>
        /// https://github.com/FFmpeg/FFmpeg/blob/a64e250680fbc7296eff714b81b54b1c0e2d185f/libavcodec/raw.c#L31-L298
        /// </summary>
        public static PixelFormatTag[] RawPixelFormatTags = new PixelFormatTag[]
        {
            new PixelFormatTag(PixelFormat.Yuv420P, mkTag('I', '4', '2', '0')), // Planar formats
            new PixelFormatTag(PixelFormat.Yuv420P, mkTag('I', 'Y', 'U', 'V')),
            new PixelFormatTag(PixelFormat.Yuv420P, mkTag('y', 'v', '1', '2')),
            new PixelFormatTag(PixelFormat.Yuv420P, mkTag('Y', 'V', '1', '2')),
            new PixelFormatTag(PixelFormat.Yuv410P, mkTag('Y', 'U', 'V', '9')),
            new PixelFormatTag(PixelFormat.Yuv410P, mkTag('Y', 'V', 'U', '9')),
            new PixelFormatTag(PixelFormat.Yuv411P, mkTag('Y', '4', '1', 'B')),
            new PixelFormatTag(PixelFormat.Yuv422P, mkTag('Y', '4', '2', 'B')),
            new PixelFormatTag(PixelFormat.Yuv422P, mkTag('P', '4', '2', '2')),
            new PixelFormatTag(PixelFormat.Yuv422P, mkTag('Y', 'V', '1', '6')),

            // yuvjXXX formats are deprecated hacks specific to libav*, they are identical to yuvXXX
            new PixelFormatTag(PixelFormat.Yuvj420P, mkTag('I', '4', '2', '0')), // Planar formats
            new PixelFormatTag(PixelFormat.Yuvj420P, mkTag('I', 'Y', 'U', 'V')),
            new PixelFormatTag(PixelFormat.Yuvj420P, mkTag('Y', 'V', '1', '2')),
            new PixelFormatTag(PixelFormat.Yuvj422P, mkTag('Y', '4', '2', 'B')),
            new PixelFormatTag(PixelFormat.Yuvj422P, mkTag('P', '4', '2', '2')),
            new PixelFormatTag(PixelFormat.Gray8, mkTag('Y', '8', '0', '0')),
            new PixelFormatTag(PixelFormat.Gray8, mkTag('Y', '8', ' ', ' ')),

            new PixelFormatTag(PixelFormat.Yuyv422, mkTag('Y', 'U', 'Y', '2')), // Packed formats
            new PixelFormatTag(PixelFormat.Yuyv422, mkTag('Y', '4', '2', '2')),
            new PixelFormatTag(PixelFormat.Yuyv422, mkTag('V', '4', '2', '2')),
            new PixelFormatTag(PixelFormat.Yuyv422, mkTag('V', 'Y', 'U', 'Y')),
            new PixelFormatTag(PixelFormat.Yuyv422, mkTag('Y', 'U', 'N', 'V')),
            new PixelFormatTag(PixelFormat.Yuyv422, mkTag('Y', 'U', 'Y', 'V')),
            new PixelFormatTag(PixelFormat.Yvyu422, mkTag('Y', 'V', 'Y', 'U')), // Philips
            new PixelFormatTag(PixelFormat.Uyvy422, mkTag('U', 'Y', 'V', 'Y')),
            new PixelFormatTag(PixelFormat.Uyvy422, mkTag('H', 'D', 'Y', 'C')),
            new PixelFormatTag(PixelFormat.Uyvy422, mkTag('U', 'Y', 'N', 'V')),
            new PixelFormatTag(PixelFormat.Uyvy422, mkTag('U', 'Y', 'N', 'Y')),
            new PixelFormatTag(PixelFormat.Uyvy422, mkTag('u', 'y', 'v', '1')),
            new PixelFormatTag(PixelFormat.Uyvy422, mkTag('2', 'V', 'u', '1')),
            new PixelFormatTag(PixelFormat.Uyvy422, mkTag('A', 'V', 'R', 'n')), // Avid AVI Codec 1:1
            new PixelFormatTag(PixelFormat.Uyvy422, mkTag('A', 'V', '1', 'x')), // Avid 1:1x
            new PixelFormatTag(PixelFormat.Uyvy422, mkTag('A', 'V', 'u', 'p')),
            new PixelFormatTag(PixelFormat.Uyvy422, mkTag('V', 'D', 'T', 'Z')), // SoftLab-NSK VideoTizer
            new PixelFormatTag(PixelFormat.Uyvy422, mkTag('a', 'u', 'v', '2')),
            new PixelFormatTag(PixelFormat.Uyvy422, mkTag('c', 'y', 'u', 'v')), // CYUV is also Creative YUV
            new PixelFormatTag(PixelFormat.Uyyvyy411, mkTag('Y', '4', '1', '1')),
            new PixelFormatTag(PixelFormat.Gray8, mkTag('G', 'R', 'E', 'Y')),
            new PixelFormatTag(PixelFormat.Nv12, mkTag('N', 'V', '1', '2')),
            new PixelFormatTag(PixelFormat.Nv21, mkTag('N', 'V', '2', '1')),

            // nut
            new PixelFormatTag(PixelFormat.Rgb555Le, mkTag('R', 'G', 'B', 15)),
            new PixelFormatTag(PixelFormat.Bgr555Le, mkTag('B', 'G', 'R', 15)),
            new PixelFormatTag(PixelFormat.Rgb565Le, mkTag('R', 'G', 'B', 16)),
            new PixelFormatTag(PixelFormat.Bgr565Le, mkTag('B', 'G', 'R', 16)),
            new PixelFormatTag(PixelFormat.Rgb555Be, mkTag(15, 'B', 'G', 'R')),
            new PixelFormatTag(PixelFormat.Bgr555Be, mkTag(15, 'R', 'G', 'B')),
            new PixelFormatTag(PixelFormat.Rgb565Be, mkTag(16, 'B', 'G', 'R')),
            new PixelFormatTag(PixelFormat.Bgr565Be, mkTag(16, 'R', 'G', 'B')),
            new PixelFormatTag(PixelFormat.Rgb444Le, mkTag('R', 'G', 'B', 12)),
            new PixelFormatTag(PixelFormat.Bgr444Le, mkTag('B', 'G', 'R', 12)),
            new PixelFormatTag(PixelFormat.Rgb444Be, mkTag(12, 'B', 'G', 'R')),
            new PixelFormatTag(PixelFormat.Bgr444Be, mkTag(12, 'R', 'G', 'B')),
            new PixelFormatTag(PixelFormat.Rgba64Le, mkTag('R', 'B', 'A', 64)),
            new PixelFormatTag(PixelFormat.Bgra64Le, mkTag('B', 'R', 'A', 64)),
            new PixelFormatTag(PixelFormat.Rgba64Be, mkTag(64, 'R', 'B', 'A')),
            new PixelFormatTag(PixelFormat.Bgra64Be, mkTag(64, 'B', 'R', 'A')),
            new PixelFormatTag(PixelFormat.Rgba, mkTag('R', 'G', 'B', 'A')),
            new PixelFormatTag(PixelFormat.Rgbx, mkTag('R', 'G', 'B', 0)),
            new PixelFormatTag(PixelFormat.Bgra, mkTag('B', 'G', 'R', 'A')),
            new PixelFormatTag(PixelFormat.Bgrx, mkTag('B', 'G', 'R', 0)),
            new PixelFormatTag(PixelFormat.Abgr, mkTag('A', 'B', 'G', 'R')),
            new PixelFormatTag(PixelFormat.Xbgr, mkTag(0, 'B', 'G', 'R')),
            new PixelFormatTag(PixelFormat.Argb, mkTag('A', 'R', 'G', 'B')),
            new PixelFormatTag(PixelFormat.Xrgb, mkTag(0, 'R', 'G', 'B')),
            new PixelFormatTag(PixelFormat.Rgb24, mkTag('R', 'G', 'B', 24)),
            new PixelFormatTag(PixelFormat.Bgr24, mkTag('B', 'G', 'R', 24)),
            new PixelFormatTag(PixelFormat.Yuv411P, mkTag('4', '1', '1', 'P')),
            new PixelFormatTag(PixelFormat.Yuv422P, mkTag('4', '2', '2', 'P')),
            new PixelFormatTag(PixelFormat.Yuvj422P, mkTag('4', '2', '2', 'P')),
            new PixelFormatTag(PixelFormat.Yuv440P, mkTag('4', '4', '0', 'P')),
            new PixelFormatTag(PixelFormat.Yuvj440P, mkTag('4', '4', '0', 'P')),
            new PixelFormatTag(PixelFormat.Yuv444P, mkTag('4', '4', '4', 'P')),
            new PixelFormatTag(PixelFormat.Yuvj444P, mkTag('4', '4', '4', 'P')),
            new PixelFormatTag(PixelFormat.Monowhite, mkTag('B', '1', 'W', '0')),
            new PixelFormatTag(PixelFormat.Monoblack, mkTag('B', '0', 'W', '1')),
            new PixelFormatTag(PixelFormat.Bgr8, mkTag('B', 'G', 'R', 8)),
            new PixelFormatTag(PixelFormat.Rgb8, mkTag('R', 'G', 'B', 8)),
            new PixelFormatTag(PixelFormat.Bgr4, mkTag('B', 'G', 'R', 4)),
            new PixelFormatTag(PixelFormat.Rgb4, mkTag('R', 'G', 'B', 4)),
            new PixelFormatTag(PixelFormat.Rgb4Byte,mkTag('B', '4', 'B', 'Y')),
            new PixelFormatTag(PixelFormat.Bgr4Byte,mkTag('R', '4', 'B', 'Y')),
            new PixelFormatTag(PixelFormat.Rgb48Le, mkTag('R', 'G', 'B', 48)),
            new PixelFormatTag(PixelFormat.Rgb48Be, mkTag(48, 'R', 'G', 'B')),
            new PixelFormatTag(PixelFormat.Bgr48Le, mkTag('B', 'G', 'R', 48)),
            new PixelFormatTag(PixelFormat.Bgr48Be, mkTag(48, 'B', 'G', 'R')),
            new PixelFormatTag(PixelFormat.Gray9Le, mkTag('Y', '1', 0, 9)),
            new PixelFormatTag(PixelFormat.Gray9Be, mkTag(9, 0, '1', 'Y')),
            new PixelFormatTag(PixelFormat.Gray10Le, mkTag('Y', '1', 0, 10)),
            new PixelFormatTag(PixelFormat.Gray10Be, mkTag(10, 0, '1', 'Y')),
            new PixelFormatTag(PixelFormat.Gray12Le, mkTag('Y', '1', 0, 12)),
            new PixelFormatTag(PixelFormat.Gray12Be, mkTag(12, 0, '1', 'Y')),
            new PixelFormatTag(PixelFormat.Gray14Le, mkTag('Y', '1', 0, 14)),
            new PixelFormatTag(PixelFormat.Gray14Be, mkTag(14, 0, '1', 'Y')),
            new PixelFormatTag(PixelFormat.Gray16Le, mkTag('Y', '1', 0, 16)),
            new PixelFormatTag(PixelFormat.Gray16Be, mkTag(16, 0, '1', 'Y')),
            new PixelFormatTag(PixelFormat.Yuv420P9Le, mkTag('Y', '3', 11, 9)),
            new PixelFormatTag(PixelFormat.Yuv420P9Be, mkTag(9, 11, '3', 'Y')),
            new PixelFormatTag(PixelFormat.Yuv422P9Le, mkTag('Y', '3', 10, 9)),
            new PixelFormatTag(PixelFormat.Yuv422P9Be, mkTag(9, 10, '3', 'Y')),
            new PixelFormatTag(PixelFormat.Yuv444P9Le, mkTag('Y', '3', 0, 9)),
            new PixelFormatTag(PixelFormat.Yuv444P9Be, mkTag(9, 0, '3', 'Y')),
            new PixelFormatTag(PixelFormat.Yuv420P10Le, mkTag('Y', '3', 11, 10)),
            new PixelFormatTag(PixelFormat.Yuv420P10Be, mkTag(10, 11, '3', 'Y')),
            new PixelFormatTag(PixelFormat.Yuv422P10Le, mkTag('Y', '3', 10, 10)),
            new PixelFormatTag(PixelFormat.Yuv422P10Be, mkTag(10, 10, '3', 'Y')),
            new PixelFormatTag(PixelFormat.Yuv444P10Le, mkTag('Y', '3', 0, 10)),
            new PixelFormatTag(PixelFormat.Yuv444P10Be, mkTag(10, 0, '3', 'Y')),
            new PixelFormatTag(PixelFormat.Yuv420P12Le, mkTag('Y', '3', 11, 12)),
            new PixelFormatTag(PixelFormat.Yuv420P12Be, mkTag(12, 11, '3', 'Y')),
            new PixelFormatTag(PixelFormat.Yuv422P12Le, mkTag('Y', '3', 10, 12)),
            new PixelFormatTag(PixelFormat.Yuv422P12Be, mkTag(12, 10, '3', 'Y')),
            new PixelFormatTag(PixelFormat.Yuv444P12Le, mkTag('Y', '3', 0, 12)),
            new PixelFormatTag(PixelFormat.Yuv444P12Be, mkTag(12, 0, '3', 'Y')),
            new PixelFormatTag(PixelFormat.Yuv420P14Le, mkTag('Y', '3', 11, 14)),
            new PixelFormatTag(PixelFormat.Yuv420P14Be, mkTag(14, 11, '3', 'Y')),
            new PixelFormatTag(PixelFormat.Yuv422P14Le, mkTag('Y', '3', 10, 14)),
            new PixelFormatTag(PixelFormat.Yuv422P14Be, mkTag(14, 10, '3', 'Y')),
            new PixelFormatTag(PixelFormat.Yuv444P14Le, mkTag('Y', '3', 0, 14)),
            new PixelFormatTag(PixelFormat.Yuv444P14Be, mkTag(14, 0, '3', 'Y')),
            new PixelFormatTag(PixelFormat.Yuv420P16Le, mkTag('Y', '3', 11, 16)),
            new PixelFormatTag(PixelFormat.Yuv420P16Be, mkTag(16, 11, '3', 'Y')),
            new PixelFormatTag(PixelFormat.Yuv422P16Le, mkTag('Y', '3', 10, 16)),
            new PixelFormatTag(PixelFormat.Yuv422P16Be, mkTag(16, 10, '3', 'Y')),
            new PixelFormatTag(PixelFormat.Yuv444P16Le, mkTag('Y', '3', 0, 16)),
            new PixelFormatTag(PixelFormat.Yuv444P16Be, mkTag(16, 0, '3', 'Y')),
            new PixelFormatTag(PixelFormat.Yuva420P, mkTag('Y', '4', 11, 8)),
            new PixelFormatTag(PixelFormat.Yuva422P, mkTag('Y', '4', 10, 8)),
            new PixelFormatTag(PixelFormat.Yuva444P, mkTag('Y', '4', 0, 8)),
            new PixelFormatTag(PixelFormat.Ya8, mkTag('Y', '2', 0, 8)),
            new PixelFormatTag(PixelFormat.Pal8, mkTag('P', 'A', 'L', 8)),

            new PixelFormatTag(PixelFormat.Yuva420P9Le, mkTag('Y', '4', 11, 9)),
            new PixelFormatTag(PixelFormat.Yuva420P9Be, mkTag(9, 11, '4', 'Y')),
            new PixelFormatTag(PixelFormat.Yuva422P9Le, mkTag('Y', '4', 10, 9)),
            new PixelFormatTag(PixelFormat.Yuva422P9Be, mkTag(9, 10, '4', 'Y')),
            new PixelFormatTag(PixelFormat.Yuva444P9Le, mkTag('Y', '4', 0, 9)),
            new PixelFormatTag(PixelFormat.Yuva444P9Be, mkTag(9, 0, '4', 'Y')),
            new PixelFormatTag(PixelFormat.Yuva420P10Le, mkTag('Y', '4', 11, 10)),
            new PixelFormatTag(PixelFormat.Yuva420P10Be, mkTag(10, 11, '4', 'Y')),
            new PixelFormatTag(PixelFormat.Yuva422P10Le, mkTag('Y', '4', 10, 10)),
            new PixelFormatTag(PixelFormat.Yuva422P10Be, mkTag(10, 10, '4', 'Y')),
            new PixelFormatTag(PixelFormat.Yuva444P10Le, mkTag('Y', '4', 0, 10)),
            new PixelFormatTag(PixelFormat.Yuva444P10Be, mkTag(10, 0, '4', 'Y')),
            new PixelFormatTag(PixelFormat.Yuva422P12Le, mkTag('Y', '4', 10, 12)),
            new PixelFormatTag(PixelFormat.Yuva422P12Be, mkTag(12, 10, '4', 'Y')),
            new PixelFormatTag(PixelFormat.Yuva444P12Le, mkTag('Y', '4', 0, 12)),
            new PixelFormatTag(PixelFormat.Yuva444P12Be, mkTag(12, 0, '4', 'Y')),
            new PixelFormatTag(PixelFormat.Yuva420P16Le, mkTag('Y', '4', 11, 16)),
            new PixelFormatTag(PixelFormat.Yuva420P16Be, mkTag(16, 11, '4', 'Y')),
            new PixelFormatTag(PixelFormat.Yuva422P16Le, mkTag('Y', '4', 10, 16)),
            new PixelFormatTag(PixelFormat.Yuva422P16Be, mkTag(16, 10, '4', 'Y')),
            new PixelFormatTag(PixelFormat.Yuva444P16Le, mkTag('Y', '4', 0, 16)),
            new PixelFormatTag(PixelFormat.Yuva444P16Be, mkTag(16, 0, '4', 'Y')),

            new PixelFormatTag(PixelFormat.Gbrp, mkTag('G', '3', 00, 8)),
            new PixelFormatTag(PixelFormat.Gbrp9Le, mkTag('G', '3', 00, 9)),
            new PixelFormatTag(PixelFormat.Gbrp9Be, mkTag(9, 00, '3', 'G')),
            new PixelFormatTag(PixelFormat.Gbrp10Le, mkTag('G', '3', 00, 10)),
            new PixelFormatTag(PixelFormat.Gbrp10Be, mkTag(10, 00, '3', 'G')),
            new PixelFormatTag(PixelFormat.Gbrp12Le, mkTag('G', '3', 00, 12)),
            new PixelFormatTag(PixelFormat.Gbrp12Be, mkTag(12, 00, '3', 'G')),
            new PixelFormatTag(PixelFormat.Gbrp14Le, mkTag('G', '3', 00, 14)),
            new PixelFormatTag(PixelFormat.Gbrp14Be, mkTag(14, 00, '3', 'G')),
            new PixelFormatTag(PixelFormat.Gbrp16Le, mkTag('G', '3', 00, 16)),
            new PixelFormatTag(PixelFormat.Gbrp16Be, mkTag(16, 00, '3', 'G')),

            new PixelFormatTag(PixelFormat.Gbrap, mkTag('G', '4', 00, 8)),
            new PixelFormatTag(PixelFormat.Gbrap10Le, mkTag('G', '4', 00, 10)),
            new PixelFormatTag(PixelFormat.Gbrap10Be, mkTag(10, 00, '4', 'G')),
            new PixelFormatTag(PixelFormat.Gbrap12Le, mkTag('G', '4', 00, 12)),
            new PixelFormatTag(PixelFormat.Gbrap12Be, mkTag(12, 00, '4', 'G')),
            new PixelFormatTag(PixelFormat.Gbrap16Le, mkTag('G', '4', 00, 16)),
            new PixelFormatTag(PixelFormat.Gbrap16Be, mkTag(16, 00, '4', 'G')),

            new PixelFormatTag(PixelFormat.Xyz12Le, mkTag('X', 'Y', 'Z', 36)),
            new PixelFormatTag(PixelFormat.Xyz12Be, mkTag(36, 'Z', 'Y', 'X')),

            new PixelFormatTag(PixelFormat.BayerBggr8, mkTag(0xBA, 'B', 'G', 8)),
            new PixelFormatTag(PixelFormat.BayerBggr16Le, mkTag(0xBA, 'B', 'G', 16)),
            new PixelFormatTag(PixelFormat.BayerBggr16Be, mkTag(16, 'G', 'B', 0xBA)),
            new PixelFormatTag(PixelFormat.BayerRggb8, mkTag(0xBA, 'R', 'G', 8)),
            new PixelFormatTag(PixelFormat.BayerRggb16Le, mkTag(0xBA, 'R', 'G', 16)),
            new PixelFormatTag(PixelFormat.BayerRggb16Be, mkTag(16, 'G', 'R', 0xBA)),
            new PixelFormatTag(PixelFormat.BayerGbrg8, mkTag(0xBA, 'G', 'B', 8)),
            new PixelFormatTag(PixelFormat.BayerGbrg16Le, mkTag(0xBA, 'G', 'B', 16)),
            new PixelFormatTag(PixelFormat.BayerGbrg16Be, mkTag(16, 'B', 'G', 0xBA)),
            new PixelFormatTag(PixelFormat.BayerGrbg8, mkTag(0xBA, 'G', 'R', 8)),
            new PixelFormatTag(PixelFormat.BayerGrbg16Le, mkTag(0xBA, 'G', 'R', 16)),
            new PixelFormatTag(PixelFormat.BayerGrbg16Be, mkTag(16, 'R', 'G', 0xBA)),

            // quicktime
            new PixelFormatTag(PixelFormat.Yuv420P, mkTag('R', '4', '2', '0')), // Radius DV YUV PAL
            new PixelFormatTag(PixelFormat.Yuv411P, mkTag('R', '4', '1', '1')), // Radius DV YUV NTSC
            new PixelFormatTag(PixelFormat.Uyvy422, mkTag('2', 'v', 'u', 'y')),
            new PixelFormatTag(PixelFormat.Uyvy422, mkTag('2', 'V', 'u', 'y')),
            new PixelFormatTag(PixelFormat.Uyvy422, mkTag('A', 'V', 'U', 'I')), // FIXME merge both fields
            new PixelFormatTag(PixelFormat.Uyvy422, mkTag('b', 'x', 'y', 'v')),
            new PixelFormatTag(PixelFormat.Yuyv422, mkTag('y', 'u', 'v', '2')),
            new PixelFormatTag(PixelFormat.Yuyv422, mkTag('y', 'u', 'v', 's')),
            new PixelFormatTag(PixelFormat.Yuyv422, mkTag('D', 'V', 'O', 'O')), // Digital Voodoo SD 8 Bit
            new PixelFormatTag(PixelFormat.Rgb555Le, mkTag('L', '5', '5', '5')),
            new PixelFormatTag(PixelFormat.Rgb565Le, mkTag('L', '5', '6', '5')),
            new PixelFormatTag(PixelFormat.Rgb565Be, mkTag('B', '5', '6', '5')),
            new PixelFormatTag(PixelFormat.Bgr24, mkTag('2', '4', 'B', 'G')),
            new PixelFormatTag(PixelFormat.Bgr24, mkTag('b', 'x', 'b', 'g')),
            new PixelFormatTag(PixelFormat.Bgra, mkTag('B', 'G', 'R', 'A')),
            new PixelFormatTag(PixelFormat.Rgba, mkTag('R', 'G', 'B', 'A')),
            new PixelFormatTag(PixelFormat.Rgb24, mkTag('b', 'x', 'r', 'g')),
            new PixelFormatTag(PixelFormat.Abgr, mkTag('A', 'B', 'G', 'R')),
            new PixelFormatTag(PixelFormat.Gray16Be, mkTag('b', '1', '6', 'g')),
            new PixelFormatTag(PixelFormat.Rgb48Be, mkTag('b', '4', '8', 'r')),
            new PixelFormatTag(PixelFormat.Rgba64Be, mkTag('b', '6', '4', 'a')),
            new PixelFormatTag(PixelFormat.BayerRggb16Be, mkTag('B', 'G', 'G', 'R')),

            // vlc
            new PixelFormatTag(PixelFormat.Yuv410P, mkTag('I', '4', '1', '0')),
            new PixelFormatTag(PixelFormat.Yuv411P, mkTag('I', '4', '1', '1')),
            new PixelFormatTag(PixelFormat.Yuv422P, mkTag('I', '4', '2', '2')),
            new PixelFormatTag(PixelFormat.Yuv440P, mkTag('I', '4', '4', '0')),
            new PixelFormatTag(PixelFormat.Yuv444P, mkTag('I', '4', '4', '4')),
            new PixelFormatTag(PixelFormat.Yuvj420P, mkTag('J', '4', '2', '0')),
            new PixelFormatTag(PixelFormat.Yuvj422P, mkTag('J', '4', '2', '2')),
            new PixelFormatTag(PixelFormat.Yuvj440P, mkTag('J', '4', '4', '0')),
            new PixelFormatTag(PixelFormat.Yuvj444P, mkTag('J', '4', '4', '4')),
            new PixelFormatTag(PixelFormat.Yuva444P, mkTag('Y', 'U', 'V', 'A')),
            new PixelFormatTag(PixelFormat.Yuva420P, mkTag('I', '4', '0', 'A')),
            new PixelFormatTag(PixelFormat.Yuva422P, mkTag('I', '4', '2', 'A')),
            new PixelFormatTag(PixelFormat.Rgb8, mkTag('R', 'G', 'B', '2')),
            new PixelFormatTag(PixelFormat.Rgb555Le, mkTag('R', 'V', '1', '5')),
            new PixelFormatTag(PixelFormat.Rgb565Le, mkTag('R', 'V', '1', '6')),
            new PixelFormatTag(PixelFormat.Bgr24, mkTag('R', 'V', '2', '4')),
            new PixelFormatTag(PixelFormat.Bgrx, mkTag('R', 'V', '3', '2')),
            new PixelFormatTag(PixelFormat.Rgba, mkTag('A', 'V', '3', '2')),
            new PixelFormatTag(PixelFormat.Yuv420P9Le, mkTag('I', '0', '9', 'L')),
            new PixelFormatTag(PixelFormat.Yuv420P9Be, mkTag('I', '0', '9', 'B')),
            new PixelFormatTag(PixelFormat.Yuv422P9Le, mkTag('I', '2', '9', 'L')),
            new PixelFormatTag(PixelFormat.Yuv422P9Be, mkTag('I', '2', '9', 'B')),
            new PixelFormatTag(PixelFormat.Yuv444P9Le, mkTag('I', '4', '9', 'L')),
            new PixelFormatTag(PixelFormat.Yuv444P9Be, mkTag('I', '4', '9', 'B')),
            new PixelFormatTag(PixelFormat.Yuv420P10Le, mkTag('I', '0', 'A', 'L')),
            new PixelFormatTag(PixelFormat.Yuv420P10Be, mkTag('I', '0', 'A', 'B')),
            new PixelFormatTag(PixelFormat.Yuv422P10Le, mkTag('I', '2', 'A', 'L')),
            new PixelFormatTag(PixelFormat.Yuv422P10Be, mkTag('I', '2', 'A', 'B')),
            new PixelFormatTag(PixelFormat.Yuv444P10Le, mkTag('I', '4', 'A', 'L')),
            new PixelFormatTag(PixelFormat.Yuv444P10Be, mkTag('I', '4', 'A', 'B')),
            new PixelFormatTag(PixelFormat.Yuv420P12Le, mkTag('I', '0', 'C', 'L')),
            new PixelFormatTag(PixelFormat.Yuv420P12Be, mkTag('I', '0', 'C', 'B')),
            new PixelFormatTag(PixelFormat.Yuv422P12Le, mkTag('I', '2', 'C', 'L')),
            new PixelFormatTag(PixelFormat.Yuv422P12Be, mkTag('I', '2', 'C', 'B')),
            new PixelFormatTag(PixelFormat.Yuv444P12Le, mkTag('I', '4', 'C', 'L')),
            new PixelFormatTag(PixelFormat.Yuv444P12Be, mkTag('I', '4', 'C', 'B')),
            new PixelFormatTag(PixelFormat.Yuv420P16Le, mkTag('I', '0', 'F', 'L')),
            new PixelFormatTag(PixelFormat.Yuv420P16Be, mkTag('I', '0', 'F', 'B')),
            new PixelFormatTag(PixelFormat.Yuv444P16Le, mkTag('I', '4', 'F', 'L')),
            new PixelFormatTag(PixelFormat.Yuv444P16Be, mkTag('I', '4', 'F', 'B')),

            // special
            new PixelFormatTag(PixelFormat.Rgb565Le, mkTag(3, 0, 0, 0)), // flipped RGB565LE
            new PixelFormatTag(PixelFormat.Yuv444P, mkTag('Y', 'V', '2', '4')), // YUV444P, swapped UV

            new PixelFormatTag(PixelFormat.None, 0),
        };
#pragma warning restore CS0618

        /// <summary>
        /// https://github.com/FFmpeg/FFmpeg/blob/a64e250680fbc7296eff714b81b54b1c0e2d185f/libavutil/macros.h#L55
        /// </summary>
        private static int mkTag(int a, int b, int c, int d) => a | (b << 8) | (c << 16) | (d << 24);

        /// <summary>
        /// https://github.com/FFmpeg/FFmpeg/blob/a64e250680fbc7296eff714b81b54b1c0e2d185f/libavcodec/raw.c#L341-L369
        /// </summary>
        public static PixelFormat FindRawPixelFormat(int fourcc)
        {
            foreach (PixelFormatTag tag in RawPixelFormatTags)
            {
                if (tag.FourCC == fourcc)
                    return tag.PixelFormat;
            }

            return PixelFormat.None;
        }
    }
}
