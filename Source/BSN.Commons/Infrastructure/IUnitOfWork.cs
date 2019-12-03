using System;
using System.Threading.Tasks;
using System.Transactions;

namespace Commons.Infrastructure
{
    public interface IUnitOfWork
    {
        void Commit();
        void AddToQueue(ITaskUnit task);
        void AddToQueue(Task task, Task rollback);
    }
}
