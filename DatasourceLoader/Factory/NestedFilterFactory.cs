using Dma.DatasourceLoader.Filters;
using Dma.DatasourceLoader.Models;
using System.Reflection;

namespace Dma.DatasourceLoader.Factory
{
    public class NestedFilterFactory<T> : AbstractFilterFactory<T> where T : class
    {
        public static bool IsApplicable(FilterOption option)
        {
            string[] properties = option.PropertyName.Split('.');
            if (properties.Length != 2)
            {
                // Property name is not in the correct format
                return false;
            }

            string property = properties[0];
            string innerProperty = properties[1];

            Type type = typeof(T);
            var propertyInfo = type.GetProperty(property);
            if (propertyInfo == null)
            {
                // Property does not exist on type T
                return false;
            }

            Type propertyType = propertyInfo.PropertyType;
            var innerPropertyInfo = propertyType.GetProperty(innerProperty);
            if (innerPropertyInfo == null)
            {
                // Inner property does not exist on propertyType
                return false;
            }

            // Both property and innerProperty exist
            return true;
        }

        public override FilterBase<T> CreateFilter(FilterOption option)
        {
            string[] properties = option.PropertyName.Split('.');
            string property = properties[0];
            string innerProperty = properties[1];

            Type type = typeof(T);
            var propertyInfo = type.GetProperty(property);
            Type propertyType = propertyInfo!.PropertyType;

            var innerOption = new FilterOption(innerProperty, option.Operator, option.Value);

            var filterFactoryType = typeof(FilterFactory<>).MakeGenericType(propertyType);
            var filterFactory = Activator.CreateInstance(filterFactoryType);
            var createMethod = filterFactoryType.GetMethod("Create");
            var abstractFactory = createMethod!.Invoke(filterFactory, new object[] { innerOption });

            var createFilterMethod = abstractFactory!.GetType().GetMethod("CreateFilter");
            var innerFilterInstance = createFilterMethod!.Invoke(abstractFactory, new object[] { innerOption });

            Type nestedFilterType = typeof(NestedFilter<,>).MakeGenericType(typeof(T), propertyType);
            var filter = (FilterBase<T>)Activator.CreateInstance(nestedFilterType, innerProperty, innerFilterInstance)!;

            return filter;
        }
    }

}
