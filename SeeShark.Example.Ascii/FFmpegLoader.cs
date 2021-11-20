// Copyright (c) The Vignette Authors
// This file is part of SeeShark.
// SeeShark is licensed under LGPL v3. See LICENSE.LESSER.md for details.

using FFmpeg.AutoGen;
using LibraryLoader = FFmpeg.AutoGen.Native.LibraryLoader;

namespace SeeShark.Example.Ascii
{
    public static class FFmpegLoader
    {
        /// <summary>
        ///     Tries to load the native libraries from the set root path. <br/>
        ///     You can specify which libraries need to be loaded with LibraryFlags.
        ///     It will try to load all librares by default. <br/>
        ///     Ideally, you would want to only call this function once, before doing anything with FFmpeg.
        ///     If you try to do that later, it might unload all of your already loaded libraries and fail to provide them again.
        /// </summary>
        /// <returns>Whether it succeeded in loading all the requested libraries.</returns>
        public static bool CanLoadLibraries(string[] libraries, string path = "")
        {
            var validated = new List<string>();
            return libraries.All((lib) => canLoadLibrary(lib, path, validated));
        }

        // Note: recurses in a very similar way to the LoadLibrary() method.
        private static bool canLoadLibrary(string lib, string path, List<string> validated)
        {
            if (validated.Contains(lib))
                return true;

            var version = ffmpeg.LibraryVersionMap[lib];
            if (!CanLoadNativeLibrary(path, lib, version))
                return false;

            validated.Add(lib);

            var dependencies = ffmpeg.LibraryDependenciesMap[lib];
            return dependencies.Except(validated).All((dep) => canLoadLibrary(dep, path, validated));
        }

        public static bool CanLoadNativeLibrary(string path, string libraryName, int version)
        {
            var nativeLibraryName = LibraryLoader.GetNativeLibraryName(libraryName, version);
            var fullName = Path.Combine(path, nativeLibraryName);
            return File.Exists(fullName);
        }

        /// <summary>
        ///     Tries to set the RootPath to the first path in which it can find all the required libraries.
        ///     Ideally, you would want to only call this function once, before doing anything with FFmpeg.
        /// </summary>
        /// <remarks>
        ///     This function will not load the native libraries but merely check if they exist.
        /// </remarks>
        /// <param name="requiredLibraries">The required libraries. If you don't need all of them, you can specify them here.</param>
        /// <param name="paths">Every path to try out. It will set the RootPath to th first one that works.</param>
        /// <returns>Whether it succeeded in setting the RootPath.</returns>
        public static bool TrySetRootPath(string[] requiredLibraries, params string[] paths)
        {
            try
            {
                ffmpeg.RootPath = paths.First((path) => CanLoadLibraries(requiredLibraries, path));
                return true;
            }
            catch (ArgumentNullException)
            {
                return false;
            }
        }
    }
}
