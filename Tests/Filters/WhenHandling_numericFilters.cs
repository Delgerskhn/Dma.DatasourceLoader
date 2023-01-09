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
    public class WhenHandling_numericFilters
    {
        private static List<SampleData> source = new List<SampleData> {
            new SampleData { IntProperty = 42 },
            new SampleData { IntProperty = 1 },
            new SampleData { IntProperty = 25 } };
        [Fact]
        public void ShouldApply_equalFilter()
        {
            var filter = new FilterCriteria
            {
                DataType = DataSourceType.Numeric,
                FilterType = FilterType.Equals,
                NumericValue = 42,
                FieldName = nameof(SampleData.IntProperty)
            };

            var res = new NumericFilter<SampleData>(filter).ApplyFilter(source.AsQueryable());

            Assert.Collection(res, (x) =>
            {
                Assert.Equal(42, x.IntProperty);
            });
        }

        [Fact]
        public void ShouldApply_lessThanFilter()
        {
            var filter = new FilterCriteria
            {
                DataType = DataSourceType.Numeric,
                FilterType = FilterType.LessThan,
                NumericValue = 25,
                FieldName = nameof(SampleData.IntProperty)
            };
            var res = new NumericFilter<SampleData>(filter).ApplyFilter(source.AsQueryable());

            Assert.Collection(res, (x) =>
            {
                Assert.Equal(1, x.IntProperty);
            });
        }

        [Fact]
        public void ShouldApply_lessThanOrEqFilter()
        {
            var filter = new FilterCriteria
            {
                DataType = DataSourceType.Numeric,
                FilterType = FilterType.LessThanOrEqual,
                NumericValue = 25,
                FieldName = nameof(SampleData.IntProperty)
            };
            var res = new NumericFilter<SampleData>(filter).ApplyFilter(source.AsQueryable());

            Assert.Collection(res, (x) =>
            {
                Assert.Equal(1, x.IntProperty);
            },
            (x) =>
            {
                Assert.Equal(25, x.IntProperty);
            });
        }
        [Fact]
        public void ShouldApply_gtThanFilter()
        {
            var filter = new FilterCriteria
            {
                DataType = DataSourceType.Numeric,
                FilterType = FilterType.GreaterThan,
                NumericValue = 25,
                FieldName = nameof(SampleData.IntProperty)
            };
            var res = new NumericFilter<SampleData>(filter).ApplyFilter(source.AsQueryable());

            Assert.Collection(res, (x) =>
            {
                Assert.Equal(42, x.IntProperty);
            });
        }

        [Fact]
        public void ShouldApply_gtThanOrEqualFilter()
        {
            var filter = new FilterCriteria
            {
                DataType = DataSourceType.Numeric,
                FilterType = FilterType.GreaterThanOrEqual,
                NumericValue = 25,
                FieldName = nameof(SampleData.IntProperty)
            };
            var res = new NumericFilter<SampleData>(filter).ApplyFilter(source.AsQueryable());

            Assert.Collection(res, (x) =>
            {
                Assert.Equal(42, x.IntProperty);
            },
            (x) =>
            {
                Assert.Equal(25, x.IntProperty);
            });
        }
    }
}
