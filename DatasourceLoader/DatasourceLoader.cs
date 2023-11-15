using System.Linq.Expressions;
using System.Reflection;
using Dma.DatasourceLoader.Analyzer;
using Dma.DatasourceLoader.Creator;
using Dma.DatasourceLoader.Models;
using static System.Linq.Expressions.Expression;

namespace Dma.DatasourceLoader
{
    public class DataSourceLoader
    {
        public static IQueryable<T> Load<T>(IQueryable<T> query, DataSourceLoadOptions options) where T : class
        {
            query = ApplyFilters(query, options.Filters);
            query = ApplyOrders(query, options.Orders);
            query = ApplyPagination(query, options.Cursor, options.Size);

            return query;
        }

        public static IQueryable<T> ApplyPagination<T>(IQueryable<T> query, int pageIndex, int size)
        {
            if (pageIndex < 0)
            {
                throw new ArgumentException("PageIndex cannot be negative.");
            }

            if (size <= 0)
            {
                throw new ArgumentException("Size must be greater than zero.");
            }

            int skipCount = pageIndex * size;
            return query.Skip(skipCount).Take(size);
        }

        public static IQueryable<T> ApplyFilters<T>(IQueryable<T> query, List<FilterRule> filters) where T : class
        {
            foreach(var f in filters)
            {
                var analyzer = new FilterAnalyzer<T>(f);
                var filter = new FilterCreator(analyzer).Create();

                query = query.Where((Expression<Func<T,bool>>)filter.GetFilterExpression());
            }
            return query;
        }

        public static IQueryable<T> ApplyOrders<T>(IQueryable<T> query, List<OrderOption> sorters) where T : class
        {
            Type elementType = typeof(T);
            query.OrderBy(x => elementType.Name);
            PropertyInfo[] props =
                elementType.GetProperties()
                .ToArray();
            foreach (var tuple in sorters.Select((r, i) => new { r, i }))
            {
                var targetField = props.Where(r => r.Name == tuple.r.Selector).FirstOrDefault();
                if (!new string[] { "asc", "desc" }.Contains(tuple.r.Desc)) continue;
                string method = tuple.r.Desc == "asc" && tuple.i == 0 ? "OrderBy" : "OrderByDescending";

                if (tuple.r.Desc == "desc" && tuple.i > 0) method = "ThenByDescending";
                if (tuple.r.Desc == "asc" && tuple.i > 0) method = "ThenBy";

                if (targetField != null)
                {
                    ParameterExpression prm = Parameter(elementType);
                    var propertyToOrderByExpression = Property(prm, targetField);
                    Expression conversion = Convert(propertyToOrderByExpression, typeof(object));

                    var runMe = Call(
                        typeof(Queryable),
                        method,
                        new Type[] { query.ElementType, typeof(object) },
                        query.Expression,
                        Lambda<Func<T, object>>(conversion, new ParameterExpression[] { prm }));
                    query = query.Provider.CreateQuery<T>(runMe);
                }

            }
            return query;
        }

    }
}
