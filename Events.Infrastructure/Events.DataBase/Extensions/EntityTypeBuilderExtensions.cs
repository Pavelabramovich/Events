using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Events.Domain.Entities;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq.Expressions;


namespace Events.DataBase.Extensions;


internal static class EntityTypeBuilderExtensions
{
    public static PropertyBuilder<TKeyProperty> HasKeyProperty<TEntity, TKeyProperty>(
        this EntityTypeBuilder<TEntity> @this, 
        Expression<Func<TEntity, TKeyProperty>> propertyExpression) where TEntity : class
    {
        @this.HasKey(ToKeyExpression(propertyExpression));

        return @this.Property(propertyExpression);
    }

    public static PropertyBuilder<TProperty> IsOptional<TProperty>(this PropertyBuilder<TProperty> @this)
    {
        return @this.IsRequired(false);
    }


    static Expression<Func<TEntity, object?>> ToKeyExpression<TEntity, TKeyProperty>(
            Expression<Func<TEntity, TKeyProperty>> propertyExpression)
    {
        var parameter = propertyExpression.Parameters[0];

        var body = Expression.Convert(propertyExpression.Body, typeof(object));

        return Expression.Lambda<Func<TEntity, object?>>(body, parameter);
    }
}
