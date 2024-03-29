﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
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
        /// <typeparam name="TEntity">The entity type being configured.</typeparam>
        /// <param name="builder">The builder for the entity type being configured.</param>
        public static void MapAllReadonlyProperties<TEntity>(this EntityTypeBuilder<TEntity> builder) where TEntity : class
        {
            MapAllReadonlyProperties<TEntity>(builder.Metadata, builder);
        }

        /// <summary>
        /// Entity Framework Core does not support mapping get-only properties to columns for dependent entity by default.
        /// So if you want to map get-only properties of dependent entity to columns, you need to use this extension method.
        /// </summary>
        /// <example>
        /// <code>
        /// modelBuilder.Entity{Foo}().OwnsOne(F => F.Bar).MapAllReadonlyProperty();
        /// modelBuilder.Entity{Foo}().OwnsOne(F => F.Bar).Ignore(B => B.SomeGetOnlyProperty); // Ignore get-only property
        /// </code>
        /// </example>
        /// <remarks>
        /// After calling this method, you can use
        /// <see cref="OwnedNavigationBuilder{TOwnerEntity, TDependentEntity}.Ignore(Expression{Func{TDependentEntity, object?}})"/> method,
        /// to ignore some get-only properties.
        /// </remarks>
        /// <typeparam name="TOwnerEntity">The entity type that of owner entity</typeparam>
        /// <typeparam name="TDependentEntity">The entity type that this relationship target</typeparam>
        /// <param name="builder">The builder for the entity type being configured.</param>
        public static void MapAllReadonlyProperties<TOwnerEntity, TDependentEntity>(this OwnedNavigationBuilder<TOwnerEntity, TDependentEntity> builder)
            where TOwnerEntity : class where TDependentEntity : class
        {
            MapAllReadonlyProperties<TDependentEntity>(builder.Metadata.DeclaringEntityType, builder);
        }


        private static void MapAllReadonlyProperties<T>(Microsoft.EntityFrameworkCore.Metadata.IMutableEntityType entityType,
            Microsoft.EntityFrameworkCore.Infrastructure.IInfrastructure<IConventionEntityTypeBuilder> builder)
        {
            var ignores = entityType.GetIgnoredMembers();
            var navigations = entityType.GetNavigations().Select(n => n.Name);
            IEnumerable<PropertyInfo> properties = from property in typeof(T).GetProperties()
                                                   where property.CanWrite == false
                                                   && property.GetCustomAttribute<NotMappedAttribute>() == null
                                                   && property.GetMethod.GetCustomAttribute<CompilerGeneratedAttribute>() != null
                                                   && !ignores.Any(ignoreProperty => ignoreProperty == property.Name)
                                                   && !navigations.Contains(property.Name)
                                                   select property;

            // about following condition in above code:
            //      && property.GetMethod.GetCustomAttribute<CompilerGeneratedAttribute>() != null
            // getter-only properties and expression-bodied properties are looking so much similar in C#
            //      1. public string FullName => $"{FirstName} {LastName}"
            //      2. public string FirstName { get; }
            // in example #1 there will be no backing-field in compilation process.
            // but in the next example (#2) we have a compiler generated backing-field.
            // By default EF marks a property as column if it be able to write on it.
            // So when we add read-only things to it, we should care about such a case.
            // to identify expression-bodied properties we can use this attribute check on GetMethod.

            foreach (var property in properties)
            {
                builder.Instance.Property(property.PropertyType, property.Name);
            }
        }
    }
}
