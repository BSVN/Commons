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

namespace BSN.Commons.Orm.Redis
{
    /// <inheritdoc />
    public class RepositoryBase<T> : IRedisRepository<T> where T : class
    {
        /// <inheritdoc />
        protected RepositoryBase(IRedisDatabaseFactory databaseFactory)
        {
            DatabaseFactory = databaseFactory;
            _redisCollection = (RedisCollection<T>)Provider.RedisCollection<T>();
            // TODO: Check that IndexCreationService is necessary or not.
            Provider.Connection.CreateIndex(typeof(T));
        }

        /// <inheritdoc />
        public bool Any()
        {
            return _redisCollection.Any();
        }

        /// <inheritdoc />
        public bool Any(Expression<Func<T, bool>> expression)
        {
            return _redisCollection.Any(expression);
        }

        /// <inheritdoc />
        public void Update(T item)
        {
            _redisCollection.Update(item);
        }

        /// <inheritdoc />
        public async Task UpdateAsync(T item)
        {
            await _redisCollection.UpdateAsync(item);
        }

        /// <inheritdoc />
        public async ValueTask UpdateAsync(IEnumerable<T> items)
        {
            await _redisCollection.UpdateAsync(items);
        }

        /// <inheritdoc />
        public async Task<IList<T>> ToListAsync()
        {
            return await _redisCollection.ToListAsync();
        }

        /// <inheritdoc />
        public async Task<bool> AnyAsync()
        {
            return await AnyAsync();
        }

        /// <inheritdoc />
        public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
        {
            return await AnyAsync(expression);
        }

        /// <inheritdoc />
        public async Task<T> FirstAsync()
        {
            return await _redisCollection.FirstAsync();
        }

        /// <inheritdoc />
        public async Task<T> FirstAsync(Expression<Func<T, bool>> expression)
        {
            return await _redisCollection.FirstAsync(expression);
        }

        /// <inheritdoc />
        public async Task<T?> FirstOrDefaultAsync()
        {
            return await _redisCollection.FirstOrDefaultAsync();
        }

        /// <inheritdoc />
        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> expression)
        {
            return await _redisCollection.FirstOrDefaultAsync(expression);
        }

        /// <inheritdoc />
        public async Task<T> SingleAsync()
        {
            return await _redisCollection.SingleAsync();
        }

        /// <inheritdoc />
        public async Task<T> SingleAsync(Expression<Func<T, bool>> expression)
        {
            return await _redisCollection.SingleAsync(expression);
        }

        /// <inheritdoc />
        public async Task<T?> SingleOrDefaultAsync()
        {
            return await _redisCollection.SingleOrDefaultAsync();
        }

        /// <inheritdoc />
        public async Task<T?> SingleOrDefaultAsync(Expression<Func<T, bool>> expression)
        {
            return await _redisCollection.SingleOrDefaultAsync(expression);
        }

        /// <inheritdoc />
        public async Task<IDictionary<string, T?>> FindByIdsAsync(IEnumerable<string> ids)
        {
            return await FindByIdsAsync(ids);
        }

        /// <inheritdoc />
        public virtual IEnumerator<T> GetEnumerator()
        {
            return _redisCollection.GetEnumerator();
        }

        /// <inheritdoc />
        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
        {
            return _redisCollection.GetAsyncEnumerator(cancellationToken);
        }

        /// <inheritdoc />
        public int Count(Expression<Func<T, bool>> expression)
        {
            return _redisCollection.Count(expression);
        }

        /// <inheritdoc />
        public async Task<int> CountAsync()
        {
            return await _redisCollection.CountAsync();
        }

        /// <inheritdoc />
        public async Task<int> CountAsync(Expression<Func<T, bool>> expression)
        {
            return await _redisCollection.CountAsync(expression);
        }

        /// <inheritdoc />
        public void Save()
        { 
            _redisCollection.Save();
        }

        /// <inheritdoc />
        public async ValueTask SaveAsync()
        {
            await _redisCollection.SaveAsync();
        }

