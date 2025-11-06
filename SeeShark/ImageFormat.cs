// Copyright (c) Speykious
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 2-Clause License. See LICENSE for details.

using System;

namespace SeeShark;

/// <summary>
/// Image formats supported by SeeShark.
/// <remarks>The construction of this list has been inspired from FFmpeg's LibAV*.</remarks>
/// </summary>
public enum ImageFormat : int
{
    /// <summary>
    /// planar YUV 4:2:0, 12bpp, (1 Cr &#38; Cb sample per 2x2 Y samples)
    /// </summary>
    Yuv420P,
    /// <summary>
    /// packed YUV 4:2:2, 16bpp, Y0 Cb Y1 Cr
    /// </summary>
    Yuyv422,
    /// <summary>
    /// packed RGB 8:8:8, 24bpp, RGBRGB...
    /// </summary>
    Rgb24,
    /// <summary>
    /// packed RGB 8:8:8, 24bpp, BGRBGR...
    /// </summary>
    Bgr24,
    /// <summary>
    /// planar YUV 4:2:2, 16bpp, (1 Cr &#38; Cb sample per 2x1 Y samples)
    /// </summary>
    Yuv422P,
    /// <summary>
    /// planar YUV 4:4:4, 24bpp, (1 Cr &#38; Cb sample per 1x1 Y samples)
    /// </summary>
    Yuv444P,
    /// <summary>
    /// planar YUV 4:1:0, 9bpp, (1 Cr &#38; Cb sample per 4x4 Y samples)
    /// </summary>
    Yuv410P,
    /// <summary>
    /// planar YUV 4:1:1, 12bpp, (1 Cr &#38; Cb sample per 4x1 Y samples)
    /// </summary>
    Yuv411P,
    /// <summary>
    /// Y , 8bpp
    /// </summary>
    Gray8,
    /// <summary>
    /// Y , 1bpp, 0 is white, 1 is black, in each byte pixels are ordered from the msb
    /// to the lsb
    /// </summary>
    Monowhite,
    /// <summary>
    /// Y , 1bpp, 0 is black, 1 is white, in each byte pixels are ordered from the msb
    /// to the lsb
    /// </summary>
    Monoblack,
    /// <summary>
    /// 8 bits with RGB32 palette
    /// </summary>
    Pal8,
    /// <summary>
    /// packed YUV 4:2:2, 16bpp, Cb Y0 Cr Y1
    /// </summary>
    Uyvy422,
    /// <summary>
    /// packed YUV 4:1:1, 12bpp, Cb Y0 Y1 Cr Y2 Y3
    /// </summary>
    Uyyvyy411,
    /// <summary>
    /// packed RGB 3:3:2, 8bpp, (msb)2B 3G 3R(lsb)
    /// </summary>
    Bgr8,
    /// <summary>
    /// packed RGB 1:2:1 bitstream, 4bpp, (msb)1B 2G 1R(lsb), a byte contains two pixels,
    /// the first pixel in the byte is the one composed by the 4 msb bits
    /// </summary>
    Bgr4,
    /// <summary>
    /// packed RGB 1:2:1, 8bpp, (msb)1B 2G 1R(lsb)
    /// </summary>
    Bgr4Byte,
    /// <summary>
    /// packed RGB 3:3:2, 8bpp, (msb)2R 3G 3B(lsb)
    /// </summary>
    Rgb8,
    /// <summary>
    /// packed RGB 1:2:1 bitstream, 4bpp, (msb)1R 2G 1B(lsb), a byte contains two pixels,
    /// the first pixel in the byte is the one composed by the 4 msb bits
    /// </summary>
    Rgb4,
    /// <summary>
    /// packed RGB 1:2:1, 8bpp, (msb)1R 2G 1B(lsb)
    /// </summary>
    Rgb4Byte,
    /// <summary>
    /// planar YUV 4:2:0, 12bpp, 1 plane for Y and 1 plane for the UV components, which
    /// are interleaved (first byte U and the following byte V)
    /// </summary>
    Nv12,
    /// <summary>
    /// as above, but U and V bytes are swapped
    /// </summary>
    Nv21,
    /// <summary>
    /// packed ARGB 8:8:8:8, 32bpp, ARGBARGB...
    /// </summary>
    Argb,
    /// <summary>
    /// packed RGBA 8:8:8:8, 32bpp, RGBARGBA...
    /// </summary>
    Rgba,
    /// <summary>
    /// packed ABGR 8:8:8:8, 32bpp, ABGRABGR...
    /// </summary>
    Abgr,
    /// <summary>
    /// packed BGRA 8:8:8:8, 32bpp, BGRABGRA...
    /// </summary>
    Bgra,
    /// <summary>
    /// Y , 16bpp, big-endian
    /// </summary>
    Gray16Be,
    /// <summary>
    /// Y , 16bpp, little-endian
    /// </summary>
    Gray16Le,
    /// <summary>
    /// planar YUV 4:4:0 (1 Cr &#38; Cb sample per 1x2 Y samples)
    /// </summary>
    Yuv440P,
    /// <summary>
    /// planar YUV 4:2:0, 20bpp, (1 Cr &#38; Cb sample per 2x2 Y &#38; A samples)
    /// </summary>
    Yuva420P,
    /// <summary>
    /// packed RGB 16:16:16, 48bpp, 16R, 16G, 16B, the 2-byte value for each R/G/B component
    /// is stored as big-endian
    /// </summary>
    Rgb48Be,
    /// <summary>
    /// packed RGB 16:16:16, 48bpp, 16R, 16G, 16B, the 2-byte value for each R/G/B component
    /// is stored as little-endian
    /// </summary>
    Rgb48Le,
    /// <summary>
    /// packed RGB 5:6:5, 16bpp, (msb) 5R 6G 5B(lsb), big-endian
    /// </summary>
    Rgb565Be,
    /// <summary>
    /// packed RGB 5:6:5, 16bpp, (msb) 5R 6G 5B(lsb), little-endian
    /// </summary>
    Rgb565Le,
    /// <summary>
    /// packed RGB 5:5:5, 16bpp, (msb)1X 5R 5G 5B(lsb), big-endian , X=unused/undefined
    /// </summary>
    Rgb555Be,
    /// <summary>
    /// packed RGB 5:5:5, 16bpp, (msb)1X 5R 5G 5B(lsb), little-endian, X=unused/undefined
    /// </summary>
    Rgb555Le,
    /// <summary>
    /// packed BGR 5:6:5, 16bpp, (msb) 5B 6G 5R(lsb), big-endian
    /// </summary>
    Bgr565Be,
    /// <summary>
    /// packed BGR 5:6:5, 16bpp, (msb) 5B 6G 5R(lsb), little-endian
    /// </summary>
    Bgr565Le,
    /// <summary>
    /// packed BGR 5:5:5, 16bpp, (msb)1X 5B 5G 5R(lsb), big-endian , X=unused/undefined
    /// </summary>
    Bgr555Be,
    /// <summary>
    /// packed BGR 5:5:5, 16bpp, (msb)1X 5B 5G 5R(lsb), little-endian, X=unused/undefined
    /// </summary>
    Bgr555Le,
    /// <summary>
    /// Hardware acceleration through VA-API, data[3] contains a VASurfaceID.
    /// </summary>
    Vaapi,
    /// <summary>
    /// planar YUV 4:2:0, 24bpp, (1 Cr &#38; Cb sample per 2x2 Y samples), little-endian
    /// </summary>
    Yuv420P16Le,
    /// <summary>
    /// planar YUV 4:2:0, 24bpp, (1 Cr &#38; Cb sample per 2x2 Y samples), big-endian
    /// </summary>
    Yuv420P16Be,
    /// <summary>
    /// planar YUV 4:2:2, 32bpp, (1 Cr &#38; Cb sample per 2x1 Y samples), little-endian
    /// </summary>
    Yuv422P16Le,
    /// <summary>
    /// planar YUV 4:2:2, 32bpp, (1 Cr &#38; Cb sample per 2x1 Y samples), big-endian
    /// </summary>
    Yuv422P16Be,
    /// <summary>
    /// planar YUV 4:4:4, 48bpp, (1 Cr &#38; Cb sample per 1x1 Y samples), little-endian
    /// </summary>
    Yuv444P16Le,
    /// <summary>
    /// planar YUV 4:4:4, 48bpp, (1 Cr &#38; Cb sample per 1x1 Y samples), big-endian
    /// </summary>
    Yuv444P16Be,
    /// <summary>
    /// HW decoding through DXVA2, Picture.data[3] contains a LPDIRECT3DSURFACE9 pointer
    /// </summary>
    Dxva2Vld,
    /// <summary>
    /// packed RGB 4:4:4, 16bpp, (msb)4X 4R 4G 4B(lsb), little-endian, X=unused/undefined
    /// </summary>
    Rgb444Le,
    /// <summary>
    /// packed RGB 4:4:4, 16bpp, (msb)4X 4R 4G 4B(lsb), big-endian, X=unused/undefined
    /// </summary>
    Rgb444Be,
    /// <summary>
    /// packed BGR 4:4:4, 16bpp, (msb)4X 4B 4G 4R(lsb), little-endian, X=unused/undefined
    /// </summary>
    Bgr444Le,
    /// <summary>
    /// packed BGR 4:4:4, 16bpp, (msb)4X 4B 4G 4R(lsb), big-endian, X=unused/undefined
    /// </summary>
    Bgr444Be,
    /// <summary>
    /// 8 bits gray, 8 bits alpha (also named Y400A, Gray8A)
    /// </summary>
    Ya8,
    /// <summary>
    /// packed RGB 16:16:16, 48bpp, 16B, 16G, 16R, the 2-byte value for each R/G/B component
    /// is stored as big-endian
    /// </summary>
    Bgr48Be,
    /// <summary>
    /// packed RGB 16:16:16, 48bpp, 16B, 16G, 16R, the 2-byte value for each R/G/B component
    /// is stored as little-endian
    /// </summary>
    Bgr48Le,
    /// <summary>
    /// planar YUV 4:2:0, 13.5bpp, (1 Cr &#38; Cb sample per 2x2 Y samples), big-endian
    /// </summary>
    Yuv420P9Be,
    /// <summary>
    /// planar YUV 4:2:0, 13.5bpp, (1 Cr &#38; Cb sample per 2x2 Y samples), little-endian
    /// </summary>
    Yuv420P9Le,
    /// <summary>
    /// planar YUV 4:2:0, 15bpp, (1 Cr &#38; Cb sample per 2x2 Y samples), big-endian
    /// </summary>
    Yuv420P10Be,
    /// <summary>
    /// planar YUV 4:2:0, 15bpp, (1 Cr &#38; Cb sample per 2x2 Y samples), little-endian
    /// </summary>
    Yuv420P10Le,
    /// <summary>
    /// planar YUV 4:2:2, 20bpp, (1 Cr &#38; Cb sample per 2x1 Y samples), big-endian
    /// </summary>
    Yuv422P10Be,
    /// <summary>
    /// planar YUV 4:2:2, 20bpp, (1 Cr &#38; Cb sample per 2x1 Y samples), little-endian
    /// </summary>
    Yuv422P10Le,
    /// <summary>
    /// planar YUV 4:4:4, 27bpp, (1 Cr &#38; Cb sample per 1x1 Y samples), big-endian
    /// </summary>
    Yuv444P9Be,
    /// <summary>
    /// planar YUV 4:4:4, 27bpp, (1 Cr &#38; Cb sample per 1x1 Y samples), little-endian
    /// </summary>
    Yuv444P9Le,
    /// <summary>
    /// planar YUV 4:4:4, 30bpp, (1 Cr &#38; Cb sample per 1x1 Y samples), big-endian
    /// </summary>
    Yuv444P10Be,
    /// <summary>
    /// planar YUV 4:4:4, 30bpp, (1 Cr &#38; Cb sample per 1x1 Y samples), little-endian
    /// </summary>
    Yuv444P10Le,
    /// <summary>
    /// planar YUV 4:2:2, 18bpp, (1 Cr &#38; Cb sample per 2x1 Y samples), big-endian
    /// </summary>
    Yuv422P9Be,
    /// <summary>
    /// planar YUV 4:2:2, 18bpp, (1 Cr &#38; Cb sample per 2x1 Y samples), little-endian
    /// </summary>
    Yuv422P9Le,
    /// <summary>
    /// planar GBR 4:4:4 24bpp (also named Gbr24P)
    /// </summary>
    Gbrp,
    /// <summary>
    /// planar GBR 4:4:4 27bpp, big-endian
    /// </summary>
    Gbrp9Be,
    /// <summary>
    /// planar GBR 4:4:4 27bpp, little-endian
    /// </summary>
    Gbrp9Le,
    /// <summary>
    /// planar GBR 4:4:4 30bpp, big-endian
    /// </summary>
    Gbrp10Be,
    /// <summary>
    /// planar GBR 4:4:4 30bpp, little-endian
    /// </summary>
    Gbrp10Le,
    /// <summary>
    /// planar GBR 4:4:4 48bpp, big-endian
    /// </summary>
    Gbrp16Be,
    /// <summary>
    /// planar GBR 4:4:4 48bpp, little-endian
    /// </summary>
    Gbrp16Le,
    /// <summary>
    /// planar YUV 4:2:2 24bpp, (1 Cr &#38; Cb sample per 2x1 Y &#38; A samples)
    /// </summary>
    Yuva422P,
    /// <summary>
    /// planar YUV 4:4:4 32bpp, (1 Cr &#38; Cb sample per 1x1 Y &#38; A samples)
    /// </summary>
    Yuva444P,
    /// <summary>
    /// planar YUV 4:2:0 22.5bpp, (1 Cr &#38; Cb sample per 2x2 Y &#38; A samples), big-endian
    /// </summary>
    Yuva420P9Be,
    /// <summary>
    /// planar YUV 4:2:0 22.5bpp, (1 Cr &#38; Cb sample per 2x2 Y &#38; A samples), little-endian
    /// </summary>
    Yuva420P9Le,
    /// <summary>
    /// planar YUV 4:2:2 27bpp, (1 Cr &#38; Cb sample per 2x1 Y &#38; A samples), big-endian
    /// </summary>
    Yuva422P9Be,
    /// <summary>
    /// planar YUV 4:2:2 27bpp, (1 Cr &#38; Cb sample per 2x1 Y &#38; A samples), little-endian
    /// </summary>
    Yuva422P9Le,
    /// <summary>
    /// planar YUV 4:4:4 36bpp, (1 Cr &#38; Cb sample per 1x1 Y &#38; A samples), big-endian
    /// </summary>
    Yuva444P9Be,
    /// <summary>
    /// planar YUV 4:4:4 36bpp, (1 Cr &#38; Cb sample per 1x1 Y &#38; A samples), little-endian
    /// </summary>
    Yuva444P9Le,
    /// <summary>
    /// planar YUV 4:2:0 25bpp, (1 Cr &#38; Cb sample per 2x2 Y &#38; A samples, big-endian)
    /// </summary>
    Yuva420P10Be,
    /// <summary>
    /// planar YUV 4:2:0 25bpp, (1 Cr &#38; Cb sample per 2x2 Y &#38; A samples, little-endian)
    /// </summary>
    Yuva420P10Le,
    /// <summary>
    /// planar YUV 4:2:2 30bpp, (1 Cr &#38; Cb sample per 2x1 Y &#38; A samples, big-endian)
    /// </summary>
    Yuva422P10Be,
    /// <summary>
    /// planar YUV 4:2:2 30bpp, (1 Cr &#38; Cb sample per 2x1 Y &#38; A samples, little-endian)
    /// </summary>
    Yuva422P10Le,
    /// <summary>
    /// planar YUV 4:4:4 40bpp, (1 Cr &#38; Cb sample per 1x1 Y &#38; A samples, big-endian)
    /// </summary>
    Yuva444P10Be,
    /// <summary>
    /// planar YUV 4:4:4 40bpp, (1 Cr &#38; Cb sample per 1x1 Y &#38; A samples, little-endian)
    /// </summary>
    Yuva444P10Le,
    /// <summary>
    /// planar YUV 4:2:0 40bpp, (1 Cr &#38; Cb sample per 2x2 Y &#38; A samples, big-endian)
    /// </summary>
    Yuva420P16Be,
    /// <summary>
    /// planar YUV 4:2:0 40bpp, (1 Cr &#38; Cb sample per 2x2 Y &#38; A samples, little-endian)
    /// </summary>
    Yuva420P16Le,
    /// <summary>
    /// planar YUV 4:2:2 48bpp, (1 Cr &#38; Cb sample per 2x1 Y &#38; A samples, big-endian)
    /// </summary>
    Yuva422P16Be,
    /// <summary>
    /// planar YUV 4:2:2 48bpp, (1 Cr &#38; Cb sample per 2x1 Y &#38; A samples, little-endian)
    /// </summary>
    Yuva422P16Le,
    /// <summary>
    /// planar YUV 4:4:4 64bpp, (1 Cr &#38; Cb sample per 1x1 Y &#38; A samples, big-endian)
    /// </summary>
    Yuva444P16Be,
    /// <summary>
    /// planar YUV 4:4:4 64bpp, (1 Cr &#38; Cb sample per 1x1 Y &#38; A samples, little-endian)
    /// </summary>
    Yuva444P16Le,
    /// <summary>
    /// HW acceleration through VDPAU, Picture.data[3] contains a VdpVideoSurface
    /// </summary>
    Vdpau,
    /// <summary>
    /// packed XYZ 4:4:4, 36 bpp, (msb) 12X, 12Y, 12Z (lsb), the 2-byte value for each
    /// X/Y/Z is stored as little-endian, the 4 lower bits are set to 0
    /// </summary>
    Xyz12Le,
    /// <summary>
    /// packed XYZ 4:4:4, 36 bpp, (msb) 12X, 12Y, 12Z (lsb), the 2-byte value for each
    /// X/Y/Z is stored as big-endian, the 4 lower bits are set to 0
    /// </summary>
    Xyz12Be,
    /// <summary>
    /// interleaved chroma YUV 4:2:2, 16bpp, (1 Cr &#38; Cb sample per 2x1 Y samples)
    /// </summary>
    Nv16,
    /// <summary>
    /// interleaved chroma YUV 4:2:2, 20bpp, (1 Cr &#38; Cb sample per 2x1 Y samples), little-endian
    /// </summary>
    Nv20Le,
    /// <summary>
    /// interleaved chroma YUV 4:2:2, 20bpp, (1 Cr &#38; Cb sample per 2x1 Y samples), big-endian
    /// </summary>
    Nv20Be,
    /// <summary>
    /// packed RGBA 16:16:16:16, 64bpp, 16R, 16G, 16B, 16A, the 2-byte value for each
    /// R/G/B/A component is stored as big-endian
    /// </summary>
    Rgba64Be,
    /// <summary>
    /// packed RGBA 16:16:16:16, 64bpp, 16R, 16G, 16B, 16A, the 2-byte value for each
    /// R/G/B/A component is stored as little-endian
    /// </summary>
    Rgba64Le,
    /// <summary>
    /// packed RGBA 16:16:16:16, 64bpp, 16B, 16G, 16R, 16A, the 2-byte value for each
    /// R/G/B/A component is stored as big-endian
    /// </summary>
    Bgra64Be,
    /// <summary>
    /// packed RGBA 16:16:16:16, 64bpp, 16B, 16G, 16R, 16A, the 2-byte value for each
    /// R/G/B/A component is stored as little-endian
    /// </summary>
    Bgra64Le,
    /// <summary>
    /// packed YUV 4:2:2, 16bpp, Y0 Cr Y1 Cb
    /// </summary>
    Yvyu422,
    /// <summary>
    /// 16 bits gray, 16 bits alpha (big-endian)
    /// </summary>
    Ya16Be,
    /// <summary>
    /// 16 bits gray, 16 bits alpha (little-endian)
    /// </summary>
    Ya16Le,
    /// <summary>
    /// planar GBRA 4:4:4:4 32bpp
    /// </summary>
    Gbrap,
    /// <summary>
    /// planar GBRA 4:4:4:4 64bpp, big-endian
    /// </summary>
    Gbrap16Be,
    /// <summary>
    /// planar GBRA 4:4:4:4 64bpp, little-endian
    /// </summary>
    Gbrap16Le,
    /// <summary>
    /// HW acceleration through QSV, data[3] contains a pointer to the mfxFrameSurface1
    /// structure.
    /// </summary>
    Qsv,
    /// <summary>
    /// HW acceleration though MMAL, data[3] contains a pointer to the MMAL_BUFFER_HEADER_T
    /// structure.
    /// </summary>
    Mmal,
    /// <summary>
    /// HW decoding through Direct3D11 via old API, Picture.data[3] contains a ID3D11VideoDecoderOutputView
    /// pointer
    /// </summary>
    D3D11VaVld,
    /// <summary>
    /// HW acceleration through CUDA. data[i] contain CUdeviceptr pointers exactly as
    /// for system memory frames.
    /// </summary>
    Cuda,
    /// <summary>
    /// packed RGB 8:8:8, 32bpp, XRGBXRGB... X=unused/undefined
    /// </summary>
    Xrgb,
    /// <summary>
    /// packed RGB 8:8:8, 32bpp, RGBXRGBX... X=unused/undefined
    /// </summary>
    Rgbx,
    /// <summary>
    /// packed BGR 8:8:8, 32bpp, XBGRXBGR... X=unused/undefined
    /// </summary>
    Xbgr,
    /// <summary>
    /// packed BGR 8:8:8, 32bpp, BGRXBGRX... X=unused/undefined
    /// </summary>
    Bgrx,
    /// <summary>
    /// planar YUV 4:2:0,18bpp, (1 Cr &#38; Cb sample per 2x2 Y samples), big-endian
    /// </summary>
    Yuv420P12Be,
    /// <summary>
    /// planar YUV 4:2:0,18bpp, (1 Cr &#38; Cb sample per 2x2 Y samples), little-endian
    /// </summary>
    Yuv420P12Le,
    /// <summary>
    /// planar YUV 4:2:0,21bpp, (1 Cr &#38; Cb sample per 2x2 Y samples), big-endian
    /// </summary>
    Yuv420P14Be,
    /// <summary>
    /// planar YUV 4:2:0,21bpp, (1 Cr &#38; Cb sample per 2x2 Y samples), little-endian
    /// </summary>
    Yuv420P14Le,
    /// <summary>
    /// planar YUV 4:2:2,24bpp, (1 Cr &#38; Cb sample per 2x1 Y samples), big-endian
    /// </summary>
    Yuv422P12Be,
    /// <summary>
    /// planar YUV 4:2:2,24bpp, (1 Cr &#38; Cb sample per 2x1 Y samples), little-endian
    /// </summary>
    Yuv422P12Le,
    /// <summary>
    /// planar YUV 4:2:2,28bpp, (1 Cr &#38; Cb sample per 2x1 Y samples), big-endian
    /// </summary>
    Yuv422P14Be,
    /// <summary>
    /// planar YUV 4:2:2,28bpp, (1 Cr &#38; Cb sample per 2x1 Y samples), little-endian
    /// </summary>
    Yuv422P14Le,
    /// <summary>
    /// planar YUV 4:4:4,36bpp, (1 Cr &#38; Cb sample per 1x1 Y samples), big-endian
    /// </summary>
    Yuv444P12Be,
    /// <summary>
    /// planar YUV 4:4:4,36bpp, (1 Cr &#38; Cb sample per 1x1 Y samples), little-endian
    /// </summary>
    Yuv444P12Le,
    /// <summary>
    /// planar YUV 4:4:4,42bpp, (1 Cr &#38; Cb sample per 1x1 Y samples), big-endian
    /// </summary>
    Yuv444P14Be,
    /// <summary>
    /// planar YUV 4:4:4,42bpp, (1 Cr &#38; Cb sample per 1x1 Y samples), little-endian
    /// </summary>
    Yuv444P14Le,
    /// <summary>
    /// planar GBR 4:4:4 36bpp, big-endian
    /// </summary>
    Gbrp12Be,
    /// <summary>
    /// planar GBR 4:4:4 36bpp, little-endian
    /// </summary>
    Gbrp12Le,
    /// <summary>
    /// planar GBR 4:4:4 42bpp, big-endian
    /// </summary>
    Gbrp14Be,
    /// <summary>
    /// planar GBR 4:4:4 42bpp, little-endian
    /// </summary>
    Gbrp14Le,
    /// <summary>
    /// bayer, BGBG..(odd line), GRGR..(even line), 8-bit samples
    /// </summary>
    BayerBggr8,
    /// <summary>
    /// bayer, RGRG..(odd line), GBGB..(even line), 8-bit samples
    /// </summary>
    BayerRggb8,
    /// <summary>
    /// bayer, GBGB..(odd line), RGRG..(even line), 8-bit samples
    /// </summary>
    BayerGbrg8,
    /// <summary>
    /// bayer, GRGR..(odd line), BGBG..(even line), 8-bit samples
    /// </summary>
    BayerGrbg8,
    /// <summary>
    /// bayer, BGBG..(odd line), GRGR..(even line), 16-bit samples, little-endian
    /// </summary>
    BayerBggr16Le,
    /// <summary>
    /// bayer, BGBG..(odd line), GRGR..(even line), 16-bit samples, big-endian
    /// </summary>
    BayerBggr16Be,
    /// <summary>
    /// bayer, RGRG..(odd line), GBGB..(even line), 16-bit samples, little-endian
    /// </summary>
    BayerRggb16Le,
    /// <summary>
    /// bayer, RGRG..(odd line), GBGB..(even line), 16-bit samples, big-endian
    /// </summary>
    BayerRggb16Be,
    /// <summary>
    /// bayer, GBGB..(odd line), RGRG..(even line), 16-bit samples, little-endian
    /// </summary>
    BayerGbrg16Le,
    /// <summary>
    /// bayer, GBGB..(odd line), RGRG..(even line), 16-bit samples, big-endian
    /// </summary>
    BayerGbrg16Be,
    /// <summary>
    /// bayer, GRGR..(odd line), BGBG..(even line), 16-bit samples, little-endian
    /// </summary>
    BayerGrbg16Le,
    /// <summary>
    /// bayer, GRGR..(odd line), BGBG..(even line), 16-bit samples, big-endian
    /// </summary>
    BayerGrbg16Be,
    /// <summary>
    /// XVideo Motion Acceleration via common packet passing
    /// </summary>
    Xvmc,
    /// <summary>
    /// planar YUV 4:4:0,20bpp, (1 Cr &#38; Cb sample per 1x2 Y samples), little-endian
    /// </summary>
    Yuv440P10Le,
    /// <summary>
    /// planar YUV 4:4:0,20bpp, (1 Cr &#38; Cb sample per 1x2 Y samples), big-endian
    /// </summary>
    Yuv440P10Be,
    /// <summary>
    /// planar YUV 4:4:0,24bpp, (1 Cr &#38; Cb sample per 1x2 Y samples), little-endian
    /// </summary>
    Yuv440P12Le,
    /// <summary>
    /// planar YUV 4:4:0,24bpp, (1 Cr &#38; Cb sample per 1x2 Y samples), big-endian
    /// </summary>
    Yuv440P12Be,
    /// <summary>
    /// packed AYUV 4:4:4,64bpp (1 Cr &#38; Cb sample per 1x1 Y &#38; A samples), little-endian
    /// </summary>
    Ayuv64Le,
    /// <summary>
    /// packed AYUV 4:4:4,64bpp (1 Cr &#38; Cb sample per 1x1 Y &#38; A samples), big-endian
    /// </summary>
    Ayuv64Be,
    /// <summary>
    /// hardware decoding through Videotoolbox
    /// </summary>
    Videotoolbox,
    /// <summary>
    /// like NV12, with 10bpp per component, data in the high bits, zeros in the low
    /// bits, little-endian
    /// </summary>
    P010Le,
    /// <summary>
    /// like NV12, with 10bpp per component, data in the high bits, zeros in the low
    /// bits, big-endian
    /// </summary>
    P010Be,
    /// <summary>
    /// planar GBR 4:4:4:4 48bpp, big-endian
    /// </summary>
    Gbrap12Be,
    /// <summary>
    /// planar GBR 4:4:4:4 48bpp, little-endian
    /// </summary>
    Gbrap12Le,
    /// <summary>
    /// planar GBR 4:4:4:4 40bpp, big-endian
    /// </summary>
    Gbrap10Be,
    /// <summary>
    /// planar GBR 4:4:4:4 40bpp, little-endian
    /// </summary>
    Gbrap10Le,
    /// <summary>
    /// hardware decoding through MediaCodec
    /// </summary>
    Mediacodec,
    /// <summary>
    /// Y , 12bpp, big-endian
    /// </summary>
    Gray12Be,
    /// <summary>
    /// Y , 12bpp, little-endian
    /// </summary>
    Gray12Le,
    /// <summary>
    /// Y , 10bpp, big-endian
    /// </summary>
    Gray10Be,
    /// <summary>
    /// Y , 10bpp, little-endian
    /// </summary>
    Gray10Le,
    /// <summary>
    /// like NV12, with 16bpp per component, little-endian
    /// </summary>
    P016Le,
    /// <summary>
    /// like NV12, with 16bpp per component, big-endian
    /// </summary>
    P016Be,
    /// <summary>
    /// Hardware surfaces for Direct3D11.
    /// </summary>
    D3D11,
    /// <summary>
    /// Y , 9bpp, big-endian
    /// </summary>
    Gray9Be,
    /// <summary>
    /// Y , 9bpp, little-endian
    /// </summary>
    Gray9Le,
    /// <summary>
    /// IEEE-754 single precision planar GBR 4:4:4, 96bpp, big-endian
    /// </summary>
    Gbrpf32Be,
    /// <summary>
    /// IEEE-754 single precision planar GBR 4:4:4, 96bpp, little-endian
    /// </summary>
    Gbrpf32Le,
    /// <summary>
    /// IEEE-754 single precision planar GBRA 4:4:4:4, 128bpp, big-endian
    /// </summary>
    Gbrapf32Be,
    /// <summary>
    /// IEEE-754 single precision planar GBRA 4:4:4:4, 128bpp, little-endian
    /// </summary>
    Gbrapf32Le,
    /// <summary>
    /// DRM-managed buffers exposed through PRIME buffer sharing.
    /// </summary>
    DrmPrime,
    /// <summary>
    /// Hardware surfaces for OpenCL.
    /// </summary>
    Opencl,
    /// <summary>
    /// Y , 14bpp, big-endian
    /// </summary>
    Gray14Be,
    /// <summary>
    /// Y , 14bpp, little-endian
    /// </summary>
    Gray14Le,
    /// <summary>
    /// IEEE-754 single precision Y, 32bpp, big-endian
    /// </summary>
    Grayf32Be,
    /// <summary>
    /// IEEE-754 single precision Y, 32bpp, little-endian
    /// </summary>
    Grayf32Le,
    /// <summary>
    /// planar YUV 4:2:2,24bpp, (1 Cr &#38; Cb sample per 2x1 Y samples), 12b alpha, big-endian
    /// </summary>
    Yuva422P12Be,
    /// <summary>
    /// planar YUV 4:2:2,24bpp, (1 Cr &#38; Cb sample per 2x1 Y samples), 12b alpha, little-endian
    /// </summary>
    Yuva422P12Le,
    /// <summary>
    /// planar YUV 4:4:4,36bpp, (1 Cr &#38; Cb sample per 1x1 Y samples), 12b alpha, big-endian
    /// </summary>
    Yuva444P12Be,
    /// <summary>
    /// planar YUV 4:4:4,36bpp, (1 Cr &#38; Cb sample per 1x1 Y samples), 12b alpha, little-endian
    /// </summary>
    Yuva444P12Le,
    /// <summary>
    /// planar YUV 4:4:4, 24bpp, 1 plane for Y and 1 plane for the UV components, which
    /// are interleaved (first byte U and the following byte V)
    /// </summary>
    Nv24,
    /// <summary>
    /// as above, but U and V bytes are swapped
    /// </summary>
    Nv42,
    /// <summary>
    /// Vulkan hardware images.
    /// </summary>
    Vulkan,
    /// <summary>
    /// packed YUV 4:2:2 like YUYV422, 20bpp, data in the high bits, big-endian
    /// </summary>
    Y210Be,
    /// <summary>
    /// packed YUV 4:2:2 like YUYV422, 20bpp, data in the high bits, little-endian
    /// </summary>
    Y210Le,
    /// <summary>
    /// packed RGB 10:10:10, 30bpp, (msb)2X 10R 10G 10B(lsb), little-endian, X=unused/undefined
    /// </summary>
    X2Rgb10Le,
    /// <summary>
    /// packed RGB 10:10:10, 30bpp, (msb)2X 10R 10G 10B(lsb), big-endian, X=unused/undefined
    /// </summary>
    X2Rgb10Be,
    /// <summary>
    /// packed BGR 10:10:10, 30bpp, (msb)2X 10B 10G 10R(lsb), little-endian, X=unused/undefined
    /// </summary>
    X2Bgr10Le,
    /// <summary>
    /// packed BGR 10:10:10, 30bpp, (msb)2X 10B 10G 10R(lsb), big-endian, X=unused/undefined
    /// </summary>
    X2Bgr10Be,
    /// <summary>
    /// interleaved chroma YUV 4:2:2, 20bpp, data in the high bits, big-endian
    /// </summary>
    P210Be,
    /// <summary>
    /// interleaved chroma YUV 4:2:2, 20bpp, data in the high bits, little-endian
    /// </summary>
    P210Le,
    /// <summary>
    /// interleaved chroma YUV 4:4:4, 30bpp, data in the high bits, big-endian
    /// </summary>
    P410Be,
    /// <summary>
    /// interleaved chroma YUV 4:4:4, 30bpp, data in the high bits, little-endian
    /// </summary>
    P410Le,
    /// <summary>
    /// interleaved chroma YUV 4:2:2, 32bpp, big-endian
    /// </summary>
    P216Be,
    /// <summary>
    /// interleaved chroma YUV 4:2:2, 32bpp, liddle-endian
    /// </summary>
    P216Le,
    /// <summary>
    /// interleaved chroma YUV 4:4:4, 48bpp, big-endian
    /// </summary>
    P416Be,
    /// <summary>
    /// interleaved chroma YUV 4:4:4, 48bpp, little-endian
    /// </summary>
    P416Le,

