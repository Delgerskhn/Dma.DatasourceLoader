﻿using Dma.DatasourceLoader.Filters.PrimaryFilters;
using System.Linq.Expressions;

namespace Dma.DatasourceLoader.Filters.ComplexFilters
{
    public class NestedFilter<T> : FilterBase<T>
    {
        private readonly Filter _innerFilter;

        public NestedFilter(string nestedProperty, Filter innerFilter) : base(nestedProperty)
        {
            _innerFilter = innerFilter;
        }
        public override Expression<Func<T, bool>> GetFilterExpression()
        {
            ParameterExpression outerParameter = Expression.Parameter(typeof(T));
            MemberExpression innerProperty = Expression.Property(outerParameter, propertyName);

            var notNullFilterExpression = new IsNotNullFilter<T>(propertyName).GetFilterExpression();

            var invokeNotNullExpr = Expression.Invoke(notNullFilterExpression, outerParameter);

            var pred = _innerFilter.GetFilterExpression(); // Get the inner filter expression

            var invokeExpression = Expression.Invoke(pred, innerProperty); // Invoke the inner filter expression with innerProperty as the argument

            // Combine notNullFilterExpression and invokeExpression using AndAlso
            var combinedExpression = Expression.AndAlso(invokeNotNullExpr, invokeExpression);

            // Create a new lambda expression with outerParameter as the parameter and combinedExpression as the body
            var lambdaExpression = Expression.Lambda<Func<T, bool>>(combinedExpression, outerParameter);
            return lambdaExpression;

        }
    }
}
