using System;
using System.Threading.Tasks;
using System.Transactions;

namespace BSN.Commons.Infrastructure
{
    public interface IUnitOfWork
    {
        IDatabaseFactory DatabaseFactory { get; }

        void Commit();
        void AddToQueue(ITaskUnit task);
    }
}
