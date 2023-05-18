using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Dma.DatasourceLoader.Filters
{
    public class NestedFilter<T, TProperty> : FilterBase<T>
    {
        private readonly string _nestedProperty;
        private readonly FilterBase<TProperty> _innerFilter;

        public NestedFilter(string nestedProperty, FilterBase<TProperty> innerFilter)
        {
            _nestedProperty = nestedProperty;
            _innerFilter = innerFilter;
        }
        public override Expression<Func<T, bool>> GetFilterExpression()
        {
            ParameterExpression outerParameter = Expression.Parameter(typeof(T));
            MemberExpression innerProperty = Expression.Property(outerParameter, _nestedProperty);


            var pred = _innerFilter.GetFilterExpression(); // Get the inner filter expression

            var invokeExpression = Expression.Invoke(pred, innerProperty); // Invoke the inner filter expression with innerProperty as the argument

            return Expression.Lambda<Func<T, bool>>(invokeExpression, outerParameter);

        }
    }
}
