// Copyright (c) The Vignette Authors
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 3-Clause License. See LICENSE for details.

using System;

namespace SeeShark.Interop.Libc
{
    [Flags]
    internal enum MemoryMappedFlags
    {
        MAP_SHARED = 0x01,
        MAP_PRIVATE = 0x02,
        MAP_FIXED = 0x10
    }
}
