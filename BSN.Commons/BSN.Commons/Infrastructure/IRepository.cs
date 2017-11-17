using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Commons.Infrastructure
{
	public interface IRepository<T> where T : class
	{
		void Add(T entity);
		void AddRange(IEnumerable<T> entities);
		void Update(T entity);
		void UpdateRange(IEnumerable<T> entities);
		void Delete(T entity);
		void Delete(Expression<Func<T, bool>> where);
		void DeleteRange(IEnumerable<T> entities);
		T GetById(long id);
		T GetById(string id);
		T Get(Expression<Func<T, bool>> where);
		IEnumerable<T> GetAll();
		IEnumerable<T> GetMany(Expression<Func<T, bool>> where);
	}
}
