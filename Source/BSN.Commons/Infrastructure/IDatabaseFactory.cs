using System;

namespace Commons.Infrastructure
{
    public interface IDatabaseFactory : IDisposable
    {
        ExtendedDbContext Get();
    }
}
