using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace BSN.Commons.Orm.EntityFrameworkCore
{
    /// <summary>
    /// Model extensions for <see cref="EntityTypeBuilder"/>
    /// </summary>
    public static class ModelBuilderExtensions
    {
        /// <summary>
        /// Entity Framework Core does not support mapping get-only properties to columns by default.
        /// So if you want to map get-only properties to columns, you need to use this extension method.
        /// </summary>
        /// <example>
        /// <code>
        /// modelBuilder.Entity{Foo}().MapAllReadonlyProperty();
        /// modelBuilder.Entity{Foo}().Ignore(F => F.SomeGetOnlyProperty); // Ignore get-only property
        /// </code>
        /// </example>
        /// <remarks>
        /// After calling this method, you can use
        /// <see cref="EntityTypeBuilder{TEntity}.Ignore(Expression{Func{TEntity, object?}})"/> method,
        /// to ignore some get-only properties.
        /// </remarks>
        /// <typeparam name="T">The entity type being configured.</typeparam>
        /// <param name="builder">The builder for the entity type being configured.</param>
        public static void MapAllReadonlyProperty<T>(this EntityTypeBuilder<T> builder) where T : class
        {
            var ignores = builder.Metadata.GetIgnoredMembers();
            IEnumerable<PropertyInfo> properties = from property in typeof(T).GetProperties()
                                                   where property.CanWrite == false
                                                   && property.GetCustomAttribute<NotMappedAttribute>() == null
                                                   && !ignores.Any(ignoreProperty => ignoreProperty == property.Name)
                                                   select property;
            foreach (var property in properties)
            {
                builder.Property(property.Name);
            }
        }
    }
}
