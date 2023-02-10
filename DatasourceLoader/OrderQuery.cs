using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Linq.Expressions.Expression;

namespace Dma.DatasourceLoader
{
    public class OrderQuery
    {
        public static IQueryable<T> ApplyOrders<T>(IQueryable<T> query, Dictionary<string, string> sorters) where T : class
        {
            Type elementType = typeof(T);
            query.OrderBy(x => elementType.Name);
            PropertyInfo[] props =
                elementType.GetProperties()
                .ToArray();
            foreach (var tuple in sorters.Select((r, i) => new { r, i }))
            {
                var targetField = props.Where(r => r.Name == tuple.r.Key).FirstOrDefault();
                if (!new string[] { "asc", "desc" }.Contains(tuple.r.Value)) continue;
                string method = tuple.r.Value == "asc" && tuple.i == 0 ? "OrderBy" : "OrderByDescending";

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
