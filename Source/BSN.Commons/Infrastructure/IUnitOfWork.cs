using System;

namespace Commons.Infrastructure
{
    public interface IUnitOfWork
    {
        void Commit();
    }
}
