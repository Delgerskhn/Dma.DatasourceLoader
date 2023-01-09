using DatasourceLoader.Models;

namespace DatasourceLoader.Filters
{
    public static class FilterBaseExtensions
    {
        public static IQueryable<T> ApplyFilter<T>(this FilterBase<T> filter, IQueryable<T> query)
        {
            var q = filter.criteria.FilterType switch
            {
                FilterType.Equals => filter.Equal(query),
                FilterType.LessThan => filter.LessThan(query),
                FilterType.LessThanOrEqual => filter.LessThanOrEqual(query),
                FilterType.GreaterThanOrEqual => filter.GreaterThanOrEqual(query),
                FilterType.GreaterThan => filter.GreaterThan(query),
                FilterType.Contains => filter.Contains(query),
                _ => query
            };
            return q;
        }
    }
}