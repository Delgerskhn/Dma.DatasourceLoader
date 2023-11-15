using Dma.DatasourceLoader.Creator;
using Dma.DatasourceLoader.Models;
using System.Linq.Expressions;

namespace Dma.DatasourceLoader.Analyzer
{


    public class FilterGroupAnalyzer<T> : IFilterAnalyzer where T:class
    {
        private FilterGroup filterGroup;

        public FilterGroupAnalyzer(FilterGroup filterGroup)
        {
            this.filterGroup = filterGroup;
        }

        public List<IFilterCreator> GetCreators()
        {
            throw new NotImplementedException();
        }
    }
} 