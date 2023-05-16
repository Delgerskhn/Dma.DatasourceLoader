using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Dma.DatasourceLoader.Models;
using static System.Linq.Expressions.Expression;

namespace Dma.DatasourceLoader.Filters
{
    public class TextFilter<T> : FilterBase<T>
    {
        private PropertyInfo? targetField = null;
        private ParameterExpression prm;
        private ConstantExpression? value = null;
        public TextFilter(FilterCriteria criteria) : base(criteria)
        {
            Type elementType = typeof(T);

            targetField = elementType.GetProperties().Where(x => x.Name == criteria.FieldName).FirstOrDefault();
            prm = Parameter(elementType);
            if (criteria.TextValue != null) value = Constant(criteria.TextValue.ToUpper());
        }

        public override IQueryable<T> Contains(IQueryable<T> source)
        {
            if (targetField == null || value == null) return source;
            MethodInfo containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) })!;

            Expression exp = Call(
                                Call(
                                    Property(prm, targetField),
                                    "ToUpper", null
                                ),
                                containsMethod,
                                value
                            );
            Expression<Func<T, bool>> lambda = Lambda<Func<T, bool>>(exp, prm);

            return source.Where(lambda);
        }
        public override IQueryable<T> NotEqual(IQueryable<T> source)
        {
            if (targetField == null || value == null) return source;

            Expression exp = Expression.NotEqual(
                                Call(
                                    Property(prm, targetField),
                                    "ToUpper", null
                                ),
                                value
                            );
            Expression<Func<T, bool>> lambda = Lambda<Func<T, bool>>(exp, prm);
            return source.Where(lambda);
        }
        public override IQueryable<T> Equal(IQueryable<T> source)
        {
            if (targetField == null || value == null) return source;

            Expression exp = Expression.Equal(
                                Call(
                                    Property(prm, targetField),
                                    "ToUpper", null
                                ),
                                value
                            );
            Expression<Func<T, bool>> lambda = Lambda<Func<T, bool>>(exp, prm);
            return source.Where(lambda);

        }

        public IQueryable<T> AllFieldTextFilter(IQueryable<T> source, string term)
        {
            if (string.IsNullOrEmpty(term)) { return source; }

            // T is a compile-time placeholder for the element type of the query.
            Type elementType = typeof(T);

            // Get all the string properties on this specific type.
            PropertyInfo[] stringProperties =
                elementType.GetProperties()
                    .Where(x => x.PropertyType == typeof(string))
                    .ToArray();
            if (!stringProperties.Any()) { return source; }

            // Get the right overload of String.Contains
            MethodInfo containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) })!;

            // Create a parameter for the expression tree:
            // the 'x' in 'x => x.PropertyName.Contains("term")'
            // The type of this parameter is the query's element type
            ParameterExpression prm = Parameter(elementType);

            // Map each property to an expression tree node
            IEnumerable<Expression> expressions = stringProperties
                .Select(prp =>
                    // For each property, we have to construct an expression tree node like x.PropertyName.Contains("term")
                    Call(                  // .Contains(...) 
                        Property(          // .PropertyName
                            prm,           // x 
                            prp
                        ),
                        containsMethod,
                        Constant(term)     // "term" 
                    )
                );

            // Combine all the resultant expression nodes using ||
            Expression body = expressions
                .Aggregate(
                    (prev, current) => Or(prev, current)
                );

            // Wrap the expression body in a compile-time-typed lambda expression
            Expression<Func<T, bool>> lambda = Lambda<Func<T, bool>>(body, prm);

            // Because the lambda is compile-time-typed (albeit with a generic parameter), we can use it with the Where method
            return source.Where(lambda);
        }

        public override IQueryable<T> GreaterThan(IQueryable<T> source)
        {
            throw new NotImplementedException();
        }

        public override IQueryable<T> GreaterThanOrEqual(IQueryable<T> source)
        {
            throw new NotImplementedException();
        }

        public override IQueryable<T> LessThan(IQueryable<T> source)
        {
            throw new NotImplementedException();
        }

        public override IQueryable<T> LessThanOrEqual(IQueryable<T> source)
        {
            throw new NotImplementedException();
        }


    }
}
