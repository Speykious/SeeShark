// Copyright (c) The Vignette Authors
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 3-Clause License. See LICENSE for details.

namespace SeeShark.Interop.Libc
{
    internal enum FileOpenFlags
    {
        O_RDONLY = 0x00,
        O_RDWR = 0x02,
        O_NONBLOCK = 0x800,
        O_SYNC = 0x101000
    }
}
