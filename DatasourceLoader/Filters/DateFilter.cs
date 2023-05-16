using System.Linq.Expressions;
using System.Reflection;
using Dma.DatasourceLoader.Models;
using static System.Linq.Expressions.Expression;

namespace Dma.DatasourceLoader.Filters
{
    public class DateFilter<T> : FilterBase<T>
    {
        private PropertyInfo? targetField = null;
        private ParameterExpression prm;
        private UnaryExpression? value = null;
        public DateFilter(FilterCriteria criteria) : base(criteria)
        {
            Type elementType = typeof(T);
            targetField = elementType.GetProperties().Where(x => x.Name == criteria.FieldName).FirstOrDefault();
            prm = Parameter(elementType);
            if (targetField != null) value = getDateValue(criteria.DateValue, targetField);

        }

        private UnaryExpression getDateValue(DateTime? date, PropertyInfo targetField)
        {
            var constant = Constant(date);
            var value = Convert(constant, typeof(DateTime));
            if (Nullable.GetUnderlyingType(targetField.PropertyType) != null)
            {
                // It's nullable
                value = Convert(constant, typeof(DateTime?));
            }
            return value;
        }
        public override IQueryable<T> NotEqual(IQueryable<T> source)
        {
            if (targetField == null || value == null) return source;

            Expression exp = Expression.NotEqual(
                Property(prm, targetField),
                value
                );

            Expression<Func<T, bool>> lambda = Lambda<Func<T, bool>>(exp, prm);

            return source.Where(lambda);
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
            if (targetField == null || value == null) return source;

            Expression exp = Expression.LessThan(
                Property(prm, targetField),
                value
                );

            Expression<Func<T, bool>> lambda = Lambda<Func<T, bool>>(exp, prm);

            return source.Where(lambda);
        }
        public override IQueryable<T> LessThanOrEqual(IQueryable<T> source)
        {
            if (targetField == null || value == null) return source;

            Expression exp = Expression.LessThanOrEqual(
                Property(prm, targetField),
                value
                );

            Expression<Func<T, bool>> lambda = Lambda<Func<T, bool>>(exp, prm);

            return source.Where(lambda);
        }
        public override IQueryable<T> GreaterThan(IQueryable<T> source)
        {
            if (targetField == null || value == null) return source;

            Expression exp = Expression.GreaterThan(
                Property(prm, targetField),
                value
                );

            Expression<Func<T, bool>> lambda = Lambda<Func<T, bool>>(exp, prm);

            return source.Where(lambda);
        }
        public override IQueryable<T> GreaterThanOrEqual(IQueryable<T> source)
        {
            if (targetField == null || value == null) return source;

            Expression exp = Expression.GreaterThanOrEqual(
                Property(prm, targetField),
                value
                );

            Expression<Func<T, bool>> lambda = Lambda<Func<T, bool>>(exp, prm);

            return source.Where(lambda);
        }

        public override IQueryable<T> Contains(IQueryable<T> source)
        {
            throw new NotImplementedException();
        }


    }
}
