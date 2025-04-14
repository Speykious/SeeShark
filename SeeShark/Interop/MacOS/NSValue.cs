// Copyright (c) Speykious
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 2-Clause License. See LICENSE for details.

using System.Runtime.Versioning;

namespace SeeShark.Interop.MacOS;

[SupportedOSPlatform("Macos")]
internal struct NSValue<T> : INSObject where T : unmanaged
{
    private readonly nint id;

    internal NSValue(nint id)
    {
        this.id = id;
    }

    public nint ID => id;

    private static readonly Selector sel_getValue = ObjC.sel_registerName("getValue:size:");

    internal T GetValue()
    {
        T value;
        unsafe
        {
            ObjC.objc_msgSend(id, sel_getValue, (nint)(&value), (nuint)sizeof(T));
        }
        return value;
    }
}
