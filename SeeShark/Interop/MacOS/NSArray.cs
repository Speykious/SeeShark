// Copyright (c) Speykious
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 2-Clause License. See LICENSE for details.

using System;
using System.Runtime.Versioning;

namespace SeeShark.Interop.MacOS;

[SupportedOSPlatform("Macos")]
internal struct NSArray : INSObject
{
    private readonly nint id;

    internal NSArray(nint id)
    {
        this.id = id;
    }

    public nint ID => id;

    private static readonly OClass classPtr = ObjC.GetClass(nameof(NSArray));

    private static readonly Selector sel_count = ObjC.sel_registerName("count");
    private static readonly Selector sel_objectAtIndex = ObjC.sel_registerName("objectAtIndex:");
    private static readonly Selector sel_arrayWithObjects_count = ObjC.sel_registerName("arrayWithObjects:count:");

    internal readonly uint Count => ObjC.objc_msgSend_uint(id, sel_count);

    internal nint ObjectAtIndex(int index) => ObjC.objc_msgSend_id(id, sel_objectAtIndex, index);

    internal static NSArray WithObjects(nint[] objects, int count)
    {
        return new NSArray(ObjC.objc_msgSend_id(classPtr.ID, sel_arrayWithObjects_count, objects, count));
    }

    internal nint[] ToArray()
    {
        nint[] result = new nint[Count];
        for (int i = 0; i < result.Length; i++)
            result[i] = ObjectAtIndex(i);
        return result;
    }

    internal T[] ToTypedArray<T>(Func<nint, T> f)
    {
        T[] result = new T[Count];
        for (int i = 0; i < result.Length; i++)
            result[i] = f(ObjectAtIndex(i));
        return result;
    }
}