    /// <summary>
    /// MJPEG compressed image
    /// </summary>
    Mjpeg,
}

public readonly struct ImageFormatInfo
{
    /// <summary>
    /// The <see href="https://wiki.multimedia.cx/index.php/FourCC"/>FourCC</see> code of this image format.
    /// </summary>
    public readonly uint FourCC = 0x00000000;

    public readonly string Name;
    public readonly string? FFmpegInputFormat;
    public readonly bool IsRaw;
    public readonly string Description;

    internal ImageFormatInfo(uint fourCC, string name, string? ffmpegInputFormat, bool isRaw, string description)
    {
        FourCC = fourCC;
        Name = name;
        FFmpegInputFormat = ffmpegInputFormat;
        IsRaw = isRaw;
        Description = description;
    }

    public static ImageFormatInfo FromImageFormat(ImageFormat imageFormat)
    {
        return imageFormat switch
        {
#pragma warning disable format
            ImageFormat.Bgr24   => new ImageFormatInfo((uint)imageFormat, "BGR",    "bgr",     isRaw: true,  "BGR 24-bit"),
            ImageFormat.Rgb24   => new ImageFormatInfo((uint)imageFormat, "RGB",    "rgb",     isRaw: true,  "RGB 24-bit"),
            ImageFormat.Argb    => new ImageFormatInfo((uint)imageFormat, "ARGB",   "argb",    isRaw: true,  "ARGB 32-bit"),
            ImageFormat.Bgra    => new ImageFormatInfo((uint)imageFormat, "BGRA",   "bgra",    isRaw: true,  "BGRA 32-bit"),
            ImageFormat.Abgr    => new ImageFormatInfo((uint)imageFormat, "ABGR",   "abgr",    isRaw: true,  "ABGR 32-bit"),
            ImageFormat.Rgba    => new ImageFormatInfo((uint)imageFormat, "RGBA",   "rgba",    isRaw: true,  "RGBA 32-bit"),

            ImageFormat.Yuyv422 => new ImageFormatInfo((uint)imageFormat, "YUYV",   "yuyv422", isRaw: true,  "Component Y'CbCr 8-bit 4:2:2, ordered Y'0 Cb Y'1 Cr"),
            ImageFormat.Yvyu422 => new ImageFormatInfo((uint)imageFormat, "YVYU",   "yvyu422", isRaw: true,  "Component Y'CbCr 8-bit 4:2:2, ordered Y'0 Cr Y'1 Cb"),
            ImageFormat.Uyvy422 => new ImageFormatInfo((uint)imageFormat, "UYVY",   "uyvy422", isRaw: true,  "Component Y'CbCr 8-bit 4:2:2, ordered Cb Y'0 Cr Y'1"),
            ImageFormat.Yuv411P => new ImageFormatInfo((uint)imageFormat, "Y41P",   "yuv411p", isRaw: true,  "YUV 4:1:1"),
            ImageFormat.Yuv444P => new ImageFormatInfo((uint)imageFormat, "YUV444", "yuv444p", isRaw: true,  "YUV 4:4:4"),

            ImageFormat.Mjpeg   => new ImageFormatInfo((uint)imageFormat, "MJPG",   "mjpeg",   isRaw: false, "MJPEG compressed"),
#pragma warning restore format
            _ => throw new NotImplementedException(),
        };

    }
}
