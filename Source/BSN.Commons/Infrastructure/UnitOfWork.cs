using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace Commons.Infrastructure
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly IDatabaseFactory _databaseFactory;
		private DbContext _dataContext;
        private Queue<Func<bool>> Queue { get; set; }


        protected DbContext DataContext => _dataContext ?? (_dataContext = _databaseFactory.Get());


		public UnitOfWork(IDatabaseFactory databaseFactory)
		{
			_databaseFactory = databaseFactory;
            Queue = new Queue<Func<bool>>();
		}

        public void AddToQueue(Func<bool> func)
        {
            Queue.Enqueue(func);
        }

        public void Commit()
		{
			DataContext.SaveChanges();

            while (Queue.Count > 0)
            {
                var Function = Queue.Dequeue();
                Function.Invoke();
            }
        }
	}
}