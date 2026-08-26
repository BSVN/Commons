using BSN.Commons.Infrastructure;
using Redis.OM;
using Redis.OM.Contracts;
using Redis.OM.Searching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace BSN.Commons.Orm.Redis
{
    /// <summary>
    /// Repository Base for Redis Implementation
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RepositoryBase<T> : IRepository<T>, IAsyncRepository<T>
        where T : class
    {
        /// <summary>
        /// Constructor for Redis Repository Base
        /// </summary>
        /// <param name="databaseFactory">Database Factory Containing an IRedisDbContext</param>
        protected RepositoryBase(IDatabaseFactory databaseFactory)
        {
            if (databaseFactory == null)
                throw new ArgumentNullException(nameof(databaseFactory));

            DatabaseFactory = databaseFactory;

            dbCollection = DataContext.RedisCollection<T>();

            DataContext.Connection.CreateIndex(typeof(T));
        }

        /// <inheritdoc />
        public virtual void Add(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            dbCollection.Insert(entity);
        }

        /// <inheritdoc />
        public virtual Task AddAsync(
            T entity,
            CancellationToken cancellationToken = default)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            cancellationToken.ThrowIfCancellationRequested();

            return dbCollection.InsertAsync(entity);
        }


        /// <inheritdoc />
        public virtual void AddRange(
            IEnumerable<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            foreach (var entity in entities)
            {
                Add(entity);
            }
        }


        /// <inheritdoc />
        public virtual Task AddRangeAsync(
            IEnumerable<T> entities,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return dbCollection.InsertAsync(entities);
        }


        /// <inheritdoc />
        public virtual void Update(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            dbCollection.Update(entity);
        }


        /// <inheritdoc />
        public virtual void Update(
            T entity,
            Action<IUpdateConfig<T>> configurer)
        {
            throw new NotSupportedException(
                "Redis repository does not support partial update configuration.");
        }


        /// <inheritdoc />
        public virtual void UpdateRange(
            IEnumerable<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            foreach (var entity in entities)
            {
                Update(entity);
            }
        }


        /// <inheritdoc />
        public virtual void UpdateRange(
            IEnumerable<T> entities,
            Action<IUpdateConfig<T>> configurer)
        {
            throw new NotSupportedException(
                "Redis repository does not support partial update configuration.");
        }


        /// <inheritdoc />
        public virtual void Delete(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            dbCollection.Delete(entity);
        }


        /// <inheritdoc />
        public virtual void Delete(
            Expression<Func<T, bool>> where)
        {
            if (where == null)
                throw new ArgumentNullException(nameof(where));

            DeleteRange(
                dbCollection.Where(where));
        }


        /// <inheritdoc />
        public virtual void DeleteRange(
            IEnumerable<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            dbCollection.Delete(entities);
        }


        /// <inheritdoc />
        public virtual T GetById<KeyType>(
            KeyType id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));


            if (id is string)
            {
                var entity =
                    dbCollection.FindById(id.ToString());

                if (entity == null)
                {
                    throw new KeyNotFoundException(
                        $"Entity with key {id} was not found.");
                }

                return entity;
            }


            throw new NotSupportedException(
                $"Redis repository does not support key type {typeof(KeyType)}.");
        }


        /// <inheritdoc />
        public virtual async Task<T> GetByIdAsync<KeyType>(
            KeyType id,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            if (id is string)
            {
                var entity = await dbCollection.FindByIdAsync(id.ToString());

                if (entity == null)
                {
                    throw new KeyNotFoundException(
                        $"Entity with key {id} was not found.");
                }

                return entity;
            }

            throw new NotSupportedException(
                $"Redis repository does not support key type {typeof(KeyType)}.");

        }

        public virtual T Get(
            Expression<Func<T, bool>> where,
            bool asNoTracking = false)
        {
            if (where == null)
                throw new ArgumentNullException(nameof(where));

            return dbCollection
                .FirstOrDefault(where);
        }

        /// <inheritdoc />
        public virtual Task<T> GetAsync(
            Expression<Func<T, bool>> where,
            bool asNoTracking = false,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (where == null)
                throw new ArgumentNullException(nameof(where));

            return dbCollection
                .FirstOrDefaultAsync(where);
        }


        /// <inheritdoc />
        public virtual IEnumerable<T> GetAll(
            bool asNoTracking = false)
        {
            return dbCollection
                .Where(entity => true)
                .AsEnumerable();
        }


        /// <inheritdoc />
        public virtual async Task<IEnumerable<T>> GetAllAsync(
            bool asNoTracking = false,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var result = await dbCollection
                 .Where(entity => true)
                 .ToListAsync();

            return result.AsEnumerable();
        }


        /// <inheritdoc />
        public virtual IEnumerable<T> GetMany(
            Expression<Func<T, bool>> where,
            bool asNoTracking = false)
        {
            if (where == null)
                throw new ArgumentNullException(nameof(where));

            return dbCollection.Where(where);
        }


        /// <inheritdoc />
        public virtual async Task<IEnumerable<T>> GetManyAsync(
            Expression<Func<T, bool>> where,
            bool asNoTracking = false,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (where == null)
                throw new ArgumentNullException(nameof(where));

            var result = await dbCollection
                .Where(where)
                .ToListAsync();

            return result.AsEnumerable();
        }

        protected readonly IRedisCollection<T> dbCollection;

        protected IDatabaseFactory DatabaseFactory { get; private set; }

        protected IRedisConnectionProvider DataContext => _dataContext ?? (_dataContext = (IRedisConnectionProvider)DatabaseFactory.Get());

        private IRedisConnectionProvider _dataContext;
    }
}