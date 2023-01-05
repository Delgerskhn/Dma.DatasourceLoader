using DatasourceLoader.Filters;
using DatasourceLoader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.DatasourceLoader;

namespace Tests
{

    public class WhenHandling_collectionFilter
    {
        private static List<SampleData> source = new List<SampleData> {
            new SampleData {
                StrCollection = new List<string>(){ "asdf", "wrqw","erty"},
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
        public void ShouldApply_containsFilterOnPrimitiveType()
        {
            var criteria = new FilterCriteria {
                DataType = DataSourceType.Collection,
                CollectionFieldDataType = DataSourceType.Text,
                FilterType = FilterType.Contains, 
                TextValue = "erty", 
                FieldName = nameof(SampleData.StrCollection) 
            };

            var res = new CollectionFilter(criteria).TextContains(source.AsQueryable());

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
        public void ShouldApply_containsFilterOnCompositeType()
        {
            var criteria = new FilterCriteria
            {
                DataType = DataSourceType.Collection,
                CollectionFieldDataType = DataSourceType.Text,
                FilterType = FilterType.Contains,
                TextValue = "eRty",
                FieldName = nameof(SampleData.NestedCollection),
                CollectionFieldName = nameof(SampleNestedData.StrProperty)
            };

            var res = new CollectionFilter(criteria).TextFieldContains(source.AsQueryable());

            Assert.Collection(res, (r) =>
            {
                Assert.Equal("asdf", r.StrCollection.First());
            });
        }

        
        public  void Test()
        {
            var res = source.Where(r => r.NestedCollection.Any(r => r.StrProperty == "sdlfkj"));
        }
        public void Test1()
        {
            var res = source.Where(r => r.StrCollection.Any(r => r == "sdlfkj"));
        }
    }
}
