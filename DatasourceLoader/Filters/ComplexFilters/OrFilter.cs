using System.Linq.Expressions;

namespace Dma.DatasourceLoader.Filters.ComplexFilters
{
    public class OrFilter<T> : Filter
    {
        private readonly Filter left;
        private readonly Filter right;

        public OrFilter(Filter left, Filter right)
        {
            this.left = left;
            this.right = right;
        }

        public override LambdaExpression GetFilterExpression()
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T));
            var leftExpr = left.GetFilterExpression();
            var invokeExpression = Expression.Invoke(leftExpr, parameter);
            var rightExpr = right.GetFilterExpression();
            var rinvokeExpression = Expression.Invoke(rightExpr, parameter);
            // Combine the two expressions with AndAlso
            BinaryExpression andExpression = Expression.OrElse(
                invokeExpression,
                rinvokeExpression
            );

            return Expression.Lambda<Func<T, bool>>(andExpression, parameter);
        }
    }
}
