// Copyright (c) Speykious
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 2-Clause License. See LICENSE for details.

using System;
using System.Collections.Generic;
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
    private static readonly Selector sel_objectForKey = ObjC.sel_registerName("objectForKey:");
    private static readonly Selector sel_dictionaryWithObjects_forKeys = ObjC.sel_registerName("dictionaryWithObjects:forKeys:");

    internal ulong Count() => ObjC.objc_msgSend_ulong(id, sel_count);

    internal void GetObjectsAndKeys(nint[] objects, nint[] keys, uint count)
    {
        ObjC.objc_msgSend(id, sel_getObjects_andKeys_count, objects, keys, count);
    }

    internal nint ObjectForKey(string key)
    {
        NSString nsstring = NSString.FromUTF8String(key);
        return ObjC.objc_msgSend_id(id, sel_objectForKey, nsstring.ID);
    }

    internal static NSDictionary DictionaryWithObjectsAndKeys(NSArray objects, NSArray keys)
    {
        return new NSDictionary(ObjC.objc_msgSend_id(classPtr.ID, sel_dictionaryWithObjects_forKeys, objects.ID, keys.ID));
    }

    internal void DebugPrint()
    {
        Console.Error.WriteLine("NSDictionary\n{");
        debugPrint(1);
        Console.Error.WriteLine("}");
    }

    private void debugPrint(int level)
    {
        uint count = (uint)Count();

        nint[] objects = new nint[count];
        nint[] keys = new nint[count];
        GetObjectsAndKeys(objects, keys, count);

        string indent = new string(' ', level * 2);

        for (int i = 0; i < count; i++)
        {
            string key = new NSString(keys[i]).ToUTF8String();
            string objectClassName = ObjC.GetClassName(objects[i]);

            switch (objectClassName)
            {
                case "__NSCFString":
                case "__NSCFConstantString":
                    Console.Error.WriteLine($"{indent}{key,-36}: string \"{new NSString(objects[i]).ToUTF8String()}\"");
                    break;
                case "__NSDictionaryM":
                    NSDictionary subDict = new NSDictionary(objects[i]);
                    Console.Error.WriteLine($"{indent}{key,-36}: dict ({subDict.Count()})");
                    subDict.debugPrint(level + 1);
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
