using System.Linq.Expressions;
using System.Reflection;

namespace Dma.DatasourceLoader.Filters.StringFilters
{
    public class EndsWithFilter<T> : FilterBase<T>
    {
        private readonly string value;

        public EndsWithFilter(string propertyName, string value) : base(propertyName)
        {
            this.value = value;
        }

        public override Expression<Func<T, bool>> GetFilterExpression()
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T));
            MemberExpression property = Expression.Property(parameter, propertyName);
            MethodInfo endsWithMethod = typeof(string).GetMethod("EndsWith", new[] { typeof(string) })!;
            ConstantExpression constant = Expression.Constant(value);
            MethodCallExpression endsWithExpression = Expression.Call(property, endsWithMethod, constant);

            BinaryExpression isNotNullExpression = Expression.NotEqual(property, Expression.Constant(null));
            var notNull_endsWithExpression = Expression.AndAlso(isNotNullExpression, endsWithExpression);


            return Expression.Lambda<Func<T, bool>>(notNull_endsWithExpression, parameter);
        }
    }
}
