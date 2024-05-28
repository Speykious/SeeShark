// Copyright (c) Speykious
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 2-Clause License. See LICENSE for details.

using System.Runtime.Versioning;
using System.Text;

namespace SeeShark.Interop.MacOS;

[SupportedOSPlatform("Macos")]
internal struct NSString : INSObject
{
    private readonly nint id;

    internal NSString(nint id)
    {
        this.id = id;
    }

    public nint ID => id;

    private static readonly OClass classPtr = ObjC.GetClass(nameof(NSString));

    private static readonly Selector sel_UTF8String = ObjC.sel_registerName("UTF8String");
    private static readonly Selector sel_alloc = ObjC.sel_registerName("alloc");
    private static readonly Selector sel_initWithBytes = ObjC.sel_registerName("initWithBytes:length:encoding:");

    internal static NSString FromUTF8String(string s)
    {
        nint nss = ObjC.objc_msgSend_id(classPtr.ID, sel_alloc);

        byte[] utf8Bytes = Encoding.UTF8.GetBytes(s);
        unsafe
        {
            fixed (byte* sptr = utf8Bytes)
            {
                uint length = (uint)utf8Bytes.Length;
                uint encoding = (uint)NSStringEncoding.UTF8;
                nss = ObjC.objc_msgSend_id(nss, sel_initWithBytes, (nint)sptr, length, encoding);
            }
        }

        return new NSString(nss);
    }

    internal string ToUTF8String()
    {
        unsafe
        {
            return new string((sbyte*)ObjC.objc_msgSend_id(id, sel_UTF8String));
        }
    }
}
