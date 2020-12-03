using System;

namespace BSN.Commons.Infrastructure
{
    public interface IDatabaseFactory : IDisposable
    {
        IDbContext Get();
    }
}