        /// <inheritdoc />
        public string Insert(T item)
        {
            return _redisCollection.Insert(item);
        }

        /// <inheritdoc />
        public string Insert(T item, TimeSpan timeSpan)
        {
            return _redisCollection.Insert(item, timeSpan);
        }

        /// <inheritdoc />
        public string? Insert(T item, WhenKey when, TimeSpan? timeSpan = null)
        {
            return _redisCollection.Insert(item, when, timeSpan);
        }

        /// <inheritdoc />
        public async Task<string> InsertAsync(T item)
        {
            return await _redisCollection.InsertAsync(item);
        }

        /// <inheritdoc />
        public async Task<string> InsertAsync(T item, TimeSpan timeSpan)
        {
            return await _redisCollection.InsertAsync(item, timeSpan);
        }

        /// <inheritdoc />
        public Task<string?> InsertAsync(T item, WhenKey when, TimeSpan? timeSpan = null)
        {
            return _redisCollection.InsertAsync(item, when, timeSpan);
        }

        /// <inheritdoc />
        public T? FindById(string id)
        { 
            return _redisCollection.FindById(id);
        }
        
        /// <inheritdoc />
        public virtual async Task<T?> FindByIdAsync(string id)
        {
            return await _redisCollection.FindByIdAsync(id);
        }

        /// <inheritdoc />
        public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> expression)
        {
            return _redisCollection.Where(expression);
        }

        /// <inheritdoc />
        public virtual IEnumerable<TR> Select<TR>(Expression<Func<T, TR>> expression) where TR : notnull
        {
            return _redisCollection.Select(expression);
        }

        /// <inheritdoc />
        public virtual IEnumerable<T> Skip(int count)
        {
            return _redisCollection.Skip(count);
        }

        /// <inheritdoc />
        public virtual IEnumerable<T> Take(int count)
        {
            return _redisCollection.Take(count);
        }

        /// <inheritdoc />
        public virtual IEnumerable<T> OrderBy<TField>(Expression<Func<T, TField>> expression)
        {
            return _redisCollection.OrderBy(expression);
        }

        /// <inheritdoc />
        public virtual IEnumerable<T> OrderByDescending<TField>(Expression<Func<T, TField>> expression)
        {
            return _redisCollection.OrderByDescending(expression);
        }

        /// <inheritdoc />
        public virtual IEnumerable<T> GeoFilter(Expression<Func<T, GeoLoc?>> expression, double lon, double lat, double radius, GeoLocDistanceUnit unit)
        {
            return _redisCollection.GeoFilter(expression, lon, lat, radius, unit);
        }

        /// <inheritdoc />
        public void DeleteById(string id)
        {
            _provider.Connection.Unlink(id);
        }

        /// <inheritdoc />
        public void Delete(T item)
        {
            _redisCollection.Delete(item);
        }

        /// <inheritdoc />
        public void Delete(IEnumerable<T> items)
        {
            _redisCollection.Delete(items);
        }

        /// <inheritdoc />
        public async Task DeleteAsync(T item)
        {
            await _redisCollection.DeleteAsync(item);
        }

        /// <inheritdoc />
        public async Task DeleteAsync(IEnumerable<T> items)
        {
            await _redisCollection.DeleteAsync(items);
        }

        /// <inheritdoc />
        public string Execute(string command, string[] args)
        {
            return _provider.Connection.ExecuteAsync(command, args).Result;
        }

        /// <summary>
        /// TODO: complete doc
        /// </summary>
        protected readonly RedisCollection<T> _redisCollection;

        /// <summary>
        /// TODO: complete doc
        /// </summary>
        protected RedisConnectionProvider Provider => _provider ?? (_provider = DatabaseFactory.Get());

        /// <summary>
        /// TODO: complete doc
        /// </summary>
        protected IRedisDatabaseFactory DatabaseFactory { get; private set; }

        private RedisConnectionProvider _provider;

    }
}
