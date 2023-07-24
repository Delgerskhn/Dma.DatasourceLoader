using Dma.DatasourceLoader.Analyzer;
using Dma.DatasourceLoader.Creator;
using Dma.DatasourceLoader.Filters;
using Dma.DatasourceLoader.Models;
using NSubstitute;

namespace Tests.Filters
{
    public class FilterCreatorTests
    {
        [Fact] 
        public void ShouldCreateNestedFilter_inGivenOrder()
        {
            var analyzer = Substitute.For<IFilterAnalyzer>();
            var filter1 = Substitute.For<IFilterCreator>();
            var filter2 = Substitute.For<IFilterCreator>();
            var filter3 = Substitute.For<IFilterCreator>();

            analyzer.GetCreators().Returns(new List<IFilterCreator> {
               filter1,
               filter2,
               filter3
            });
            var creator = new FilterCreator(analyzer);

            FilterBaseBase result = creator.Create();

            Assert.NotNull(result);
            filter1.Received().CreateFilter();
            filter2.Received().CreateFilter();
            filter3.Received().CreateFilter();
        }
    }
}
