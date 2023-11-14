using System.Linq.Expressions;
using System.Reflection;

namespace Dma.DatasourceLoader.Filters.StringFilters
{
    public class NotContainsFilter<T> : FilterBase<T>
    {
        private readonly string value;

        public NotContainsFilter(string propertyName, string value) : base(propertyName)
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
            UnaryExpression notContainsExpression = Expression.Not(containsExpression);
            BinaryExpression isNullExpression = Expression.Equal(property, Expression.Constant(null));

            var notNull_notContainsExpression = Expression.OrElse(isNullExpression, notContainsExpression);

            return Expression.Lambda<Func<T, bool>>(notNull_notContainsExpression, parameter);
        }
    }

}
