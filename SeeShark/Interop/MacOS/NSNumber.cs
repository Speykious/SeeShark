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
    private static readonly Selector sel_unsignedIntValue = ObjC.sel_registerName("unsignedIntValue");
    private static readonly Selector sel_numberWithUnsignedInt = ObjC.sel_registerName("numberWithUnsignedInt:");

    internal string StringValue() => new NSString(ObjC.objc_msgSend_id(id, sel_stringValue)).ToUTF8String();
    internal uint UIntValue() => ObjC.objc_msgSend_uint(id, sel_unsignedIntValue);

    internal static NSNumber UInt(uint value)
    {
        return new NSNumber(ObjC.objc_msgSend_id(classPtr.ID, sel_numberWithUnsignedInt, value));
    }
}
