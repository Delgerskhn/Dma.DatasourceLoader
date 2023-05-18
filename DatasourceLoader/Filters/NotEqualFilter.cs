using System.Linq.Expressions;

namespace Dma.DatasourceLoader.Filters
{
    public class NotEqualFilter<T> : FilterBase<T>
    {
        private readonly string propertyName;
        private readonly object value;

        public NotEqualFilter(string propertyName, object value)
        {
            this.propertyName = propertyName;
            this.value = value;
        }

        public override Expression<Func<T, bool>> GetFilterExpression()
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T));
            MemberExpression property = Expression.Property(parameter, propertyName);
            ConstantExpression constant = Expression.Constant(value);
            BinaryExpression notEqualExpression = Expression.NotEqual(property, constant);

            return Expression.Lambda<Func<T, bool>>(notEqualExpression, parameter);
        }
    }

}
