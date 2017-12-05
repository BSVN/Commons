using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq.Expressions;
using BSN.Commons.Infrastructure;

namespace Commons.Infrastructure
{
    public abstract partial class RepositoryBase<T> where T : class
	{
		public abstract class RepositoryBaseUpdateFluentInterface<TEntity> : IRepositoryUpdateFluentInterface<TEntity>
			where TEntity : class
		{
			protected readonly DbContext DbContext;
			protected readonly DbSet<TEntity> DbSet;
			protected readonly List<string> PropertyNames;
			protected bool _updateWholeEntity;
			protected bool _autoDetectChangedProperties;


			internal RepositoryBaseUpdateFluentInterface(RepositoryBase<TEntity> repository)
			{
				DbContext = repository.DataContext;
				DbSet = DbContext.Set<TEntity>();
				_updateWholeEntity = false;
				_autoDetectChangedProperties = true;
				PropertyNames = new List<string>();
			}

			public IRepositoryUpdateFluentInterface<TEntity> IncludeProperty<TProperty>(
				Expression<Func<TEntity, TProperty>> propertyAccessExpression)
			{
				PropertyNames.Add(
						(propertyAccessExpression.Body as MemberExpression)?.Member.Name ??
						throw new ArgumentException(nameof(propertyAccessExpression))
				);

				_autoDetectChangedProperties = false;
				_updateWholeEntity = false;
				return this;
			}

			public IRepositoryUpdateFluentInterface<TEntity> IncludeAllProperties()
			{
				_autoDetectChangedProperties = false;
				_updateWholeEntity = true;
				return this;
			}

			public IRepositoryUpdateFluentInterface<TEntity> AutoDetectChangedProperties()
			{
				_autoDetectChangedProperties = true;
				_updateWholeEntity = false;
				return this;
			}

			public abstract void Commit();

		}

		public class RepositoryBaseUpdateRangeFluentInterface<TEntity> : RepositoryBaseUpdateFluentInterface<TEntity>
			where TEntity : class
		{
			private readonly IEnumerable<TEntity> _entities;

			internal RepositoryBaseUpdateRangeFluentInterface(RepositoryBase<TEntity> repository, IEnumerable<TEntity> entities)
				: base(repository)
			{
				_entities = entities;
			}

			public override void Commit()
			{
				if (!_autoDetectChangedProperties)
					DbContext.Configuration.AutoDetectChangesEnabled = false;

				if (_updateWholeEntity)
				{
					foreach (TEntity entity in _entities)
					{
						DbSet.Attach(entity);
						DbContext.Entry(entity).State = EntityState.Modified;
					}
				}
				else if (!_autoDetectChangedProperties)
				{
					foreach (TEntity entity in _entities)
					{
						DbSet.Attach(entity);
						foreach (string propertyName in PropertyNames)
							DbContext.Entry(entity).Property(propertyName).IsModified = true;
					}
				}
				else
				{
					foreach (TEntity entity in _entities)
						DbSet.Attach(entity);
				}

				if (!_autoDetectChangedProperties)
					DbContext.Configuration.AutoDetectChangesEnabled = true;
			}
		}

		public class RepositoryBaseSingleUpdateFluentInterface<TEntity> : RepositoryBaseUpdateFluentInterface<TEntity>
			where TEntity : class
		{
			private readonly TEntity _entity;

			internal RepositoryBaseSingleUpdateFluentInterface(RepositoryBase<TEntity> repository, TEntity entity)
				: base(repository)
			{
				_entity = entity;
			}

			public override void Commit()
			{
				if (!_autoDetectChangedProperties)
					DbContext.Configuration.AutoDetectChangesEnabled = false;

				DbSet.Attach(_entity);

				if (_updateWholeEntity)
				{
					DbContext.Entry(_entity).State = EntityState.Modified;
				}
				else if (!_autoDetectChangedProperties)
				{
					foreach (string propertyName in PropertyNames)
						DbContext.Entry(_entity).Property(propertyName).IsModified = true;
				}

				if (!_autoDetectChangedProperties)
					DbContext.Configuration.AutoDetectChangesEnabled = true;
			}
		}
	}
}
