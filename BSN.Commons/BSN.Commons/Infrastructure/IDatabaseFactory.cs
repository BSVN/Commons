using System;
using System.Data.Entity;

namespace BSN.Commons.Infrastructure
{
    public interface IDatabaseFactory : IDisposable
    {
        DbContext Get();
    }
}
