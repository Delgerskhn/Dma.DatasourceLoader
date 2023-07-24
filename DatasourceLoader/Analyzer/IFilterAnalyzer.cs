using Dma.DatasourceLoader.Creator;

namespace Dma.DatasourceLoader.Analyzer
{
    public interface IFilterAnalyzer
    {
        List<IFilterCreator> GetCreators();
    }
}