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
    public class RepositoryBase<T> : IRepository<T>
        where T : class
    {
        protected readonly DbSet<T> dbSet;

        protected DbContext _dataContext;


        protected RepositoryBase(IDatabaseFactory databaseFactory)
        {
            if (databaseFactory == null)
                throw new ArgumentNullException(nameof(databaseFactory));

            DatabaseFactory = databaseFactory;
            dbSet = DataContext.Set<T>();
        }


        protected DbContext DataContext
        {
            get
            {
                if (_dataContext == null)
                    _dataContext = (DbContext)DatabaseFactory.Get();

                return _dataContext;
            }
        }


        protected IDatabaseFactory DatabaseFactory { get; private set; }


        public void Add(T entity)
        {
            dbSet.Add(entity);
        }


        public async Task AddAsync(
            T entity,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            await dbSet
                .AddAsync(entity, cancellationToken)
                .ConfigureAwait(false);
        }


        public void AddRange(IEnumerable<T> entities)
        {
            dbSet.AddRange(entities);
        }


        public Task AddRangeAsync(
            IEnumerable<T> entities,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return dbSet.AddRangeAsync(
                entities,
                cancellationToken);
        }


        public void Delete(T entity)
        {
            dbSet.Remove(entity);
        }


        public void Delete(Expression<Func<T, bool>> where)
        {
            dbSet.RemoveRange(
                dbSet.Where(where));
        }


        public void DeleteRange(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }


        public virtual T GetById<KeyType>(
            KeyType id)
        {
            return dbSet.Find(id);
        }


        public virtual async Task<T> GetByIdAsync<KeyType>(
            KeyType id,
            CancellationToken cancellationToken = default(CancellationToken))
        {
                return await dbSet
                    .FindAsync(id ,
                        cancellationToken)
                    .ConfigureAwait(false);
        }


        public virtual IEnumerable<T> GetAll(
            bool asNoTracking = false)
        {
            IQueryable<T> query = dbSet;

            if (asNoTracking)
                query = query.AsNoTracking();

            return query.ToList();
        }


        public virtual async Task<IEnumerable<T>> GetAllAsync(
            bool asNoTracking = false,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            IQueryable<T> query = dbSet;

            if (asNoTracking)
                query = query.AsNoTracking();

            return await query
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }


        public virtual IEnumerable<T> GetMany(
            Expression<Func<T, bool>> where,
            bool asNoTracking = false)
        {
            IQueryable<T> query = dbSet.Where(where);

            if (asNoTracking)
                query = query.AsNoTracking();

            return query;
        }


        public virtual async Task<IEnumerable<T>> GetManyAsync(
            Expression<Func<T, bool>> where,
            bool asNoTracking = false,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            IQueryable<T> query = dbSet.Where(where);

            if (asNoTracking)
                query = query.AsNoTracking();

            return await query
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }


        public T Get(
            Expression<Func<T, bool>> where,
            bool asNoTracking = false)
        {
            IQueryable<T> query = dbSet.Where(where);

            if (asNoTracking)
                query = query.AsNoTracking();

            return query.FirstOrDefault();
        }


        public async Task<T> GetAsync(
            Expression<Func<T, bool>> where,
            bool asNoTracking = false,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            IQueryable<T> query = dbSet.Where(where);

            if (asNoTracking)
                query = query.AsNoTracking();

            return await query
                .FirstOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);
        }


        public void Update(T entity)
        {
            Update(entity, cfg => cfg.IncludeAllProperties());
        }


        public void Update(
            T entity,
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


        public void UpdateRange(IEnumerable<T> entities)
        {
            UpdateRange(
                entities,
                cfg => cfg.IncludeAllProperties());
        }


        public void UpdateRange(
            IEnumerable<T> entities,
            Action<IUpdateConfig<T>> configurer)
        {
            var updateConfig = new UpdateConfig<T>();

            configurer(updateConfig);

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
    }
}