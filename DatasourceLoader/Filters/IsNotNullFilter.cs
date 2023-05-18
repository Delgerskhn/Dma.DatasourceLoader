using System.Linq.Expressions;

namespace Dma.DatasourceLoader.Filters
{
    public class IsNotNullFilter<T> : FilterBase<T>
    {
        private readonly string propertyName;

        public IsNotNullFilter(string propertyName)
        {
            this.propertyName = propertyName;
        }

        public override Expression<Func<T, bool>> GetFilterExpression()
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T));
            MemberExpression property = Expression.Property(parameter, propertyName);
            ConstantExpression nullValue = Expression.Constant(null, typeof(object));
            BinaryExpression isNotNullExpression = Expression.NotEqual(property, nullValue);

            return Expression.Lambda<Func<T, bool>>(isNotNullExpression, parameter);
        }
    }
}
