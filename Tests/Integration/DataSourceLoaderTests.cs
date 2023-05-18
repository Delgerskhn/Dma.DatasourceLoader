using Dma.DatasourceLoader;
using Dma.DatasourceLoader.Models;
using Tests.DatasourceLoader;

namespace Tests.Integration
{
    public class DataSourceLoaderTests
    {
        private List<FilterOption> criterias = new List<FilterOption>()
            {
                //new FilterOption()
        };

        private List<OrderOption> orders = new List<OrderOption> {
            new OrderOption{
                Selector = nameof(SampleData.DateProperty),
                Desc = "desc"
            },
            new OrderOption{
                Selector = nameof(SampleData.IntProperty),
                Desc = "asc"
            }

            };
        private List<SampleData> source = new List<SampleData> {
            new SampleData { DateProperty = new DateTime(2020, 11, 5), IntProperty = 30 },
            new SampleData { DateProperty = new DateTime(2020, 11, 5), IntProperty = 20 },
            new SampleData { DateProperty = new DateTime(2020, 10, 5)  },
            new SampleData { DateProperty = new DateTime(2020, 12, 5)  } };

        [Fact]
        public void ShouldApplyBothFiltersAndOrders()
        {
            var options = new DataSourceLoadOptions() { Filters = criterias, Orders = orders };

            var res = DataSourceLoader.Load(source.AsQueryable(), options);

            Assert.Collection(res,
                (r) =>
                {
                    Assert.Equal(new DateTime(2020, 12, 5), r.DateProperty);
                },
                (r) =>
                {
                    Assert.Equal(new DateTime(2020, 11, 5), r.DateProperty);
                    Assert.Equal(20, r.IntProperty);
                },
                (r) =>
                {
                    Assert.Equal(new DateTime(2020, 11, 5), r.DateProperty);
                    Assert.Equal(30, r.IntProperty);
                });
        }

        [Fact]
        public void ShouldApplyPagination()
        {
            Assert.False(true);
        }

        [Fact]
        public void ShouldLoadFilters()
        {
            var res = DataSourceLoader.ApplyFilters<SampleData>(source.AsQueryable(), criterias);


            Assert.Collection(res, (r) =>
            {
                Assert.Equal(new DateTime(2020, 11, 5), r.DateProperty);
            },
            (r) =>
            {
                Assert.Equal(new DateTime(2020, 11, 5), r.DateProperty);
            },
            (r) =>
            {
                Assert.Equal(new DateTime(2020, 12, 5), r.DateProperty);
            });
        }

        [Fact]
        public void ShouldApplyOrder()
        {

            var res = DataSourceLoader.ApplyOrders<SampleData>(source.AsQueryable(), orders);
            Assert.Collection(res, (r) =>
            {
                Assert.Equal(new DateTime(2020, 12, 5), r.DateProperty);
            },
            (r) =>
            {
                Assert.Equal(new DateTime(2020, 11, 5), r.DateProperty);
                Assert.Equal(20, r.IntProperty);
            },
            (r) =>
            {
                Assert.Equal(new DateTime(2020, 11, 5), r.DateProperty);
                Assert.Equal(30, r.IntProperty);
            },
            (r) =>
            {
                Assert.Equal(new DateTime(2020, 10, 5), r.DateProperty);
            });
        }


    }
}
