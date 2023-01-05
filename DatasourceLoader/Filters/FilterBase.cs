using DatasourceLoader.Models;

namespace DatasourceLoader.Filters
{
    public abstract class FilterBase
    {
        public readonly FilterCriteria criteria;
        public FilterBase(FilterCriteria criteria)
        {
            this.criteria = criteria;
        }
        public abstract IQueryable<T> Equal<T>(IQueryable<T> source);
        public abstract IQueryable<T> GreaterThan<T>(IQueryable<T> source);
        public abstract IQueryable<T> GreaterThanOrEqual<T>(IQueryable<T> source);
        public abstract IQueryable<T> LessThan<T>(IQueryable<T> source);
        public abstract IQueryable<T> LessThanOrEqual<T>(IQueryable<T> source);
        public abstract IQueryable<T> Contains<T>(IQueryable<T> source);
    }
}