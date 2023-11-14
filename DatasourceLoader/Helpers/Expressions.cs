using System.Linq.Expressions;
using System.Reflection;

namespace Dma.DatasourceLoader.Helpers
{
    public class Expressions
    {
        public static Expression CastNonNullable(MemberExpression property)
        {
            Expression propertyValue = property;
            if (property.Type.IsGenericType && property.Type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                // Handle nullable int properties
                MethodInfo? getValueOrDefaultMethod = property.Type.GetMethod("GetValueOrDefault", Type.EmptyTypes);

                propertyValue = Expression.Call(property, getValueOrDefaultMethod!);
            }

            return propertyValue;
        }
    }
}
