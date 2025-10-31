// Copyright (c) Speykious
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 2-Clause License. See LICENSE for details.

using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace SeeShark.Interop.MacOS;

[SupportedOSPlatform("Macos")]
internal static class ObjC
{
    private const string lib_objc = "/usr/lib/libobjc.dylib";
    private const string lib_system = "libSystem.B.dylib";
    private const string lib_avfoundation = "/System/Library/Frameworks/AVFoundation.framework/AVFoundation";

    internal static nint AVFoundationHandle;
    internal static OClass NSObjectClass;

    internal static Selector Sel_alloc;
    internal static Selector Sel_init;

    static ObjC()
    {
        AVFoundationHandle = DL.dlopen(lib_avfoundation, DL.RTLD_NOW);
        NSObjectClass = GetClass("NSObject");

        Sel_alloc = sel_registerName("alloc");
        Sel_init = sel_registerName("init");
    }

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

    internal static string GetClassName(nint id)
    {
        unsafe
        {
            return new string((sbyte*)object_getClassName(id));
        }
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
    internal static extern nint object_getClassName(nint id);
    [DllImport(lib_objc)]
    internal static extern nint class_createInstance(OClass cls, nuint extraBytes);
    [DllImport(lib_objc)]
    internal static extern nint class_destructInstance(nint obj);
    [DllImport(lib_objc)]
    internal static extern bool class_addIvar(OClass cls, string name, nuint size, byte alignment, string types);
    [DllImport(lib_objc)]
    internal static extern nint class_getInstanceVariable(OClass cls, string name);
    [DllImport(lib_objc)]
    internal static extern nint ivar_getOffset(nint ivar);
    [DllImport(lib_objc)]
    internal static extern bool class_addProtocol(OClass cls, nint protocol);
    [DllImport(lib_objc)]
    internal static unsafe extern bool class_addMethod(OClass cls, Selector name, delegate* unmanaged[Cdecl]<nint, Selector, nint, nint, nint, void> imp, string typeEncoding);

    [DllImport(lib_objc, EntryPoint = "objc_msgSend")]
    internal static extern void objc_msgSend(nint self, Selector op);

    [DllImport(lib_objc, EntryPoint = "objc_msgSend")]
    internal static extern void objc_msgSend(nint self, Selector op, bool arg1);

    [DllImport(lib_objc, EntryPoint = "objc_msgSend")]
    internal static extern void objc_msgSend(nint self, Selector op, nint arg1);

    [DllImport(lib_objc, EntryPoint = "objc_msgSend")]
    internal static extern void objc_msgSend(nint self, Selector op, nint arg1, nint arg2);

    [DllImport(lib_objc, EntryPoint = "objc_msgSend")]
    internal static extern void objc_msgSend(nint self, Selector op, nint arg1, nuint arg2);

    [DllImport(lib_objc, EntryPoint = "objc_msgSend")]
    internal static extern void objc_msgSend(nint self, Selector op, nint[] arg1, nint[] arg2, uint arg3);

    [DllImport(lib_objc, EntryPoint = "objc_msgSend")]
    internal static extern void objc_msgSend(nint self, Selector op, CMTime time);

    [DllImport(lib_objc, EntryPoint = "objc_msgSend")]
    internal static extern nint objc_msgSend_id(nint self, Selector op);

    [DllImport(lib_objc, EntryPoint = "objc_msgSend")]
    internal static extern nint objc_msgSend_id(nint self, Selector op, int arg1);
    [DllImport(lib_objc, EntryPoint = "objc_msgSend")]
    internal static extern nint objc_msgSend_id(nint self, Selector op, uint arg1);

    [DllImport(lib_objc, EntryPoint = "objc_msgSend")]
    internal static extern nint objc_msgSend_id(nint self, Selector op, nint arg1);

    [DllImport(lib_objc, EntryPoint = "objc_msgSend")]
    internal static extern nint objc_msgSend_id(nint self, Selector op, nint arg1, nint arg2);

    [DllImport(lib_objc, EntryPoint = "objc_msgSend")]
    internal static extern nint objc_msgSend_id(nint self, Selector op, nint[] arg1, nint arg2);

    [DllImport(lib_objc, EntryPoint = "objc_msgSend")]
    internal static extern nint objc_msgSend_id(nint self, Selector op, nint arg1, uint arg2, uint agr3);

    [DllImport(lib_objc, EntryPoint = "objc_msgSend")]
    internal static extern int objc_msgSend_int(nint self, Selector op);

    [DllImport(lib_objc, EntryPoint = "objc_msgSend")]
    internal static extern int objc_msgSend_int(nint self, Selector op, nint arg1);

    [DllImport(lib_objc, EntryPoint = "objc_msgSend")]
    internal static extern uint objc_msgSend_uint(nint self, Selector op);

    [DllImport(lib_objc, EntryPoint = "objc_msgSend")]
    internal static extern ulong objc_msgSend_ulong(nint self, Selector op);

    [DllImport(lib_objc, EntryPoint = "objc_msgSend")]
    internal static extern double objc_msgSend_double(nint self, Selector op);

    [DllImport(lib_objc, EntryPoint = "objc_msgSend")]
    internal static extern bool objc_msgSend_bool(nint self, Selector op);

    [DllImport(lib_objc, EntryPoint = "objc_msgSend")]
    internal static extern bool objc_msgSend_bool(nint self, Selector op, nint arg1);

    [DllImport(lib_objc, EntryPoint = "objc_msgSend")]
    internal static extern CMTime objc_msgSend_cmTime(nint self, Selector op);

    [DllImport(lib_objc)]
    internal static extern Selector sel_registerName(string name);

    [DllImport(lib_objc)]
    internal static extern nint dispatch_queue_create(string label, nint attr);
    [DllImport(lib_objc)]
    internal static extern void dispatch_release(nint dispatchObject);

    #region ObjC blocks
    // Arguments such as completionHandler from AVCaptureDevice.requestAccessForMediaType are
    // actually not function pointers but something very specific to Objective C called "blocks".
    // See https://clang.llvm.org/docs/Block-ABI-Apple.html for documentation on this.
    // Man what a nightmare of just-barely-documented shit this runtime is...

    [DllImport(lib_system)]
    internal static extern nint dlsym(nint handle, string symbol);

    internal static nint NSConcreteGlobalBlock = dlsym(0, "_NSConcreteGlobalBlock");

    [Flags]
    internal enum BlockFlags : int
    {
        /// <summary>
        /// Set to true on blocks that have captures (and thus are not true
        /// global blocks) but are known not to escape for various other
        /// reasons. For backward compatibility with old runtimes, whenever
        /// BLOCK_IS_NOESCAPE is set, BLOCK_IS_GLOBAL is set too. Copying a
        /// non-escaping block returns the original block and releasing such a
        /// block is a no-op, which is exactly how global blocks are handled.
        /// </summary>
        IS_NOESCAPE = 1 << 23,

        HAS_COPY_DISPOSE = 1 << 25,
        HAS_CTOR = 1 << 26, // helpers have C++ code
        IS_GLOBAL = 1 << 28,
        HAS_STRET = 1 << 29, // IFF BLOCK_HAS_SIGNATURE
        HAS_SIGNATURE = 1 << 30,
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct BlockLiteral
    {
        /// <summary>
        /// initialized to <c>&_NSConcreteStackBlock</c> or <c>&_NSConcreteGlobalBlock</c>
        /// </summary>
        internal nint Isa;
        internal BlockFlags Flags;
        internal int Reserved;
        internal nint Invoke;
        internal nint Descriptor;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct BlockDescriptor
    {
        /// <summary>
        /// <c>NULL</c>
        /// </summary>
        internal ulong Reserved;
        /// <summary>
        /// sizeof(BlockLiteral)
        /// </summary>
        internal ulong Size;
        /// <summary>
        /// Optional helper function, IFF <c>HAS_COPY_DISPOSE</c>
        /// </summary>
        internal nint CopyHelper;
        /// <summary>
        /// Optional helper function, IFF <c>HAS_COPY_DISPOSE</c>
        /// </summary>
        internal nint DisposeHelper;
        /// <summary>
        /// Required <c>ABI.2010.3.16</c>, IFF <c>HAS_SIGNATURE</c>
        /// </summary>
        internal nint Signature;
    }
    #endregion
}

internal interface INSObject
{
    public nint ID { get; }
}

#pragma warning disable CS0169
internal struct Selector
{
    private nint id;
}
#pragma warning restore CS0169

#pragma warning disable CS0649
internal struct OClass
{
    internal nint ID;
}
#pragma warning restore CS0649
