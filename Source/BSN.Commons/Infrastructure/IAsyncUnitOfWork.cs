using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BSN.Commons.Infrastructure
{
    public interface IAsyncUnitOfWork
    {
        IDatabaseFactory DatabaseFactory { get; }

        IReadOnlyCollection<Exception> Exceptions { get; }

        void AddToQueue(ITaskUnit task);

        Task CommitAsync(
            CancellationToken cancellationToken = default);
    }
}