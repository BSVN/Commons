using BSN.Commons.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace BSN.Commons.Orm.EntityFrameworkCore
{

    /// <inheritdoc />
    public class RepositoryBase<T> : IRepository<T>, IAsyncRepository<T>
        where T : class
    {
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
        public virtual async Task AddAsync(
            T entity,
            CancellationToken cancellationToken = default)
        {
            await dbSet
                .AddAsync(entity, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public virtual void AddRange(IEnumerable<T> entities)
        {
            dbSet.AddRange(entities);
        }

        /// <inheritdoc />
        public virtual Task AddRangeAsync(
            IEnumerable<T> entities,
            CancellationToken cancellationToken = default)
        {
            return dbSet.AddRangeAsync(
                entities,
                cancellationToken);
        }

        /// <inheritdoc />
        public virtual void Delete(T entity)
        {
            dbSet.Remove(entity);
        }

        /// <inheritdoc />
        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            DeleteRange(
                dbSet.Where(where));
        }

        /// <inheritdoc />
        public virtual void DeleteRange(IEnumerable<T> entities)
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
        public virtual async Task<T> GetByIdAsync<KeyType>(
            KeyType id,
            CancellationToken cancellationToken = default)
        {
            return await dbSet
                .FindAsync(id,
                    cancellationToken)
                .ConfigureAwait(false);
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
            CancellationToken cancellationToken = default)
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
            CancellationToken cancellationToken = default)
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
            CancellationToken cancellationToken = default)
        {
            IQueryable<T> query = dbSet.Where(where);

            if (asNoTracking)
                query = query.AsNoTracking();

            return await query
                .FirstOrDefaultAsync(cancellationToken);
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

            // TODO: Why this behaviour exist?
            if (updateConfig.AutoDetectChangedPropertiesEnabled)
            {
                _dataContext.ChangeTracker.AutoDetectChangesEnabled = true;
                return;
            }

            bool previous =
                _dataContext.ChangeTracker.AutoDetectChangesEnabled;

            try
            {
                _dataContext.ChangeTracker.AutoDetectChangesEnabled = true;

                dbSet.Attach(entity);

                if (updateConfig.IncludeAllPropertiesEnabled)
                {
                    _dataContext.Entry(entity).State =
                        EntityState.Modified;
                }
                else
                {
                    foreach (var propertyName in updateConfig.PropertyNames)
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
                _dataContext.ChangeTracker.AutoDetectChangesEnabled =
                    previous;
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
                _dataContext.ChangeTracker.AutoDetectChangesEnabled = true;
                return;
            }

            bool previous =
                _dataContext.ChangeTracker.AutoDetectChangesEnabled;

            try
            {
                _dataContext.ChangeTracker.AutoDetectChangesEnabled = false;

                foreach (var entity in entities)
                {
                    dbSet.Attach(entity);

                    if (updateConfig.IncludeAllPropertiesEnabled)
                    {
                        _dataContext.Entry(entity).State =
                            EntityState.Modified;
                    }
                    else
                    {
                        foreach (var propertyName in updateConfig.PropertyNames)
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
                _dataContext.ChangeTracker.AutoDetectChangesEnabled =
                    previous;
            }
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