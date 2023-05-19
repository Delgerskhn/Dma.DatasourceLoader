using System.Linq.Expressions;
using System.Reflection;

namespace Dma.DatasourceLoader.Filters
{

    public class NotInFilter<T, TValue> : FilterBase<T>
    {
        private readonly IEnumerable<TValue> values;

        public NotInFilter(string propertyName, IEnumerable<TValue> values) : base(propertyName)
        {
            this.values = values;
        }

        public override Expression<Func<T, bool>> GetFilterExpression()
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T));
            MemberExpression property = Expression.Property(parameter, propertyName);
            ConstantExpression valuesArray = Expression.Constant(values);
            MethodInfo containsMethod = typeof(Enumerable).GetMethods()
            .Where(m => m.Name == "Contains" && m.GetParameters().Length == 2)
            .Single()
            .MakeGenericMethod(typeof(TValue));
            MethodCallExpression containsExpression = Expression.Call(null, containsMethod, valuesArray, property);
            UnaryExpression notContainsExpression = Expression.Not(containsExpression);

            return Expression.Lambda<Func<T, bool>>(notContainsExpression, parameter);
        }
    }
}
