using System.Linq.Expressions;
using System.Reflection;

namespace Dma.DatasourceLoader.Filters.StringFilters
{
    public class ContainsFilter<T> : FilterBase<T>
    {
        private readonly string value;

        public ContainsFilter(string propertyName, string value) : base(propertyName)
        {
            this.value = value;
        }

        public override Expression<Func<T, bool>> GetFilterExpression()
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T));
            MemberExpression property = Expression.Property(parameter, propertyName);
            MethodInfo containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) })!;
            ConstantExpression constant = Expression.Constant(value);
            MethodCallExpression containsExpression = Expression.Call(property, containsMethod, constant);

            return Expression.Lambda<Func<T, bool>>(containsExpression, parameter);
        }
    }

}
