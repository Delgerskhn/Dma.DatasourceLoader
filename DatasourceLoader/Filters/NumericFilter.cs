using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Dma.DatasourceLoader.Models;
using static System.Linq.Expressions.Expression;

namespace Dma.DatasourceLoader.Filters
{
    public class NumericFilter<T> : FilterBase<T>
    {
        private PropertyInfo? targetField = null;
        private ParameterExpression prm;
        private UnaryExpression? value = null;
        public NumericFilter(FilterCriteria criteria) : base(criteria)
        {
            Type elementType = typeof(T);
            prm = Parameter(elementType);
            targetField = elementType.GetProperties().Where(x => x.Name == criteria.FieldName).FirstOrDefault();
            var constant = Constant(criteria.NumericValue);
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
