using Dma.DatasourceLoader;
using Dma.DatasourceLoader.Analyzer;
using Dma.DatasourceLoader.Filters.ComplexFilters;
using Dma.DatasourceLoader.Models;

namespace Tests
{
    public class FilterGroupAnalyzerTests
    {

        [Fact]
        public void ShouldAnalyzeAndFilterCreator()
        {
            var group = new FilterGroup(
                new FilterExpression[] { 
                    new FilterGroup.And(new FilterRule("StrProperty", FilterOperators.StartsWith, "Text")),
                    new FilterGroup.And(new FilterRule("IntProperty", FilterOperators.Eq, 1))
                });

            var analyzer = new FilterGroupAnalyzer<SampleData>(group);

            var andCreator = analyzer.GetCreators();

            Assert.IsType<AndFilterCreator>(andCreator);
        }

        [Fact]
        public void ShouldAnalyzeOrFilterCreator() { 
            var group = new FilterGroup(
                new FilterExpression[] { 
                    new FilterGroup.And(new FilterRule("StrProperty", FilterOperators.StartsWith, "Text")),
                    new FilterGroup.Or(new FilterRule("IntProperty", FilterOperators.Eq, 1))
                });

            var analyzer = new FilterGroupAnalyzer<SampleData>(group);

            var andCreator = analyzer.GetCreators();

            Assert.IsType<OrFilterCreator>(andCreator);

        }

        [Fact]
        public void ShouldAnalyzeSubgroupFilterCreator() {
            var group = new FilterGroup(
                new FilterExpression[] { 
                    new FilterGroup.And(
                        new FilterRule("IntProperty", FilterOperators.Eq, 1)
                    ),
                    new FilterGroup.Or(
                        new FilterGroup(new FilterExpression[]{
                            new FilterGroup.And(new FilterRule("StrProperty", FilterOperators.Contains, "Text"))
                        })
                    ),
                });
            var analyzer = new FilterGroupAnalyzer<SampleData>(group);
            var orFilter = analyzer.GetCreators();

            Assert.IsType<OrFilterCreator>(orFilter);
        }

        [Fact]
        public void ShouldThrowError_ifGroupsAreNestedWithoutLogicOperator() {
             var group = new FilterGroup(
                new FilterExpression[] { 
                    new FilterGroup.And(
                        new FilterRule("IntProperty", FilterOperators.Eq, 1)
                    ),
                    new FilterGroup(new FilterExpression[]{
                            new FilterGroup.And(new FilterRule("StrProperty", FilterOperators.Contains, "Text"))
                    })
                });
            var analyzer = new FilterGroupAnalyzer<SampleData>(group);
            Assert.Throws<NotSupportedException>(()=>{
                analyzer.GetCreators();
            });
        }
    }
}
