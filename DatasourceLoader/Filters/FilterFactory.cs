using DatasourceLoader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatasourceLoader.Filters
{
    public class FilterFactory
    {
        public static FilterBase<T> Create<T>(FilterCriteria criteria)
        {
            FilterBase<T> filter = criteria.DataType switch
            {
                DataSourceType.Numeric => new NumericFilter<T>(criteria),
                DataSourceType.Text => new TextFilter<T>(criteria),
                DataSourceType.DateTime => new DateFilter<T>(criteria),
                DataSourceType.Collection => new CompositeCollectionFilter<T>(criteria),
                DataSourceType.PrimitiveCollection => new PrimitiveCollectionFilter<T>(criteria),
                _ => throw new NotSupportedException("Filter doesn't exists.")
            };
            return filter;
        }
    }
}
