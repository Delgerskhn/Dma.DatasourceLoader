using DatasourceLoader.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static System.Linq.Expressions.Expression;

namespace DatasourceLoader.Filters
{
    public class CompositeCollectionFilter<T> : FilterBase<T>
    {
        private PropertyInfo? targetField = null;
        private ParameterExpression prm;
        public CompositeCollectionFilter(FilterCriteria criteria) : base(criteria)
        {
            Type elementType = typeof(T);
            targetField = elementType.GetProperties().Where(x => x.Name == criteria.FieldName).FirstOrDefault();
            prm = Parameter(elementType);
        }

        public override IQueryable<T> Contains(IQueryable<T> source)
        {
            if (targetField == null || criteria.CollectionFieldName == null) return source;

            //Collection navigation to apply filter
            var collection = Property(prm, targetField);

            //Type of collection item
            var itemType = collection.Type.GetInterfaces()
                .Single(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                .GetGenericArguments()[0];

            LambdaExpression itemPredicate = GetPredicate(itemType);

            var body = Call(
                typeof(Enumerable),
                "Any",
                new[] { itemType },
                collection,
                itemPredicate
             );

            Expression<Func<T, bool>> lambda = Lambda<Func<T, bool>>(body, prm);

            return source.Where(lambda);
        }

        private LambdaExpression GetPredicate(Type itemType)
        {
            var item = Parameter(itemType, "e");
            var itemProperty = Property(item, criteria.CollectionFieldName);//StrProperty

            if (criteria.CollectionDataType == DataSourceType.Text)
            {
                return Lambda(Expression.Equal(Call(itemProperty, "ToLower", null), Constant(criteria.TextValue.ToLower())), item);
            }
            if (criteria.CollectionDataType == DataSourceType.DateTime)
            {
                var constant = Constant(criteria.DateValue);
                var value = Convert(constant, typeof(DateTime));
                if (Nullable.GetUnderlyingType(targetField.PropertyType) != null)
                {
                    // It's nullable
                    value = Convert(constant, typeof(DateTime?));
                }
                return Lambda(Expression.Equal(itemProperty, value), item);
            }


            return Lambda(Expression.Equal(itemProperty, Convert(Constant(criteria.NumericValue), itemProperty.Type)), item);
        }

        public override IQueryable<T> Equal(IQueryable<T> source)
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

        public override IQueryable<T> LessThan(IQueryable<T> source)
        {
            throw new NotImplementedException();
        }

        public override IQueryable<T> LessThanOrEqual(IQueryable<T> source)
        {
            throw new NotImplementedException();
        }
    }
}
