/**
 * Enumeration cases in ObjC cannot be obtained through the ObjC runtime,
 * so I log their values instead and redefine my own enum elsewhere.
 *
 * Compile and run:
 * clang -framework Foundation -framework CoreVideo get_kCVPixelFormatType_values.m -o get_kCVPixelFormatType_values && ./get_kCVPixelFormatType_values
 **/

#import <CoreVideo/CoreVideo.h>
#import <Foundation/Foundation.h>

// Gets the FourCC code as a C string. The `s` buffer has to be at least 5 chars long.
char *getFourCC(uint32_t code, char *s) {
    s[0] = (code >> 24) & 0xFF;
    s[1] = (code >> 16) & 0xFF;
    s[2] = (code >> 8) & 0xFF;
    s[3] = code & 0xFF;
    s[4] = '\0';
    return s;
}

// Quick macro to log a constant and its internal value.
#define LOG_CONST(buf, c) NSLog(@"%s = 0x%08X, // '%s'", #c, c, getFourCC(c, buf))

int main(int argc, const char *argv[]) {
    char buf4cc[5];

    @autoreleasepool {
        LOG_CONST(buf4cc, kCVPixelFormatType_1Monochrome);
        LOG_CONST(buf4cc, kCVPixelFormatType_2Indexed);
        LOG_CONST(buf4cc, kCVPixelFormatType_4Indexed);
        LOG_CONST(buf4cc, kCVPixelFormatType_8Indexed);
        LOG_CONST(buf4cc, kCVPixelFormatType_1IndexedGray_WhiteIsZero);
        LOG_CONST(buf4cc, kCVPixelFormatType_2IndexedGray_WhiteIsZero);
        LOG_CONST(buf4cc, kCVPixelFormatType_4IndexedGray_WhiteIsZero);
        LOG_CONST(buf4cc, kCVPixelFormatType_8IndexedGray_WhiteIsZero);
        LOG_CONST(buf4cc, kCVPixelFormatType_16BE555);
        LOG_CONST(buf4cc, kCVPixelFormatType_16LE555);
        LOG_CONST(buf4cc, kCVPixelFormatType_16LE5551);
        LOG_CONST(buf4cc, kCVPixelFormatType_16BE565);
        LOG_CONST(buf4cc, kCVPixelFormatType_16LE565);
        LOG_CONST(buf4cc, kCVPixelFormatType_24RGB);
        LOG_CONST(buf4cc, kCVPixelFormatType_24BGR);
        LOG_CONST(buf4cc, kCVPixelFormatType_32ARGB);
        LOG_CONST(buf4cc, kCVPixelFormatType_32BGRA);
        LOG_CONST(buf4cc, kCVPixelFormatType_32ABGR);
        LOG_CONST(buf4cc, kCVPixelFormatType_32RGBA);
        LOG_CONST(buf4cc, kCVPixelFormatType_64ARGB);
        LOG_CONST(buf4cc, kCVPixelFormatType_48RGB);
        LOG_CONST(buf4cc, kCVPixelFormatType_32AlphaGray);
        LOG_CONST(buf4cc, kCVPixelFormatType_16Gray);
        LOG_CONST(buf4cc, kCVPixelFormatType_30RGB);
        LOG_CONST(buf4cc, kCVPixelFormatType_422YpCbCr8);
        LOG_CONST(buf4cc, kCVPixelFormatType_4444YpCbCrA8);
        LOG_CONST(buf4cc, kCVPixelFormatType_4444YpCbCrA8R);
        LOG_CONST(buf4cc, kCVPixelFormatType_4444AYpCbCr8);
        LOG_CONST(buf4cc, kCVPixelFormatType_4444AYpCbCr16);
        LOG_CONST(buf4cc, kCVPixelFormatType_444YpCbCr8);
        LOG_CONST(buf4cc, kCVPixelFormatType_422YpCbCr16);
        LOG_CONST(buf4cc, kCVPixelFormatType_422YpCbCr10);
        LOG_CONST(buf4cc, kCVPixelFormatType_444YpCbCr10);
        LOG_CONST(buf4cc, kCVPixelFormatType_420YpCbCr8Planar);
        LOG_CONST(buf4cc, kCVPixelFormatType_420YpCbCr8PlanarFullRange);
        LOG_CONST(buf4cc, kCVPixelFormatType_422YpCbCr_4A_8BiPlanar);
        LOG_CONST(buf4cc, kCVPixelFormatType_420YpCbCr8BiPlanarVideoRange);
        LOG_CONST(buf4cc, kCVPixelFormatType_420YpCbCr8BiPlanarFullRange);
        LOG_CONST(buf4cc, kCVPixelFormatType_422YpCbCr8_yuvs);
        LOG_CONST(buf4cc, kCVPixelFormatType_422YpCbCr8FullRange);
        LOG_CONST(buf4cc, kCVPixelFormatType_OneComponent8);
        LOG_CONST(buf4cc, kCVPixelFormatType_TwoComponent8);
        LOG_CONST(buf4cc, kCVPixelFormatType_OneComponent16Half);
        LOG_CONST(buf4cc, kCVPixelFormatType_OneComponent32Float);
        LOG_CONST(buf4cc, kCVPixelFormatType_TwoComponent16Half);
        LOG_CONST(buf4cc, kCVPixelFormatType_TwoComponent32Float);
        LOG_CONST(buf4cc, kCVPixelFormatType_64RGBAHalf);
        LOG_CONST(buf4cc, kCVPixelFormatType_128RGBAFloat);
        LOG_CONST(buf4cc, kCVPixelFormatType_14Bayer_BGGR);
        LOG_CONST(buf4cc, kCVPixelFormatType_14Bayer_GBRG);
        LOG_CONST(buf4cc, kCVPixelFormatType_14Bayer_GRBG);
        LOG_CONST(buf4cc, kCVPixelFormatType_14Bayer_RGGB);
        LOG_CONST(buf4cc, kCVPixelFormatType_30RGBLEPackedWideGamut);
        LOG_CONST(buf4cc, kCVPixelFormatType_ARGB2101010LEPacked);
        LOG_CONST(buf4cc, kCVPixelFormatType_420YpCbCr10BiPlanarFullRange);
        LOG_CONST(buf4cc, kCVPixelFormatType_420YpCbCr10BiPlanarVideoRange);
        LOG_CONST(buf4cc, kCVPixelFormatType_422YpCbCr10BiPlanarFullRange);
        LOG_CONST(buf4cc, kCVPixelFormatType_422YpCbCr10BiPlanarVideoRange);
        LOG_CONST(buf4cc, kCVPixelFormatType_444YpCbCr10BiPlanarFullRange);
        LOG_CONST(buf4cc, kCVPixelFormatType_444YpCbCr10BiPlanarVideoRange);
        LOG_CONST(buf4cc, kCVPixelFormatType_DepthFloat16);
        LOG_CONST(buf4cc, kCVPixelFormatType_DepthFloat32);
        LOG_CONST(buf4cc, kCVPixelFormatType_DisparityFloat16);
        LOG_CONST(buf4cc, kCVPixelFormatType_DisparityFloat32);
        LOG_CONST(buf4cc, kCVPixelFormatType_16VersatileBayer);
        LOG_CONST(buf4cc, kCVPixelFormatType_40ARGBLEWideGamut);
        LOG_CONST(buf4cc, kCVPixelFormatType_40ARGBLEWideGamutPremultiplied);
        LOG_CONST(buf4cc, kCVPixelFormatType_420YpCbCr8VideoRange_8A_TriPlanar);
        LOG_CONST(buf4cc, kCVPixelFormatType_422YpCbCr16BiPlanarVideoRange);
        LOG_CONST(buf4cc, kCVPixelFormatType_422YpCbCr8BiPlanarFullRange);
        LOG_CONST(buf4cc, kCVPixelFormatType_422YpCbCr8BiPlanarVideoRange);
        LOG_CONST(buf4cc, kCVPixelFormatType_4444AYpCbCrFloat);
        LOG_CONST(buf4cc, kCVPixelFormatType_444YpCbCr16BiPlanarVideoRange);
        LOG_CONST(buf4cc, kCVPixelFormatType_444YpCbCr16VideoRange_16A_TriPlanar);
        LOG_CONST(buf4cc, kCVPixelFormatType_444YpCbCr8BiPlanarFullRange);
        LOG_CONST(buf4cc, kCVPixelFormatType_444YpCbCr8BiPlanarVideoRange);
        LOG_CONST(buf4cc, kCVPixelFormatType_64RGBALE);
        LOG_CONST(buf4cc, kCVPixelFormatType_64RGBA_DownscaledProResRAW);
        LOG_CONST(buf4cc, kCVPixelFormatType_OneComponent10);
        LOG_CONST(buf4cc, kCVPixelFormatType_OneComponent12);
        LOG_CONST(buf4cc, kCVPixelFormatType_OneComponent16);
        LOG_CONST(buf4cc, kCVPixelFormatType_TwoComponent16);
    }

    return 0;
}
