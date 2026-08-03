using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace BSN.Commons.Infrastructure
{
    public class UnitOfWork : IUnitOfWork, IDisposable, IAsyncDisposable
    {
        private readonly Queue<ITaskUnit> _tasks;
        private readonly List<Exception> _exceptions;

        private IDbContext _dataContext;
        private bool _disposed;

        public IDatabaseFactory DatabaseFactory { get; private set; }

        public IReadOnlyCollection<Exception> Exceptions
        {
            get { return _exceptions.AsReadOnly(); }
        }

        protected IDbContext DataContext
        {
            get
            {
                if (_dataContext == null)
                {
                    _dataContext = DatabaseFactory.Get();
                }

                return _dataContext;
            }
        }


        public UnitOfWork(IDatabaseFactory databaseFactory)
        {
            if (databaseFactory == null)
                throw new ArgumentNullException(nameof(databaseFactory));

            DatabaseFactory = databaseFactory;

            _tasks = new Queue<ITaskUnit>();
            _exceptions = new List<Exception>();
        }


        public void AddToQueue(ITaskUnit task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            ThrowIfDisposed();

            _tasks.Enqueue(task);
        }


        public void Commit()
        {
            ThrowIfDisposed();           

            Queue<ITaskUnit> executedTasks =
                new Queue<ITaskUnit>();

            try
            {
                using (var transaction = new TransactionScope())
                {
                    while (_tasks.Count > 0)
                    {
                        ITaskUnit task = _tasks.Dequeue();

                        executedTasks.Enqueue(task);

                        Transaction.Current.EnlistVolatile(
                            task,
                            EnlistmentOptions.None);
                    }

                    DataContext.SaveChanges();

                    transaction.Complete();
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                CollectExceptions(executedTasks);
            }
        }


        public async Task CommitAsync(
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();

          Queue<ITaskUnit> executedTasks =
                new Queue<ITaskUnit>();

            try
            {
                using (var transaction = new TransactionScope(
                    TransactionScopeOption.Required,
                    new TransactionOptions
                    {
                        IsolationLevel = IsolationLevel.ReadCommitted
                    },
                    TransactionScopeAsyncFlowOption.Enabled))
                {
                    while (_tasks.Count > 0)
                    {
                        ITaskUnit task = _tasks.Dequeue();

                        executedTasks.Enqueue(task);

                        Transaction.Current.EnlistVolatile(
                            task,
                            EnlistmentOptions.None);
                    }

                    await DataContext.SaveChangesAsync(cancellationToken);

                    transaction.Complete();
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                CollectExceptions(executedTasks);
            }
        }


        private void CollectExceptions(
            IEnumerable<ITaskUnit> executedTasks)
        {
            _exceptions.Clear();

            foreach (ITaskUnit task in executedTasks)
            {
                if (task.Exception != null)
                {
                    _exceptions.Add(task.Exception);
                }
            }
        }


        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                if (_dataContext != null)
                {
                    _dataContext.Dispose();
                    _dataContext = null;
                }
            }

            _disposed = true;
        }


        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }


        public async ValueTask DisposeAsync()
        {
            if (_disposed)
                return;

            if (_dataContext != null)
            {
                await _dataContext.DisposeAsync();
                _dataContext = null;
            }

            _disposed = true;

            GC.SuppressFinalize(this);
        }


        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(
                    GetType().FullName);
            }
        }
    }
}