using Dma.DatasourceLoader.Filters;

namespace Dma.DatasourceLoader.Creator
{
    public interface IFilterCreator
    {
        FilterBaseBase CreateFilter();
    }
}
