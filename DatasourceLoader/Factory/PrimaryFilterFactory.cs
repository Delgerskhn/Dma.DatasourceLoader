using Dma.DatasourceLoader.Filters;
using Dma.DatasourceLoader.Models;

namespace Dma.DatasourceLoader.Factory
{
    public class PrimaryFilterFactory<T> : AbstractFilterFactory<T> where T : class
    {
        public override FilterBase<T> CreateFilter(FilterOption option)
        {
            if (option.Operator == FilterOperators.Eq) return new EqualFilter<T>(option.PropertyName, option.Value);
            if (option.Operator == FilterOperators.NotEq) return new NotEqualFilter<T>(option.PropertyName, option.Value);
            if (option.Operator == FilterOperators.Contains) return new ContainsFilter<T>(option.PropertyName, option.Value);
            if (option.Operator == FilterOperators.NotContains) return new NotContainsFilter<T>(option.PropertyName, option.Value);
            if (option.Operator == FilterOperators.StartsWith) return new StartsWithFilter<T>(option.PropertyName, option.Value);
            if (option.Operator == FilterOperators.EndsWith) return new EndsWithFilter<T>(option.PropertyName, option.Value);
            if (option.Operator == FilterOperators.Gt) return new GreaterThanFilter<T>(option.PropertyName, option.Value);
            if (option.Operator == FilterOperators.Gte) return new GreaterThanOrEqualFilter<T>(option.PropertyName, option.Value);
            if (option.Operator == FilterOperators.Lt) return new LessThanFilter<T>(option.PropertyName, option.Value);
            if (option.Operator == FilterOperators.Lte) return new LessThanOrEqualFilter<T>(option.PropertyName, option.Value);
            if (option.Operator == FilterOperators.Null) return new IsNullFilter<T>(option.PropertyName);
            if (option.Operator == FilterOperators.NotNull) return new IsNotNullFilter<T>(option.PropertyName);

            throw new InvalidOperationException("Operator not supported");
        }
    }
}
