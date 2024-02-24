using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Redis.OM.Searching;
using Redis.OM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using BSN.Commons.Infrastructure;
using Apache.NMS.ActiveMQ.Util.Synchronization;
using static Amazon.S3.Util.S3EventNotification;
using StackExchange.Redis;
using Redis.OM.Contracts;
using Redis.OM.Aggregation;
using Redis.OM.Modeling;
using Redis.OM.Searching.Query;
using BSN.Commons.Infrastructure.Redis;

namespace BSN.Commons.Orm.Redis
{
    /// <summary>
    /// Repository Base for Redis Implementation
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RedisRepositoryBase<T> : IRepository<T> where T : class
    {
        /// <summary>
        /// Constructor for Redis Repository Base
        /// </summary>
        /// <param name="databaseFactory">Database Factory Containing an IRedisDbContext</param>
        protected RedisRepositoryBase(IDatabaseFactory databaseFactory)
        {
            DatabaseFactory = databaseFactory;
            _redisCollection = (RedisCollection<T>)Provider.RedisCollection<T>();
            // TODO: Check that IndexCreationService is necessary or not.
            Provider.Connection.CreateIndex(typeof(T));
        }

        /// <inheritdoc />
        public void Add(T entity)
        {
            _redisCollection.Insert(entity);
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
            _redisCollection.Update(entity);
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
            _redisCollection.Delete(entity);
        }

        /// <inheritdoc />
        public void Delete(Expression<Func<T, bool>> where)
        {
            DeleteRange(_redisCollection.Where(where));
        }

        /// <inheritdoc />
        public void DeleteRange(IEnumerable<T> entities)
        {
            _redisCollection.Delete(entities);
        }

        /// <inheritdoc />
        public T GetById<KeyType>(KeyType id)
        {
            if (id is string str_id)
            {
                T? entity = _redisCollection.FindById(str_id);
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
            return _redisCollection.Where(where).FirstOrDefault();
        }

        /// <inheritdoc />
        public IEnumerable<T> GetAll()
        {
            return _redisCollection.Where(entity => true);
        }

        /// <inheritdoc />
        public IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return _redisCollection.Where(where);
        }
        
        /// <summary>
        /// Redis Collection accosiated with the type of T
        /// </summary>
        protected readonly RedisCollection<T> _redisCollection;

        /// <summary>
        /// Redis Connection Provider to access collections
        /// </summary>
        protected IRedisConnectionProvider Provider
        {
            get
            {
                if (_provider == null)
                {
                    if (DatabaseFactory.Get() is IRedisDbContext dbContext)
                    {
                        _provider = dbContext.GetConnectionProvider();
                    }
                    else
                    {
                        throw new InvalidCastException("The database factory for redis must return an IRedisDbContext");
                    }
                }

                return _provider;
            }
        }

        /// <summary>
        /// Redis Database Factory
        /// </summary>
        protected IDatabaseFactory DatabaseFactory { get; private set; }

        protected IRedisConnectionProvider? _provider;
    }
}