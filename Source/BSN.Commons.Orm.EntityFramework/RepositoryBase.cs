using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Commons.Orm.EntityFramework
{
    using Commons.Infrastructure;

    public abstract partial class RepositoryBase<T> : IRepository<T> where T : class
    {
        private DbContext _dataContext;
        private readonly IUnitOfWork _unitOfWork;
        protected readonly DbSet<T> dbSet;

        protected DbContext DataContext => _dataContext ?? (_dataContext = (DbContext)DatabaseFactory.Get());
        protected IDatabaseFactory DatabaseFactory { get; private set; }


        protected RepositoryBase(IDatabaseFactory databaseFactory)
        {
            DatabaseFactory = databaseFactory;
            dbSet = DataContext.Set<T>();
        }

        protected RepositoryBase(IDatabaseFactory databaseFactory, IUnitOfWork unitOfWork) : this(databaseFactory)
        {
            _unitOfWork = unitOfWork;
        }

        public virtual void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public virtual void AddRange(IEnumerable<T> entities)
        {
            dbSet.AddRange(entities);
        }

        public virtual void Update(T entity)
        {
            Update(entity, cfg => cfg.IncludeAllProperties());
        }

        public virtual void Update(T entity, Action<IUpdateConfig<T>> configurer)
        {
            var updateConfig = new UpdateConfig();
            configurer.Invoke(updateConfig);

            if (updateConfig.AutoDetectChangedPropertiesEnabled)
            {
                _dataContext.Configuration.AutoDetectChangesEnabled = true;
                return;
            }

            bool autoDetectChangesPreviousValue = _dataContext.Configuration.AutoDetectChangesEnabled;

            try
            {
                _dataContext.Configuration.AutoDetectChangesEnabled = false;

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
                _dataContext.Configuration.AutoDetectChangesEnabled = autoDetectChangesPreviousValue;
            }
        }

        public virtual void UpdateRange(IEnumerable<T> entities)
        {
            UpdateRange(entities, cfg => cfg.IncludeAllProperties());
        }

        public virtual void UpdateRange(IEnumerable<T> entities, Action<IUpdateConfig<T>> configurer)
        {
            var updateConfig = new UpdateConfig();
            configurer.Invoke(updateConfig);

            if (updateConfig.AutoDetectChangedPropertiesEnabled)
            {
                _dataContext.Configuration.AutoDetectChangesEnabled = true;
                return;
            }

            bool autoDetectChangesPreviousValue = _dataContext.Configuration.AutoDetectChangesEnabled;

            try
            {
                _dataContext.Configuration.AutoDetectChangesEnabled = false;

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
                _dataContext.Configuration.AutoDetectChangesEnabled = autoDetectChangesPreviousValue;
            }
        }

        public virtual void Delete(T entity)
        {
            dbSet.Remove(entity);
        }

        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            var objects = dbSet.Where(where).AsEnumerable();
            foreach (var obj in objects)
                dbSet.Remove(obj);
        }

        public virtual void DeleteRange(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }

        public virtual T GetById(long id)
        {
            return dbSet.Find(id);
        }

        public virtual T GetById(string id)
        {
            return dbSet.Find(id);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return dbSet.ToList();
        }

        public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return dbSet.Where(where);
        }

        public T Get(Expression<Func<T, bool>> where)
        {
            return dbSet.Where(where).FirstOrDefault();
        }
    }

	public abstract partial class RepositoryBase<T> where T : class
	{
		private class UpdateConfig : IUpdateConfig<T> 
		{
			public bool IncludeAllPropertiesEnabled
			{
				get
				{
					return _includeAllPropertiesEnabled;
				}
				private set
				{
					_includeAllPropertiesEnabled = value;
					if (value == true)
						_autoDetectChangedPropertiesEnabled = false;
				}
			}

			public bool AutoDetectChangedPropertiesEnabled
			{
				get
				{
					return _autoDetectChangedPropertiesEnabled;
				}
				private set
				{
					_autoDetectChangedPropertiesEnabled = value;
					if (value == true)
						_includeAllPropertiesEnabled = false;
				}
			}

			public IList<string> PropertyNames { get; }

			public UpdateConfig()
			{
				PropertyNames = new List<string>();
			}

			public void IncludeAllProperties()
			{
				IncludeAllPropertiesEnabled = true;
			}

			public void AutoDetectChangedProperties()
			{
				AutoDetectChangedPropertiesEnabled = true;
			}

			public IUpdateConfig<T> IncludeProperty<TProperty>(Expression<Func<T, TProperty>> propertyAccessExpression)
			{
				PropertyNames.Add(
					(propertyAccessExpression.Body as MemberExpression)?.Member.Name ??
						throw new ArgumentException(nameof(propertyAccessExpression))
				);

				AutoDetectChangedPropertiesEnabled = false;
				IncludeAllPropertiesEnabled = false;
				return this;
			}

			#region Fields

			private bool _includeAllPropertiesEnabled;

			private bool _autoDetectChangedPropertiesEnabled;

			#endregion
		}
    }

}