using System.Linq.Expressions;

namespace Dma.DatasourceLoader.Filters
{
    public abstract class FilterBase<T>
    {
        public abstract Expression<Func<T, bool>> GetFilterExpression();
    }
}