using Dma.DatasourceLoader.Models;

namespace Tests
{
    public class FilterGroupAnalyzerTests
    {

        [Fact]
        public void ShouldGroupAndFilters()
        {
            var group = new FilterGroup(new FilterGroupItem[] { 
                new FilterGroupItem.And(new FilterRule("StrProperty", FilterOperators.StartsWith, "Text")),
                new FilterGroupItem.Or(new FilterGroupItem.SubGroup(
                    new FilterGroupItem[] {
                        new FilterGroupItem.And(new FilterRule("IntProperty", FilterOperators.Eq, 1)),
                        new FilterGroupItem.And(new FilterRule("StrProperty", FilterOperators.Contains, "Text"))
                    }
                ))
            });

            var analyzer = new FilterGroupAnalyzer<SampleData>(group);

            var expr = analyzer.Analyze();

            //Assert correct condition was composed 
            //x=> x.StrProperty.StartsWith("Text") || (x.IntProperty == 1 && x.StrProperty.Contains("Text"))
            
        }

        [Fact]
        public void ShouldGroupOrFilters() { }
    }
}
