﻿using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;

using Redis.OM;
using Redis.OM.Contracts;
using Redis.OM.Searching;

using BSN.Commons.Infrastructure;
using BSN.Commons.Infrastructure.Redis;
using System.Data.Common;

namespace BSN.Commons.Orm.Redis
{
    /// <summary>
    /// Repository Base for Redis Implementation
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RepositoryBase<T> : IRepository<T> where T : class
    {
        /// <summary>
        /// Constructor for Redis Repository Base
        /// </summary>
        /// <param name="databaseFactory">Database Factory Containing an IRedisDbContext</param>
        protected RepositoryBase(IDatabaseFactory databaseFactory)
        {
            DatabaseFactory = databaseFactory;
            dbCollection = DataContext.RedisCollection<T>();

            // TODO: Check that IndexCreationService is necessary or not.
            DataContext.Connection.CreateIndex(typeof(T));
        }

        /// <inheritdoc />
        public void Add(T entity)
        {
            dbCollection.Insert(entity);
        }

        /// <inheritdoc />
        public void AddRange(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                Add(entity);
            }
        }

        /// <inheritdoc />
        public void Update(T entity)
        {
            dbCollection.Update(entity);
        }

        /// <inheritdoc />
        public void Update(T entity, Action<IUpdateConfig<T>> configurer)
        {
            throw new NotImplementedException("We don't have a way to update with a configuration on redis");
        }

        /// <inheritdoc />
        public void UpdateRange(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                Update(entity);
            }
        }

        /// <inheritdoc />
        public void UpdateRange(IEnumerable<T> entities, Action<IUpdateConfig<T>> configurer)
        {
            throw new NotImplementedException("We don't have a way to update range with a configuration on redis");
        }

        /// <inheritdoc />
        public void Delete(T entity)
        {
            dbCollection.Delete(entity);
        }

        /// <inheritdoc />
        public void Delete(Expression<Func<T, bool>> where)
        {
            DeleteRange(dbCollection.Where(where));
        }

        /// <inheritdoc />
        public void DeleteRange(IEnumerable<T> entities)
        {
            dbCollection.Delete(entities);
        }

        /// <inheritdoc />
        public T GetById<KeyType>(KeyType id)
        {
            if (id is string str_id)
            {
                T? entity = dbCollection.FindById(str_id);
                if (entity == null)
                {
                    throw new KeyNotFoundException($"entity with key of {id} was not found.");
                }

                return entity;
            }

            throw new NotImplementedException($"KeyType of {typeof(KeyType)} is not supported.");
        }

        /// <inheritdoc />
        public T Get(Expression<Func<T, bool>> where)
        {
            return dbCollection.Where(where).FirstOrDefault();
        }

        /// <inheritdoc />
        public IEnumerable<T> GetAll()
        {
            return dbCollection.Where(entity => true);
        }

        /// <inheritdoc />
        public IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return dbCollection.Where(where);
        }

        protected readonly IRedisCollection<T> dbCollection;

        protected IDatabaseFactory DatabaseFactory { get; private set; }

        protected IRedisConnectionProvider DataContext => _dataContext ?? (_dataContext = (IRedisConnectionProvider)DatabaseFactory.Get());

        private IRedisConnectionProvider _dataContext;
    }
}