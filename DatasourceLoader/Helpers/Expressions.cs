using System.Linq.Expressions;
using System.Reflection;

namespace Dma.DatasourceLoader.Helpers
{
    public static class Expressions
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

        public static ConstantExpression ConstantForMember(this MemberExpression expression, object value)
        {
            Type expressionType = expression.Type;

            if (expressionType.IsGenericType && expressionType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                var underlyingType = Nullable.GetUnderlyingType(expressionType);
                value = Convert.ChangeType(value, underlyingType!);
            }

            return Expression.Constant(value, expression.Type);
        }
    }
}
