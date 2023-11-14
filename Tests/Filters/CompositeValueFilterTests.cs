using Dma.DatasourceLoader.Filters;
using Dma.DatasourceLoader.Models;
using Tests.Fixture;

namespace Tests.Filters
{
    public class CompositeValueFilterTests
    {
        private List<SampleData> source = SampleDataCollection.CreateCollection();


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

            Assert.Equal(3, resp.Count);
        }

        [Fact]
        public void ShouldApplyBetweenFilterOnDate()
        {
            var min = new DateTime(2023, 1, 10);
            var max = new DateTime(2023, 2, 5);

            var filter = new BetweenFilter<SampleData>("DateProperty", (min, max));

            var filterExpression = filter.GetFilterExpression();

            var filteredData = source.AsQueryable().Where(filterExpression);

            Assert.Equal(1, filteredData.Count());
        }

    }
}
