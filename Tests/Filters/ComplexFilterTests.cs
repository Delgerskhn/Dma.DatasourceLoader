using Dma.DatasourceLoader.Filters;
using Dma.DatasourceLoader.Models;
using Tests.Fixture;

namespace Tests.Filters
{
    public class ComplexFilterTests
    {
        private static List<SampleData> source = new();

        public ComplexFilterTests() {
            source = SampleDataCollection.CreateCollection();
        }


        [Fact]
        public void ItShouldApplyDeepNestedFilter()
        {
            //navigationfilter <- nestedfilter <- equals filter
            var eqFilter = new EqualFilter<DeepNestedData>($"{nameof(SampleData.StrProperty)}", "DeepNestedText");
            var nestedFilter = new NestedFilter<SampleNestedData>(nameof(SampleNestedData.DeepNestedData), eqFilter);
            var navfilter = new NavigationFilter<SampleData>(nameof(SampleData.NestedCollection), nestedFilter);

            var resp = source.AsQueryable().Where(navfilter.GetFilterExpression()).ToList();

            Assert.Single(resp);
        }

        [Fact]
        public void ItShouldApplyNavigationFilter()
        {
            var filter = new ContainsFilter<SampleNestedData>(nameof(SampleNestedData.StrProperty), "Text1");
            var navfilter = new NavigationFilter<SampleData>(nameof(SampleData.NestedCollection), filter);
            var resp = source.AsQueryable().Where(navfilter.GetFilterExpression()).ToList();
            Assert.Single(resp);
        }

        [Fact]
        public void NavigationFilterShouldWorkOnNullFields()
        {
            source[0].NestedCollection = null!;
            var filter = new ContainsFilter<SampleNestedData>(nameof(SampleNestedData.StrProperty), "Text1");
            var navfilter = new NavigationFilter<SampleData>(nameof(SampleData.NestedCollection), filter);
            var resp = source.AsQueryable().Where(navfilter.GetFilterExpression()).ToList();
            Assert.Single(resp);
        }

        [Fact]
        public void ItShouldApplyNestedFilter()
        {
            var filter = new EqualFilter<SampleNestedData>(nameof(SampleNestedData.StrProperty), "Text1");
            //it should generate expression like source.Where(x=>x.NestedData.StrProperty == "Text1")
            var nestedFilter = new NestedFilter<SampleData>(nameof(SampleData.NestedData), filter);

            var resp = source.AsQueryable().Where(nestedFilter.GetFilterExpression()).ToList();

            Assert.Single(resp);
        }

        [Fact]
        public void NestedFilterShouldWorkOnNullFields()
        {
            source[0].NestedData = null!;
            var filter = new EqualFilter<SampleNestedData>(nameof(SampleNestedData.StrProperty), "Text1");
            //it should generate expression like source.Where(x=>x.NestedData.StrProperty == "Text1")
            var nestedFilter = new NestedFilter<SampleData>(nameof(SampleData.NestedData), filter);

            var resp = source.AsQueryable().Where(nestedFilter.GetFilterExpression()).ToList();

            Assert.Single(resp);
        }

    }
}
