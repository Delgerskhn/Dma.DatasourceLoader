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
    public class PrimitiveCollectionFilter<T> : FilterBase<T>
    {
        private PropertyInfo? targetField = null;
        private ParameterExpression prm;
        public PrimitiveCollectionFilter(FilterCriteria criteria) : base(criteria)
        {
            Type elementType = typeof(T);
            targetField = elementType.GetProperties().Where(x => x.Name == criteria.FieldName).FirstOrDefault();
            prm = Parameter(elementType);
        }


        public override IQueryable<T> Contains(IQueryable<T> source)
        {
            if (!criteria.CollectionDataType.HasValue) { return source; }

            var collection = Property(prm, targetField);

            //Type of collection item
            var itemType = collection.Type.GetInterfaces()
                .Single(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                .GetGenericArguments()[0];

            
            LambdaExpression itemPredicate = GetPredicate(itemType);

           
            var body = Expression.Call(typeof(Enumerable), "Any", new[] { itemType },
             collection, itemPredicate);

            Expression<Func<T, bool>> lambda = Lambda<Func<T, bool>>(body, prm);

            return source.Where(lambda);
        }

        private LambdaExpression GetPredicate(Type itemType)
        {
            var item = Expression.Parameter(itemType, "e");
            if(criteria.CollectionDataType == DataSourceType.Numeric)
                return Lambda(Expression.Equal(item, Convert(Constant(criteria.NumericValue), itemType)), item);
            if (criteria.CollectionDataType == DataSourceType.DateTime)
            {
                var constant = Constant(criteria.DateValue);
                var value = Convert(constant, typeof(DateTime));
                if (Nullable.GetUnderlyingType(targetField.PropertyType) != null)
                {
                    // It's nullable
                    value = Convert(constant, typeof(DateTime?));
                }
                return Lambda(Expression.Equal(item, value), item);
            }

            return Lambda(
                    Expression.Equal(
                        Call(
                            item, "ToUpper", null
                        ),
                        Constant(criteria.TextValue?.ToUpper() ?? "")
                    ), 
                    item
                );


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
