using System;
using System.Linq.Expressions;

namespace Commons.Infrastructure
{
    public interface IUpdateConfig<TEntity> where TEntity : class 
    {
		void IncludeAllProperties();
		void AutoDetectChangedProperties();
		IUpdateConfig<TEntity> IncludeProperty<TProperty>(Expression<Func<TEntity, TProperty>> propertyAccessExpression);
	}
}
