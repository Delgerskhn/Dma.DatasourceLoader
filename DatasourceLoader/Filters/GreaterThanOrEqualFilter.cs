﻿using System.Linq.Expressions;

namespace Dma.DatasourceLoader.Filters
{
    public class GreaterThanOrEqualFilter<T> : FilterBase<T>
    {
        private readonly object value;

        public GreaterThanOrEqualFilter(string propertyName, object value) : base(propertyName) 
        {
            this.value = value;
        }

        public override Expression<Func<T, bool>> GetFilterExpression()
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T));
            MemberExpression property = Expression.Property(parameter, propertyName);
            ConstantExpression constant = Expression.Constant(value);
            BinaryExpression greaterThanExpression = Expression.GreaterThanOrEqual(property, constant);

            return Expression.Lambda<Func<T, bool>>(greaterThanExpression, parameter);
        }
    }

}
