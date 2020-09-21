namespace Commons.Infrastructure
{
	public interface IDbSet<TEntity> where TEntity : class
	{

	}

	public interface IDbContext
	{
		void SaveChanges();
		IDbSet<TEntity> Set<TEntity>() where TEntity : class;
	}
}
