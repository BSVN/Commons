using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace Commons.Infrastructure
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly IDatabaseFactory _databaseFactory;
		private ExtendedDbContext _dataContext;


        protected ExtendedDbContext DataContext => _dataContext ?? (_dataContext = _databaseFactory.Get());


		public UnitOfWork(IDatabaseFactory databaseFactory)
		{
			_databaseFactory = databaseFactory;
		}

        public void Commit()
		{

            using (var dbContextTransaction = DataContext.Database.BeginTransaction())
            {
                try
                {
                    DataContext.SaveChanges();

                    while (DataContext.QueueCount > 0)
                    {
                        var Function = DataContext.RemoveFromQueue();

                        bool result = Function.Invoke();

                        if (!result)
                            throw new Exception("Function not invoked.");
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
	}
}