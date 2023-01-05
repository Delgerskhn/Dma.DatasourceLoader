using DatasourceLoader.Filters;
using DatasourceLoader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class WhenHandling_filterFactory
    {
        [Fact]
        public void ItShouldCreateNumericFilter()
        {
            var criteria = new FilterCriteria { DataType = DataSourceType.Numeric };
            var filter = FilterFactory.Create(criteria);
            Assert.IsType<NumericFilter>(filter);
        }
        [Fact]
        public void ItShouldCreateTextFilter()
        {
            var criteria = new FilterCriteria { DataType = DataSourceType.Text };
            var filter = FilterFactory.Create(criteria);
            Assert.IsType<TextFilter>(filter);
        }

        [Fact]
        public void ItShouldCreateDateFilter()
        {
            var criteria = new FilterCriteria { DataType = DataSourceType.DateTime };
            var filter = FilterFactory.Create(criteria);
            Assert.IsType<DateFilter>(filter);
        }

        [Fact]
        public void ItShouldCreateCollectionFilter()
        {
            var criteria = new FilterCriteria { DataType = DataSourceType.Collection };
            var filter = FilterFactory.Create(criteria);
            Assert.IsType<CollectionFilter>(filter);
        }
    }
}
