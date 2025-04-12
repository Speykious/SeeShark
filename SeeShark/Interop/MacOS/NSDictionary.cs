// Copyright (c) Speykious
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 2-Clause License. See LICENSE for details.

using System;
using System.Runtime.Versioning;

namespace SeeShark.Interop.MacOS;

[SupportedOSPlatform("Macos")]
internal struct NSDictionary : INSObject
{
    private readonly nint id;

    internal NSDictionary(nint id)
    {
        this.id = id;
    }

    public nint ID => id;

    private static readonly OClass classPtr = ObjC.GetClass(nameof(NSDictionary));

    private static readonly Selector sel_count = ObjC.sel_registerName("count");
    private static readonly Selector sel_getObjects_andKeys_count = ObjC.sel_registerName("getObjects:andKeys:count:");

    internal ulong Count() => ObjC.objc_msgSend_ulong(id, sel_count);

    internal void GetObjectsAndKeys(nint[] objects, nint[] keys, uint count)
    {
        ObjC.objc_msgSend(id, sel_getObjects_andKeys_count, objects, keys, count);
    }

    internal void DebugPrintNSDictionary(NSDictionary dict)
    {
        debugPrintNSDictionary(dict, 0);
    }

    private void debugPrintNSDictionary(NSDictionary dict, int level)
    {
        uint count = (uint)dict.Count();

        nint[] objects = new nint[count];
        nint[] keys = new nint[count];
        dict.GetObjectsAndKeys(objects, keys, count);

        string indent = new string(' ', level * 2);

        for (int i = 0; i < count; i++)
        {
            string key = new NSString(keys[i]).ToUTF8String();
            string objectClassName = ObjC.GetClassName(objects[i]);

            switch (objectClassName)
            {
                case "__NSCFConstantString":
                    Console.Error.WriteLine($"{indent}{key,-36}: string \"{new NSString(objects[i]).ToUTF8String()}\"");
                    break;
                case "__NSDictionaryM":
                    NSDictionary subDict = new NSDictionary(objects[i]);
                    Console.Error.WriteLine($"{indent}{key,-36}: dict ({subDict.Count()})");
                    debugPrintNSDictionary(subDict, level + 1);
                    break;
                case "__NSCFNumber":
                    Console.Error.WriteLine($"{indent}{key,-36}: num ({new NSNumber(objects[i]).StringValue()})");
                    break;
                default:
                    Console.Error.WriteLine($"{indent}{key,-36}: ({objectClassName})");
                    break;
            }
        }
    }

}
