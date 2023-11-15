namespace Dma.DatasourceLoader.Models
{
    public class FilterGroup 
    {
        private readonly FilterGroupItem[] items;
        public FilterGroupItem[] Items => items;
        public FilterGroup(FilterGroupItem[] items)
        {
            this.items = items;
        }
    }


    public record FilterExpression
    {
    }

    public record FilterGroupItem : FilterExpression
    {
        public record And(FilterExpression expr) : FilterGroupItem();
        public record Or(FilterExpression expr) : FilterGroupItem();
        public record SubGroup(FilterExpression[] exprs) : FilterGroupItem();
    }
}
