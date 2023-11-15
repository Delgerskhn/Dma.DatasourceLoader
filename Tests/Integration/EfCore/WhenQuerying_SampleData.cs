using Dma.DatasourceLoader;
using Dma.DatasourceLoader.Models;

namespace Tests.Integration.EfCore
{
    public class WhenQuerying_SampleData
    {
        private ApplicationDb db;
        public WhenQuerying_SampleData()
        {
            db = Context.GetRealContext();
            db.SampleDatas.Add(new()
            {
                IntProperty = 1,
                DateProperty = new DateTime(2022, 11, 12),
                NestedCollection = new List<SampleNestedData> {
                    new SampleNestedData(){IntProperty=1, DateProperty=new DateTime(2022,11,14),StrProperty = "Nested1"},
                    new SampleNestedData(){IntProperty=1, DateProperty=new DateTime(2022,11,15),StrProperty = "Nested2"},
                },
                StrProperty = "Apple"
            });
            db.SampleDatas.Add(new()
            {
                IntProperty = 2,
                DateProperty = new DateTime(2022, 12, 12),
                NestedCollection = new List<SampleNestedData> {
                   new SampleNestedData(){IntProperty=2, DateProperty=new DateTime(2022,12,14),StrProperty = "Nested1"},
                   new SampleNestedData(){IntProperty=2, DateProperty=new DateTime(2022,12,15),StrProperty = "Nested2"},
               },
                StrProperty = "Sample"
            });
            db.SampleDatas.Add(new()
            {
                IntProperty = 1,
                DateProperty = new DateTime(2023, 11, 12),
                NestedCollection = new List<SampleNestedData> {
                   new SampleNestedData(){IntProperty=1, DateProperty=new DateTime(2023,11,14),StrProperty = "Nested1"},
                   new SampleNestedData(){IntProperty=1, DateProperty=new DateTime(2023,11,15),StrProperty = "Nested2"},
               },
                StrProperty = "Triple"
            });
            db.SampleDatas.Add(new()
            {
                IntProperty = 1,
                DateProperty = new DateTime(2023, 12, 12),
                NestedCollection = new List<SampleNestedData> {
                   new SampleNestedData(){IntProperty=1, DateProperty=new DateTime(2023, 12,14),StrProperty = "Nested1"},
                   new SampleNestedData(){IntProperty=1, DateProperty=new DateTime(2023, 12,15),StrProperty = "Nested2"},
               },
                StrProperty = "QWErty"
            });
            db.SampleDatas.Add(new()
            {
                IntProperty = 32,
                DateProperty = new DateTime(2024, 12, 12),
                NestedCollection = new List<SampleNestedData> {
                   new SampleNestedData(){IntProperty=1, DateProperty=new DateTime(2024, 12,14),StrProperty = "Nested5",
                   DeepNestedData = new()
                   {
                       StrProperty = "DeepNestedText"
                   }
                   },
                   new SampleNestedData(){IntProperty=1, DateProperty=new DateTime(2024, 12,15),StrProperty = "Nested2"},
               },
                StrProperty = "QWErty2"
            });
            db.SaveChanges();
        }

        [Fact]
        public void ShouldApplyFilterOnNavigationFilters()
        {
            var res = DataSourceLoader.Load(db.SampleDatas, new()
            {
                Filters = new List<FilterRule>
                {
                    new FilterRule($"{nameof(SampleData.NestedCollection)}.{nameof(SampleNestedData.StrProperty)}", FilterOperators.Contains, "Nested1")
                }
            });

            Assert.Equal(4, res.Count());

        }

        [Fact]
        public void ShouldApplyFilterOnDate()
        {
            var res = DataSourceLoader.Load(db.SampleDatas, new()
            {
                Filters = new List<FilterRule>
                {
                    new FilterRule($"{nameof(SampleData.DateProperty)}", FilterOperators.Gte, new DateTime(2023,12,12))

                }
            });

            Assert.Equal(2, res.Count());
        }

        [Fact]
        public void ShouldApplyFilterOnNumber()
        {
            var res = DataSourceLoader.Load(db.SampleDatas, new()
            {
                Filters = new List<FilterRule>
                {
                    new FilterRule($"{nameof(SampleData.IntProperty)}", FilterOperators.Lt, 20)
                }
            });

            Assert.Equal(4, res.Count());
        }

        [Fact]
        public void ShouldApplyFilterOnText()
        {

            var res = DataSourceLoader.Load(db.SampleDatas, new()
            {
                Filters = new List<FilterRule>
                {
                    new FilterRule($"{nameof(SampleData.StrProperty)}", FilterOperators.Contains, "ple")
                }
            });
            Assert.Equal(3, res.Count());

            res = DataSourceLoader.Load(db.SampleDatas, new()
            {
                Filters = new() {
                    new FilterRule($"{nameof(SampleData.StrProperty)}", FilterOperators.Eq, "QWErty")

                }
            });

            Assert.Equal(1, res.Count());
        }

        [Fact]
        public void ShouldApplyFilterOnDeepNestedObject()
        {
            //SampleData.NestedCollection.NestedObject.StrProperty Equals SampleText
            var filter = new FilterRule($"NestedCollection.DeepNestedData.StrProperty", FilterOperators.Contains, "DeepNestedText");
            var res = DataSourceLoader.Load(db.SampleDatas, new()
            {
                Filters = new List<FilterRule>
                {
                    filter
                }
            });
            Assert.Single(res);
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
                Filters = new List<FilterRule>
                {
                    new FilterRule($"NestedCollection.{nameof(SampleNestedData.StrProperty)}", FilterOperators.Contains, "Nested1")
                }
            });

            Assert.Equal(4, res.Count());
        }
    }
}
