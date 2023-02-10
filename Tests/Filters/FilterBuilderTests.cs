using Dma.DatasourceLoader.Filters;
using Dma.DatasourceLoader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.DatasourceLoader;

namespace Tests.Filters
{
    public class FilterBuilderTests
    {
        private List<FilterCriteria> criterias = new List<FilterCriteria>()
            {
                new FilterCriteria
                {
                    DataType = DataSourceType.DateTime,
                    FilterType = FilterType.GreaterThanOrEqual,
                    DateValue = new DateTime(2020, 11, 5),
                    FieldName = nameof(SampleData.DateProperty)
                },
                new FilterCriteria
                {
                    DataType = DataSourceType.Numeric,
                    FilterType = FilterType.GreaterThan,
                    NumericValue = 25,
                    FieldName = nameof(SampleData.IntProperty)
                },
                new FilterCriteria {
                    DataType = DataSourceType.Text,
                    FilterType = FilterType.Equals,
                    TextValue = "Text1",
                    FieldName = nameof(SampleData.StrProperty)
                },
                new FilterCriteria
                {
                    DataType = DataSourceType.Collection,
                    CollectionDataType = DataSourceType.Text,
                    FilterType = FilterType.Contains,
                    TextValue = "erty",
                    FieldName = nameof(SampleData.StrCollection)
                },
                new FilterCriteria
                {
                    DataType = DataSourceType.PrimitiveCollection,
                    CollectionDataType = DataSourceType.Text,
                    FilterType = FilterType.Contains,
                    TextValue = "eRty",
                    FieldName = nameof(SampleData.NestedCollection),
                    CollectionFieldName = nameof(SampleNestedData.StrProperty)
                }
        };
        private static List<SampleData> source = new List<SampleData> {
            new SampleData { DateProperty = new DateTime(2020, 11, 5) },
            new SampleData { DateProperty = new DateTime(2020, 10, 5)  },
            new SampleData { DateProperty = new DateTime(2020, 12, 5)  } };

        [Fact]
        public void ShouldApplyFilters_onQuery()
        {

            var builder = new FilterBuilder<SampleData>(criterias, source.AsQueryable());

            List<FilterBase<SampleData>> res = builder.build();
            Assert.Collection(res,
                r =>
                {
                    Assert.IsType<DateFilter<SampleData>>(r);
                },
                r =>
                {
                    Assert.IsType<NumericFilter<SampleData>>(r);
                },
                r =>
                {
                    Assert.IsType<TextFilter<SampleData>>(r);
                },
                r =>
                {
                    Assert.IsType<CompositeCollectionFilter<SampleData>>(r);
                },
                r =>
                {
                    Assert.IsType<PrimitiveCollectionFilter<SampleData>>(r);
                }
            );
        }
    }
}
