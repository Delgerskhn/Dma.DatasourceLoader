using Dma.DatasourceLoader.Filters.ComplexFilters;
using Dma.DatasourceLoader.Filters.PrimaryFilters;
using Dma.DatasourceLoader.Filters.StringFilters;
using Dma.DatasourceLoader.Models;
using System.Linq.Expressions;
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
        public void ShouldApplyAndFilter()
        {
            var filter = new GreaterThanOrEqualFilter<SampleData>("IntProperty", 1);
            var filter1 = new ContainsFilter<SampleData>("NullableStringProperty", "null");
            var andExpr = new AndFilter<SampleData>(filter, filter1).GetFilterExpression();

            var result = source.AsQueryable().Where((Expression<Func<SampleData, bool>>)andExpr);
            Assert.Single(result);
        }

        [Fact]
        public void ShouldApplyOrFilter()
        {
            var filter1 = new EqualFilter<SampleData>("IntProperty", 1);
            var filter2 = new EqualFilter<SampleData>("IntProperty", 0);
            var filter3 = new EqualFilter<SampleData>("IntProperty", -20);

            var or1 = new OrFilter<SampleData>(filter1, filter2);
            var or2 = new OrFilter<SampleData>(or1, filter3);

            var result = source.AsQueryable().Where((Expression<Func<SampleData, bool>>)or2.GetFilterExpression());

            Assert.Equal(3, result.Count());
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
