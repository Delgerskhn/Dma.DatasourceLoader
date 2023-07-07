using Dma.DatasourceLoader;
using Dma.DatasourceLoader.Models;

namespace Tests.Integration
{
    public class DataSourceLoaderTests
    {
        private List<FilterOption> criterias = new List<FilterOption>()
        {
                new FilterOption(nameof(SampleData.DateProperty), "not_equals", new DateTime(2020, 10, 5)),
        };

        private List<OrderOption> orders = new List<OrderOption> {
            new OrderOption(nameof(SampleData.DateProperty),"desc"),
            new OrderOption(nameof(SampleData.IntProperty), "asc")
            };
        private List<SampleData> source = new List<SampleData> {
            new SampleData { DateProperty = new DateTime(2020, 11, 5), IntProperty = 30 },
            new SampleData { DateProperty = new DateTime(2020, 11, 5), IntProperty = 20 },
            new SampleData { DateProperty = new DateTime(2020, 10, 5)  },
            new SampleData { DateProperty = new DateTime(2020, 12, 5)  } };

        [Fact]
        public void ShouldApplyNavigationFilter()
        {
            var filter1 = new FilterOption($"{nameof(SampleData.NestedCollection)}.{nameof(SampleNestedData.StrProperty)}", "contains", "text");
            var filter2 = new FilterOption($"{nameof(SampleData.NestedCollection)}.{nameof(SampleNestedData.IntProperty)}", "in", new int[] { 1, 3 });
            List<FilterOption> filter = new List<FilterOption> { filter1,
                filter2
            };

            List<SampleData> source = new List<SampleData> {
            new SampleData { NestedCollection = new(){ 
                new()
                {
                    StrProperty = "4text5", 
                    IntProperty = 1,
                },
                new()
                {
                    StrProperty = "4text5",
                },
            } },
            new SampleData { DateProperty = new DateTime(2020, 11, 5), IntProperty = 20 },
            new SampleData { DateProperty = new DateTime(2020, 10, 5)  },
            new SampleData { DateProperty = new DateTime(2020, 12, 5)  } };

            var res = DataSourceLoader.Load(source.AsQueryable(), new() { Filters = filter});

            Assert.Single(res);
        }

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
            var options = new DataSourceLoadOptions() { Cursor = 1, Size = 1 };
            var res = DataSourceLoader.Load(source.AsQueryable(), options);

            Assert.Single(res, (x) => x.IntProperty == 20);
        }

        [Fact]
        public void ShouldLoadFilters()
        {
            var res = DataSourceLoader.ApplyFilters
                (source.AsQueryable(), criterias);


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
