// Copyright (c) The Vignette Authors
// This file is part of SeeShark.
// SeeShark is licensed under the BSD 3-Clause License. See LICENSE for details.

using System;

namespace SeeShark
{
    public abstract class Disposable : IDisposable
    {
        public bool Disposed { get; private set; }

        public Disposable()
        {
            Disposed = false;
        }

        private void dispose(bool disposing)
        {
            if (Disposed)
                return;

            if (disposing)
                DisposeManaged();

            FreeUnmanaged();

            Disposed = true;
        }

        public void Dispose()
        {
            dispose(true);
            GC.SuppressFinalize(this);
        }

        protected abstract void DisposeManaged();
        protected abstract void FreeUnmanaged();

        ~Disposable()
        {
            dispose(false);
        }
    }
}
