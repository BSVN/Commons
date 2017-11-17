using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Commons.Infrastructure
{
	public abstract class RepositoryBase<T> where T : class
	{
		private DbContext _dataContext;
		protected readonly DbSet<T> dbSet;


		protected DbContext DataContext { get { return _dataContext ?? (_dataContext = DatabaseFactory.Get()); } }
		protected IDatabaseFactory DatabaseFactory { get; private set; }


		protected RepositoryBase(IDatabaseFactory databaseFactory)
		{
			DatabaseFactory = databaseFactory;
			dbSet = DataContext.Set<T>();
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
			dbSet.Attach(entity);
			_dataContext.Entry(entity).State = EntityState.Modified;
		}

		public virtual void UpdateRange(IEnumerable<T> entities)
		{
			foreach (T entity in entities)
				Update(entity);
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
}