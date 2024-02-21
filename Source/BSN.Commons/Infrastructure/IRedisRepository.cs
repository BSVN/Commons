using Redis.OM.Modeling;
using Redis.OM;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace BSN.Commons.Infrastructure
{
    /// <summary>
    /// Repository Pattern Interface for abstract communicating with DataBase
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRedisRepository<T> where T : class
    {
        bool Any();
        bool Any(Expression<Func<T, bool>> expression);
        void Update(T item);
        Task UpdateAsync(T item);
        ValueTask UpdateAsync(IEnumerable<T> items);
        Task<IList<T>> ToListAsync();
        Task<bool> AnyAsync();
        Task<bool> AnyAsync(Expression<Func<T, bool>> expression);
        Task<T> FirstAsync();
        Task<T> FirstAsync(Expression<Func<T, bool>> expression);
        Task<T> FirstOrDefaultAsync();
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> expression);
        Task<T> SingleAsync();
        Task<T> SingleAsync(Expression<Func<T, bool>> expression);
        Task<T> SingleOrDefaultAsync();
        Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> expression);
        Task<IDictionary<string, T>> FindByIdsAsync(IEnumerable<string> ids);
        IEnumerator<T> GetEnumerator();
        IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken));
        int Count(Expression<Func<T, bool>> expression);
        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<T, bool>> expression);
        void Save();
        ValueTask SaveAsync();
        string Insert(T item);
        string Insert(T item, TimeSpan timeSpan);
        string Insert(T item, WhenKey when, TimeSpan? timeSpan = null);
        Task<string> InsertAsync(T item);
        Task<string> InsertAsync(T item, TimeSpan timeSpan);
        Task<string> InsertAsync(T item, WhenKey when, TimeSpan? timeSpan = null);
        T FindById(string id);
        Task<T> FindByIdAsync(string id);
        IEnumerable<T> GetMany(Expression<Func<T, bool>> expression);
        IEnumerable<TR> Select<TR>(Expression<Func<T, TR>> expression);
        IEnumerable<T> Skip(int count);
        IEnumerable<T> Take(int count);
        IEnumerable<T> OrderBy<TField>(Expression<Func<T, TField>> expression);
        IEnumerable<T> OrderByDescending<TField>(Expression<Func<T, TField>> expression);
        IEnumerable<T> GeoFilter(Expression<Func<T, GeoLoc?>> expression, double lon, double lat, double radius, GeoLocDistanceUnit unit);
        void DeleteById(string id);
        void Delete(T item);
        void Delete(IEnumerable<T> items);
        Task DeleteAsync(T item);
        Task DeleteAsync(IEnumerable<T> items);
        string Execute(string command, string[] args);
    }
}
