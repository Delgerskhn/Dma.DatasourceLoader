namespace Dma.DatasourceLoader.Models
{
    public record FilterGroup : FilterExpression
    {
        private readonly FilterExpression[] expressions;
        public FilterExpression[] Expressions => expressions;
        public FilterGroup(FilterExpression[] expressions)
        {
            this.expressions = expressions;
        }

        public record And(FilterExpression expr) : FilterExpression();
        public record Or(FilterExpression expr) : FilterExpression();
    }


    public record FilterExpression
    {
    }
}
