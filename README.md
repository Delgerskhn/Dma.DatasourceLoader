# DatasourceLoader

This is a little package is written for loading filter and order queries 
from a IQueryable data source. The table filtering and sorting actions 
are really common use case for many applications. Thus, I created this package to handle filters and sorters out 
of the box. Made all my dashboard table actions easier and faster.

## Installation
<a href="https://www.nuget.org/packages/Dma.DatasourceLoader">Nuget package</a>
```bash
dotnet add package Dma.DatasourceLoader
```

## Quickstart

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


## Implemented filter criterias
<ul>
    <li>On text fields
        <ul>
            <li>Contains</li>
            <li>Equal</li>
        </ul>
    </li>
    <li>On numeric fields
        <ul>
            <li>Equal</li>
            <li>LessThan</li>
            <li>LessThanOrEqual</li>
            <li>GreaterThan</li>
            <li>GreaterThanOrEqual</li>
        </ul>
    </li>
    <li>On date fields
        <ul>
            <li>Equal</li>
            <li>LessThan</li>
            <li>LessThanOrEqual</li>
            <li>GreaterThan</li>
            <li>GreaterThanOrEqual</li>
        </ul>
    </li>
    <li>On IEnumerable<span><</span>T<span>></span> fields 
        <ul>
            <li>String field of T
                <ul>
                    <li>Equal</li>
                </ul>
            </li>
            <li>Numeric field of T
                <ul>
                    <li>Equal</li>
                </ul>
            </li>
            <li>DateTime field of T
                <ul>
                    <li>Equal</li>
                </ul>
            </li>
        </ul>
    </li>
</ul>

## Usage
The DataSourceLoadOptions class has properties called Filters and Orders. 
The FilterCriteria object reflects the object you want to apply filter on.

As for orders, you can specify the name of a field you want to order with the sorting criteria <b>"asc"</b> or <b>"desc"</b>. 

<b>Remember</b>, the orders are applied according to the order of index they are specified.
From example below, you can see that the query will be ordered by the field "DateProperty" in descending order, 
then by the field "IntProperty" in ascending order.


```csharp

var options = new DataSourceLoadOptions
{
    Filters = new List<FilterCriteria>
    {
        //Example LessThan numeric filter on IntProperty of type YourEntity. 
        new FilterCriteria()
        {
            DataType = DataSourceType.Numeric,
            NumericValue = 20,
            FieldName= nameof(YourEntity.IntProperty),
            FilterType = FilterType.LessThan,
        }
    },
    Orders = new List<(string, string)> {
        ( nameof(YourEntity.DateProperty), "desc" ),
        ( nameof(YourEntity.IntProperty), "asc" ),
    }
};
var query = DatasourceLoader.Load<YourEntity>(yourDataSource, options);

```

### How to define a FilterCriteria
The FilterCriteria object contains information about which field you want 
to filter, the type of target field and which kind of criteria you want to apply.

```csharp
public class FilterCriteria
{
    public string FieldName { get; set; } = string.Empty;
    public DataSourceType DataType { get; set; }
    public DataSourceType? CollectionDataType { get; set; }
    public FilterType FilterType { get; set; }
    public DateTime? DateValue { get; set; }
    public string? TextValue { get; set; } = string.Empty;
    public double? NumericValue { get; set; }
    public string? CollectionFieldName { get; set; } = string.Empty;
}
```

#### When filtering field of primitive type
You will need the FieldName, DataType, FilterType and one of the Value fields
based on the value of DataType. You need to assign TextValue, NumericValue or DateValue respectively 
for DataSourceType.Text, DataSourceType.Numeric and DataSourceType.DateTime.

```csharp
//for numbers
new FilterCriteria()
{
    DataType = DataSourceType.Numeric,
    NumericValue = 20,
    FieldName= nameof(YourEntity.IntProperty),
    FilterType = FilterType.LessThan,
}

//for text
new FilterCriteria()
{
    DataType = DataSourceType.Text,
    TextValue = "some text",
    FieldName= nameof(YourEntity.StrProperty),
    FilterType = FilterType.Contains,
}


//for date
new FilterCriteria()
{
    DataType = DataSourceType.DateTime,
    DateValue = DateTime.Now(),
    FieldName= nameof(YourEntity.DateProperty),
    FilterType = FilterType.LessThan,
}
```

#### When filtering collection fields
There are two different collection filters. The 
PrimitiveCollectionFilter is used for filtering collections like 
List<a><</a>int<a>></a> whereas CompositeCollectionFilter is designed
to filter Custom types. 

```csharp
//For composite collection of SampleNestedData
var criteria = new FilterCriteria
{
    DataType = DataSourceType.Collection,
    CollectionDataType = DataSourceType.Text,
    FilterType = FilterType.Contains,
    TextValue = "eRty",
    FieldName = nameof(SampleData.NestedCollection),
    CollectionFieldName = nameof(SampleNestedData.StrProperty)
};

//For primitive collection StrCollection of SampleData
FilterCriteria criteria = new FilterCriteria()
{
    DataType = DataSourceType.PrimitiveCollection,
    TextValue = "Nested1",
    FieldName = nameof(SampleData.StrCollection),
    CollectionDataType = DataSourceType.Text,
    FilterType = FilterType.Contains,
};
```

### The filtering capability
A filter criteria can either filter one of the primitive type field or a collection
type field of the target class.


### Entity framework support
You can load the data source from entity framework with this package. 
However the primitive type of collection is not supported by EF, the 
proper projection could be used for PrimitiveCollectionFilter. 

```csharp
 FilterCriteria criteria = new FilterCriteria()
    {
        DataType = DataSourceType.PrimitiveCollection,
        TextValue = "Nested1",
        FieldName = nameof(SampleData.StrCollection),
        CollectionDataType = DataSourceType.Text,
        FilterType = FilterType.Contains,
    };

 var query = db.SampleDatas.Select(r => new SampleData
            {
                StrCollection = r.NestedCollection.Select(r => r.StrProperty).ToList()
            });

 var res = DataSourceLoader.Load(query, new()
            {
                Filters = new List<FilterCriteria>
                {
                    criteria
                }
            });


```