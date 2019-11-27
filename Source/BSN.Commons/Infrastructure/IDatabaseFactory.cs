using System;
using System.Data.Entity;

namespace Commons.Infrastructure
{
    public interface IDatabaseFactory : IDisposable
    {
        ExtendedDbContext Get();
    }
}
