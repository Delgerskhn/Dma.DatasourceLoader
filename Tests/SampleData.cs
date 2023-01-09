using System.ComponentModel.DataAnnotations.Schema;

namespace Tests.DatasourceLoader
{
    public class SampleData
    {
        public int IntProperty { get; set; }
        public DateTime DateProperty { get; set; }
        public string StrProperty { get; set; } = "";
        public ICollection<string> StrCollection { get; set;} = new string[0];
        public ICollection<SampleNestedData> NestedCollection { get; set; } = new List<SampleNestedData>();
        public List<DateTime> DateCollection { get; internal set; } = new();
        public List<int> NumericCollection { get; internal set; } = new();
        public int Id { get; set; }
    }

    public class SampleNestedData
    {
        public int Id { get; set; }
        public int IntProperty { get; set; }
        public DateTime DateProperty { get; set; }
        public string StrProperty { get; set; } = "";
    }
}
