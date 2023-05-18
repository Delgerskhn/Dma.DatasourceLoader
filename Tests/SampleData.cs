﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Tests.DatasourceLoader
{
    public class SampleData
    {
        public int IntProperty { get; set; }
        public bool BooleanProperty { get; set; }
        public DateTime DateProperty { get; set; }
        public SampleNestedData NestedData { get; set; } = new();
        public string StrProperty { get; set; } = "";
        public List<string> StrCollection { get; set; } = new ();
        public List<SampleNestedData> NestedCollection { get; set; } = new List<SampleNestedData>();
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
