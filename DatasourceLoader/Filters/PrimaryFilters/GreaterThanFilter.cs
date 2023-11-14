using Dma.DatasourceLoader.Helpers;
using System.Linq.Expressions;

namespace Dma.DatasourceLoader.Filters.PrimaryFilters
{
    public class GreaterThanFilter<T> : FilterBase<T>
    {
        private readonly object value;

        public GreaterThanFilter(string propertyName, object value) : base(propertyName)
        {
            this.value = value;
        }

        public override Expression<Func<T, bool>> GetFilterExpression()
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T));
            MemberExpression property = Expression.Property(parameter, propertyName);
            ConstantExpression constant = property.ConstantForMember(value);
            BinaryExpression greaterThanExpression = Expression.GreaterThan(property, constant);

            return Expression.Lambda<Func<T, bool>>(greaterThanExpression, parameter);
        }
    }

}
