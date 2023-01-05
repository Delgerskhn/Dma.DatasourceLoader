using DatasourceLoader.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Linq.Expressions.Expression;

namespace DatasourceLoader.Filters
{
    public class CollectionFilter : FilterBase
    {
        public CollectionFilter(FilterCriteria criteria) : base(criteria)
        {
        }

        public override IQueryable<T> Contains<T>(IQueryable<T> source)
        {
            if (!criteria.CollectionFieldDataType.HasValue) { return source; }



            if (criteria.CollectionFieldDataType.Value == DataSourceType.Text)
            {
                return TextContains(source);
            }
            throw new NotImplementedException();
        }

        public IQueryable<T> TextContains<T>(IQueryable<T> source)
        {
            if (string.IsNullOrEmpty(criteria.TextValue)) { return source; }
            Type elementType = typeof(T);
            var targetField = elementType.GetProperties().Where(x => x.Name == criteria.FieldName).FirstOrDefault();
            if (targetField == null) return source;

            ParameterExpression prm = Parameter(elementType);

            Expression<Func<string, bool>> predicate =
                a => a.ToLower() == criteria.TextValue;
            var body = Expression.Call(typeof(Enumerable), "Any", new[] { typeof(string) },
             Property(prm, targetField), predicate);

            Expression<Func<T, bool>> lambda = Lambda<Func<T, bool>>(body, prm);

            return source.Where(lambda);
        }

        public IQueryable<T> TextFieldContains<T>(IQueryable<T> source)
        {
            if (string.IsNullOrEmpty(criteria.TextValue)) { return source; }
            if (criteria.CollectionFieldName == null) return source;
            Type elementType = typeof(T);
            var targetField = elementType.GetProperties().Where(x => x.Name == criteria.FieldName).FirstOrDefault();
            if (targetField == null) return source;

            ParameterExpression prm = Parameter(elementType);
            var collection = Property(prm, targetField);//NestedCollection - ICollection<SampleNestedData>

            var itemType = collection.Type.GetInterfaces()
                .Single(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                .GetGenericArguments()[0];//SampleNestedData

            var item = Expression.Parameter(itemType, "e");
            var itemProperty = Expression.Property(item, criteria.CollectionFieldName);//StrProperty

          
            var itemPredicate = Lambda(Expression.Equal(Call(itemProperty, "ToLower", null), Constant(criteria.TextValue.ToLower())) , item);

            var body = Expression.Call(
                typeof(Enumerable),
                "Any",
                new[] { itemType },
                collection,
                itemPredicate
             );

            Expression<Func<T, bool>> lambda = Lambda<Func<T, bool>>(body, prm);

            return source.Where(lambda);
        }

        public override IQueryable<T> Equal<T>(IQueryable<T> source)
        {
            throw new NotImplementedException();
        }

        public override IQueryable<T> GreaterThan<T>(IQueryable<T> source)
        {
            throw new NotImplementedException();
        }

        public override IQueryable<T> GreaterThanOrEqual<T>(IQueryable<T> source)
        {
            throw new NotImplementedException();
        }

        public override IQueryable<T> LessThan<T>(IQueryable<T> source)
        {
            throw new NotImplementedException();
        }

        public override IQueryable<T> LessThanOrEqual<T>(IQueryable<T> source)
        {
            throw new NotImplementedException();
        }
    }
}
