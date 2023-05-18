namespace Dma.DatasourceLoader.Models
{
    public class DataSourceLoadOptions
    {
        public List<FilterOption> Filters { get; set; } = new();
        public List<OrderOption> Orders { get; set; } = new();
        public int Cursor { get; set; }
        public int Size { get; set; }
    }
}
