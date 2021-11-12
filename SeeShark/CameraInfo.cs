// Copyright (c) The Vignette Authors
// This file is part of SeeShark.
// SeeShark is licensed under LGPL v3. See LICENSE.LESSER.md for details.

namespace SeeShark
{
    public struct CameraInfo
    {
        public string Name { get; set; }
        public string Path { get; set; }

        public CameraInfo(string name, string path)
        {
            Name = name;
            Path = path;
        }
    }
}
