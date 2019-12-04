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
            using (var transaction = new TransactionScope())
            {
                Queue<ITaskUnit> executedTasks = new Queue<ITaskUnit>();

                while (_tasks.Count > 0)
                {
                    var task = _tasks.Dequeue();
                    executedTasks.Enqueue(task);

                    task.Execute();

                    if (task.Exception != null)
                    {
                        _exceptions.Add(task.Exception);

                        while (executedTasks.Count > 0)
                        {
                            var rollbackTask = executedTasks.Dequeue();

                            rollbackTask.Rollback();

                            if (rollbackTask.Exception != null)
                                _exceptions.Add(rollbackTask.Exception);
                        }
                        throw task.Exception;
                    }
                }

                DataContext.SaveChanges();
                transaction.Complete();
            }
        }
    }
}
