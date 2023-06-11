using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BSN.Commons.Infrastructure
{
    /// <summary>
    /// Repository Pattern Interface for abstract communicating with DataBase
    /// </summary>
    /// <typeparam name="T"></typeparam>
	public interface IRepository<T> where T : class
	{
        /// <summary>
        /// Add new object to repository.
        /// </summary>
        /// <param name="entity">New Object</param>
		void Add(T entity);

        /// <summary>
        /// Add a range of objects to the repository.
        /// </summary>
        /// <param name="entities">New Objects</param>
		void AddRange(IEnumerable<T> entities);

        /// <summary>
        /// Submit the update of an existing object in the repository.
        /// </summary>
        /// <param name="entity">Updated Object</param>
        void Update(T entity);

        /// <summary>
        /// Submit the update of an existing object in the repository using specific Configuration.
        /// </summary>
        /// <param name="entity">Updated Object</param>
        /// <param name="configurer">Configuration Object</param>
        void Update(T entity, Action<IUpdateConfig<T>> configurer);

        /// <summary>
        /// Submit the update of a range of existing objects in the repository.
        /// </summary>
        /// <param name="entities"></param>
        void UpdateRange(IEnumerable<T> entities);

        /// <summary>
        /// Submit the update of a range of existing objects in the repository using specific Configuration.
        /// </summary>
        /// <param name="entities">Updated Objects</param>
        /// <param name="configurer">Configuration Object</param>
		void UpdateRange(IEnumerable<T> entities, Action<IUpdateConfig<T>> configurer);

        /// <summary>
        /// Delete an existing object.
        /// </summary>
        /// <param name="entity">Object to Delete</param>
        void Delete(T entity);

        /// <summary>
        /// Delete an existing object using Expression.
        /// </summary>
        void Delete(Expression<Func<T, bool>> where);

        /// <summary>
        /// Delete a range of existing objects.
        /// </summary>
        /// <param name="entities">Objects to Delete</param>
        void DeleteRange(IEnumerable<T> entities);

        /// <summary>
        /// Get Object by id.
        /// </summary>
        /// <param name="id">Object Id</param>
        /// <returns>Retrived Object or null</returns>
        T GetById<KeyType>(KeyType id);

        /// <summary>
        /// Get Object by Expression.
        /// </summary>
        /// <param name="where">Expression</param>
        /// <returns>Retrived Object or null</returns>
        T Get(Expression<Func<T, bool>> where);

        /// <summary>
        /// Get all Objects in the current repository.
        /// </summary>
        /// <returns>List of all Objects</returns>
        IEnumerable<T> GetAll();

        /// <summary>
        /// Get List of existing objects using Expression.
        /// </summary>
        /// <param name="where">Expression</param>
        /// <returns>List of Objects</returns>
		IEnumerable<T> GetMany(Expression<Func<T, bool>> where);
	}
}
