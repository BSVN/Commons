using System;
using System.Collections.Generic;
using System.Transactions;
using System.Linq;

namespace BSN.Commons.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        public List<Exception> Exceptions { get; private set; }

        protected IDbContext DataContext => _dataContext ?? (_dataContext = _databaseFactory.Get());

        public UnitOfWork(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
            _tasks = new Queue<ITaskUnit>();
            Exceptions = new List<Exception>();
        }

        public void AddToQueue(ITaskUnit task)
        {
            task = task ?? throw new ArgumentNullException(nameof(task));
            _tasks.Enqueue(task);
        }

        public void Commit()
        {
            Queue<ITaskUnit> executedTasks = new Queue<ITaskUnit>();

            try
            {
                using (var transaction = new TransactionScope())
                {
                    while (_tasks.Count > 0)
                    {
                        var task = _tasks.Dequeue();
                        executedTasks.Enqueue(task);
                        Transaction.Current.EnlistVolatile(task, EnlistmentOptions.None);
                    }

                    DataContext.SaveChanges();
                    transaction.Complete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Exceptions.AddRange(executedTasks.Select(a => a.Exception));
            }
        }

        private readonly IDatabaseFactory _databaseFactory;
        private IDbContext _dataContext;
        private readonly Queue<ITaskUnit> _tasks;
    }
}