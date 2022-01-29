// Copyright (c) The Vignette Authors
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 3-Clause License. See LICENSE for details.

using System;
using System.Threading;

namespace SeeShark
{
    /// <remarks>
    /// based on <see href="https://github.com/shimat/opencvsharp/blob/9a5f9828a74cfa3995562a06716e177705cde038/src/OpenCvSharp/Fundamentals/DisposableObject.cs">OpenCvSharp</see>
    /// </remarks>
    public abstract class Disposable : IDisposable
    {
        private volatile int disposeSignaled = 0;

        public bool IsDisposed { get; protected set; }
        protected bool IsOwner { get; private set; }

        protected Disposable(bool isOwner = true)
        {
            IsDisposed = false;
            IsOwner = isOwner;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (Interlocked.Exchange(ref disposeSignaled, 1) != 0)
                return;

            IsDisposed = true;

            if (disposing)
                DisposeManaged();

            DisposeUnmanaged();
        }

        ~Disposable()
        {
            Dispose(false);
        }

        protected virtual void DisposeManaged() { }
        protected virtual void DisposeUnmanaged() { }

        public void TransferOwnership() => IsOwner = false;

        public void ThrowIfDisposed()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(GetType().FullName);
        }
    }
}
