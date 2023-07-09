using System;

namespace BSN.Commons.Infrastructure
{
	public interface IDbContext: IDisposable
	{
		int SaveChanges();
	}
}
