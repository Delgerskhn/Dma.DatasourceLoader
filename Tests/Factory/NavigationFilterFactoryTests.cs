using Dma.DatasourceLoader.Factory;
using Dma.DatasourceLoader.Filters;
using Dma.DatasourceLoader.Models;

namespace Tests.Factory
{
    public class NavigationFilterFactoryTests
    {
        [Fact]
        public void ItShouldIndicateFilterIsApplicable()
        {
            var option = new FilterOption($"{nameof(SampleData.NestedCollection)}.{nameof(SampleNestedData.StrProperty)}", "equals", "text");
            var res = NavigationFilterFactory<SampleData>.IsApplicable(option);

            Assert.True(res);

            option = new FilterOption($"{nameof(SampleData.NestedData)}.{nameof(SampleNestedData.StrProperty)}", "equals", "text");
            res = NavigationFilterFactory<SampleData>.IsApplicable(option);
            Assert.False(res);

            option = new FilterOption($"{nameof(SampleData.StrProperty)}", "equals", "text");
            res = NavigationFilterFactory<SampleData>.IsApplicable(option);
            Assert.False(res);
        }

        [Fact]
        public void ItShouldCreateFilter()
        {
            var option = new FilterOption($"{nameof(SampleData.NestedCollection)}.{nameof(SampleNestedData.StrProperty)}", "equals", "text");
            var filter = new NavigationFilterFactory<SampleData>().CreateFilter(option);

            Assert.IsType<NavigationFilter<SampleData, SampleNestedData>>(filter);
        }
    }
}
