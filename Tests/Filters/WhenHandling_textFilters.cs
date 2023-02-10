using Dma.DatasourceLoader;
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

    public class WhenHandling_textFilters
    {
        private static List<SampleData> source = new List<SampleData> {
            new SampleData { StrProperty = "Text1" },
            new SampleData { StrProperty = "Text2" },
            new SampleData { StrProperty = "TextText3435Text" }
        };

        [Fact]
        public void ShouldApply_equalFilter()
        {
            var filter = new FilterCriteria { DataType = DataSourceType.Text, FilterType = FilterType.Equals, TextValue = "Text1", FieldName = nameof(SampleData.StrProperty) };

            var res = new TextFilter<SampleData>(filter).ApplyFilter(source.AsQueryable());

            Assert.Collection(res, (x) =>
            {
                Assert.Equal("Text1", x.StrProperty);
            });
        }


        [Fact]
        public void ShouldApply_containsFilter()
        {
            var filter = new FilterCriteria { DataType = DataSourceType.Text, FilterType = FilterType.Contains, TextValue = "text", FieldName = nameof(SampleData.StrProperty) };

            var res = new TextFilter<SampleData>(filter).ApplyFilter(source.AsQueryable());

            Assert.Collection(res, (x) =>
            {
                Assert.Equal("Text1", x.StrProperty);
            },
            (x) =>
            {
                Assert.Equal("Text2", x.StrProperty);
            },
            (x) =>
            {
                Assert.Equal("TextText3435Text", x.StrProperty);
            });

            filter = new FilterCriteria { DataType = DataSourceType.Text, FilterType = FilterType.Contains, TextValue = "text3", FieldName = nameof(SampleData.StrProperty) };
            res = new TextFilter<SampleData>(filter).ApplyFilter(source.AsQueryable());

            Assert.Collection(res,
            (x) =>
            {
                Assert.Equal("TextText3435Text", x.StrProperty);
            });
        }
    }
}
