using System.Data.Entity;

namespace Commons.Infrastructure
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly IDatabaseFactory _databaseFactory;
		private DbContext _dataContext;


		protected DbContext DataContext => _dataContext ?? (_dataContext = _databaseFactory.Get());


		public UnitOfWork(IDatabaseFactory databaseFactory)
		{
			_databaseFactory = databaseFactory;
		}


		public void Commit()
		{
			DataContext.SaveChanges();
		}
	}
}