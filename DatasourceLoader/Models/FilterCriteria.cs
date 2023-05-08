namespace Dma.DatasourceLoader.Models
{
    public class FilterCriteria
    {

        public string FieldName { get; set; } = string.Empty;
        public DataSourceType DataType { get; set; }
        public DataSourceType? CollectionDataType { get; set; }
        public FilterType FilterType { get; set; }
        public DateTime? DateValue { get; set; }
        public string? TextValue { get; set; } = string.Empty;
        public double? NumericValue { get; set; }
        public bool BooleanValue { get; set; }
        public string? CollectionFieldName { get; set; } = string.Empty;
    }
}
