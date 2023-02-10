using Dma.DatasourceLoader.Filters;
using Dma.DatasourceLoader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.DatasourceLoader;

namespace Tests.Filters
{
    public class FilterFactoryTests
    {
        [Fact]
        public void ItShouldCreateNumericFilter()
        {
            var criteria = new FilterCriteria { DataType = DataSourceType.Numeric };
            var filter = FilterFactory.Create<SampleData>(criteria);
            Assert.IsType<NumericFilter<SampleData>>(filter);
        }
        [Fact]
        public void ItShouldCreateTextFilter()
        {
            var criteria = new FilterCriteria { DataType = DataSourceType.Text };
            var filter = FilterFactory.Create<SampleData>(criteria);
            Assert.IsType<TextFilter<SampleData>>(filter);
        }

        [Fact]
        public void ItShouldCreateDateFilter()
        {
            var criteria = new FilterCriteria { DataType = DataSourceType.DateTime };
            var filter = FilterFactory.Create<SampleData>(criteria);
            Assert.IsType<DateFilter<SampleData>>(filter);
        }

        [Fact]
        public void ItShouldCreateCollectionFilter()
        {
            var criteria = new FilterCriteria { DataType = DataSourceType.Collection };
            var filter = FilterFactory.Create<SampleData>(criteria);
            Assert.IsType<CompositeCollectionFilter<SampleData>>(filter);
        }

        [Fact]
        public void ItShouldCreatePrimitiveCollectionFilter()
        {
            var criteria = new FilterCriteria { DataType = DataSourceType.PrimitiveCollection };
            var filter = FilterFactory.Create<SampleData>(criteria);
            Assert.IsType<PrimitiveCollectionFilter<SampleData>>(filter);
        }
    }
}
