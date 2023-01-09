using DatasourceLoader;
using DatasourceLoader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.DatasourceLoader;

namespace Tests.Integration.EfCore
{
    public class WhenQuerying_SampleData
    {
        private ApplicationDb db;
        public WhenQuerying_SampleData()
        {
            db = Context.GetContext("SampleDb");
            db.SampleDatas.Add(new()
            {
                IntProperty = 1,
                StrCollection = new List<string> { "Sample1" },
                DateProperty = new DateTime(2022,11,12),
                DateCollection= new List<DateTime> { new DateTime(2022,11,13) },
                NestedCollection= new List<SampleNestedData> { 
                    new SampleNestedData(){IntProperty=1, DateProperty=new DateTime(2022,11,14),StrProperty = "Nested1"},
                    new SampleNestedData(){IntProperty=1, DateProperty=new DateTime(2022,11,15),StrProperty = "Nested2"},
                },
                NumericCollection = new List<int> { 1, 2, 3,},
                StrProperty = "Sample1"
            });
            db.SampleDatas.Add(new()
            {
                IntProperty = 2,
                StrCollection = new List<string> { "Sample2" },
                DateProperty = new DateTime(2022, 12, 12),
                DateCollection = new List<DateTime> { new DateTime(2022, 12, 13) },
                NestedCollection = new List<SampleNestedData> {
                    new SampleNestedData(){IntProperty=2, DateProperty=new DateTime(2022,12,14),StrProperty = "Nested1"},
                    new SampleNestedData(){IntProperty=2, DateProperty=new DateTime(2022,12,15),StrProperty = "Nested2"},
                },
                NumericCollection = new List<int> { 1, 2, 3, },
                StrProperty = "Sample2"
            });
            db.SampleDatas.Add(new()
            {
                IntProperty = 1,
                StrCollection = new List<string> { "Sample3" },
                DateProperty = new DateTime(2023, 11, 12),
                DateCollection = new List<DateTime> { new DateTime(2023, 11, 13) },
                NestedCollection = new List<SampleNestedData> {
                    new SampleNestedData(){IntProperty=1, DateProperty=new DateTime(2023,11,14),StrProperty = "Nested1"},
                    new SampleNestedData(){IntProperty=1, DateProperty=new DateTime(2023,11,15),StrProperty = "Nested2"},
                },
                NumericCollection = new List<int> { 1, 2, 3, },
                StrProperty = "Sample3"
            });
            db.SampleDatas.Add(new()
            {
                IntProperty = 1,
                StrCollection = new List<string> { "Sample4" },
                DateProperty = new DateTime(2023, 12, 12),
                DateCollection = new List<DateTime> { new DateTime(2023, 12, 13) },
                NestedCollection = new List<SampleNestedData> {
                    new SampleNestedData(){IntProperty=1, DateProperty=new DateTime(2023, 12,14),StrProperty = "Nested1"},
                    new SampleNestedData(){IntProperty=1, DateProperty=new DateTime(2023, 12,15),StrProperty = "Nested2"},
                },
                NumericCollection = new List<int> { 1, 2, 3, },
                StrProperty = "Sample3"
            });
            db.SampleDatas.Add(new()
            {
                IntProperty = 1,
                StrCollection = new List<string> { "Sample5" },
                DateProperty = new DateTime(2024, 12, 12),
                DateCollection = new List<DateTime> { new DateTime(2024, 12, 13) },
                NestedCollection = new List<SampleNestedData> {
                    new SampleNestedData(){IntProperty=1, DateProperty=new DateTime(2024, 12,14),StrProperty = "Nested5"},
                    new SampleNestedData(){IntProperty=1, DateProperty=new DateTime(2024, 12,15),StrProperty = "Nested2"},
                },
                NumericCollection = new List<int> { 1, 2, 3, },
                StrProperty = "Sample5"
            });
            db.SaveChanges();
        }

        [Fact]
        public void ShouldApplyFilterOnNavigationFilters()
        {
            var res = DataSourceLoader.Load(db.SampleDatas, new()
            {
                Filters = new List<FilterCriteria>
                {
                    new FilterCriteria()
                    {
                        DataType = DataSourceType.Collection,
                        CollectionDataType = DataSourceType.Text,
                        CollectionFieldName= nameof(SampleNestedData.StrProperty),
                        TextValue = "Nested1",
                        FieldName= nameof(SampleData.NestedCollection),
                        FilterType = FilterType.Contains,
                    }
                }
            });

            Assert.Equal(4, res.Count());

        }
    }
}
