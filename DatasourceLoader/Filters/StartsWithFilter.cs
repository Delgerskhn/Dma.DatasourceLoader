using System.Linq.Expressions;
using System.Reflection;

namespace Dma.DatasourceLoader.Filters
{
    public class StartsWithFilter<T> : FilterBase<T>
    {
        private readonly string value;

        public StartsWithFilter(string propertyName, string value) : base(propertyName)
        {
            this.value = value;
        }

        public override Expression<Func<T, bool>> GetFilterExpression()
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T));
            MemberExpression property = Expression.Property(parameter, propertyName);
            MethodInfo startsWithMethod = typeof(string).GetMethod("StartsWith", new[] { typeof(string) })!;
            ConstantExpression constant = Expression.Constant(value);
            MethodCallExpression startsWithExpression = Expression.Call(property, startsWithMethod, constant);

            return Expression.Lambda<Func<T, bool>>(startsWithExpression, parameter);
        }
    }
}
