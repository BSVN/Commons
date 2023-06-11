using BSN.Commons.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BSN.Commons.Orm.EntityFrameworkCore
{
    /// <inheritdoc />
    public class RepositoryBase<T> : IRepository<T> where T : class
    {
        /// <inheritdoc />
        protected RepositoryBase(IDatabaseFactory databaseFactory)
        {
            DatabaseFactory = databaseFactory;
            dbSet = DataContext.Set<T>();
        }

        /// <inheritdoc />
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        /// <inheritdoc />
        public void AddRange(IEnumerable<T> entities)
        {
            dbSet.AddRange(entities);
        }

        /// <inheritdoc />
        public void Delete(T entity)
        {
            dbSet.Remove(entity);
        }

        /// <inheritdoc />
        public void Delete(Expression<Func<T, bool>> where)
        {
            dbSet.RemoveRange(dbSet.Where(where));
        }

        /// <inheritdoc />
        public void DeleteRange(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }

        /// <inheritdoc />
        public virtual T GetById<KeyType>(KeyType id) 
        {
            return dbSet.Find(id);
        }

        /// <inheritdoc />
        public virtual IEnumerable<T> GetAll()
        {
            return dbSet.ToList();
        }

        /// <inheritdoc />
        public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return dbSet.Where(where);
        }

        /// <inheritdoc />
        public T Get(Expression<Func<T, bool>> where)
        {
            return dbSet.Where(where).FirstOrDefault();
        }

        /// <inheritdoc />
        public void Update(T entity)
        {
            Update(entity, cfg => cfg.IncludeAllProperties());
        }

        /// <inheritdoc />
        public void Update(T entity, Action<IUpdateConfig<T>> configurer)
        {
            var updateConfig = new UpdateConfig<T>();
            configurer.Invoke(updateConfig);

            // TODO: Why this behaviour exist?
            if (updateConfig.AutoDetectChangedPropertiesEnabled)
            {
                _dataContext.ChangeTracker.AutoDetectChangesEnabled = true;
                return;
            }

            bool autoDetectChangesPreviousValue = _dataContext.ChangeTracker.AutoDetectChangesEnabled;

            try
            {
                _dataContext.ChangeTracker.AutoDetectChangesEnabled = true;

                dbSet.Attach(entity);

                if (updateConfig.IncludeAllPropertiesEnabled)
                {
                    _dataContext.Entry(entity).State = EntityState.Modified;
                }
                else
                {
                    foreach (string propertyName in updateConfig.PropertyNames)
                        _dataContext.Entry(entity).Property(propertyName).IsModified = true;
                }
            }
            finally
            {
                _dataContext.ChangeTracker.AutoDetectChangesEnabled = autoDetectChangesPreviousValue;
            }
        }

        /// <inheritdoc />
        public void UpdateRange(IEnumerable<T> entities)
        {
            UpdateRange(entities, cfg => cfg.IncludeAllProperties());
        }

        /// <inheritdoc />
        public void UpdateRange(IEnumerable<T> entities, Action<IUpdateConfig<T>> configurer)
        {
            var updateConfig = new UpdateConfig<T>();
            configurer.Invoke(updateConfig);

            if (updateConfig.AutoDetectChangedPropertiesEnabled)
            {
                _dataContext.ChangeTracker.AutoDetectChangesEnabled = true;
                return;
            }

            bool autoDetectChangesPreviousValue = _dataContext.ChangeTracker.AutoDetectChangesEnabled;

            try
            {
                _dataContext.ChangeTracker.AutoDetectChangesEnabled = false;

                if (updateConfig.IncludeAllPropertiesEnabled)
                {
                    foreach (T entity in entities)
                    {
                        dbSet.Attach(entity);
                        _dataContext.Entry(entity).State = EntityState.Modified;
                    }
                }
                else
                {
                    foreach (T entity in entities)
                    {
                        dbSet.Attach(entity);
                        foreach (string propertyName in updateConfig.PropertyNames)
                            _dataContext.Entry(entity).Property(propertyName).IsModified = true;
                    }
                }
            }
            finally
            {
                _dataContext.ChangeTracker.AutoDetectChangesEnabled = autoDetectChangesPreviousValue;
            }
        }

        /// <summary>
        /// TODO: complete doc
        /// </summary>
        protected readonly DbSet<T> dbSet;

        /// <summary>
        /// TODO: complete doc
        /// </summary>
        protected DbContext DataContext => _dataContext ?? (_dataContext = (DbContext)DatabaseFactory.Get());

        /// <summary>
        /// TODO: complete doc
        /// </summary>
        protected IDatabaseFactory DatabaseFactory { get; private set; }

        private DbContext _dataContext;
    }
}
