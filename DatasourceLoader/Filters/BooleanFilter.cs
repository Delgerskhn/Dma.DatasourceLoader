using System.Linq.Expressions;
using System.Reflection;
using Dma.DatasourceLoader.Models;
using static System.Linq.Expressions.Expression;

namespace Dma.DatasourceLoader.Filters
{
    public class BooleanFilter<T> : FilterBase<T>
    {
        private PropertyInfo? targetField = null;
        private ParameterExpression prm;
        private UnaryExpression? value = null;
        public BooleanFilter(FilterCriteria criteria) : base(criteria)
        {
            Type elementType = typeof(T);
            prm = Parameter(elementType);
            targetField = elementType.GetProperties().Where(x => x.Name == criteria.FieldName).FirstOrDefault();
            var constant = Constant(criteria.BooleanValue);
            if (targetField != null) value = Convert(constant, targetField.PropertyType);
        }

        public override IQueryable<T> Equal(IQueryable<T> source)
        {
            if (targetField == null || value == null) return source;

            Expression exp = Expression.Equal(
                Property(prm, targetField),
                value
                );

            Expression<Func<T, bool>> lambda = Lambda<Func<T, bool>>(exp, prm);

            return source.Where(lambda);
        }
        public override IQueryable<T> LessThan(IQueryable<T> source)
        {
            throw new NotImplementedException();

        }
        public override IQueryable<T> LessThanOrEqual(IQueryable<T> source)
        {
            throw new NotImplementedException();

        }
        public override IQueryable<T> GreaterThan(IQueryable<T> source)
        {
            throw new NotImplementedException();

        }
        public override IQueryable<T> GreaterThanOrEqual(IQueryable<T> source)
        {
            throw new NotImplementedException();
        }

        public override IQueryable<T> Contains(IQueryable<T> source)
        {
            throw new NotImplementedException();
        }
    }
}
