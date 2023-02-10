using Dma.DatasourceLoader;
using Dma.DatasourceLoader.Models;
using NSubstitute;
using NSubstitute.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.DatasourceLoader;

namespace Tests.Integration
{
    public class DataSourceLoaderTests
    {
        private List<FilterCriteria> criterias = new List<FilterCriteria>()
            {
                new FilterCriteria
                {
                    DataType = DataSourceType.DateTime,
                    FilterType = FilterType.GreaterThanOrEqual,
                    DateValue = new DateTime(2020, 11, 5),
                    FieldName = nameof(SampleData.DateProperty)
                }
        };

        private List<OrderCriteria> orders = new List<OrderCriteria> {
            new OrderCriteria{
                Selector = nameof(SampleData.DateProperty),
                Desc = "desc"
            },
            new OrderCriteria{
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
        public void ShouldLoadFilters()
        {
            var res = DataSourceLoader.LoadFilters<SampleData>(source.AsQueryable(), criterias);


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
