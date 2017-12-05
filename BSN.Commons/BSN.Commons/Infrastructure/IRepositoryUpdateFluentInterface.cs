using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace BSN.Commons.Infrastructure
{
    public interface IRepositoryUpdateFluentInterface<TEntity>
    {
	    IRepositoryUpdateFluentInterface<TEntity> IncludeProperty<TProperty>(Expression<Func<TEntity, TProperty>> propertyAccessExpression);
		IRepositoryUpdateFluentInterface<TEntity> IncludeAllProperties();
	    IRepositoryUpdateFluentInterface<TEntity> AutoDetectChangedProperties();

		void Commit();
    }
}
