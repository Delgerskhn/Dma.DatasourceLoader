using Dma.DatasourceLoader.Filters;
using Dma.DatasourceLoader.Filters.ComplexFilters;

namespace Dma.DatasourceLoader.Creator
{
    public class NavigationFilterCreator : IFilterCreator
    {
        private readonly string _navigationProperty;
        private readonly IFilterCreator innerFilter;
        private readonly Type _entityType;


        public NavigationFilterCreator(string navigationProperty, IFilterCreator innerFilter, Type entityType)
        {
            _navigationProperty = navigationProperty;
            this.innerFilter = innerFilter;
            _entityType = entityType;
        }

        public FilterBaseBase CreateFilter()
        {


            var innerFilterInstance = innerFilter.CreateFilter();

            Type navigationFilterType = typeof(NavigationFilter<>).MakeGenericType(_entityType);
            var filter = (FilterBaseBase)Activator.CreateInstance(navigationFilterType, _navigationProperty, innerFilterInstance)!;

            return filter;
        }


    }
}
