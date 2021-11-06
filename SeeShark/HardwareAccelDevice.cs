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
}