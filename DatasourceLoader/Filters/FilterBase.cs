using Dma.DatasourceLoader.Models;

namespace Dma.DatasourceLoader.Filters
{
    public abstract class FilterBase<T>
    {
        public readonly FilterCriteria criteria;
        public FilterBase(FilterCriteria criteria)
        {
            this.criteria = criteria;
        }
        public abstract IQueryable<T> Equal(IQueryable<T> source);
        public abstract IQueryable<T> GreaterThan(IQueryable<T> source);
        public abstract IQueryable<T> GreaterThanOrEqual(IQueryable<T> source);
        public abstract IQueryable<T> LessThan(IQueryable<T> source);
        public abstract IQueryable<T> LessThanOrEqual(IQueryable<T> source);
        public abstract IQueryable<T> Contains(IQueryable<T> source);
    }
}