using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BSN.Commons.Infrastructure
{
    public interface IUnitOfWork : IDisposable, IAsyncDisposable
    {
        IDatabaseFactory DatabaseFactory { get; }

        IReadOnlyCollection<Exception> Exceptions { get; }

        void AddToQueue(ITaskUnit task);

        void Commit();

        Task CommitAsync(
            CancellationToken cancellationToken = default);
    }
}