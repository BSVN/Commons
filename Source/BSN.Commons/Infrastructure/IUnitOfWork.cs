using System;

namespace Commons.Infrastructure
{
    public interface IUnitOfWork
    {
        void Commit();

        void AddToQueue(Func<bool> func);
    }
}
