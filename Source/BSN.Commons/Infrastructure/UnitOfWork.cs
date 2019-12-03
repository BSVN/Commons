using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Transactions;

namespace Commons.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDatabaseFactory _databaseFactory;
        private DbContext _dataContext;
        private readonly List<ITaskUnit> _tasks;
        public List<Exception> _exceptions { get; set; }

        protected DbContext DataContext => _dataContext ?? (_dataContext = _databaseFactory.Get());


        public UnitOfWork(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
            _exceptions = new List<Exception>();
            _tasks = new List<ITaskUnit>();
        }

        public void AddToQueue(ITaskUnit task)
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task));
            }

            _tasks.Add(task);
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

            _tasks.Add(new EnlistTask(executeFunction, rollbackFunction));
        }



        public void Commit()
        {
            using (var tran = new TransactionScope())
            {
                foreach (var task in _tasks)
                {
                    Transaction.Current.EnlistVolatile(task, EnlistmentOptions.EnlistDuringPrepareRequired);
                }

                DataContext.SaveChanges();

                tran.Complete();
            }
            foreach (var task in _tasks)
            {
                if (task._exception != null)
                {
                    _exceptions.Add(task._exception);
                }
            }
        }
    }
}
