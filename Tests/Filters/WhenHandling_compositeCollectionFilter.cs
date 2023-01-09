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

    public class WhenHandling_compositeCollectionFilter
    {
        private static List<SampleData> source = new List<SampleData> {
            new SampleData {
                StrCollection = new List<string>(){ "asdf", "wrqw","erty"},
                NestedCollection = new List<SampleNestedData>(){
                    new()
                    {
                        IntProperty = 32,
                        DateProperty = new DateTime(2022, 12,12),
                        StrProperty = "erty",
                    }
                }
            },
            new SampleData { StrCollection = new List<string>(){ "erty", "asdf","qwerty"}  },
            new SampleData { StrCollection = new List<string>(){"qwer", "asdf","zxcv"}  }
        };


        [Fact]
        public void ShouldApply_containsFilterOnTextField()
        {
            var criteria = new FilterCriteria
            {
                DataType = DataSourceType.Collection,
                CollectionDataType = DataSourceType.Text,
                FilterType = FilterType.Contains,
                TextValue = "eRty",
                FieldName = nameof(SampleData.NestedCollection),
                CollectionFieldName = nameof(SampleNestedData.StrProperty)
            };

            var res = new CompositeCollectionFilter<SampleData>(criteria).Contains(source.AsQueryable());

            Assert.Collection(res, (r) =>
            {
                Assert.Equal("asdf", r.StrCollection.First());
            });
        }

        [Fact]
        public void ShouldApply_containsFilterOnNumericField()
        {
            var criteria = new FilterCriteria
            {
                DataType = DataSourceType.Collection,
                CollectionDataType = DataSourceType.Numeric,
                FilterType = FilterType.Contains,
                NumericValue = 32,
                FieldName = nameof(SampleData.NestedCollection),
                CollectionFieldName = nameof(SampleNestedData.IntProperty)
            };

            var res = new CompositeCollectionFilter<SampleData>(criteria).Contains(source.AsQueryable());

            Assert.Collection(res, (r) =>
            {
                Assert.Equal("asdf", r.StrCollection.First());
            });
        }

        [Fact]
        public void ShouldApply_containsFilterOnDateField()
        {
            var criteria = new FilterCriteria
            {
                DataType = DataSourceType.Collection,
                CollectionDataType = DataSourceType.DateTime,
                FilterType = FilterType.Contains,
                DateValue = new DateTime(2022, 12, 12),
                FieldName = nameof(SampleData.NestedCollection),
                CollectionFieldName = nameof(SampleNestedData.DateProperty)
            };

            var res = new CompositeCollectionFilter<SampleData>(criteria).Contains(source.AsQueryable());

            Assert.Collection(res, (r) =>
            {
                Assert.Equal("asdf", r.StrCollection.First());
            });
        }

    }
}
