// Copyright (c) The Vignette Authors
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 3-Clause License. See LICENSE for details.

using FFmpeg.AutoGen;

namespace SeeShark
{
    public enum HardwareAccelDevice : int
    {
        None = AVHWDeviceType.AV_HWDEVICE_TYPE_NONE,
        Vdpau = AVHWDeviceType.AV_HWDEVICE_TYPE_VDPAU,
        Cuda = AVHWDeviceType.AV_HWDEVICE_TYPE_CUDA,
        Vaapi = AVHWDeviceType.AV_HWDEVICE_TYPE_VAAPI,
        Dxva2 = AVHWDeviceType.AV_HWDEVICE_TYPE_DXVA2,
        Qsv = AVHWDeviceType.AV_HWDEVICE_TYPE_QSV,
        Videotoolbox = AVHWDeviceType.AV_HWDEVICE_TYPE_VIDEOTOOLBOX,
        D3D11Va = AVHWDeviceType.AV_HWDEVICE_TYPE_D3D11VA,
        Drm = AVHWDeviceType.AV_HWDEVICE_TYPE_DRM,
        Opencl = AVHWDeviceType.AV_HWDEVICE_TYPE_OPENCL,
        Mediacodec = AVHWDeviceType.AV_HWDEVICE_TYPE_MEDIACODEC,
        Vulkan = AVHWDeviceType.AV_HWDEVICE_TYPE_VULKAN
    }

    public static class HardwareAccelDeviceExtension
    {
        public static PixelFormat ToPixelFormat(this HardwareAccelDevice hwAccelDevice)
        {
            return hwAccelDevice switch
            {
                HardwareAccelDevice.Vdpau => PixelFormat.Vdpau,
                HardwareAccelDevice.Cuda => PixelFormat.Cuda,
                HardwareAccelDevice.Vaapi => PixelFormat.Vaapi,
                HardwareAccelDevice.Dxva2 => PixelFormat.Dxva2Vld,
                HardwareAccelDevice.Qsv => PixelFormat.Qsv,
                HardwareAccelDevice.Videotoolbox => PixelFormat.Videotoolbox,
                HardwareAccelDevice.D3D11Va => PixelFormat.D3D11VaVld,
                HardwareAccelDevice.Drm => PixelFormat.DrmPrime,
                HardwareAccelDevice.Opencl => PixelFormat.Opencl,
                HardwareAccelDevice.Mediacodec => PixelFormat.Mediacodec,
                HardwareAccelDevice.Vulkan => PixelFormat.Vulkan,
                _ => PixelFormat.None
            };
        }
    }
}
