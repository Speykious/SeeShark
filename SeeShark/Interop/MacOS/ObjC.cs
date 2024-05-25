// Copyright (c) Speykious
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 2-Clause License. See LICENSE for details.

using System;
using System.Runtime.InteropServices;

namespace SeeShark.Interop.MacOS;

internal static class ObjC
{
    private const string lib_objc = "/usr/lib/libobjc.dylib";
    private const string lib_avfoundation = "/System/Library/Frameworks/AVFoundation.framework/AVFoundation";

    internal static nint AVFoundationHandle = DL.dlopen(lib_avfoundation, DL.RTLD_NOW);
    internal static OClass NSObjectClass = GetClass("NSObject");

    internal static Selector Sel_alloc = ObjC.sel_registerName("alloc");
    internal static Selector Sel_init = ObjC.sel_registerName("init");

    /// <summary>
    /// Returns the class definition of a specified class.
    /// </summary>
    /// <param name="name">The name of the class to look up.</param>
    internal static OClass GetClass(string name)
    {
        OClass cls = objc_getClass(name);
        if (cls.ID == nint.Zero)
            throw new ArgumentException("Unknown ObjC class: " + name);

        return cls;
    }

    [DllImport(lib_objc)]
    internal static extern OClass objc_getClass(string name);
    [DllImport(lib_objc)]
    internal static extern OClass objc_allocateClassPair(OClass superclass, string name, nuint extraBytes);
    [DllImport(lib_objc)]
    internal static extern void objc_registerClassPair(OClass cls);
    [DllImport(lib_objc)]
    internal static extern nint objc_getProtocol(string name);
    [DllImport(lib_objc)]
    internal static extern nint class_createInstance(OClass cls, nuint extraBytes);
    [DllImport(lib_objc)]
    internal static extern nint class_destructInstance(nint obj);
    [DllImport(lib_objc)]
    internal static extern bool class_addProtocol(OClass cls, nint protocol);
    [DllImport(lib_objc)]
    internal static unsafe extern bool class_addMethod(OClass cls, Selector name, delegate* unmanaged[Cdecl]<nint, Selector, nint, nint, nint, void> imp, string typeEncoding);

    [DllImport(lib_objc, EntryPoint = "objc_msgSend")]
    internal static extern void objc_msgSend(nint self, Selector op);

    [DllImport(lib_objc, EntryPoint = "objc_msgSend")]
    internal static extern void objc_msgSend(nint self, Selector op, nint arg1);

    [DllImport(lib_objc, EntryPoint = "objc_msgSend")]
    internal static extern void objc_msgSend(nint self, Selector op, nint arg1, nint arg2);

    [DllImport(lib_objc, EntryPoint = "objc_msgSend")]
    internal static extern nint objc_msgSend_id(nint self, Selector op);

    [DllImport(lib_objc, EntryPoint = "objc_msgSend")]
    internal static extern nint objc_msgSend_id(nint self, Selector op, int arg1);

    [DllImport(lib_objc, EntryPoint = "objc_msgSend")]
    internal static extern nint objc_msgSend_id(nint self, Selector op, nint arg1);

    [DllImport(lib_objc, EntryPoint = "objc_msgSend")]
    internal static extern nint objc_msgSend_id(nint self, Selector op, nint arg1, nint arg2);

    [DllImport(lib_objc, EntryPoint = "objc_msgSend")]
    internal static extern nint objc_msgSend_id(nint self, Selector op, nint arg1, uint arg2, uint agr3);

    [DllImport(lib_objc, EntryPoint = "objc_msgSend")]
    internal static extern int objc_msgSend_int(nint self, Selector op);

    [DllImport(lib_objc, EntryPoint = "objc_msgSend")]
    internal static extern uint objc_msgSend_uint(nint self, Selector op);

    [DllImport(lib_objc, EntryPoint = "objc_msgSend")]
    internal static extern bool objc_msgSend_bool(nint self, Selector op, nint arg1);

    [DllImport(lib_objc)]
    internal static extern Selector sel_registerName(string name);

    [DllImport(lib_objc)]
    internal static extern nint dispatch_queue_create(string label, nint attr);
    [DllImport(lib_objc)]
    internal static extern void dispatch_release(nint dispatchObject);
}

internal interface INSObject
{
    public nint ID { get; }
}

internal struct Selector
{
    private nint id;
}

internal struct OClass
{
    internal nint ID;
}
