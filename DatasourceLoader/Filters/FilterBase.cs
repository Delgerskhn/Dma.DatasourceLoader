using System.Linq.Expressions;

namespace Dma.DatasourceLoader.Filters
{
    public abstract class FilterBase<T>
    {
        protected readonly string propertyName;
        public string PropertyName { get { return propertyName; } } 

        public FilterBase(string propertyName)
        {
            this.propertyName = propertyName;
        }
        public abstract Expression<Func<T, bool>> GetFilterExpression();
    }
}