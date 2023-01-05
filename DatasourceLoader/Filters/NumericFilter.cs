using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using DatasourceLoader.Models;
using static System.Linq.Expressions.Expression;

namespace DatasourceLoader.Filters
{
    public class NumericFilter : FilterBase
    {
        public NumericFilter(FilterCriteria criteria) : base(criteria)
        {
        }

        public override IQueryable<T> Equal<T>(IQueryable<T> source)
        {
            Type elementType = typeof(T);

            var targetField = elementType.GetProperties().Where(x => x.Name == criteria.FieldName).FirstOrDefault();

            if (targetField == null) return source;
            ParameterExpression prm = Parameter(elementType);
            var constant = Constant(criteria.NumericValue);
            var value = Convert(constant, targetField.PropertyType);
            Expression exp = Expression.Equal(
                Property(prm, targetField),
                value
                );

            Expression<Func<T, bool>> lambda = Lambda<Func<T, bool>>(exp, prm);

            return source.Where(lambda);
        }
        public override IQueryable<T> LessThan<T>(IQueryable<T> source)
        {
            Type elementType = typeof(T);

            var targetField = elementType.GetProperties().Where(x => x.Name == criteria.FieldName).FirstOrDefault();

            if (targetField == null) return source;
            ParameterExpression prm = Parameter(elementType);
            var constant = Constant(criteria.NumericValue);
            var value = Convert(constant, targetField.PropertyType);
            Expression exp = Expression.LessThan(
                Property(prm, targetField),
                value
                );

            Expression<Func<T, bool>> lambda = Lambda<Func<T, bool>>(exp, prm);

            return source.Where(lambda);
        }
        public override IQueryable<T> LessThanOrEqual<T>(IQueryable<T> source)
        {
            Type elementType = typeof(T);

            var targetField = elementType.GetProperties().Where(x => x.Name == criteria.FieldName).FirstOrDefault();

            if (targetField == null) return source;
            ParameterExpression prm = Parameter(elementType);
            var constant = Constant(criteria.NumericValue);
            var value = Convert(constant, targetField.PropertyType);
            Expression exp = Expression.LessThanOrEqual(
                Property(prm, targetField),
                value
                );

            Expression<Func<T, bool>> lambda = Lambda<Func<T, bool>>(exp, prm);

            return source.Where(lambda);
        }
        public override IQueryable<T> GreaterThan<T>(IQueryable<T> source)
        {
            Type elementType = typeof(T);

            var targetField = elementType.GetProperties().Where(x => x.Name == criteria.FieldName).FirstOrDefault();

            if (targetField == null) return source;
            ParameterExpression prm = Parameter(elementType);
            var constant = Constant(criteria.NumericValue);
            var value = Convert(constant, targetField.PropertyType);
            Expression exp = Expression.GreaterThan(
                Property(prm, targetField),
                value
                );

            Expression<Func<T, bool>> lambda = Lambda<Func<T, bool>>(exp, prm);

            return source.Where(lambda);
        }
        public override IQueryable<T> GreaterThanOrEqual<T>(IQueryable<T> source)
        {
            Type elementType = typeof(T);

            var targetField = elementType.GetProperties().Where(x => x.Name == criteria.FieldName).FirstOrDefault();

            if (targetField == null) return source;
            ParameterExpression prm = Parameter(elementType);
            var constant = Constant(criteria.NumericValue);
            var value = Convert(constant, targetField.PropertyType);
            Expression exp = Expression.GreaterThanOrEqual(
                Property(prm, targetField),
                value
                );

            Expression<Func<T, bool>> lambda = Lambda<Func<T, bool>>(exp, prm);

            return source.Where(lambda);
        }

        public override IQueryable<T> Contains<T>(IQueryable<T> source)
        {
            throw new NotImplementedException();
        }
    }
}
