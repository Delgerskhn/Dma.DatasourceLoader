using System.Linq.Expressions;
using System.Reflection;

namespace Dma.DatasourceLoader.Filters
{

    public class NotInFilter<T> : FilterBase<T>
    {
        private readonly IEnumerable<object> values;

        public NotInFilter(string propertyName, object values) : base(propertyName)
        {
            if (values is Array array && array.GetType().GetElementType() != typeof(object))
            {
                Type elementType = array.GetType().GetElementType();
                this.values = array.Cast<object>();
                return;
            }
            // Handle the case when the object is not an array or when it's already of type IEnumerable<object>
            this.values = values as IEnumerable<object>;
        }

        public override Expression<Func<T, bool>> GetFilterExpression()
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T));
            MemberExpression property = Expression.Property(parameter, propertyName);



            var castedType = typeof(Enumerable)
                .GetMethod("Cast")
                .MakeGenericMethod(property.Type)
                .Invoke(null, new object[] { values });

            ConstantExpression valuesArray = Expression.Constant(castedType);
            MethodInfo containsMethod = typeof(Enumerable).GetMethods()
            .Where(m => m.Name == "Contains" && m.GetParameters().Length == 2)
            .Single()
            .MakeGenericMethod(property.Type);
            MethodCallExpression containsExpression = Expression.Call(null, containsMethod, valuesArray, property);
            UnaryExpression notContainsExpression = Expression.Not(containsExpression);

            return Expression.Lambda<Func<T, bool>>(notContainsExpression, parameter);
        }
    }
}
