using System.Linq.Expressions;
using System.Reflection;
using static System.Linq.Expressions.Expression;

namespace Dma.DatasourceLoader.Filters
{
    public class NavigationFilter<T, TProperty> : FilterBase<T>
    {
        private readonly string _navigationProperty;
        private readonly FilterBase<TProperty> _innerFilter;

        public NavigationFilter(string navigationProperty, FilterBase<TProperty> innerFilter)
        {
            _navigationProperty = navigationProperty;
            _innerFilter = innerFilter;
        }

        public override Expression<Func<T, bool>> GetFilterExpression()
        {
            Type elementType = typeof(T);
            PropertyInfo? targetField = elementType.GetProperties().Where(x => x.Name == _navigationProperty).FirstOrDefault();
            ParameterExpression prm = Parameter(elementType);
            //Collection navigation to apply filter
            var collection = Property(prm, targetField);

            //Type of collection item
            var itemType = collection.Type.GetInterfaces()
                .Single(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                .GetGenericArguments()[0];

            LambdaExpression itemPredicate = _innerFilter.GetFilterExpression();

            var body = Call(
                typeof(Enumerable),
                "Any",
                new[] { itemType },
                collection,
                itemPredicate
             );

            Expression<Func<T, bool>> lambda = Lambda<Func<T, bool>>(body, prm);
            return lambda;
        }



    }


}
