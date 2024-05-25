// Copyright (c) Speykious
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 2-Clause License. See LICENSE for details.

namespace SeeShark.Interop.MacOS;

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

    internal readonly uint Count => ObjC.objc_msgSend_uint(id, sel_count);

    internal nint ObjectAtIndex(int index) => ObjC.objc_msgSend_id(id, sel_objectAtIndex, index);

    internal nint[] ToArray()
    {
        nint[] result = new nint[Count];
        for (int i = 0; i < result.Length; i++)
            result[i] = ObjectAtIndex(i);
        return result;
    }
}
