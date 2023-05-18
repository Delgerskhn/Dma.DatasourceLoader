using System.Linq.Expressions;
using System.Reflection;

namespace Dma.DatasourceLoader.Filters
{

    public class InFilter<T, TValue> : FilterBase<T>
    {
        private readonly string propertyName;
        private readonly IEnumerable<TValue> values;

        public InFilter(string propertyName, IEnumerable<TValue> values)
        {
            this.propertyName = propertyName;
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

            return Expression.Lambda<Func<T, bool>>(containsExpression, parameter);
        }
    }
}
