using System.Linq.Expressions;

namespace Dma.DatasourceLoader.Filters
{
    public class IsNullFilter<T> : FilterBase<T>
    {
        private readonly string propertyName;

        public IsNullFilter(string propertyName)
        {
            this.propertyName = propertyName;
        }

        public override Expression<Func<T, bool>> GetFilterExpression()
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T));
            MemberExpression property = Expression.Property(parameter, propertyName);
            ConstantExpression nullValue = Expression.Constant(null, typeof(object));
            BinaryExpression isNullExpression = Expression.Equal(property, nullValue);

            return Expression.Lambda<Func<T, bool>>(isNullExpression, parameter);
        }
    }
}
