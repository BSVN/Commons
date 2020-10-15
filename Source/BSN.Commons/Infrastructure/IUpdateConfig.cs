using System;
using System.Linq.Expressions;

namespace Commons.Infrastructure
{
    /// <summary>
    /// Update Config is useful when you are using Update method on Repository classes
    /// It will Inject some existing parameters to configure Update method 
    /// </summary>
    /// <typeparam name="T">Entity Type</typeparam>
    public interface IUpdateConfig<TEntity> where TEntity : class 
    {
        /// <summary>
        /// This ensure the context that all related property to the current instance of the object are 
        /// Included in Update Operation.
        /// </summary>
        void IncludeAllProperties();

        /// <summary>
        /// This ensures the context is aware of any changes to tracked entity instances 
        /// before performing operations such as SaveChanges() or returning change tracking information
        /// </summary>
        void AutoDetectChangedProperties();

        /// <summary>
        /// List of modifid properties of the updated object.
        /// this will include mentioned properties as a changed value in Update process.
        /// </summary>
        IUpdateConfig<TEntity> IncludeProperty<TProperty>(Expression<Func<TEntity, TProperty>> propertyAccessExpression);
	}
}
