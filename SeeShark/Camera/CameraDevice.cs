// Copyright (c) Speykious
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 2-Clause License. See LICENSE for details.

using System;
using System.Collections.Generic;
using System.Threading;
using SeeShark.Linux;
using SeeShark.MacOS;

namespace SeeShark.Camera;

public abstract class CameraDevice : IDisposable
{
    public CameraPath Path { get; init; }
    public VideoFormat CurrentFormat { get; init; }

    public string? Name => Path.Name;

    ~CameraDevice() => DisposeUnmanagedResources();

    public void Dispose()
    {
        DisposeUnmanagedResources();
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Free any unmanaged resources the camera might be holding, to satisfy the <see cref="IDisposable"/> interface.
    /// </summary>
    protected virtual void DisposeUnmanagedResources() { }

    public static CameraDevice Open(CameraPath cameraInfo) => Open(cameraInfo, new VideoFormatOptions());

    public static CameraDevice Open(CameraPath cameraInfo, VideoFormatOptions options)
    {
        if (OperatingSystem.IsLinux())
            return V4l2.OpenCamera(cameraInfo, options);
        else if (OperatingSystem.IsMacOS())
            return AVFoundation.OpenCamera(cameraInfo, options);
        else
            throw new NotImplementedException();
    }

    public static List<CameraPath> Available()
    {
        if (OperatingSystem.IsLinux())
            return V4l2.AvailableCameras();
        else if (OperatingSystem.IsMacOS())
            return AVFoundation.AvailableCameras();
        else
            throw new NotImplementedException();
    }

    public static List<VideoFormat> AvailableFormats(CameraPath device)
    {
        if (OperatingSystem.IsLinux())
            return V4l2.AvailableFormats(device);
        else if (OperatingSystem.IsMacOS())
            return AVFoundation.AvailableFormats(device);
        else
            throw new NotImplementedException();
    }

    public List<VideoFormat> AvailableFormats()
    {
        return AvailableFormats(Path);
    }

    public abstract void StartCapture();

    public abstract void StopCapture();

    public void ReadFrame(ref Frame frame)
    {
        while (true)
        {
            if (TryReadFrame(ref frame))
            {
                Thread.Sleep(1);
                break;
            }
        }
    }

    public abstract bool TryReadFrame(ref Frame frame);
}
