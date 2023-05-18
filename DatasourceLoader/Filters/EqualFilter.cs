using System.Linq.Expressions;

namespace Dma.DatasourceLoader.Filters
{
    public class EqualFilter<T> : FilterBase<T>
    {
        private readonly string propertyName;
        private readonly object value;

        public EqualFilter(string propertyName, object value)
        {
            this.propertyName = propertyName;
            this.value = value;
        }

        public override Expression<Func<T, bool>> GetFilterExpression()
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T));
            MemberExpression property = Expression.Property(parameter, propertyName);
            ConstantExpression constant = Expression.Constant(value);
            BinaryExpression equalExpression = Expression.Equal(property, constant);

            return Expression.Lambda<Func<T, bool>>(equalExpression, parameter);
        }
    }
}
