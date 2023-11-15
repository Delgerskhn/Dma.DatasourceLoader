using Dma.DatasourceLoader.Models;
using System.Linq.Expressions;

namespace Dma.DatasourceLoader.Analyzer
{


    public class FilterGroupAnalyzer
    {
        public Expression<Func<SampleData, bool>> Analyze(FilterGroup group)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(SampleData), "x");
            Expression body = AnalyzeGroup(group, parameter);
            return Expression.Lambda<Func<SampleData, bool>>(body, parameter);
        }

        private Expression AnalyzeGroup(FilterGroup group, ParameterExpression parameter)
        {
            Expression result = null;

            foreach (var item in group.Items)
            {
                Expression itemExpression = null;

                if (item is FilterGroupItem.And andItem)
                {
                    itemExpression = AnalyzeGroup(andItem.Expr, parameter);
                }
                else if (item is FilterGroupItem.Or orItem)
                {
                    itemExpression = AnalyzeGroup(orItem.Expr, parameter);
                }
                else if (item is FilterGroupItem.SubGroup subGroupItem)
                {
                    itemExpression = subGroupItem.Exprs
                        .Select(subItem => AnalyzeGroup(subItem, parameter))
                        .Aggregate(Expression.OrElse);
                }
                else if (item is FilterRule filterOption)
                {
                    itemExpression = AnalyzeFilterOption(filterOption, parameter);
                }

                result = result == null ? itemExpression : Expression.AndAlso(result, itemExpression);
            }

            return result;
        }

        private Expression AnalyzeFilterOption(FilterRule option, ParameterExpression parameter)
        {
            Expression property = Expression.Property(parameter, option.PropertyName);

            var constant = Expression.Constant(option.Value);
            Expression body = option.Operator switch
            {
                FilterOperators.StartsWith => Expression.Call(property, "StartsWith", null, constant),
                FilterOperators.Contains => Expression.Call(property, "Contains", null, constant),
                FilterOperators.Eq => Expression.Equal(property, constant),
                // Handle other operators as needed
                _ => throw new NotSupportedException("Operator not supported"),
            };

            return body;
        }
    }

}
