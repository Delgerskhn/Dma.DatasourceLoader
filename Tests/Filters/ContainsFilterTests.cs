using Dma.DatasourceLoader.Filters;
using Dma.DatasourceLoader.Models;

namespace Tests.Filters
{
    public class ContainsFilterTests
    {
        private static List<SampleData> source = new();
        public ContainsFilterTests()
        {
            source = new List<SampleData> {
                new SampleData { StrProperty = "Text1", IntProperty = 1 },
                new SampleData { StrProperty = "Text2" , IntProperty = 2},
                new SampleData {
                    StrProperty = "TextText3435Text",
                    NestedData = new(){

                        StrProperty = "Text1"
                    }
                },
                new SampleData { NestedCollection = new List<SampleNestedData>(){
                    new()
                    {
                        StrProperty = "Text1"
                    }
                } },

            };
        }


        [Fact]
        public void ItShouldApplyContainsFilter()
        {
            var filter = new ContainsFilter<SampleData>(nameof(SampleData.StrProperty), "Text1");
            var resp = source.AsQueryable().Where(filter.GetFilterExpression()).ToList();

            Assert.Single(resp);
        }

        [Fact]
        public void ItShouldApplyNotContainsFilter()
        {
            var filter = new NotContainsFilter<SampleData>(nameof(SampleData.StrProperty), "Text1");
            var resp = source.AsQueryable().Where(filter.GetFilterExpression()).ToList();

            Assert.Equal(3, resp.Count);
        }
    }
}
