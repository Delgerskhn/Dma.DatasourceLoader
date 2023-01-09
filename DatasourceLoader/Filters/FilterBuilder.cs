using DatasourceLoader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatasourceLoader.Filters
{
    public class FilterBuilder<T>
    {
        private readonly ICollection<FilterCriteria> filterCriterias;
        private IQueryable<T> query;

        public FilterBuilder(ICollection<FilterCriteria> filterCriterias, IQueryable<T> query)
        {
            this.filterCriterias = filterCriterias;
            this.query = query;
        }

        public List<FilterBase<T>> build()
        {
            var filters = filterCriterias.Select(r=>FilterFactory.Create<T>(r)).ToList();
            return filters;
        }
    }
}
