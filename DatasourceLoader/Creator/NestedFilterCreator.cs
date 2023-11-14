using Dma.DatasourceLoader.Filters;
using Dma.DatasourceLoader.Filters.ComplexFilters;

namespace Dma.DatasourceLoader.Creator
{
    public class NestedFilterCreator : IFilterCreator
    {
        private readonly string _innerProperty;
        private readonly IFilterCreator innerFilter;
        private readonly Type entityType;

        public NestedFilterCreator(IFilterCreator innerFilter, string innerProperty, Type entityType)
        {
            this.innerFilter = innerFilter;
            _innerProperty = innerProperty;
            this.entityType = entityType;
        }


        public FilterBaseBase CreateFilter()
        {
            var innerFilterInstance = innerFilter.CreateFilter();

            Type nestedFilterType = typeof(NestedFilter<>).MakeGenericType(entityType);
            var filter = (FilterBaseBase)Activator.CreateInstance(nestedFilterType, _innerProperty, innerFilterInstance)!;

            return filter;
        }
    }

}
