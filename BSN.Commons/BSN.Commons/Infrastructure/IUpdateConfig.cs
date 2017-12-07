using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace BSN.Commons.Infrastructure
{
    public interface IUpdateConfig<TEntity> where TEntity : class 
    {
	    IUpdateConfig<TEntity> IncludeAllProperties();
		IUpdateConfig<TEntity> AutoDetectChangedProperties();
		IUpdateConfig<TEntity> IncludeProperty<TProperty>(Expression<Func<TEntity, TProperty>> propertyAccessExpression);
	}
}
