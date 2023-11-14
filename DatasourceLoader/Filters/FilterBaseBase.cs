using System.Linq.Expressions;

namespace Dma.DatasourceLoader.Filters
{
    public abstract class Filter
    {
        public abstract LambdaExpression GetFilterExpression();
    }
}