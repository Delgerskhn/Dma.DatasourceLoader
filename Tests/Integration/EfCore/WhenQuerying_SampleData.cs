using Dma.DatasourceLoader;
using Dma.DatasourceLoader.Models;

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
                DateProperty = new DateTime(2022, 11, 12),
                DateCollection = new List<DateTime> { new DateTime(2022, 11, 13) },
                NestedCollection = new List<SampleNestedData> {
                    new SampleNestedData(){IntProperty=1, DateProperty=new DateTime(2022,11,14),StrProperty = "Nested1"},
                    new SampleNestedData(){IntProperty=1, DateProperty=new DateTime(2022,11,15),StrProperty = "Nested2"},
                },
                NumericCollection = new List<int> { 1, 2, 3, },
                StrProperty = "Apple"
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
                StrProperty = "Sample"
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
                StrProperty = "Triple"
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
                StrProperty = "QWErty"
            });
            db.SampleDatas.Add(new()
            {
                IntProperty = 32,
                StrCollection = new List<string> { "Sample5" },
                DateProperty = new DateTime(2024, 12, 12),
                DateCollection = new List<DateTime> { new DateTime(2024, 12, 13) },
                NestedCollection = new List<SampleNestedData> {
                    new SampleNestedData(){IntProperty=1, DateProperty=new DateTime(2024, 12,14),StrProperty = "Nested5"},
                    new SampleNestedData(){IntProperty=1, DateProperty=new DateTime(2024, 12,15),StrProperty = "Nested2"},
                },
                NumericCollection = new List<int> { 1, 2, 3, },
                StrProperty = "QWErty2"
            });
            db.SaveChanges();
        }

        [Fact]
        public void ShouldApplyFilterOnNavigationFilters()
        {
            var res = DataSourceLoader.Load(db.SampleDatas, new()
            {
                Filters = new List<FilterOption>
                {
                    new FilterOption($"{nameof(SampleData.NestedCollection)}.{nameof(SampleNestedData.StrProperty)}", "contains", "Nested1")
                }
            });

            Assert.Equal(4, res.Count());

        }

        [Fact]
        public void ShouldApplyFilterOnDate()
        {
            var res = DataSourceLoader.Load(db.SampleDatas, new()
            {
                Filters = new List<FilterOption>
                {
                    new FilterOption($"{nameof(SampleData.DateProperty)}", "gte", new DateTime(2023,12,12))

                }
            });

            Assert.Equal(2, res.Count());
        }

        [Fact]
        public void ShouldApplyFilterOnNumber()
        {
            var res = DataSourceLoader.Load(db.SampleDatas, new()
            {
                Filters = new List<FilterOption>
                {
                    new FilterOption($"{nameof(SampleData.IntProperty)}", "lt", 20)
                }
            });

            Assert.Equal(4, res.Count());
        }

        [Fact]
        public void ShouldApplyFilterOnText()
        {

            var res = DataSourceLoader.Load(db.SampleDatas, new()
            {
                Filters = new List<FilterOption>
                { 
                    new FilterOption($"{nameof(SampleData.StrProperty)}", "contains", "ple")
                }
            });
            Assert.Equal(3, res.Count());

            res = DataSourceLoader.Load(db.SampleDatas, new()
            {
                Filters = new() { 
                    new FilterOption($"{nameof(SampleData.StrProperty)}", "equals", "QWErty")

                }
            });

            Assert.Equal(1, res.Count());
        }

        [Fact]
        public void ShouldApplyFilterOnProjectedCollection()
        {
            var query = db.SampleDatas.Select(r => new 
            {
                r.NestedCollection
            });

            var res = DataSourceLoader.Load(query, new()
            {
                Filters = new List<FilterOption>
                { 
                    new FilterOption($"NestedCollection.{nameof(SampleNestedData.StrProperty)}", "contains", "Nested1") 
                }
            });

            Assert.Equal(4, res.Count());
        }
    }
}
