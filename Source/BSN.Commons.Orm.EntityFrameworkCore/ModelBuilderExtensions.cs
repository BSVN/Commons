using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
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
        /// Map all readonly property of entity to model
        /// </summary>
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
