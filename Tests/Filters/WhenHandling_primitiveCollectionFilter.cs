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

    public class WhenHandling_primitiveCollectionFilter
    {
        FilterCriteria criteria;

        public WhenHandling_primitiveCollectionFilter()
        {
            this.criteria = new FilterCriteria
            {
                DataType = DataSourceType.Collection,
                FilterType = FilterType.Contains,
                TextValue = "erty",
                DateValue = new DateTime(2022, 12, 12),
                NumericValue = 32,
                FieldName = nameof(SampleData.StrCollection)
            };
        }

        private static List<SampleData> source = new List<SampleData> {
            new SampleData {
                StrCollection = new List<string>(){ "asdf", "wrqw","erty"},
                DateCollection = new List<DateTime>(){ new DateTime(2022,12,12)},
                NumericCollection = new List<int>(){ 32 },
                NestedCollection = new List<SampleNestedData>(){
                    new()
                    {
                        StrProperty = "erty",
                    }
                }
            },
            new SampleData { StrCollection = new List<string>(){ "erty", "asdf","qwerty"}  },
            new SampleData { StrCollection = new List<string>(){"qwer", "asdf","zxcv"}  }
        };

        [Fact]
        public void ShouldApply_containsFilterOnText()
        {
            criteria.CollectionDataType = DataSourceType.Text;
            criteria.FieldName = nameof(SampleData.StrCollection);
            var res = new PrimitiveCollectionFilter<SampleData>(criteria).Contains(source.AsQueryable());

            Assert.Collection(res, (r) =>
            {
                Assert.Equal("asdf", r.StrCollection.First());
            },
            (r) =>
            {
                Assert.Equal("erty", r.StrCollection.First());
            });
        }

        [Fact]
        public void ShouldApply_containsFilterOnDate()
        {
            criteria.FieldName = nameof(SampleData.DateCollection);
            criteria.CollectionDataType = DataSourceType.DateTime;
            var res = new PrimitiveCollectionFilter<SampleData>(criteria).Contains(source.AsQueryable());
            Assert.Collection(res, (r) =>
            {
                Assert.Equal("asdf", r.StrCollection.First());
            }
            );
        }

        [Fact]
        public void ShouldApply_containsFilterOnNumber()
        {
            criteria.FieldName = nameof(SampleData.NumericCollection);
            criteria.CollectionDataType = DataSourceType.Numeric;
            var res = new PrimitiveCollectionFilter<SampleData>(criteria).Contains(source.AsQueryable());
            Assert.Collection(res, (r) =>
            {
                Assert.Equal("asdf", r.StrCollection.First());
            }
            );
        }

    }
}
