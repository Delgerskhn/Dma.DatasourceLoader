namespace Dma.DatasourceLoader.Models
{
    public record FilterRule(string PropertyName, string Operator, object Value) : FilterExpression
    {
    }
}
