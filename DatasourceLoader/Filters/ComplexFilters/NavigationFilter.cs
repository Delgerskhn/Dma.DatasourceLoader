using System.Linq.Expressions;
using System.Reflection;
using static System.Linq.Expressions.Expression;

namespace Dma.DatasourceLoader.Filters.ComplexFilters
{
    public class NavigationFilter<T> : FilterBase<T>
    {
        private readonly string _navigationProperty;
        private readonly FilterBaseBase _innerFilter;

        public NavigationFilter(string navigationProperty, FilterBaseBase innerFilter) : base(navigationProperty)
        {
            _navigationProperty = navigationProperty;
            _innerFilter = innerFilter;
        }

        public override Expression<Func<T, bool>> GetFilterExpression()
        {
            Type elementType = typeof(T);
            PropertyInfo? targetField = elementType.GetProperties().Where(x => x.Name == _navigationProperty).FirstOrDefault();
            ParameterExpression prm = Parameter(elementType);


            var notNullFilterExpression = new IsNotNullFilter<T>(propertyName).GetFilterExpression();
            var invokeNotNullExpr = Invoke(notNullFilterExpression, prm);


            //Collection navigation to apply filter
            var collection = Property(prm, targetField!);

            //Type of collection item
            var itemType = collection.Type.GetInterfaces()
                .Single(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                .GetGenericArguments()[0];

            LambdaExpression itemPredicate = _innerFilter.GetFilterExpression();

            var anyCall = Call(
                typeof(Enumerable),
                "Any",
                new[] { itemType },
                collection,
                itemPredicate
            );

            var combinedExpression = AndAlso(invokeNotNullExpr, anyCall);

            var lambdaExpression = Lambda<Func<T, bool>>(combinedExpression, prm);

            return lambdaExpression;
        }



    }


}
