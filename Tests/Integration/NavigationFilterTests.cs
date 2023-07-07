using Dma.DatasourceLoader.Filters;
using Dma.DatasourceLoader.Models;

namespace Tests.Integration
{
    public class NavigationFilterTests
    {

        [Fact]
        public void ItShouldApplyContainsFilter_onPropertyOfNavigationProperty()
        {
            var filter1 = new FilterOption($"{nameof(SampleData.NestedCollection)}.{nameof(SampleNestedData.StrProperty)}", "contains", "text");


            //var filter = new NavigationFilter<SampleData, SampleNestedData>(nameof());
        }
    }
}
