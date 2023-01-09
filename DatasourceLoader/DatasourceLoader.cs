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
            query = ApplyOrders(query, options.Orders);
            return query;
        }

        public static IQueryable<T> LoadFilters<T>(IQueryable<T> query, List<FilterCriteria> filters)
        {
            var builder = new FilterBuilder<T>(filters, query);
            foreach (var f in builder.build()) query = f.ApplyFilter(query);
            return query;
        }

        public static IQueryable<T> ApplyOrders<T>(IQueryable<T> query, List<(string, string)> sorters) where T : class
        {
            Type elementType = typeof(T);
            query.OrderBy(x => elementType.Name);
            PropertyInfo[] props =
                elementType.GetProperties()
                .ToArray();
            foreach (var tuple in sorters.Select((r,i)=>new {r, i}))
            {
                var targetField = props.Where(r => r.Name == tuple.r.Item1).FirstOrDefault();
                if(! new string[] {"asc", "desc" }.Contains(tuple.r.Item2)) continue;
                string method = tuple.r.Item2 == "asc" && tuple.i == 0? "OrderBy" : "OrderByDescending";

                if (tuple.r.Item2 == "desc" && tuple.i > 0) method = "ThenByDescending";
                if (tuple.r.Item2 == "asc" && tuple.i > 0) method = "ThenBy";

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
