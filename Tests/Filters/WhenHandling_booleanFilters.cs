using Dma.DatasourceLoader.Filters;
using Dma.DatasourceLoader.Models;
using Tests.DatasourceLoader;

namespace Tests.Filters
{
    public class WhenHandling_booleanFilters
    {
        private static List<SampleData> source = new List<SampleData> {
            new SampleData { BooleanProperty = true },
            new SampleData { BooleanProperty = true },
            new SampleData { BooleanProperty = false }
        };
        [Fact]
        public void ShouldApply_equalFilter()
        {
            var filter = new FilterCriteria
            {
                DataType = DataSourceType.Boolean,
                FilterType = FilterType.Equals,
                BooleanValue = false,
                FieldName = nameof(SampleData.BooleanProperty)
            };

            var res = new BooleanFilter<SampleData>(filter).ApplyFilter(source.AsQueryable());

            Assert.Collection(res, (x) =>
            {
                Assert.Equal(false, x.BooleanProperty);
            });
        }
    }
}
