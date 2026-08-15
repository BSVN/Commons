using BSN.Commons.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace BSN.Commons.Orm.EntityFramework
{
    /// <inheritdoc />
    public abstract partial class RepositoryBase<T> : IRepository<T>
        where T : class
    {
        /// <inheritdoc />
        protected RepositoryBase(IDatabaseFactory databaseFactory)
        {
            if (databaseFactory == null)
                throw new ArgumentNullException(nameof(databaseFactory));

            DatabaseFactory = databaseFactory;
            dbSet = DataContext.Set<T>();
        }


        /// <inheritdoc />
        public virtual void Add(T entity)
        {
            dbSet.Add(entity);
        }


        /// <inheritdoc />
        public virtual Task AddAsync(
            T entity,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            dbSet.Add(entity);

            return Task.CompletedTask;
        }


        /// <inheritdoc />
        public virtual void AddRange(IEnumerable<T> entities)
        {
            dbSet.AddRange(entities);
        }


        /// <inheritdoc />
        public virtual Task AddRangeAsync(
            IEnumerable<T> entities,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            dbSet.AddRange(entities);

            return Task.CompletedTask;
        }


        /// <inheritdoc />
        public virtual void Update(T entity)
        {
            Update(entity, cfg => cfg.IncludeAllProperties());
        }


        /// <inheritdoc />
        public virtual void Update(
            T entity,
            Action<IUpdateConfig<T>> configurer)
        {
            var updateConfig = new UpdateConfig<T>();

            configurer(updateConfig);

            if (updateConfig.AutoDetectChangedPropertiesEnabled)
            {
                _dataContext.Configuration.AutoDetectChangesEnabled = true;
                return;
            }

            bool previousValue =
                _dataContext.Configuration.AutoDetectChangesEnabled;

            try
            {
                _dataContext.Configuration.AutoDetectChangesEnabled = false;

                dbSet.Attach(entity);

                if (updateConfig.IncludeAllPropertiesEnabled)
                {
                    _dataContext.Entry(entity).State =
                        EntityState.Modified;
                }
                else
                {
                    foreach (string propertyName in updateConfig.PropertyNames)
                    {
                        _dataContext
                            .Entry(entity)
                            .Property(propertyName)
                            .IsModified = true;
                    }
                }
            }
            finally
            {
                _dataContext.Configuration.AutoDetectChangesEnabled =
                    previousValue;
            }
        }


        /// <inheritdoc />
        public virtual void UpdateRange(IEnumerable<T> entities)
        {
            UpdateRange(
                entities,
                cfg => cfg.IncludeAllProperties());
        }


        /// <inheritdoc />
        public virtual void UpdateRange(
            IEnumerable<T> entities,
            Action<IUpdateConfig<T>> configurer)
        {
            var updateConfig = new UpdateConfig<T>();

            configurer(updateConfig);

            if (updateConfig.AutoDetectChangedPropertiesEnabled)
            {
                _dataContext.Configuration.AutoDetectChangesEnabled = true;
                return;
            }

            bool previousValue =
                _dataContext.Configuration.AutoDetectChangesEnabled;

            try
            {
                _dataContext.Configuration.AutoDetectChangesEnabled = false;

                foreach (T entity in entities)
                {
                    dbSet.Attach(entity);

                    if (updateConfig.IncludeAllPropertiesEnabled)
                    {
                        _dataContext.Entry(entity).State =
                            EntityState.Modified;
                    }
                    else
                    {
                        foreach (string propertyName in updateConfig.PropertyNames)
                        {
                            _dataContext
                                .Entry(entity)
                                .Property(propertyName)
                                .IsModified = true;
                        }
                    }
                }
            }
            finally
            {
                _dataContext.Configuration.AutoDetectChangesEnabled =
                    previousValue;
            }
        }


        /// <inheritdoc />
        public virtual void Delete(T entity)
        {
            dbSet.Remove(entity);
        }


        /// <inheritdoc />
        public virtual void Delete(
            Expression<Func<T, bool>> where)
        {
            var objects = dbSet.Where(where);

            DeleteRange(objects);
        }


        /// <inheritdoc />
        public virtual void DeleteRange(
            IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }


        /// <inheritdoc />
        public virtual T GetById<KeyType>(
            KeyType id)
        {
            return dbSet.Find(id);
        }


        /// <inheritdoc />
        public virtual Task<T> GetByIdAsync<KeyType>(
            KeyType id,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            return dbSet.FindAsync(
                cancellationToken,
                id);
        }


        /// <inheritdoc />
        public virtual IEnumerable<T> GetAll(
            bool asNoTracking = false)
        {
            IQueryable<T> query = dbSet;

            if (asNoTracking)
                query = query.AsNoTracking();

            return query.ToList();
        }


        /// <inheritdoc />
        public virtual async Task<IEnumerable<T>> GetAllAsync(
            bool asNoTracking = false,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            IQueryable<T> query = dbSet;

            if (asNoTracking)
                query = query.AsNoTracking();

            return await query
                .ToListAsync(cancellationToken);
        }


        /// <inheritdoc />
        public virtual IEnumerable<T> GetMany(
            Expression<Func<T, bool>> where,
            bool asNoTracking = false)
        {
            IQueryable<T> query = dbSet.Where(where);

            if (asNoTracking)
                query = query.AsNoTracking();

            return query;
        }


        /// <inheritdoc />
        public virtual async Task<IEnumerable<T>> GetManyAsync(
            Expression<Func<T, bool>> where,
            bool asNoTracking = false,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            IQueryable<T> query = dbSet.Where(where);

            if (asNoTracking)
                query = query.AsNoTracking();

            return await query
                .ToListAsync(cancellationToken);
        }


        /// <inheritdoc />
        public virtual T Get(
            Expression<Func<T, bool>> where,
            bool asNoTracking = false)
        {
            IQueryable<T> query = dbSet.Where(where);

            if (asNoTracking)
                query = query.AsNoTracking();

            return query.FirstOrDefault();
        }


        /// <inheritdoc />
        public virtual async Task<T> GetAsync(
            Expression<Func<T, bool>> where,
            bool asNoTracking = false,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            IQueryable<T> query = dbSet.Where(where);

            if (asNoTracking)
                query = query.AsNoTracking();

            return await query
                .FirstOrDefaultAsync(cancellationToken);
        }

        /// <summary>
        /// Database Set
        /// </summary>
        protected readonly DbSet<T> dbSet;

        /// <summary>
        /// Database Context
        /// </summary>
        protected DbContext DataContext => _dataContext ?? (_dataContext = (DbContext)DatabaseFactory.Get());

        /// <summary>
        /// Database Factory
        /// </summary>
        protected IDatabaseFactory DatabaseFactory { get; private set; }

        private DbContext _dataContext;
    }
}