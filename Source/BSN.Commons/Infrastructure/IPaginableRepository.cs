namespace BSN.Commons.Infrastructure
{
    /// <summary>
    /// Add Pagination ability to Repository pattern
    /// </summary>
    /// <typeparam name="T"></typeparam>
	public interface IPaginableRepository<T> : IRepository<T> where T : class
	{
    }
}
 