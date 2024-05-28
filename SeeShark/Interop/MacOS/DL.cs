// Copyright (c) Speykious
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 2-Clause License. See LICENSE for details.

using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace SeeShark.Interop.MacOS;

[SupportedOSPlatform("Macos")]
internal static class DL
{
    internal const string LIB_DL = "libSystem.dylib";

    internal const int RTLD_NOW = 2;

    [DllImport(LIB_DL)]
    internal static extern nint dlsym(nint handle, string name);

    [DllImport(LIB_DL)]
    internal static extern nint dlopen(string fileName, int flags);

    internal static T GetConstant<T>(nint handle, string symbol) where T : unmanaged
    {
        unsafe
        {
            return *(T*)dlsym(handle, symbol);
        }
    }
}
