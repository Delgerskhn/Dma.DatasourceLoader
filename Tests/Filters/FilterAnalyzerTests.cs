using Dma.DatasourceLoader.Models;
using Dma.DatasourceLoader.Analyzer;

namespace Tests.Filters
{
    public class FilterAnalyzerTests
    {

        [Fact]
        public void ItShouldAnalyzeExistenceOfPropertiesRecursively_inFilterOption()
        {

            var option1 = new FilterOption($"" +
                $"{nameof(SampleData.NestedCollection)}." +
                $"{nameof(SampleNestedData.DeepNestedData)}." +
                $"{nameof(DeepNestedData.StrProperty)}", "equals", "text");

            
            var analyzer =  new FilterAnalyzer<SampleData>(option1);

            Assert.NotNull(analyzer);

            var option2 = new FilterOption($"{nameof(SampleData.NestedCollection)}.{nameof(SampleNestedData.DateProperty)}.{nameof(DeepNestedData.StrProperty)}", "equals", "text");

            Assert.Throws<MissingMemberException>(() => new FilterAnalyzer<SampleData>(option2));

        }

        [Fact]
        public void ItShouldPopulateCreators_inNestedOrder()
        {
            var option1 = new FilterOption($"" +
                $"{nameof(SampleData.NestedCollection)}." +
                $"{nameof(SampleNestedData.DeepNestedData)}." +
                $"{nameof(DeepNestedData.StrProperty)}", "equals", "text");
            var creators = new FilterAnalyzer<SampleData>(option1).GetCreators();

            Assert.Equal(3, creators.Count);
        }
    }
}
