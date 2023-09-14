using System.Linq.Expressions;

namespace Dma.DatasourceLoader.Filters
{
    public abstract class FilterBaseBase
    {
        public abstract LambdaExpression GetFilterExpression();
    }
}