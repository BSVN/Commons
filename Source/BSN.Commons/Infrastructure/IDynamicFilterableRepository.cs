using System;
using System.Linq.Expressions;

namespace BSN.Commons.Infrastructure
{
    /// <summary>
    /// Add Dynamic filterable query ability to <see cref="IRepository{T}"/> pattern"
    /// </summary>
    /// <typeparam name="T"></typeparam>
	public interface IDynamicFilterableRepository<T> : IRepository<T> where T : class
	{
        /// <summary>
        /// Get list of objects based on <paramref name="filters"/>
        /// </summary>
        /// <remarks>
        /// This method retrive all objects based on filters and apply pagination.
        /// So to retrive all objects that matched with filters, you need iterate all pages.
        /// </remarks>
        /// <param name="filters">
        ///  is a comma-delimited list of <c>{Name}{Operator}{Value}</c> where
        /// <list type="bullet">
        ///  <item>
        ///      <description>
        ///          <c>{Name}</c> is the name of a property with the Sieve attribute or the name of a custom filter method for <c>TEntity</c>
        ///          <list type="bullet">
        ///              <item>
        ///                  <description>
        ///                      You can also have multiple names (for OR logic) by enclosing them in brackets and using a pipe delimiter,
        ///                      eg. <c> (LikeCount|CommentCount)>10 </c> asks if LikeCount or <c> CommentCount is >10 </c>
        ///                  </description>
        ///              </item>
        ///          </list>
        ///      </description>
        ///  </item>
        ///  <item>
        ///      <description>
        ///         <c>{Operator}</c> is one of the Operators
        ///      </description>
        ///  </item>
        ///  <item>
        ///      <description>
        ///          <c>{Value}</c>` is the value to use for filtering
        ///          <list type="bullet">
        ///              <item>
        ///                  <description>
        ///                      You can also have multiple values (for OR logic) by using a pipe delimiter,
        ///                      eg. <c>Title@=new|hot</c> will return posts with titles that contain the text "new" or "hot"
        ///                  </description>
        ///              </item>
        ///          </list>
        ///      </description>
        ///  </item>
        /// </list>
        /// </param>
        /// <param name="sorts">is a comma-delimited ordered list of property names to sort by. Adding a <c>-</c>before the name switches to sorting descendingly.</param>
        /// <param name="pageNumber">is the number of page to return</param>
        /// <param name="pageSize">is the number of items returned per page</param>
        /// <returns>Paginated list of matched objects based on filters</returns>
        PagedEntityCollection<T> GetMany(string filters, string sorts, uint pageNumber, uint pageSize);

        /// <summary>
        /// Get list of objects based on <paramref name="filters"/> in scope of <paramref name="where"/> expression
        /// <see cref="GetMany(string, string, uint, uint)"/>
        /// </summary>
        /// <param name="where">A function to test each element for a condition.</param>
        /// <param name="filters"><inheritdoc cref="GetMany(string, string, uint, uint)" path='/param[@name="filters"]'/></param>
        /// <param name="sorts"><inheritdoc cref="GetMany(string, string, uint, uint)" path='/param[@name="sorts"]'/></param>
        /// <param name="pageNumber"><inheritdoc cref="GetMany(string, string, uint, uint)" path='/param[@name="pageNumber"]'/></param>
        /// <param name="pageSize"><inheritdoc cref="GetMany(string, string, uint, uint)" path='/param[@name="pageSize"]'/></param>
        /// <returns>Paginated list of matched objects based on <paramref name="filters"/> in scope of <paramref name="where"/> expression</returns>
        PagedEntityCollection<T> GetMany(Expression<Func<T, bool>> where, string filters, string sorts, uint pageNumber, uint pageSize);
    }
}
 