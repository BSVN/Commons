using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using BSN.Commons.Infrastructure;

namespace Commons.Infrastructure
{
	public abstract partial class RepositoryBase<T> where T : class
	{
		private DbContext _dataContext;
		protected readonly DbSet<T> DbSet;

		protected DbContext DataContext => _dataContext ?? (_dataContext = DatabaseFactory.Get());
		protected IDatabaseFactory DatabaseFactory { get; private set; }


		protected RepositoryBase(IDatabaseFactory databaseFactory)
		{
			DatabaseFactory = databaseFactory;
			DbSet = DataContext.Set<T>();
		}

		public virtual void Add(T entity)
		{
			DbSet.Add(entity);
		}

		public virtual void AddRange(IEnumerable<T> entities)
		{
			DbSet.AddRange(entities);
		}

		public virtual void Update(T entity)
		{
			DbSet.Attach(entity);
		}

		public virtual void UpdateRange(IEnumerable<T> entities)
		{
			_dataContext.Configuration.AutoDetectChangesEnabled = false;

			foreach (T entity in entities)
				UpdateWholeEntity(entity);

			_dataContext.Configuration.AutoDetectChangesEnabled = true;
		}

		public virtual IRepositoryUpdateFluentInterface<T> BeginUpdate(T entity)
		{
			return new RepositoryBaseSingleUpdateFluentInterface<T>(this, entity);
		}

		public virtual IRepositoryUpdateFluentInterface<T> BeginUpdateRange(IEnumerable<T> entities)
		{
			return new RepositoryBaseUpdateRangeFluentInterface<T>(this, entities);
		}

		protected virtual void UpdateWholeEntity(T entity)
		{
			DbSet.Attach(entity);
			_dataContext.Entry(entity).State = EntityState.Modified;
		}

		public virtual void Delete(T entity)
		{
			DbSet.Remove(entity);
		}

		public virtual void Delete(Expression<Func<T, bool>> where)
		{
			var objects = DbSet.Where(where).AsEnumerable();
			foreach (var obj in objects)
				DbSet.Remove(obj);
		}

		public virtual void DeleteRange(IEnumerable<T> entities)
		{
			DbSet.RemoveRange(entities);
		}

		public virtual T GetById(long id)
		{
			return DbSet.Find(id);
		}

		public virtual T GetById(string id)
		{
			return DbSet.Find(id);
		}

		public virtual IEnumerable<T> GetAll()
		{
			return DbSet.ToList();
		}

		public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
		{
			return DbSet.Where(where);
		}

		public T Get(Expression<Func<T, bool>> where)
		{
			return DbSet.Where(where).FirstOrDefault();
		}
	}
}