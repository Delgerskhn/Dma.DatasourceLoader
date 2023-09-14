using Dma.DatasourceLoader.Analyzer;
using Dma.DatasourceLoader.Filters;

namespace Dma.DatasourceLoader.Creator
{
    public class FilterCreator
    {
        private IFilterAnalyzer analyzer;

        public FilterCreator(IFilterAnalyzer analyzer)
        {
            this.analyzer = analyzer;
        }

        public FilterBaseBase Create()
        {
            FilterBaseBase? expression = null;
            IFilterCreator? lastFilter = null;
            analyzer.GetCreators().ForEach(creator =>
            {
                expression = creator.CreateFilter();
                lastFilter = creator;
            });
            return expression!;
        }
    }
}
