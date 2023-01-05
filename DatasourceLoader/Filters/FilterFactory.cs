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
        public static FilterBase Create(FilterCriteria criteria)
        {
            FilterBase filter = criteria.DataType switch
            {
                DataSourceType.Numeric => new NumericFilter(criteria),
                DataSourceType.Text => new TextFilter(criteria),
                DataSourceType.DateTime => new DateFilter(criteria),
                DataSourceType.Collection => new CollectionFilter(criteria),
                _ => throw new NotImplementedException("Filter doesn't exists.")
            };
            return filter;
        }
    }
}
