using DatasourceLoader;
using DatasourceLoader.Filters;
using DatasourceLoader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.DatasourceLoader;

namespace Tests.Filters
{
    public class WhenHandling_dateFilters
    {
        private static List<SampleData> source = new List<SampleData> {
            new SampleData { DateProperty = new DateTime(2020, 11, 5) },
            new SampleData { DateProperty = new DateTime(2020, 10, 5)  },
            new SampleData { DateProperty = new DateTime(2020, 12, 5)  } };
        [Fact]
        public void ShouldApply_equalFilter()
        {
            var date = new DateTime(2020, 12, 5);

            var criteria = new FilterCriteria
            {
                DataType = DataSourceType.DateTime,
                FilterType = FilterType.Equals,
                DateValue = date,
                FieldName = nameof(SampleData.DateProperty)
            };
            DateFilter<SampleData> filter = new(criteria);

            var res = filter.ApplyFilter(source.AsQueryable());

            Assert.Collection(res, (x) =>
            {
                Assert.Equal(date, x.DateProperty);
            });
        }

        [Fact]
        public void ShouldApply_lessThanFilter()
        {
            var date = new DateTime(2020, 11, 5);
            var criteria = new FilterCriteria
            {
                DataType = DataSourceType.DateTime,
                FilterType = FilterType.LessThan,
                DateValue = date,
                FieldName = nameof(SampleData.DateProperty)
            };
            DateFilter<SampleData> filter = new(criteria);
            var res = filter.ApplyFilter(source.AsQueryable());

            Assert.Collection(res, (x) =>
            {
                Assert.Equal(new DateTime(2020, 10, 5), x.DateProperty);
            });
        }

        [Fact]
        public void ShouldApply_lessThanOrEqFilter()
        {
            var date = new DateTime(2020, 11, 5);
            var criteria = new FilterCriteria
            {
                DataType = DataSourceType.DateTime,
                FilterType = FilterType.LessThanOrEqual,
                DateValue = date,
                FieldName = nameof(SampleData.DateProperty)
            };
            DateFilter<SampleData> filter = new(criteria);
            var res = filter.ApplyFilter(source.AsQueryable());

            Assert.Collection(res, (x) =>
            {
                Assert.Equal(new DateTime(2020, 11, 5), x.DateProperty);
            },
            (x) =>
            {
                Assert.Equal(new DateTime(2020, 10, 5), x.DateProperty);
            });
        }
        [Fact]
        public void ShouldApply_gtThanFilter()
        {
            var date = new DateTime(2020, 11, 5);
            var criteria = new FilterCriteria
            {
                DataType = DataSourceType.DateTime,
                FilterType = FilterType.GreaterThan,
                DateValue = date,
                FieldName = nameof(SampleData.DateProperty)
            };

            DateFilter<SampleData> filter = new(criteria);
            var res = filter.ApplyFilter(source.AsQueryable());

            Assert.Collection(res, (x) =>
            {
                Assert.Equal(new DateTime(2020, 12, 5), x.DateProperty);
            });
        }

        [Fact]
        public void ShouldApply_gtThanOrEqualFilter()
        {
            var date = new DateTime(2020, 11, 5);
            var criteria = new FilterCriteria
            {
                DataType = DataSourceType.DateTime,
                FilterType = FilterType.GreaterThanOrEqual,
                DateValue = date,
                FieldName = nameof(SampleData.DateProperty)
            };
            DateFilter<SampleData> filter = new(criteria);
            var res = filter.ApplyFilter(source.AsQueryable());
            Assert.Collection(res, (x) =>
            {
                Assert.Equal(new DateTime(2020, 11, 5), x.DateProperty);
            },
            (x) =>
            {
                Assert.Equal(new DateTime(2020, 12, 5), x.DateProperty);
            });
        }
    }
}
