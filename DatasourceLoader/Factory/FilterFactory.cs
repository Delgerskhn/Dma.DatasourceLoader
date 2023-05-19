using Dma.DatasourceLoader.Filters;
using Dma.DatasourceLoader.Models;

namespace Dma.DatasourceLoader.Factory
{
    public class FilterFactory<T> where T : class
    {

        public AbstractFilterFactory<T> Create(FilterOption option)
        {
            if(NestedFilterFactory<T>.IsApplicable(option)) return new NestedFilterFactory<T>();
            if(NavigationFilterFactory<T>.IsApplicable(option)) return new NavigationFilterFactory<T>(); 

            return new PrimaryFilterFactory<T>();
        }
    }
}
