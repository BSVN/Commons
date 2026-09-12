using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace BSN.Commons.Infrastructure
{
    /// <summary>
    /// Represents a unit of work that coordinates database operations
    /// and transaction-aware task units.
    /// </summary>
    public class UnitOfWork : Disposable, IUnitOfWork, IAsyncUnitOfWork
    {
        private readonly ConcurrentQueue<ITaskUnit> _tasks;
        private readonly List<Exception> _exceptions;

        private readonly SemaphoreSlim _operationLock;
        private readonly object _exceptionsLock;

        private IDbContext _dataContext;
        private IAsyncDbContext _asyncDataContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
        /// </summary>
        /// <param name="databaseFactory">
        /// The database factory that owns the database context.
        /// </param>
        public UnitOfWork(IDatabaseFactory databaseFactory)
        {
            DatabaseFactory = databaseFactory
                ?? throw new ArgumentNullException(nameof(databaseFactory));

            _tasks = new ConcurrentQueue<ITaskUnit>();
            _exceptions = new List<Exception>();

            _operationLock = new SemaphoreSlim(1, 1);
            _exceptionsLock = new object();
        }

        /// <summary>
        /// Gets the database factory.
        /// </summary>
        public IDatabaseFactory DatabaseFactory { get; }

        /// <summary>
        /// Gets the exceptions raised by executed task units.
        /// </summary>
        public IReadOnlyCollection<Exception> Exceptions
        {
            get
            {
                lock (_exceptionsLock)
                {
                    return _exceptions.ToArray();
                }
            }
        }

        /// <summary>
        /// Gets the database context.
        /// </summary>
        protected IDbContext DataContext
        {
            get
            {
                ThrowIfDisposed();

                return _dataContext = _dataContext ?? DatabaseFactory.Get();
            }
        }

        /// <summary>
        /// Gets the database context for asynchronous operations.
        /// </summary>
        protected IAsyncDbContext AsyncDataContext
        {
            get
            {
                ThrowIfDisposed();

                return _asyncDataContext = _asyncDataContext ?? DatabaseFactory.GetAsyncContext();
            }
        }

        /// <inheritdoc />
        public void AddToQueue(ITaskUnit task)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));

            _operationLock.Wait();

            try
            {
                ThrowIfDisposed();

                _tasks.Enqueue(task);
            }
            finally
            {
                _operationLock.Release();
            }
        }

        /// <inheritdoc />
        public void Commit()
        {
            _operationLock.Wait();

            try
            {
                ThrowIfDisposed();

                IEnumerable<ITaskUnit> executedTasks = Enumerable.Empty<ITaskUnit>();

                try
                {
                    using (var transaction = new TransactionScope())
                    {
                        executedTasks = DequeueTasks();

                        DataContext.SaveChanges();

                        transaction.Complete();
                    }
                }
                finally
                {
                    CollectExceptions(executedTasks);
                }
            }
            finally
            {
                _operationLock.Release();
            }
        }

        /// <inheritdoc />
        public async Task CommitAsync(
            CancellationToken cancellationToken = default)
        {
            await _operationLock.WaitAsync(cancellationToken);

            try
            {
                ThrowIfDisposed();

                IEnumerable<ITaskUnit> executedTasks = Enumerable.Empty<ITaskUnit>();
                try
                {
                    using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {
                        executedTasks = DequeueTasks();

                        await AsyncDataContext.SaveChangesAsync(
                            cancellationToken);

                        transaction.Complete();
                    }
                }
                finally
                {
                    CollectExceptions(executedTasks);
                }
            }
            finally
            {
                _operationLock.Release();
            }
        }


        /// <summary>
        /// Dequeues all task units that belong to the current commit.
        /// </summary>
        private List<ITaskUnit> DequeueTasks()
        {
            var tasks = new List<ITaskUnit>();

            while (_tasks.TryDequeue(out var task))
            {
                Transaction.Current.EnlistVolatile(
                    task,
                    EnlistmentOptions.None);
                tasks.Add(task);
            }

            return tasks;
        }

        /// <summary>
        /// Collects exceptions raised by executed task units.
        /// </summary>
        private void CollectExceptions(
            IEnumerable<ITaskUnit> executedTasks)
        {
            lock (_exceptionsLock)
            {
                _exceptions.Clear();

                foreach (var task in executedTasks)
                {
                    if (task.Exception != null)
                    {
                        _exceptions.Add(task.Exception);
                    }
                }
            }
        }

        /// <summary>
        /// Throws an exception if the unit of work has been disposed.
        /// </summary>
        private void ThrowIfDisposed()
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException(
                    GetType().FullName);
            }
        }

        /// <summary>
        /// Releases resources used by the unit of work.
        /// </summary>
        protected override void DisposeCore()
        {
            /*
             * DatabaseFactory owns the database context.
             * UnitOfWork must never dispose it.
             *
             * Waiting here guarantees that DisposeCore does not release
             * UnitOfWork resources while AddToQueue/Commit is executing.
             */
            _operationLock.Wait();

            try
            {
                _dataContext = null;
                _asyncDataContext = null;

                while (_tasks.TryDequeue(out _))
                {
                }

                lock (_exceptionsLock)
                {
                    _exceptions.Clear();
                }
            }
            finally
            {
                _operationLock.Release();
                _operationLock.Dispose();
            }
        }
    }
}