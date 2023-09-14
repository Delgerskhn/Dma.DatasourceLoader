using System.Linq.Expressions;

namespace Dma.DatasourceLoader.Filters
{
    public class EqualFilter<T> : FilterBase<T>
    {
        private readonly object _value;
        public EqualFilter(string propertyName, object value) : base(propertyName)
        {
            _value = value;
        }

        public override Expression<Func<T, bool>> GetFilterExpression()
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T));
            MemberExpression property = Expression.Property(parameter, propertyName);
            ConstantExpression constant = Expression.Constant(_value);
            BinaryExpression equalExpression = Expression.Equal(property, constant);

            return Expression.Lambda<Func<T, bool>>(equalExpression, parameter);
        }
    }
}
