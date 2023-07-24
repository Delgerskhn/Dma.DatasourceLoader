using System.Linq.Expressions;

namespace Dma.DatasourceLoader.Filters
{
    public abstract class FilterBase<T> : FilterBaseBase
    {
        protected readonly string propertyName;
        public string PropertyName { get { return propertyName; } }

        public FilterBase(string propertyName)
        {
            this.propertyName = propertyName;
        }
        override
        public abstract Expression<Func<T, bool>> GetFilterExpression();
    }
}