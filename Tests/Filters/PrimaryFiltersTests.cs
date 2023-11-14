using Dma.DatasourceLoader.Filters.PrimaryFilters;
using Dma.DatasourceLoader.Models;
using Tests.Fixture;

namespace Tests.Filters
{
    public class PrimaryFiltersTests
    {
        private static List<SampleData> source = new();

        public PrimaryFiltersTests() {
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
        public void ItShouldApplyGtFilter()
        {
            int? value = 1;
            var filter = new GreaterThanFilter<SampleData>(nameof(SampleData.IntProperty), value);
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

            Assert.Equal(5,resp.Count);
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
        public void ItShouldApplyIsNullFilter_onNullableFields()
        {
            var filter = new IsNullFilter<SampleData>(nameof(SampleData.NullableStringProperty));
            var resp = source.AsQueryable().Where(filter.GetFilterExpression()).ToList();

            Assert.Equal(5, resp.Count);
        }

        [Fact]
        public void ItShouldApplyNotEqualFilter()
        {
            var filter = new NotEqualFilter<SampleData>(nameof(SampleData.IntProperty), -20);
            var resp = source.AsQueryable().Where(filter.GetFilterExpression()).ToList();
            Assert.Equal(5, resp.Count);
        }

    }
}
