using Dma.DatasourceLoader.Filters;
using Dma.DatasourceLoader.Models;

namespace Dma.DatasourceLoader.Factory
{
    public abstract class AbstractFilterFactory<T> where T : class
    {
        public abstract FilterBase<T> CreateFilter(FilterOption option);
    }
}
