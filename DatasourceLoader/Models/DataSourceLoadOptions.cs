namespace Dma.DatasourceLoader.Models
{
    public class DataSourceLoadOptions
    {
        public List<FilterOption> Filters { get; set; } = new();
        public List<OrderOption> Orders { get; set; } = new();
        public int Cursor { get; set; } = 0;
        public int Size { get; set; } = 10;
    }
}
