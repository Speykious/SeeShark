// Copyright (c) Speykious
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 2-Clause License. See LICENSE for details.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using DirectShowLib;
using SeeShark.Windows;

namespace SeeShark.Camera;

[SupportedOSPlatform("Windows")]
public sealed class WindowsCameraDevice : CameraDevice, ISampleGrabberCB
{
    private DsDevice dsDevice { get; set; }

    private IMediaControl mediaControl;
    private ISampleGrabber sampleGrabber;
    private AMMediaType connectedMediaType = new AMMediaType();

    private FrameQueue frameDataQueue;

    internal WindowsCameraDevice(CameraPath cameraInfo, VideoFormatOptions options)
    {
        dsDevice = DShow.FindCameraDevice(cameraInfo) ?? throw new CameraDeviceIOException(cameraInfo, "Cannot open camera");
        Path = cameraInfo;

        IFilterGraph2 filterGraph = (IFilterGraph2)new FilterGraph();
        Marshal.ThrowExceptionForHR(filterGraph.AddSourceFilterForMoniker(dsDevice.Mon, null, dsDevice.Name, out IBaseFilter captureFilter));

        IBaseFilter nullRenderer = (IBaseFilter)new NullRenderer();
        Marshal.ThrowExceptionForHR(filterGraph.AddFilter(nullRenderer, "Null Renderer"));

        sampleGrabber = (ISampleGrabber)new SampleGrabber();
        Marshal.ThrowExceptionForHR(filterGraph.AddFilter((IBaseFilter)sampleGrabber, "Sample Grabber"));

        Console.Error.WriteLine("[WARNING] Video format options are currently ignored");
        AMMediaType media = new AMMediaType
        {
            majorType = MediaType.Video,
            subType = MediaSubType.ARGB32,
            formatPtr = 0,
        };
        CurrentFormat = new VideoFormat
        {
            ImageFormat = ImageFormat.Argb,
            VideoSize = (1920, 1080),
        };

        Marshal.ThrowExceptionForHR(sampleGrabber.SetMediaType(media));
        Marshal.ThrowExceptionForHR(sampleGrabber.SetBufferSamples(false));
        Marshal.ThrowExceptionForHR(sampleGrabber.SetOneShot(false));
        Marshal.ThrowExceptionForHR(sampleGrabber.SetCallback(this, (int)SampleGrabberCallbackMethod.BufferCB));

        // connect inputs and outputs in the graph
        {
            Marshal.ThrowExceptionForHR(captureFilter.EnumPins(out IEnumPins enumPins));

            IPin[] pinHolder = new IPin[1];
            while (enumPins.Next(1, pinHolder, 0) == 0)
            {
                if (DShow.ConnectFilters(filterGraph, pinHolder[0], (IBaseFilter)sampleGrabber) == 0)
                    break;
            }
        }

        mediaControl = (IMediaControl)filterGraph;
        frameDataQueue = new FrameQueue(16);
    }

    public override List<VideoFormat> AvailableFormats()
    {
        return DShow.AvailableFormats(dsDevice);
    }

    public override void StartCapture()
    {
        Marshal.ThrowExceptionForHR(mediaControl.Run());
    }

    public override void StopCapture()
    {
        Marshal.ThrowExceptionForHR(mediaControl.Stop());
    }

    public override bool TryReadFrame(ref Frame frame)
    {
        Frame? maybeFrame;
        lock (frameDataQueue)
            maybeFrame = frameDataQueue.TryDequeueFrame();

        if (maybeFrame == null)
            return false;

        frame.Data = maybeFrame.Data;
        frame.Width = maybeFrame.Width;
        frame.Height = maybeFrame.Height;
        frame.ImageFormat = maybeFrame.ImageFormat;

        return true;
    }

    // See https://learn.microsoft.com/en-us/windows/win32/directshow/isamplegrabber-setcallback#parameters
    private enum SampleGrabberCallbackMethod : int
    {
        SampleCB = 0,
        BufferCB = 1,
    }

    int ISampleGrabberCB.SampleCB(double sampleTime, IMediaSample mediaSample)
    {
        // We don't do that here
        return 0;
    }

    int ISampleGrabberCB.BufferCB(double sampleTime, nint bufferPtr, int bufferLength)
    {
        sampleGrabber.GetConnectedMediaType(connectedMediaType);
        AssertFrame(connectedMediaType.majorType == MediaType.Video, "Connected media type is not Video");
        string readableMediaType = DShow.AMMediaTypeToReadableString(connectedMediaType);
        Console.WriteLine($"Called BufferCB T={sampleTime} L={bufferLength} M={readableMediaType}");

        VideoInfoHeader? vih = Marshal.PtrToStructure<VideoInfoHeader>(connectedMediaType.formatPtr) ?? throw new CameraDeviceInvalidFrameException("No video format found in connected media type");

        byte[] pixelBuffer = new byte[bufferLength];
        Marshal.Copy(bufferPtr, pixelBuffer, 0, pixelBuffer.Length);

        Frame frame = new Frame()
        {
            Data = pixelBuffer,
            Width = (uint)vih.BmiHeader.Width,
            Height = (uint)vih.BmiHeader.Height,
            ImageFormat = ImageFormat.Argb, // TODO: infer
        };

        lock (frameDataQueue)
            frameDataQueue.EnqueueFrame(frame);

        return 0;
    }
}