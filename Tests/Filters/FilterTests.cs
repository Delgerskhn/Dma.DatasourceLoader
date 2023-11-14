using Dma.DatasourceLoader.Filters;
using Dma.DatasourceLoader.Models;
using Tests.Fixture;

namespace Tests.Filters
{
    public class FilterTests
    {
        private static List<SampleData> source = new();

        public FilterTests() {
            source = SampleDataCollection.CreateCollection();
        }

        [Fact]
        public void ItShouldApplyEqualFilter()
        {
            var filter = new EqualFilter<SampleData>(nameof(SampleData.StrProperty), "Text1");
            var resp = source.AsQueryable().Where(filter.GetFilterExpression()).ToList();
         
            Assert.Single(resp);
        }
      

        [Fact]
        public void ItShouldApplyInFilter()
        {
            var filter = new InFilter<SampleData>(nameof(SampleData.IntProperty), new int[] { 1 });
            var resp = source.AsQueryable().Where(filter.GetFilterExpression()).ToList();

            Assert.Single(resp);
        }

        [Fact]
        public void ItShouldApplyNotInFilter()
        {
            var filter = new NotInFilter<SampleData>(nameof(SampleData.StrProperty), new string[] { "Text1" });
            var resp = source.AsQueryable().Where(filter.GetFilterExpression()).ToList();

            Assert.Equal(3,resp.Count);
        }

        [Fact]
        public void ItShouldApplyGtFilter()
        {
            var filter = new GreaterThanFilter<SampleData>(nameof(SampleData.IntProperty), 1);
            var resp = source.AsQueryable().Where(filter.GetFilterExpression()).ToList();
            Assert.Single(resp);

        }

        [Fact]
        public void ItShouldApplyGteFilter()
        {
            var filter = new GreaterThanOrEqualFilter<SampleData>(nameof(SampleData.IntProperty), 2);
            var resp = source.AsQueryable().Where(filter.GetFilterExpression()).ToList();
            Assert.Single(resp);

        }

        [Fact]
        public void ItShouldApplyLtFilter()
        {
            var filter = new LessThanFilter<SampleData>(nameof(SampleData.IntProperty), 2);
            var resp = source.AsQueryable().Where(filter.GetFilterExpression()).ToList();
            Assert.Equal(3,resp.Count);

        }
        [Fact]
        public void ItShouldApplyLteFilter()
        {
            var filter = new LessThanOrEqualFilter<SampleData>(nameof(SampleData.IntProperty), 1);
            var resp = source.AsQueryable().Where(filter.GetFilterExpression()).ToList();
            Assert.Equal(3,resp.Count);
        }

        [Fact]
        public void ItShouldApplyIsNotNullFilter()
        {
            var filter = new IsNotNullFilter<SampleData>(nameof(SampleData.NestedData));
            source[0].NestedData = null!;
            var resp = source.AsQueryable().Where(filter.GetFilterExpression()).ToList();

            Assert.Equal(3,resp.Count);
        }

        [Fact]
        public void ItShouldApplyIsNullFilter()
        {
            var filter = new IsNullFilter<SampleData>(nameof(SampleData.NestedData));
            source[0].NestedData = null!;
            var resp = source.AsQueryable().Where(filter.GetFilterExpression()).ToList();

            Assert.Single(resp);
        }

        [Fact]
        public void ItShouldApplyNotEqualFilter()
        {
            var filter = new NotEqualFilter<SampleData>(nameof(SampleData.StrProperty), "Text1");
            var resp = source.AsQueryable().Where(filter.GetFilterExpression()).ToList();
            Assert.Equal(3, resp.Count);
        }

    }
}
