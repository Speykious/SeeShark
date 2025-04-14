// Copyright (c) Speykious
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 2-Clause License. See LICENSE for details.

using System;
using System.Runtime.Versioning;

namespace SeeShark.Interop.MacOS;

[SupportedOSPlatform("Macos")]
internal struct NSNumber : INSObject
{
    private readonly nint id;

    internal NSNumber(nint id)
    {
        this.id = id;
    }

    public nint ID => id;

    private static readonly OClass classPtr = ObjC.GetClass(nameof(NSNumber));

    private static readonly Selector sel_stringValue = ObjC.sel_registerName("stringValue");

    internal string StringValue()
    {
        unsafe
        {
            return new NSString(ObjC.objc_msgSend_id(id, sel_stringValue)).ToUTF8String();
        }
    }
}
