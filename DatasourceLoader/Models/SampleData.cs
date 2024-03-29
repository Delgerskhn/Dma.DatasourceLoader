﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dma.DatasourceLoader.Models
{
    public class SampleData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? IntProperty { get; set; }
        public bool BooleanProperty { get; set; }
        public DateTime DateProperty { get; set; }
        public SampleNestedData? NestedData { get; set; } = new();
        public string StrProperty { get; set; } = "";
        public string? NullableStringProperty { get; set; } 
        //public List<string> StrCollection { get; set; } = new();
        public List<SampleNestedData> NestedCollection { get; set; } = new List<SampleNestedData>();
        //public List<DateTime> DateCollection { get;  set; } = new();
        //public List<int> NumericCollection { get;  set; } = new();

    }
    public class DeepNestedData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string StrProperty { get; set; } = default!;
        public int? OwnerId { get; set; }
        public SampleNestedData? Owner;
    }
    public class SampleNestedData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int IntProperty { get; set; }
        public DateTime DateProperty { get; set; }
        public string StrProperty { get; set; } = "";
        public DeepNestedData? DeepNestedData { get; set; } = default!;
        public int? OwnerId { get; set; }
        public int? ParentId { get; set; }
        public SampleData? Owner { get; set; } = default!;
        public SampleData? Parent { get; set; } = default!;
    }
}
