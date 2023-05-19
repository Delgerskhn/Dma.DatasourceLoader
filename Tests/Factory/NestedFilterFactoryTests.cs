using Dma.DatasourceLoader.Factory;
using Dma.DatasourceLoader.Filters;
using Dma.DatasourceLoader.Models;

namespace Tests.Factory
{
    public class NestedFilterFactoryTests
    {

        [Fact]
        public void ItShouldIndicateNestedFilterIsApplicable ()
        {
            var option = new FilterOption($"{nameof(SampleData.NestedData)}.{nameof(SampleNestedData.StrProperty)}", "equals", "text");
            var res = NestedFilterFactory<SampleData>.IsApplicable(option);

            Assert.True(res);

            option = new FilterOption($"{nameof(SampleData.NestedCollection)}.{nameof(SampleNestedData.StrProperty)}", "equals", "text");
            res = NestedFilterFactory<SampleData>.IsApplicable(option);
            Assert.False(res);

            option = new FilterOption($"{nameof(SampleData.StrProperty)}", "equals", "text");
            res = NestedFilterFactory<SampleData>.IsApplicable(option);
            Assert.False(res);
        }


        [Fact]
        public void ItShouldCreateNestedFilter()
        {
            var option = new FilterOption($"{nameof(SampleData.NestedData)}.{nameof(SampleNestedData.StrProperty)}", "equals", "text");
            var filter = new NestedFilterFactory<SampleData>().CreateFilter(option);

            Assert.IsType<NestedFilter<SampleData, SampleNestedData>>(filter);
        }
    }
}
