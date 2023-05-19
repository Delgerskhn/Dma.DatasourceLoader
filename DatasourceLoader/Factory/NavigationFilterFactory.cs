using Dma.DatasourceLoader.Filters;
using Dma.DatasourceLoader.Models;

namespace Dma.DatasourceLoader.Factory
{
    public class NavigationFilterFactory<T> : AbstractFilterFactory<T> where T : class
    {
        public override FilterBase<T> CreateFilter(FilterOption option)
        {
            string[] properties = option.PropertyName.Split('.');
            string property = properties[0];
            string innerProperty = properties[1];

            Type type = typeof(T);
            var propertyInfo = type.GetProperty(property);
            Type propertyType = propertyInfo!.PropertyType;

            var innerOption = new FilterOption(innerProperty, option.Operator, option.Value);

            Type listElementType = propertyType.GetGenericArguments()[0];

            var filterFactoryType = typeof(FilterFactory<>).MakeGenericType(listElementType);
            var filterFactory = Activator.CreateInstance(filterFactoryType);
            var createMethod = filterFactoryType.GetMethod("Create");
            var abstractFactory = createMethod!.Invoke(filterFactory, new object[] { innerOption });

            var createFilterMethod = abstractFactory!.GetType().GetMethod("CreateFilter");
            var innerFilterInstance = createFilterMethod!.Invoke(abstractFactory, new object[] { innerOption });

            Type navigationFilterType = typeof(NavigationFilter<,>).MakeGenericType(typeof(T), listElementType);
            var filter = (FilterBase<T>)Activator.CreateInstance(navigationFilterType, innerProperty, innerFilterInstance)!;

            return filter;
        }

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

            //it will return List<TProperty>
            Type propertyType = propertyInfo.PropertyType;

            if (typeof(System.Collections.IEnumerable).IsAssignableFrom(propertyType) && propertyType != typeof(string))
            {
                Type listElementType = propertyType.GetGenericArguments()[0];

                var innerPropertyInfo = listElementType.GetProperty(innerProperty);
                if (innerPropertyInfo == null)
                {
                    // Inner property does not exist on listElementType
                    return false;
                }
            }
            else
            {
                // Property is not of type IEnumerable<TProperty>
                return false;
            }

            // Both property and innerProperty exist
            return true;
        }
    }
}
