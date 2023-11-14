using Dma.DatasourceLoader.Filters.StringFilters;
using Dma.DatasourceLoader.Models;
using Tests.Fixture;

namespace Tests.Filters
{
    public class StringFilterTests
    {
        private static List<SampleData> source = new();

        public StringFilterTests() {
            source = SampleDataCollection.CreateCollection();
        }



        [Fact]
        public void ItShouldApplyContainsFilter()
        {
            var filter = new ContainsFilter<SampleData>(nameof(SampleData.StrProperty), "Text1");
            var resp = source.AsQueryable().Where(filter.GetFilterExpression()).ToList();

            Assert.Single(resp);
        }

        [Fact]
        public void ItShouldApplyContainsFilter_onNullableFields()
        {
            var filter = new ContainsFilter<SampleData>(nameof(SampleData.NullableStringProperty), "null");
            var resp = source.AsQueryable().Where(filter.GetFilterExpression()).ToList();

            Assert.Single(resp);
        }

        [Fact]
        public void ItShouldApplyNotContainsFilter()
        {
            var filter = new NotContainsFilter<SampleData>(nameof(SampleData.StrProperty), "Text1");
            var resp = source.AsQueryable().Where(filter.GetFilterExpression()).ToList();

            Assert.Equal(5, resp.Count);
        }

        [Fact]
        public void ItShouldApplyNotContainsFilterOnNullableFields()
        {
            var filter = new NotContainsFilter<SampleData>(nameof(SampleData.NullableStringProperty), "null");
            var resp = source.AsQueryable().Where(filter.GetFilterExpression()).ToList();

            Assert.Equal(5,resp.Count);
        }

        [Fact]
        public void ItShouldApplyEndsWithFilter()
        {
            var filter = new EndsWithFilter<SampleData>(nameof(SampleData.StrProperty), "ext1");
            var resp = source.AsQueryable().Where(filter.GetFilterExpression()).ToList();
            Assert.Single(resp);

        }


        [Fact]
        public void ItShouldApplyEndsWithFilter_onNullableFields()
        {
            var filter = new EndsWithFilter<SampleData>(nameof(SampleData.NullableStringProperty), "able");
            var resp = source.AsQueryable().Where(filter.GetFilterExpression()).ToList();
            Assert.Single(resp);

        }



        [Fact]
        public void ItShouldApplyStartsWithFilter()
        {
            var filter = new StartsWithFilter<SampleData>(nameof(SampleData.StrProperty), "Text1");
            var resp = source.AsQueryable().Where(filter.GetFilterExpression()).ToList();
            Assert.Single(resp);
        }




        [Fact]
        public void ItShouldApplyStartsWithFilter_onNullableFields()
        {
            var filter = new StartsWithFilter<SampleData>(nameof(SampleData.NullableStringProperty), "null");
            var resp = source.AsQueryable().Where(filter.GetFilterExpression()).ToList();
            Assert.Single(resp);
        }
    }
}
