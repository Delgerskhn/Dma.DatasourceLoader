using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatasourceLoader.Models
{
    public class DataSourceLoadOptions
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int Current { get; set; }
        [Required]
        [Range(1, 100)]
        public int PageSize { get; set; }
        public List<FilterCriteria> Filters { get; set; } = new();
        public Dictionary<string, string> Sorter { get; set; } = new();
    }
}
