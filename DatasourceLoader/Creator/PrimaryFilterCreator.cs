using Dma.DatasourceLoader.Filters;
using Dma.DatasourceLoader.Filters.CompositeValueFilters;
using Dma.DatasourceLoader.Filters.PrimaryFilters;
using Dma.DatasourceLoader.Filters.StringFilters;
using Dma.DatasourceLoader.Models;

namespace Dma.DatasourceLoader.Creator
{
    public class PrimaryFilterCreator<T> : IFilterCreator where T : class
    {
        private readonly FilterRule option;

        public PrimaryFilterCreator(FilterRule option)
        {
            this.option = option;
        }

        public Filter CreateFilter()
        {
            if (option.Operator == FilterOperators.Eq) return new EqualFilter<T>(option.PropertyName, option.Value);
            if (option.Operator == FilterOperators.NotEq) return new NotEqualFilter<T>(option.PropertyName, option.Value);
            if (option.Operator == FilterOperators.Contains) return new ContainsFilter<T>(option.PropertyName, option.Value.ToString() ?? "");
            if (option.Operator == FilterOperators.NotContains) return new NotContainsFilter<T>(option.PropertyName, option.Value.ToString() ?? "");
            if (option.Operator == FilterOperators.StartsWith) return new StartsWithFilter<T>(option.PropertyName, option.Value.ToString() ?? "");
            if (option.Operator == FilterOperators.EndsWith) return new EndsWithFilter<T>(option.PropertyName, option.Value.ToString() ?? "");
            if (option.Operator == FilterOperators.Gt) return new GreaterThanFilter<T>(option.PropertyName, option.Value);
            if (option.Operator == FilterOperators.Gte) return new GreaterThanOrEqualFilter<T>(option.PropertyName, option.Value);
            if (option.Operator == FilterOperators.Lt) return new LessThanFilter<T>(option.PropertyName, option.Value);
            if (option.Operator == FilterOperators.Lte) return new LessThanOrEqualFilter<T>(option.PropertyName, option.Value);
            if (option.Operator == FilterOperators.Null) return new IsNullFilter<T>(option.PropertyName);
            if (option.Operator == FilterOperators.NotNull) return new IsNotNullFilter<T>(option.PropertyName);
            if (option.Operator == FilterOperators.In) return new InFilter<T>(option.PropertyName, option.Value);
            if (option.Operator == FilterOperators.NotIn) return new NotInFilter<T>(option.PropertyName, option.Value);

            throw new InvalidOperationException("Operator not supported");
        }
    }
}
