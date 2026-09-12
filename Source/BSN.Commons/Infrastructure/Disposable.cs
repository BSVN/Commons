using System;
using System.Threading;

namespace BSN.Commons.Infrastructure
{
    /// <summary>
    /// Provides a base implementation of the standard dispose pattern.
    /// </summary>
    public abstract class Disposable : IDisposable
    {
        private int _disposed;

        ~Disposable()
        {
            Dispose(false);
        }

        /// <summary>
        /// Gets a value indicating whether this instance has been disposed.
        /// </summary>
        protected bool IsDisposed =>
            Volatile.Read(ref _disposed) != 0;

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases resources used by this instance.
        /// </summary>
        /// <param name="disposing">
        /// <c>true</c> when called from <see cref="Dispose()"/>;
        /// <c>false</c> when called from the finalizer.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (Interlocked.Exchange(ref _disposed, 1) != 0)
                return;

            if (disposing)
            {
                DisposeCore();
            }
        }

        /// <summary>
        /// Releases managed resources.
        /// </summary>
        protected virtual void DisposeCore()
        {
        }
    }
}