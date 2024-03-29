﻿using Dma.DatasourceLoader.Creator;
using Dma.DatasourceLoader.Filters.ComplexFilters;
using Dma.DatasourceLoader.Models;
using NSubstitute;

namespace Tests.FilterCreators
{
    public class NestedFilterCreatorTests
    {

        [Fact]
        public void ItShouldCreateNestedFilter()
        {
            var innerFilter = Substitute.For<IFilterCreator>();

            var filter = new NestedFilterCreator(innerFilter, nameof(SampleNestedData.StrProperty), typeof(SampleData)).CreateFilter();

            Assert.IsType<NestedFilter<SampleData>>(filter);
        }
    }
}
