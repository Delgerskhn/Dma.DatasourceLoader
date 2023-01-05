using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DatasourceLoader.Filters;
using DatasourceLoader.Models;
using static System.Linq.Expressions.Expression;

namespace DatasourceLoader
{
    public class DataSourceLoader
    {
        public static IQueryable<T> Load<T>(IQueryable<T> query, DataSourceLoadOptions options) where T : class
        {
            query = LoadFilters(query, options.Filters);
            query = ApplyOrders(query, options.Sorter);
            return query;
        }

        private static IQueryable<T> LoadFilters<T>(IQueryable<T> query, List<FilterCriteria> filters)
        {
            //filters.ForEach(f =>
            //{
            //    query = f.Type switch
            //    {
            //        FilterCriteriaType.Text => TextFilters.SingleFieldTextFilter(query, f),
            //        FilterCriteriaType.Eq => f.DateValue != null ? DateFilters.Equal(query, f) : NumericFilters.Equal(query, f),
            //        FilterCriteriaType.GtThan => f.DateValue != null ? DateFilters.GreaterThan(query, f) : NumericFilters.GreaterThan(query, f),
            //        FilterCriteriaType.GtThanOrEq => f.DateValue != null ? DateFilters.GreaterThanOrEqual(query, f) : NumericFilters.GreaterThanOrEqual(query, f),
            //        FilterCriteriaType.LessThan => f.DateValue != null ? DateFilters.LessThan(query, f) : NumericFilters.LessThan(query, f),
            //        FilterCriteriaType.LessThanOrEqual => f.DateValue != null ? DateFilters.LessThanOrEqual(query, f) : NumericFilters.LessThanOrEqual(query, f),
            //        FilterCriteriaType.TextEq => TextFilters.Equal(query, f),
            //        _ => query
            //    };
            //});
            return query;
        }

        private static IQueryable<T> ApplyOrders<T>(IQueryable<T> query, Dictionary<string, string> sorters) where T : class
        {
            Type elementType = typeof(T);
            query.OrderBy(x => elementType.Name);
            PropertyInfo[] props =
                elementType.GetProperties()
                .ToArray();
            foreach (var tuple in sorters.Select((r,i)=>new {r, i}))
            {
                var targetField = props.Where(r => r.Name == tuple.r.Key).FirstOrDefault();
                if(! new string[] {"asc", "desc" }.Contains(tuple.r.Value)) continue;
                string method = tuple.r.Value == "asc" && tuple.i == 0? "OrderBy" : "OrderByDescending";

                if (tuple.r.Value == "desc" && tuple.i > 0) method = "ThenByDescending";
                if (tuple.r.Value == "asc" && tuple.i > 0) method = "ThenBy";

                if (targetField != null)
                {
                    ParameterExpression prm = Parameter(elementType);
                    var propertyToOrderByExpression = Property(prm, targetField);
                    Expression conversion = Expression.Convert(propertyToOrderByExpression, typeof(object));

                    var runMe = Call(
                        typeof(Queryable),
                        method,
                        new Type[] { query.ElementType, typeof(Object) },
                        query.Expression,
                        Lambda<Func<T, Object>>(conversion, new ParameterExpression[] { prm }));
                     query = query.Provider.CreateQuery<T>(runMe);
                }

            }
            return query;
        }

    }
}
