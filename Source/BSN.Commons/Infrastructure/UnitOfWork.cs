using System;

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
            DataContext.SaveChanges();

            using (var dbContextTranaction = DataContext.Database.BeginTransaction())
            {
                try
                {
                    DataContext.SaveChanges();

                    while (DataContext.QueueCount > 0)
                    {
                        var function = DataContext.RemoveFromQueue();

                        if (!function.Invoke())
                            throw new Exception();
                    }
                }
                catch (Exception)
                {
                    dbContextTranaction.Rollback();
                    throw;
                }
            }
        }
    }
}