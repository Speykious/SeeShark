using FFmpeg.AutoGen;

namespace SeeShark
{
    public enum PixelFormat : int
    {
        None = AVPixelFormat.AV_PIX_FMT_NONE,
        /// planar YUV 4:2:0, 12bpp, (1 Cr & Cb sample per 2x2 Y samples)
        Yuv420P = AVPixelFormat.AV_PIX_FMT_YUV420P,
        /// packed YUV 4:2:2, 16bpp, Y0 Cb Y1 Cr
        Yuyv422 = AVPixelFormat.AV_PIX_FMT_YUYV422,
        /// packed RGB 8:8:8, 24bpp, RGBRGB...
        Rgb24 = AVPixelFormat.AV_PIX_FMT_RGB24,
        /// packed RGB 8:8:8, 24bpp, BGRBGR...
        Bgr24 = AVPixelFormat.AV_PIX_FMT_BGR24,
        /// planar YUV 4:2:2, 16bpp, (1 Cr & Cb sample per 2x1 Y samples)
        Yuv422P = AVPixelFormat.AV_PIX_FMT_YUV422P,
        /// planar YUV 4:4:4, 24bpp, (1 Cr & Cb sample per 1x1 Y samples)
        Yuv444P = AVPixelFormat.AV_PIX_FMT_YUV444P,
        /// planar YUV 4:1:0, 9bpp, (1 Cr & Cb sample per 4x4 Y samples)
        Yuv410P = AVPixelFormat.AV_PIX_FMT_YUV410P,
        /// planar YUV 4:1:1, 12bpp, (1 Cr & Cb sample per 4x1 Y samples)
        Yuv411P = AVPixelFormat.AV_PIX_FMT_YUV411P,
        /// Y , 8bpp
        Gray8 = AVPixelFormat.AV_PIX_FMT_GRAY8,
        /// Y , 1bpp, 0 is white, 1 is black, in each byte pixels are ordered from the msb
        /// to the lsb
        Monowhite = AVPixelFormat.AV_PIX_FMT_MONOWHITE,
        /// Y , 1bpp, 0 is black, 1 is white, in each byte pixels are ordered from the msb
        /// to the lsb
        Monoblack = AVPixelFormat.AV_PIX_FMT_MONOBLACK,
        /// 8 bits with AV_PIX_FMT_RGB32 palette
        Pal8 = AVPixelFormat.AV_PIX_FMT_PAL8,
        /// planar YUV 4:2:0, 12bpp, full scale (JPEG), deprecated in favor of AV_PIX_FMT_YUV420P
        /// and setting color_range
        Yuvj420P = AVPixelFormat.AV_PIX_FMT_YUVJ420P,
        /// planar YUV 4:2:2, 16bpp, full scale (JPEG), deprecated in favor of AV_PIX_FMT_YUV422P
        /// and setting color_range
        Yuvj422P = AVPixelFormat.AV_PIX_FMT_YUVJ422P,
        /// planar YUV 4:4:4, 24bpp, full scale (JPEG), deprecated in favor of AV_PIX_FMT_YUV444P
        /// and setting color_range
        Yuvj444P = AVPixelFormat.AV_PIX_FMT_YUVJ444P,
        /// packed YUV 4:2:2, 16bpp, Cb Y0 Cr Y1
        Uyvy422 = AVPixelFormat.AV_PIX_FMT_UYVY422,
        /// packed YUV 4:1:1, 12bpp, Cb Y0 Y1 Cr Y2 Y3
        Uyyvyy411 = AVPixelFormat.AV_PIX_FMT_UYYVYY411,
        /// packed RGB 3:3:2, 8bpp, (msb)2B 3G 3R(lsb)
        Bgr8 = AVPixelFormat.AV_PIX_FMT_BGR8,
        /// packed RGB 1:2:1 bitstream, 4bpp, (msb)1B 2G 1R(lsb), a byte contains two pixels,
        /// the first pixel in the byte is the one composed by the 4 msb bits
        Bgr4 = AVPixelFormat.AV_PIX_FMT_BGR4,
        /// packed RGB 1:2:1, 8bpp, (msb)1B 2G 1R(lsb)
        Bgr4Byte = AVPixelFormat.AV_PIX_FMT_BGR4_BYTE,
        /// packed RGB 3:3:2, 8bpp, (msb)2R 3G 3B(lsb)
        Rgb8 = AVPixelFormat.AV_PIX_FMT_RGB8,
        /// packed RGB 1:2:1 bitstream, 4bpp, (msb)1R 2G 1B(lsb), a byte contains two pixels,
        /// the first pixel in the byte is the one composed by the 4 msb bits
        Rgb4 = AVPixelFormat.AV_PIX_FMT_RGB4,
        /// packed RGB 1:2:1, 8bpp, (msb)1R 2G 1B(lsb)
        Rgb4Byte = AVPixelFormat.AV_PIX_FMT_RGB4_BYTE,
        /// planar YUV 4:2:0, 12bpp, 1 plane for Y and 1 plane for the UV components, which
        /// are interleaved (first byte U and the following byte V)
        Nv12 = AVPixelFormat.AV_PIX_FMT_NV12,
        /// as above, but U and V bytes are swapped
        Nv21 = AVPixelFormat.AV_PIX_FMT_NV21,
        /// packed ARGB 8:8:8:8, 32bpp, ARGBARGB...
        Argb = AVPixelFormat.AV_PIX_FMT_ARGB,
        /// packed RGBA 8:8:8:8, 32bpp, RGBARGBA...
        Rgba = AVPixelFormat.AV_PIX_FMT_RGBA,
        /// packed ABGR 8:8:8:8, 32bpp, ABGRABGR...
        Abgr = AVPixelFormat.AV_PIX_FMT_ABGR,
        /// packed BGRA 8:8:8:8, 32bpp, BGRABGRA...
        Bgra = AVPixelFormat.AV_PIX_FMT_BGRA,
        /// Y , 16bpp, big-endian
        Gray16Be = AVPixelFormat.AV_PIX_FMT_GRAY16BE,
        /// Y , 16bpp, little-endian
        Gray16Le = AVPixelFormat.AV_PIX_FMT_GRAY16LE,
        /// planar YUV 4:4:0 (1 Cr & Cb sample per 1x2 Y samples)
        Yuv440P = AVPixelFormat.AV_PIX_FMT_YUV440P,
        /// planar YUV 4:4:0 full scale (JPEG), deprecated in favor of AV_PIX_FMT_YUV440P
        /// and setting color_range
        Yuvj440P = AVPixelFormat.AV_PIX_FMT_YUVJ440P,
        /// planar YUV 4:2:0, 20bpp, (1 Cr & Cb sample per 2x2 Y & A samples)
        Yuva420P = AVPixelFormat.AV_PIX_FMT_YUVA420P,
        /// packed RGB 16:16:16, 48bpp, 16R, 16G, 16B, the 2-byte value for each R/G/B component
        /// is stored as big-endian
        Rgb48Be = AVPixelFormat.AV_PIX_FMT_RGB48BE,
        /// packed RGB 16:16:16, 48bpp, 16R, 16G, 16B, the 2-byte value for each R/G/B component
        /// is stored as little-endian
        Rgb48Le = AVPixelFormat.AV_PIX_FMT_RGB48LE,
        /// packed RGB 5:6:5, 16bpp, (msb) 5R 6G 5B(lsb), big-endian
        Rgb565Be = AVPixelFormat.AV_PIX_FMT_RGB565BE,
        /// packed RGB 5:6:5, 16bpp, (msb) 5R 6G 5B(lsb), little-endian
        Rgb565Le = AVPixelFormat.AV_PIX_FMT_RGB565LE,
        /// packed RGB 5:5:5, 16bpp, (msb)1X 5R 5G 5B(lsb), big-endian , X=unused/undefined
        Rgb555Be = AVPixelFormat.AV_PIX_FMT_RGB555BE,
        /// packed RGB 5:5:5, 16bpp, (msb)1X 5R 5G 5B(lsb), little-endian, X=unused/undefined
        Rgb555Le = AVPixelFormat.AV_PIX_FMT_RGB555LE,
        /// packed BGR 5:6:5, 16bpp, (msb) 5B 6G 5R(lsb), big-endian
        Bgr565Be = AVPixelFormat.AV_PIX_FMT_BGR565BE,
        /// packed BGR 5:6:5, 16bpp, (msb) 5B 6G 5R(lsb), little-endian
        Bgr565Le = AVPixelFormat.AV_PIX_FMT_BGR565LE,
        /// packed BGR 5:5:5, 16bpp, (msb)1X 5B 5G 5R(lsb), big-endian , X=unused/undefined
        Bgr555Be = AVPixelFormat.AV_PIX_FMT_BGR555BE,
        /// packed BGR 5:5:5, 16bpp, (msb)1X 5B 5G 5R(lsb), little-endian, X=unused/undefined
        Bgr555Le = AVPixelFormat.AV_PIX_FMT_BGR555LE,
        /// HW acceleration through VA API at motion compensation entry-point, Picture.data[3]
        /// contains a vaapi_render_state struct which contains macroblocks as well as various
        /// fields extracted from headers
        VaapiMoco = AVPixelFormat.AV_PIX_FMT_VAAPI_MOCO,
        /// HW acceleration through VA API at IDCT entry-point, Picture.data[3] contains
        /// a vaapi_render_state struct which contains fields extracted from headers
        VaapiIdct = AVPixelFormat.AV_PIX_FMT_VAAPI_IDCT,
        /// HW decoding through VA API, Picture.data[3] contains a VASurfaceID
        VaapiVld = AVPixelFormat.AV_PIX_FMT_VAAPI_VLD,
        /// @}
        Vaapi = AVPixelFormat.AV_PIX_FMT_VAAPI,
        /// planar YUV 4:2:0, 24bpp, (1 Cr & Cb sample per 2x2 Y samples), little-endian
        Yuv420P16Le = AVPixelFormat.AV_PIX_FMT_YUV420P16LE,
        /// planar YUV 4:2:0, 24bpp, (1 Cr & Cb sample per 2x2 Y samples), big-endian
        Yuv420P16Be = AVPixelFormat.AV_PIX_FMT_YUV420P16BE,
        /// planar YUV 4:2:2, 32bpp, (1 Cr & Cb sample per 2x1 Y samples), little-endian
        Yuv422P16Le = AVPixelFormat.AV_PIX_FMT_YUV422P16LE,
        /// planar YUV 4:2:2, 32bpp, (1 Cr & Cb sample per 2x1 Y samples), big-endian
        Yuv422P16Be = AVPixelFormat.AV_PIX_FMT_YUV422P16BE,
        /// planar YUV 4:4:4, 48bpp, (1 Cr & Cb sample per 1x1 Y samples), little-endian
        Yuv444P16Le = AVPixelFormat.AV_PIX_FMT_YUV444P16LE,
        /// planar YUV 4:4:4, 48bpp, (1 Cr & Cb sample per 1x1 Y samples), big-endian
        Yuv444P16Be = AVPixelFormat.AV_PIX_FMT_YUV444P16BE,
        /// HW decoding through DXVA2, Picture.data[3] contains a LPDIRECT3DSURFACE9 pointer
        Dxva2Vld = AVPixelFormat.AV_PIX_FMT_DXVA2_VLD,
        /// packed RGB 4:4:4, 16bpp, (msb)4X 4R 4G 4B(lsb), little-endian, X=unused/undefined
        Rgb444Le = AVPixelFormat.AV_PIX_FMT_RGB444LE,
        /// packed RGB 4:4:4, 16bpp, (msb)4X 4R 4G 4B(lsb), big-endian, X=unused/undefined
        Rgb444Be = AVPixelFormat.AV_PIX_FMT_RGB444BE,
        /// packed BGR 4:4:4, 16bpp, (msb)4X 4B 4G 4R(lsb), little-endian, X=unused/undefined
        Bgr444Le = AVPixelFormat.AV_PIX_FMT_BGR444LE,
        /// packed BGR 4:4:4, 16bpp, (msb)4X 4B 4G 4R(lsb), big-endian, X=unused/undefined
        Bgr444Be = AVPixelFormat.AV_PIX_FMT_BGR444BE,
        /// 8 bits gray, 8 bits alpha
        Ya8 = AVPixelFormat.AV_PIX_FMT_YA8,
        /// alias for AV_PIX_FMT_YA8
        Y400A = AVPixelFormat.AV_PIX_FMT_Y400A,
        /// alias for AV_PIX_FMT_YA8
        Gray8A = AVPixelFormat.AV_PIX_FMT_GRAY8A,
        /// packed RGB 16:16:16, 48bpp, 16B, 16G, 16R, the 2-byte value for each R/G/B component
        /// is stored as big-endian
        Bgr48Be = AVPixelFormat.AV_PIX_FMT_BGR48BE,
        /// packed RGB 16:16:16, 48bpp, 16B, 16G, 16R, the 2-byte value for each R/G/B component
        /// is stored as little-endian
        Bgr48Le = AVPixelFormat.AV_PIX_FMT_BGR48LE,
        /// planar YUV 4:2:0, 13.5bpp, (1 Cr & Cb sample per 2x2 Y samples), big-endian
        Yuv420P9Be = AVPixelFormat.AV_PIX_FMT_YUV420P9BE,
        /// planar YUV 4:2:0, 13.5bpp, (1 Cr & Cb sample per 2x2 Y samples), little-endian
        Yuv420P9Le = AVPixelFormat.AV_PIX_FMT_YUV420P9LE,
        /// planar YUV 4:2:0, 15bpp, (1 Cr & Cb sample per 2x2 Y samples), big-endian
        Yuv420P10Be = AVPixelFormat.AV_PIX_FMT_YUV420P10BE,
        /// planar YUV 4:2:0, 15bpp, (1 Cr & Cb sample per 2x2 Y samples), little-endian
        Yuv420P10Le = AVPixelFormat.AV_PIX_FMT_YUV420P10LE,
        /// planar YUV 4:2:2, 20bpp, (1 Cr & Cb sample per 2x1 Y samples), big-endian
        Yuv422P10Be = AVPixelFormat.AV_PIX_FMT_YUV422P10BE,
        /// planar YUV 4:2:2, 20bpp, (1 Cr & Cb sample per 2x1 Y samples), little-endian
        Yuv422P10Le = AVPixelFormat.AV_PIX_FMT_YUV422P10LE,
        /// planar YUV 4:4:4, 27bpp, (1 Cr & Cb sample per 1x1 Y samples), big-endian
        Yuv444P9Be = AVPixelFormat.AV_PIX_FMT_YUV444P9BE,
        /// planar YUV 4:4:4, 27bpp, (1 Cr & Cb sample per 1x1 Y samples), little-endian
        Yuv444P9Le = AVPixelFormat.AV_PIX_FMT_YUV444P9LE,
        /// planar YUV 4:4:4, 30bpp, (1 Cr & Cb sample per 1x1 Y samples), big-endian
        Yuv444P10Be = AVPixelFormat.AV_PIX_FMT_YUV444P10BE,
        /// planar YUV 4:4:4, 30bpp, (1 Cr & Cb sample per 1x1 Y samples), little-endian
        Yuv444P10Le = AVPixelFormat.AV_PIX_FMT_YUV444P10LE,
        /// planar YUV 4:2:2, 18bpp, (1 Cr & Cb sample per 2x1 Y samples), big-endian
        Yuv422P9Be = AVPixelFormat.AV_PIX_FMT_YUV422P9BE,
        /// planar YUV 4:2:2, 18bpp, (1 Cr & Cb sample per 2x1 Y samples), little-endian
        Yuv422P9Le = AVPixelFormat.AV_PIX_FMT_YUV422P9LE,
        /// planar GBR 4:4:4 24bpp
        Gbrp = AVPixelFormat.AV_PIX_FMT_GBRP,
        Gbr24P = AVPixelFormat.AV_PIX_FMT_GBR24P,
        /// planar GBR 4:4:4 27bpp, big-endian
        Gbrp9Be = AVPixelFormat.AV_PIX_FMT_GBRP9BE,
        /// planar GBR 4:4:4 27bpp, little-endian
        Gbrp9Le = AVPixelFormat.AV_PIX_FMT_GBRP9LE,
        /// planar GBR 4:4:4 30bpp, big-endian
        Gbrp10Be = AVPixelFormat.AV_PIX_FMT_GBRP10BE,
        /// planar GBR 4:4:4 30bpp, little-endian
        Gbrp10Le = AVPixelFormat.AV_PIX_FMT_GBRP10LE,
        /// planar GBR 4:4:4 48bpp, big-endian
        Gbrp16Be = AVPixelFormat.AV_PIX_FMT_GBRP16BE,
        /// planar GBR 4:4:4 48bpp, little-endian
        Gbrp16Le = AVPixelFormat.AV_PIX_FMT_GBRP16LE,
        /// planar YUV 4:2:2 24bpp, (1 Cr & Cb sample per 2x1 Y & A samples)
        Yuva422P = AVPixelFormat.AV_PIX_FMT_YUVA422P,
        /// planar YUV 4:4:4 32bpp, (1 Cr & Cb sample per 1x1 Y & A samples)
        Yuva444P = AVPixelFormat.AV_PIX_FMT_YUVA444P,
        /// planar YUV 4:2:0 22.5bpp, (1 Cr & Cb sample per 2x2 Y & A samples), big-endian
        Yuva420P9Be = AVPixelFormat.AV_PIX_FMT_YUVA420P9BE,
        /// planar YUV 4:2:0 22.5bpp, (1 Cr & Cb sample per 2x2 Y & A samples), little-endian
        Yuva420P9Le = AVPixelFormat.AV_PIX_FMT_YUVA420P9LE,
        /// planar YUV 4:2:2 27bpp, (1 Cr & Cb sample per 2x1 Y & A samples), big-endian
        Yuva422P9Be = AVPixelFormat.AV_PIX_FMT_YUVA422P9BE,
        /// planar YUV 4:2:2 27bpp, (1 Cr & Cb sample per 2x1 Y & A samples), little-endian
        Yuva422P9Le = AVPixelFormat.AV_PIX_FMT_YUVA422P9LE,
        /// planar YUV 4:4:4 36bpp, (1 Cr & Cb sample per 1x1 Y & A samples), big-endian
        Yuva444P9Be = AVPixelFormat.AV_PIX_FMT_YUVA444P9BE,
        /// planar YUV 4:4:4 36bpp, (1 Cr & Cb sample per 1x1 Y & A samples), little-endian
        Yuva444P9Le = AVPixelFormat.AV_PIX_FMT_YUVA444P9LE,
        /// planar YUV 4:2:0 25bpp, (1 Cr & Cb sample per 2x2 Y & A samples, big-endian)
        Yuva420P10Be = AVPixelFormat.AV_PIX_FMT_YUVA420P10BE,
        /// planar YUV 4:2:0 25bpp, (1 Cr & Cb sample per 2x2 Y & A samples, little-endian)
        Yuva420P10Le = AVPixelFormat.AV_PIX_FMT_YUVA420P10LE,
        /// planar YUV 4:2:2 30bpp, (1 Cr & Cb sample per 2x1 Y & A samples, big-endian)
        Yuva422P10Be = AVPixelFormat.AV_PIX_FMT_YUVA422P10BE,
        /// planar YUV 4:2:2 30bpp, (1 Cr & Cb sample per 2x1 Y & A samples, little-endian)
        Yuva422P10Le = AVPixelFormat.AV_PIX_FMT_YUVA422P10LE,
        /// planar YUV 4:4:4 40bpp, (1 Cr & Cb sample per 1x1 Y & A samples, big-endian)
        Yuva444P10Be = AVPixelFormat.AV_PIX_FMT_YUVA444P10BE,
        /// planar YUV 4:4:4 40bpp, (1 Cr & Cb sample per 1x1 Y & A samples, little-endian)
        Yuva444P10Le = AVPixelFormat.AV_PIX_FMT_YUVA444P10LE,
        /// planar YUV 4:2:0 40bpp, (1 Cr & Cb sample per 2x2 Y & A samples, big-endian)
        Yuva420P16Be = AVPixelFormat.AV_PIX_FMT_YUVA420P16BE,
        /// planar YUV 4:2:0 40bpp, (1 Cr & Cb sample per 2x2 Y & A samples, little-endian)
        Yuva420P16Le = AVPixelFormat.AV_PIX_FMT_YUVA420P16LE,
        /// planar YUV 4:2:2 48bpp, (1 Cr & Cb sample per 2x1 Y & A samples, big-endian)
        Yuva422P16Be = AVPixelFormat.AV_PIX_FMT_YUVA422P16BE,
        /// planar YUV 4:2:2 48bpp, (1 Cr & Cb sample per 2x1 Y & A samples, little-endian)
        Yuva422P16Le = AVPixelFormat.AV_PIX_FMT_YUVA422P16LE,
        /// planar YUV 4:4:4 64bpp, (1 Cr & Cb sample per 1x1 Y & A samples, big-endian)
        Yuva444P16Be = AVPixelFormat.AV_PIX_FMT_YUVA444P16BE,
        /// planar YUV 4:4:4 64bpp, (1 Cr & Cb sample per 1x1 Y & A samples, little-endian)
        Yuva444P16Le = AVPixelFormat.AV_PIX_FMT_YUVA444P16LE,
        /// HW acceleration through VDPAU, Picture.data[3] contains a VdpVideoSurface
        Vdpau = AVPixelFormat.AV_PIX_FMT_VDPAU,
        /// packed XYZ 4:4:4, 36 bpp, (msb) 12X, 12Y, 12Z (lsb), the 2-byte value for each
        /// X/Y/Z is stored as little-endian, the 4 lower bits are set to 0
        Xyz12Le = AVPixelFormat.AV_PIX_FMT_XYZ12LE,
        /// packed XYZ 4:4:4, 36 bpp, (msb) 12X, 12Y, 12Z (lsb), the 2-byte value for each
        /// X/Y/Z is stored as big-endian, the 4 lower bits are set to 0
        Xyz12Be = AVPixelFormat.AV_PIX_FMT_XYZ12BE,
        /// interleaved chroma YUV 4:2:2, 16bpp, (1 Cr & Cb sample per 2x1 Y samples)
        Nv16 = AVPixelFormat.AV_PIX_FMT_NV16,
        /// interleaved chroma YUV 4:2:2, 20bpp, (1 Cr & Cb sample per 2x1 Y samples), little-endian
        Nv20Le = AVPixelFormat.AV_PIX_FMT_NV20LE,
        /// interleaved chroma YUV 4:2:2, 20bpp, (1 Cr & Cb sample per 2x1 Y samples), big-endian
        Nv20Be = AVPixelFormat.AV_PIX_FMT_NV20BE,
        /// packed RGBA 16:16:16:16, 64bpp, 16R, 16G, 16B, 16A, the 2-byte value for each
        /// R/G/B/A component is stored as big-endian
        Rgba64Be = AVPixelFormat.AV_PIX_FMT_RGBA64BE,
        /// packed RGBA 16:16:16:16, 64bpp, 16R, 16G, 16B, 16A, the 2-byte value for each
        /// R/G/B/A component is stored as little-endian
        Rgba64Le = AVPixelFormat.AV_PIX_FMT_RGBA64LE,
        /// packed RGBA 16:16:16:16, 64bpp, 16B, 16G, 16R, 16A, the 2-byte value for each
        /// R/G/B/A component is stored as big-endian
        Bgra64Be = AVPixelFormat.AV_PIX_FMT_BGRA64BE,
        /// packed RGBA 16:16:16:16, 64bpp, 16B, 16G, 16R, 16A, the 2-byte value for each
        /// R/G/B/A component is stored as little-endian
        Bgra64Le = AVPixelFormat.AV_PIX_FMT_BGRA64LE,
        /// packed YUV 4:2:2, 16bpp, Y0 Cr Y1 Cb
        Yvyu422 = AVPixelFormat.AV_PIX_FMT_YVYU422,
        /// 16 bits gray, 16 bits alpha (big-endian)
        Ya16Be = AVPixelFormat.AV_PIX_FMT_YA16BE,
        /// 16 bits gray, 16 bits alpha (little-endian)
        Ya16Le = AVPixelFormat.AV_PIX_FMT_YA16LE,
        /// planar GBRA 4:4:4:4 32bpp
        Gbrap = AVPixelFormat.AV_PIX_FMT_GBRAP,
        /// planar GBRA 4:4:4:4 64bpp, big-endian
        Gbrap16Be = AVPixelFormat.AV_PIX_FMT_GBRAP16BE,
        /// planar GBRA 4:4:4:4 64bpp, little-endian
        Gbrap16Le = AVPixelFormat.AV_PIX_FMT_GBRAP16LE,
        /// HW acceleration through QSV, data[3] contains a pointer to the mfxFrameSurface1
        /// structure.
        Qsv = AVPixelFormat.AV_PIX_FMT_QSV,
        /// HW acceleration though MMAL, data[3] contains a pointer to the MMAL_BUFFER_HEADER_T
        /// structure.
        Mmal = AVPixelFormat.AV_PIX_FMT_MMAL,
        /// HW decoding through Direct3D11 via old API, Picture.data[3] contains a ID3D11VideoDecoderOutputView
        /// pointer
        D3D11VaVld = AVPixelFormat.AV_PIX_FMT_D3D11VA_VLD,
        /// HW acceleration through CUDA. data[i] contain CUdeviceptr pointers exactly as
        /// for system memory frames.
        Cuda = AVPixelFormat.AV_PIX_FMT_CUDA,
        /// packed RGB 8:8:8, 32bpp, XRGBXRGB... X=unused/undefined
        ZeroRgb = AVPixelFormat.AV_PIX_FMT_0RGB,
        /// packed RGB 8:8:8, 32bpp, RGBXRGBX... X=unused/undefined
        Rgbx = AVPixelFormat.AV_PIX_FMT_RGB0,
        /// packed BGR 8:8:8, 32bpp, XBGRXBGR... X=unused/undefined
        Xbgr = AVPixelFormat.AV_PIX_FMT_0BGR,
        /// packed BGR 8:8:8, 32bpp, BGRXBGRX... X=unused/undefined
        Bgrx = AVPixelFormat.AV_PIX_FMT_BGR0,
        /// planar YUV 4:2:0,18bpp, (1 Cr & Cb sample per 2x2 Y samples), big-endian
        Yuv420P12Be = AVPixelFormat.AV_PIX_FMT_YUV420P12BE,
        /// planar YUV 4:2:0,18bpp, (1 Cr & Cb sample per 2x2 Y samples), little-endian
        Yuv420P12Le = AVPixelFormat.AV_PIX_FMT_YUV420P12LE,
        /// planar YUV 4:2:0,21bpp, (1 Cr & Cb sample per 2x2 Y samples), big-endian
        Yuv420P14Be = AVPixelFormat.AV_PIX_FMT_YUV420P14BE,
        /// planar YUV 4:2:0,21bpp, (1 Cr & Cb sample per 2x2 Y samples), little-endian
        Yuv420P14Le = AVPixelFormat.AV_PIX_FMT_YUV420P14LE,
        /// planar YUV 4:2:2,24bpp, (1 Cr & Cb sample per 2x1 Y samples), big-endian
        Yuv422P12Be = AVPixelFormat.AV_PIX_FMT_YUV422P12BE,
        /// planar YUV 4:2:2,24bpp, (1 Cr & Cb sample per 2x1 Y samples), little-endian
        Yuv422P12Le = AVPixelFormat.AV_PIX_FMT_YUV422P12LE,
        /// planar YUV 4:2:2,28bpp, (1 Cr & Cb sample per 2x1 Y samples), big-endian
        Yuv422P14Be = AVPixelFormat.AV_PIX_FMT_YUV422P14BE,
        /// planar YUV 4:2:2,28bpp, (1 Cr & Cb sample per 2x1 Y samples), little-endian
        Yuv422P14Le = AVPixelFormat.AV_PIX_FMT_YUV422P14LE,
        /// planar YUV 4:4:4,36bpp, (1 Cr & Cb sample per 1x1 Y samples), big-endian
        Yuv444P12Be = AVPixelFormat.AV_PIX_FMT_YUV444P12BE,
        /// planar YUV 4:4:4,36bpp, (1 Cr & Cb sample per 1x1 Y samples), little-endian
        Yuv444P12Le = AVPixelFormat.AV_PIX_FMT_YUV444P12LE,
        /// planar YUV 4:4:4,42bpp, (1 Cr & Cb sample per 1x1 Y samples), big-endian
        Yuv444P14Be = AVPixelFormat.AV_PIX_FMT_YUV444P14BE,
        /// planar YUV 4:4:4,42bpp, (1 Cr & Cb sample per 1x1 Y samples), little-endian
        Yuv444P14Le = AVPixelFormat.AV_PIX_FMT_YUV444P14LE,
        /// planar GBR 4:4:4 36bpp, big-endian
        Gbrp12Be = AVPixelFormat.AV_PIX_FMT_GBRP12BE,
        /// planar GBR 4:4:4 36bpp, little-endian
        Gbrp12Le = AVPixelFormat.AV_PIX_FMT_GBRP12LE,
        /// planar GBR 4:4:4 42bpp, big-endian
        Gbrp14Be = AVPixelFormat.AV_PIX_FMT_GBRP14BE,
        /// planar GBR 4:4:4 42bpp, little-endian
        Gbrp14Le = AVPixelFormat.AV_PIX_FMT_GBRP14LE,
        /// planar YUV 4:1:1, 12bpp, (1 Cr & Cb sample per 4x1 Y samples) full scale (JPEG),
        /// deprecated in favor of AV_PIX_FMT_YUV411P and setting color_range
        Yuvj411P = AVPixelFormat.AV_PIX_FMT_YUVJ411P,
        /// bayer, BGBG..(odd line), GRGR..(even line), 8-bit samples
        BayerBggr8 = AVPixelFormat.AV_PIX_FMT_BAYER_BGGR8,
        /// bayer, RGRG..(odd line), GBGB..(even line), 8-bit samples
        BayerRggb8 = AVPixelFormat.AV_PIX_FMT_BAYER_RGGB8,
        /// bayer, GBGB..(odd line), RGRG..(even line), 8-bit samples
        BayerGbrg8 = AVPixelFormat.AV_PIX_FMT_BAYER_GBRG8,
        /// bayer, GRGR..(odd line), BGBG..(even line), 8-bit samples
        BayerGrbg8 = AVPixelFormat.AV_PIX_FMT_BAYER_GRBG8,
        /// bayer, BGBG..(odd line), GRGR..(even line), 16-bit samples, little-endian
        BayerBggr16Le = AVPixelFormat.AV_PIX_FMT_BAYER_BGGR16LE,
        /// bayer, BGBG..(odd line), GRGR..(even line), 16-bit samples, big-endian
        BayerBggr16Be = AVPixelFormat.AV_PIX_FMT_BAYER_BGGR16BE,
        /// bayer, RGRG..(odd line), GBGB..(even line), 16-bit samples, little-endian
        BayerRggb16Le = AVPixelFormat.AV_PIX_FMT_BAYER_RGGB16LE,
        /// bayer, RGRG..(odd line), GBGB..(even line), 16-bit samples, big-endian
        BayerRggb16Be = AVPixelFormat.AV_PIX_FMT_BAYER_RGGB16BE,
        /// bayer, GBGB..(odd line), RGRG..(even line), 16-bit samples, little-endian
        BayerGbrg16Le = AVPixelFormat.AV_PIX_FMT_BAYER_GBRG16LE,
        /// bayer, GBGB..(odd line), RGRG..(even line), 16-bit samples, big-endian
        BayerGbrg16Be = AVPixelFormat.AV_PIX_FMT_BAYER_GBRG16BE,
        /// bayer, GRGR..(odd line), BGBG..(even line), 16-bit samples, little-endian
        BayerGrbg16Le = AVPixelFormat.AV_PIX_FMT_BAYER_GRBG16LE,
        /// bayer, GRGR..(odd line), BGBG..(even line), 16-bit samples, big-endian
        BayerGrbg16Be = AVPixelFormat.AV_PIX_FMT_BAYER_GRBG16BE,
        /// XVideo Motion Acceleration via common packet passing
        Xvmc = AVPixelFormat.AV_PIX_FMT_XVMC,
        /// planar YUV 4:4:0,20bpp, (1 Cr & Cb sample per 1x2 Y samples), little-endian
        Yuv440P10Le = AVPixelFormat.AV_PIX_FMT_YUV440P10LE,
        /// planar YUV 4:4:0,20bpp, (1 Cr & Cb sample per 1x2 Y samples), big-endian
        Yuv440P10Be = AVPixelFormat.AV_PIX_FMT_YUV440P10BE,
        /// planar YUV 4:4:0,24bpp, (1 Cr & Cb sample per 1x2 Y samples), little-endian
        Yuv440P12Le = AVPixelFormat.AV_PIX_FMT_YUV440P12LE,
        /// planar YUV 4:4:0,24bpp, (1 Cr & Cb sample per 1x2 Y samples), big-endian
        Yuv440P12Be = AVPixelFormat.AV_PIX_FMT_YUV440P12BE,
        /// packed AYUV 4:4:4,64bpp (1 Cr & Cb sample per 1x1 Y & A samples), little-endian
        Ayuv64Le = AVPixelFormat.AV_PIX_FMT_AYUV64LE,
        /// packed AYUV 4:4:4,64bpp (1 Cr & Cb sample per 1x1 Y & A samples), big-endian
        Ayuv64Be = AVPixelFormat.AV_PIX_FMT_AYUV64BE,
        /// hardware decoding through Videotoolbox
        Videotoolbox = AVPixelFormat.AV_PIX_FMT_VIDEOTOOLBOX,
        /// like NV12, with 10bpp per component, data in the high bits, zeros in the low
        /// bits, little-endian
        P010Le = AVPixelFormat.AV_PIX_FMT_P010LE,
        /// like NV12, with 10bpp per component, data in the high bits, zeros in the low
        /// bits, big-endian
        P010Be = AVPixelFormat.AV_PIX_FMT_P010BE,
        /// planar GBR 4:4:4:4 48bpp, big-endian
        Gbrap12Be = AVPixelFormat.AV_PIX_FMT_GBRAP12BE,
        /// planar GBR 4:4:4:4 48bpp, little-endian
        Gbrap12Le = AVPixelFormat.AV_PIX_FMT_GBRAP12LE,
        /// planar GBR 4:4:4:4 40bpp, big-endian
        Gbrap10Be = AVPixelFormat.AV_PIX_FMT_GBRAP10BE,
        /// planar GBR 4:4:4:4 40bpp, little-endian
        Gbrap10Le = AVPixelFormat.AV_PIX_FMT_GBRAP10LE,
        /// hardware decoding through MediaCodec
        Mediacodec = AVPixelFormat.AV_PIX_FMT_MEDIACODEC,
        /// Y , 12bpp, big-endian
        Gray12Be = AVPixelFormat.AV_PIX_FMT_GRAY12BE,
        /// Y , 12bpp, little-endian
        Gray12Le = AVPixelFormat.AV_PIX_FMT_GRAY12LE,
        /// Y , 10bpp, big-endian
        Gray10Be = AVPixelFormat.AV_PIX_FMT_GRAY10BE,
        /// Y , 10bpp, little-endian
        Gray10Le = AVPixelFormat.AV_PIX_FMT_GRAY10LE,
        /// like NV12, with 16bpp per component, little-endian
        P016Le = AVPixelFormat.AV_PIX_FMT_P016LE,
        /// like NV12, with 16bpp per component, big-endian
        P016Be = AVPixelFormat.AV_PIX_FMT_P016BE,
        /// Hardware surfaces for Direct3D11.
        D3D11 = AVPixelFormat.AV_PIX_FMT_D3D11,
        /// Y , 9bpp, big-endian
        Gray9Be = AVPixelFormat.AV_PIX_FMT_GRAY9BE,
        /// Y , 9bpp, little-endian
        Gray9Le = AVPixelFormat.AV_PIX_FMT_GRAY9LE,
        /// IEEE-754 single precision planar GBR 4:4:4, 96bpp, big-endian
        Gbrpf32Be = AVPixelFormat.AV_PIX_FMT_GBRPF32BE,
        /// IEEE-754 single precision planar GBR 4:4:4, 96bpp, little-endian
        Gbrpf32Le = AVPixelFormat.AV_PIX_FMT_GBRPF32LE,
        /// IEEE-754 single precision planar GBRA 4:4:4:4, 128bpp, big-endian
        Gbrapf32Be = AVPixelFormat.AV_PIX_FMT_GBRAPF32BE,
        /// IEEE-754 single precision planar GBRA 4:4:4:4, 128bpp, little-endian
        Gbrapf32Le = AVPixelFormat.AV_PIX_FMT_GBRAPF32LE,
        /// DRM-managed buffers exposed through PRIME buffer sharing.
        DrmPrime = AVPixelFormat.AV_PIX_FMT_DRM_PRIME,
        /// Hardware surfaces for OpenCL.
        Opencl = AVPixelFormat.AV_PIX_FMT_OPENCL,
        /// Y , 14bpp, big-endian
        Gray14Be = AVPixelFormat.AV_PIX_FMT_GRAY14BE,
        /// Y , 14bpp, little-endian
        Gray14Le = AVPixelFormat.AV_PIX_FMT_GRAY14LE,
        /// IEEE-754 single precision Y, 32bpp, big-endian
        Grayf32Be = AVPixelFormat.AV_PIX_FMT_GRAYF32BE,
        /// IEEE-754 single precision Y, 32bpp, little-endian
        Grayf32Le = AVPixelFormat.AV_PIX_FMT_GRAYF32LE,
        /// planar YUV 4:2:2,24bpp, (1 Cr & Cb sample per 2x1 Y samples), 12b alpha, big-endian
        Yuva422P12Be = AVPixelFormat.AV_PIX_FMT_YUVA422P12BE,
        /// planar YUV 4:2:2,24bpp, (1 Cr & Cb sample per 2x1 Y samples), 12b alpha, little-endian
        Yuva422P12Le = AVPixelFormat.AV_PIX_FMT_YUVA422P12LE,
        /// planar YUV 4:4:4,36bpp, (1 Cr & Cb sample per 1x1 Y samples), 12b alpha, big-endian
        Yuva444P12Be = AVPixelFormat.AV_PIX_FMT_YUVA444P12BE,
        /// planar YUV 4:4:4,36bpp, (1 Cr & Cb sample per 1x1 Y samples), 12b alpha, little-endian
        Yuva444P12Le = AVPixelFormat.AV_PIX_FMT_YUVA444P12LE,
        /// planar YUV 4:4:4, 24bpp, 1 plane for Y and 1 plane for the UV components, which
        /// are interleaved (first byte U and the following byte V)
        Nv24 = AVPixelFormat.AV_PIX_FMT_NV24,
        /// as above, but U and V bytes are swapped
        Nv42 = AVPixelFormat.AV_PIX_FMT_NV42,
        /// Vulkan hardware images.
        Vulkan = AVPixelFormat.AV_PIX_FMT_VULKAN,
        /// packed YUV 4:2:2 like YUYV422, 20bpp, data in the high bits, big-endian
        Y210Be = AVPixelFormat.AV_PIX_FMT_Y210BE,
        /// packed YUV 4:2:2 like YUYV422, 20bpp, data in the high bits, little-endian
        Y210Le = AVPixelFormat.AV_PIX_FMT_Y210LE,
        /// packed RGB 10:10:10, 30bpp, (msb)2X 10R 10G 10B(lsb), little-endian, X=unused/undefined
        X2Rgb10Le = AVPixelFormat.AV_PIX_FMT_X2RGB10LE,
        /// packed RGB 10:10:10, 30bpp, (msb)2X 10R 10G 10B(lsb), big-endian, X=unused/undefined
        X2Rgb10Be = AVPixelFormat.AV_PIX_FMT_X2RGB10BE,
        /// number of pixel formats, DO NOT USE THIS if you want to link with shared libav*
        /// because the number of formats might differ between versions
        AvPixFmtNb = AVPixelFormat.AV_PIX_FMT_NB
    }
}