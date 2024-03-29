﻿using Dma.DatasourceLoader.Creator;
using Dma.DatasourceLoader.Filters.ComplexFilters;
using Dma.DatasourceLoader.Models;
using NSubstitute;

namespace Tests.FilterCreators
{
    public class NavigationFilterCreatorTests
    {

        [Fact]
        public void ItShouldCreateFilter()
        {
            var innerFilter = Substitute.For<IFilterCreator>();

            var filter = new NavigationFilterCreator(nameof(SampleData.NestedCollection), innerFilter, typeof(SampleData)).CreateFilter();

            Assert.IsType<NavigationFilter<SampleData>>(filter);
        }

    }
}
