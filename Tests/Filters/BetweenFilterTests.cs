using Dma.DatasourceLoader.Filters;
using Dma.DatasourceLoader.Models;
using Tests.Fixture;

namespace Tests.Filters
{
    public class BetweenFilterTests
    {
        [Fact]
        public void ShouldApplyBetweenFilterOnDate()
        {
            var data = SampleDataCollection.CreateCollection();
            var min = new DateTime(2023, 1, 10);
            var max = new DateTime(2023, 2, 5);

            var filter = new BetweenFilter<SampleData>("DateProperty", (min, max));

            var filterExpression = filter.GetFilterExpression();

            var filteredData = data.AsQueryable().Where(filterExpression);

            Assert.Equal(1, filteredData.Count());
        }
    }
}
