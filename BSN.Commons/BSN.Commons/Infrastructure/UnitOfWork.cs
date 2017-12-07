using System.Data.Entity;

namespace BSN.Commons.Infrastructure
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly IDatabaseFactory _databaseFactory;
		private DbContext _dataContext;


		protected DbContext DataContext { get { return _dataContext ?? (_dataContext = _databaseFactory.Get()); } }


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