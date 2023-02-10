using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dma.DatasourceLoader.Models
{
    public class DataSourceLoadOptions
    {
        public List<FilterCriteria> Filters { get; set; } = new();
        public List<OrderCriteria> Orders { get; set; } = new();
    }
}
