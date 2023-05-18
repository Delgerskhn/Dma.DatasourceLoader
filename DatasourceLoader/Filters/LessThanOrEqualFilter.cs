using System.Linq.Expressions;

namespace Dma.DatasourceLoader.Filters
{
    public class LessThanOrEqualFilter<T> : FilterBase<T>
    {
        private readonly string propertyName;
        private readonly object value;

        public LessThanOrEqualFilter(string propertyName, object value)
        {
            this.propertyName = propertyName;
            this.value = value;
        }

        public override Expression<Func<T, bool>> GetFilterExpression()
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T));
            MemberExpression property = Expression.Property(parameter, propertyName);
            ConstantExpression constant = Expression.Constant(value);
            BinaryExpression lessThanExpression = Expression.LessThanOrEqual(property, constant);

            return Expression.Lambda<Func<T, bool>>(lessThanExpression, parameter);
        }
    }

}
