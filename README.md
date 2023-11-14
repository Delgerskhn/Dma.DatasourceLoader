# DatasourceLoader

The DatasourceLoader is a small package designed to simplify the loading of filter and order queries from an IQueryable data source. Filtering and sorting tables are common tasks in many applications, and this package streamlines these operations, making dashboard table actions easier and faster.

## Installation

You can install the package via NuGet:

[Nuget package](https://www.nuget.org/packages/Dma.DatasourceLoader)

```bash
dotnet add package Dma.DatasourceLoader
```

## Quickstart

Here's an example of how to use the DatasourceLoader in your C# code:

```csharp
using Dma.DatasourceLoader;
using Dma.DatasourceLoader.Models;

namespace MyProject.Controllers {

    public class MyController : ApiController {
        private readonly MyDbContext _context;

        public MyController(MyDbContext context) {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] DataSourceLoadOptions options) {
            var result = DataSourceLoader.Load(_context.SampleDatas, options);
            return Json(await result.ToListAsync());
        }
    }
}
```

## Implemented filters

The DatasourceLoader supports various filters, including:

- Complex filters
  - Navigation filter (for one-to-many relationships)
  - Nested filter (for one-to-one relationships)
  - And filter
  - Or filter
- Composite value filters
  - Between filter
  - In filter
  - Not in filter
- String filters
  - Contains filter
  - Endswith filter
  - Not contains filter
  - Starts with filter
- Primary filters
  - Equals
  - Greater than
  - Greater than or equal
  - Not null
  - Null
  - Less than
  - Less than or equal
  - Not equal

# Usage

The DataSourceLoadOptions class includes properties such as Filters, Orders, Cursor, and Size. The FilterOption record provides information about the object and property on which you want to apply a filter.

For ordering, specify the field name along with sorting criteria, either "asc" or "desc".

Pagination can be easily applied by using the Cursor and Size attributes.

Keep in mind that the order of the specified orders determines the order of application. In the example below, the query will first order by the "DateProperty" field in descending order and then by the "IntProperty" field in ascending order:

```csharp

var options = new DataSourceLoadOptions
{
    Filters = new List<FilterOption>()
    {
            new FilterOption(nameof(SampleData.DateProperty), "not_equals", new DateTime(2020, 10, 5)),
    },
    Orders = new List<OrderOption> {
            new OrderOption(nameof(SampleData.DateProperty),"desc"),
            new OrderOption(nameof(SampleData.IntProperty), "asc")
    }
};
var query = DatasourceLoader.Load<YourEntity>(yourDataSource, options);

```

### Defining a FilterOption

The FilterOption object contains information about property name, filter operation and the value to filter against.

```csharp
public record FilterOption(string PropertyName, string Operator, object Value){}
```

For example, consider the following data model:

```csharp
 public class SampleData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int IntProperty { get; set; }
        public bool BooleanProperty { get; set; }
        public DateTime DateProperty { get; set; }
        public SampleNestedData? NestedData { get; set; } = new();
        public string StrProperty { get; set; } = "";
        public List<SampleNestedData> NestedCollection { get; set; } = new List<SampleNestedData>();
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
```

The FilterOption record doesn't take information about the target Entity class, because the package infers the Entity class in runtime. You only need to be aware of possible attributes and properties of the entity class so that your filters won't mismatch with the actual class property names.

Let's say you have registered only SampleData class inside your EF DbContext like below.

```csharp
 public class ApplicationDb : DbContext
    {
        //...omitted for simplicity

        public virtual DbSet<SampleData> SampleDatas { get; set; } = default!;

        //...omitted for simplicity
    }
```

Knowing the structure of SampleData class and its properties you can now apply filters on corresponding properties.

```csharp
    /*SampleData class has a navigation called NestedCollection of type SampleNestedData
    SampleNestedData class has a property StrProperty of type string
    Thus, the filter below will search for SampleData whose NestedCollection navigation has a StrProperty that contains the text 'Awesome!'
    */
    var containsFilter = new FilterOption(
        "NestedCollection.StrProperty",
        FilterOperators.Contains,
        "Awesome!"
    );

    var res = DataSourceLoader.Load(db.SampleDatas, new()
        {
            Filters = new List<FilterOption>
            {
                containsFilter
            }
        });
```

#### When applying filters on non-relational properties

Its just simple as you need to provide the property name, filter type, and the value to search for.

```csharp
//Returns data whose StrProperty contains text "You can't guess"
var containsFilter = new FilterOption(
    "StrProperty",
    FilterOperators.Contains,
    "You can't guess");
//Returns data whose IntProperty is either 1 or 3
var inFilter = new FilterOption(
    "IntProperty",
    FilterOperators.In,
    new int[] { 1, 3 }
);
//Returns data whose DateProperty is greater than or equals 2023-12-12
var dateFilter = new FilterOption(
    "DateProperty",
    FilterOperators.Gte,
    new DateTime(2023,12,12)
);

```

#### When filtering navigation properties

Filtering on relational properties is a little bit trickier. The relationship could be either one-to-many or one-to-one. We treat them same when declaring FilterOptions.

```csharp
/*
NestedCollection is of type List<SampleNestedData>
SampleNestedData class has property DeepNestedData of type DeepNestedData?
DeepNestedData class has property Id of type int
*/
var navigationFilter = new FilterOption(
    "NestedCollection.DeepNestedData.Id",
    FilterOperators.In,
    new int[] {1, 3}
);

/*
SampleData class has property NestedData of type SampleNestedData?
SampleNestedData class has property DeepNestedData of type DeepNestedData?
DeepNestedData class has property StrProperty of type string
*/
var nestedFilter = new FilterOption(
    "NestedData.DeepNestedData.StrProperty",
    FilterOperators.Contains,
    "I am deep nested"
);
```

As you can see, you can use the dot **"."** character marking if the property is a composite type. When nesting the filters like this, the ending property must be always of primitive type (int, float, string, datetime...) like ".StrProperty", ".IntProperty", ".DateProperty".

The PropertyName attribute could be nested many levels according to your source class properties. "AncestorClass.ChildProperty.GreatChildProperty.PrimitiveProperty..."

### Entity framework support

You can load the data source from entity framework with this package.
Since this package is written on top of LINQ, you can do projections using select statement using EF and apply filters on them or vice versa.

```csharp
 FilterOption filter = new FilterOption("IntProperty", "not_in", int[] {22, 23});


 var query = db.SampleDatas.SelectMany(r => r.NestedCollection);

//The result type would be IQueryable<SampleNestedData>
 var res = DataSourceLoader.Load(query, new()
            {
                Filters = new List<FilterOption>
                {
                    filter
                }
            });


```
