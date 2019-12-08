using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Transactions;
using System.Linq;

namespace Commons.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDatabaseFactory _databaseFactory;
        private DbContext _dataContext;
        private readonly Queue<ITaskUnit> _tasks;
        public List<Exception> _exceptions { get; set; }

        protected DbContext DataContext => _dataContext ?? (_dataContext = _databaseFactory.Get());


        public UnitOfWork(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
            _exceptions = new List<Exception>();
            _tasks = new Queue<ITaskUnit>();
        }

        public void AddToQueue(ITaskUnit task)
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task));
            }

            _tasks.Enqueue(task);
        }

        public void AddToQueue(Task executeFunction, Task rollbackFunction)
        {
            if (executeFunction == null)
            {
                throw new ArgumentNullException(nameof(executeFunction));
            }
            if (rollbackFunction == null)
            {
                throw new ArgumentNullException(nameof(rollbackFunction));
            }

            _tasks.Enqueue(new EnlistTask(executeFunction, rollbackFunction));
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
                _exceptions.AddRange(executedTasks.Select(a => a.Exception));
            }
        }
    }
}
