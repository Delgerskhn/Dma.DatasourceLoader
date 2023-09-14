using Dma.DatasourceLoader.Filters;
using Dma.DatasourceLoader.Models;

namespace Tests.Filters
{
    public class FilterTests
    {
        private static List<SampleData> source = new();

        public FilterTests() {
            source = new List<SampleData> {
                new SampleData { StrProperty = "Text1", IntProperty = 1 },
                new SampleData { StrProperty = "Text2" , IntProperty = 2},
                new SampleData {
                    StrProperty = "TextText3435Text",
                    NestedData = new(){
                        DeepNestedData = new()
                        {
                            StrProperty = "DeepNestedText"
                        },
                        StrProperty = "Text1"
                    },
                    
                },
                new SampleData { 
                    NestedCollection = new List<SampleNestedData>(){
                    new()
                    {
                        DeepNestedData = new()
                        {
                            StrProperty = "DeepNestedText"
                        },
                        StrProperty = "Text1"
                    }
                } },

            };
        }

        [Fact]
        public void ItShouldApplyEqualFilter()
        {
            var filter = new EqualFilter<SampleData>(nameof(SampleData.StrProperty), "Text1");
            var resp = source.AsQueryable().Where(filter.GetFilterExpression()).ToList();
         
            Assert.Single(resp);
        }

        [Fact]
        public void ItShouldApplyDeepNestedFilter()
        {
            //navigationfilter <- nestedfilter <- equals filter
            var eqFilter = new EqualFilter<DeepNestedData>($"{nameof(SampleData.StrProperty)}", "DeepNestedText");
            var nestedFilter = new NestedFilter<SampleNestedData>(nameof(SampleNestedData.DeepNestedData), eqFilter);
            var navfilter = new NavigationFilter<SampleData>(nameof(SampleData.NestedCollection), nestedFilter);

            var resp = source.AsQueryable().Where(navfilter.GetFilterExpression()).ToList();

            Assert.Single(resp);
        }

        [Fact]
        public void ItShouldApplyNavigationFilter()
        {
            var filter = new ContainsFilter<SampleNestedData>(nameof(SampleNestedData.StrProperty), "Text1");
            var navfilter = new NavigationFilter<SampleData>(nameof(SampleData.NestedCollection), filter);
            var resp = source.AsQueryable().Where(navfilter.GetFilterExpression()).ToList();
            Assert.Single(resp);
        }

        [Fact]
        public void NavigationFilterShouldWorkOnNullFields()
        {
            source[0].NestedCollection = null!;
            var filter = new ContainsFilter<SampleNestedData>(nameof(SampleNestedData.StrProperty), "Text1");
            var navfilter = new NavigationFilter<SampleData>(nameof(SampleData.NestedCollection), filter);
            var resp = source.AsQueryable().Where(navfilter.GetFilterExpression()).ToList();
            Assert.Single(resp);
        }

        [Fact]
        public void ItShouldApplyNestedFilter()
        {
            var filter = new EqualFilter<SampleNestedData>(nameof(SampleNestedData.StrProperty), "Text1");
            //it should generate expression like source.Where(x=>x.NestedData.StrProperty == "Text1")
            var nestedFilter = new NestedFilter<SampleData>(nameof(SampleData.NestedData), filter);

            var resp = source.AsQueryable().Where(nestedFilter.GetFilterExpression()).ToList();

            Assert.Single(resp);
        }

        [Fact]
        public void NestedFilterShouldWorkOnNullFields()
        {
            source[0].NestedData = null!;
            var filter = new EqualFilter<SampleNestedData>(nameof(SampleNestedData.StrProperty), "Text1");
            //it should generate expression like source.Where(x=>x.NestedData.StrProperty == "Text1")
            var nestedFilter = new NestedFilter<SampleData>(nameof(SampleData.NestedData), filter);

            var resp = source.AsQueryable().Where(nestedFilter.GetFilterExpression()).ToList();

            Assert.Single(resp);
        }

        [Fact]
        public void ItShouldApplyEndsWithFilter()
        {
            var filter = new EndsWithFilter<SampleData>(nameof(SampleData.StrProperty), "ext1");
            var resp = source.AsQueryable().Where(filter.GetFilterExpression()).ToList();
            Assert.Single(resp);

        }

      

        [Fact]
        public void ItShouldApplyInFilter()
        {
            var filter = new InFilter<SampleData>(nameof(SampleData.IntProperty), new int[] { 1 });
            var resp = source.AsQueryable().Where(filter.GetFilterExpression()).ToList();

            Assert.Single(resp);
        }

        [Fact]
        public void ItShouldApplyNotInFilter()
        {
            var filter = new NotInFilter<SampleData>(nameof(SampleData.StrProperty), new string[] { "Text1" });
            var resp = source.AsQueryable().Where(filter.GetFilterExpression()).ToList();

            Assert.Equal(3,resp.Count);
        }

        [Fact]
        public void ItShouldApplyGtFilter()
        {
            var filter = new GreaterThanFilter<SampleData>(nameof(SampleData.IntProperty), 1);
            var resp = source.AsQueryable().Where(filter.GetFilterExpression()).ToList();
            Assert.Single(resp);

        }

        [Fact]
        public void ItShouldApplyGteFilter()
        {
            var filter = new GreaterThanOrEqualFilter<SampleData>(nameof(SampleData.IntProperty), 2);
            var resp = source.AsQueryable().Where(filter.GetFilterExpression()).ToList();
            Assert.Single(resp);

        }

        [Fact]
        public void ItShouldApplyLtFilter()
        {
            var filter = new LessThanFilter<SampleData>(nameof(SampleData.IntProperty), 2);
            var resp = source.AsQueryable().Where(filter.GetFilterExpression()).ToList();
            Assert.Equal(3,resp.Count);

        }
        [Fact]
        public void ItShouldApplyLteFilter()
        {
            var filter = new LessThanOrEqualFilter<SampleData>(nameof(SampleData.IntProperty), 1);
            var resp = source.AsQueryable().Where(filter.GetFilterExpression()).ToList();
            Assert.Equal(3,resp.Count);

        }

        [Fact]
        public void ItShouldApplyIsNotNullFilter()
        {
            var filter = new IsNotNullFilter<SampleData>(nameof(SampleData.NestedData));
            source[0].NestedData = null!;
            var resp = source.AsQueryable().Where(filter.GetFilterExpression()).ToList();

            Assert.Equal(3,resp.Count);
        }

        [Fact]
        public void ItShouldApplyIsNullFilter()
        {
            var filter = new IsNullFilter<SampleData>(nameof(SampleData.NestedData));
            source[0].NestedData = null!;
            var resp = source.AsQueryable().Where(filter.GetFilterExpression()).ToList();

            Assert.Single(resp);
        }

        [Fact]
        public void ItShouldApplyNotEqualFilter()
        {
            var filter = new NotEqualFilter<SampleData>(nameof(SampleData.StrProperty), "Text1");
            var resp = source.AsQueryable().Where(filter.GetFilterExpression()).ToList();
            Assert.Equal(3, resp.Count);
        }


        [Fact]
        public void ItShouldApplyStartsWithFilter()
        {
            var filter = new StartsWithFilter<SampleData>(nameof(SampleData.StrProperty), "Text1");
            var resp = source.AsQueryable().Where(filter.GetFilterExpression()).ToList();
            Assert.Single(resp);
        }
    }
}
