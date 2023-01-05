using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using DatasourceLoader.Models;
using static System.Linq.Expressions.Expression;

namespace DatasourceLoader.Filters
{
    public class TextFilter : FilterBase
    {
        public TextFilter(FilterCriteria criteria) : base(criteria)
        {
        }

        public  override IQueryable<T> Contains<T>(IQueryable<T> source)
        {
            if (string.IsNullOrEmpty(criteria.TextValue)) { return source; }
            Type elementType = typeof(T);

            var targetField = elementType.GetProperties().Where(x => x.Name == criteria.FieldName).FirstOrDefault();

            if (targetField == null) return source;
            MethodInfo containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) })!;
            ParameterExpression prm = Parameter(elementType);
            Expression exp = Call(
                            Call( // <=== this one is new
                                Property(prm, targetField),
                                "ToUpper", null
                            ),
                            containsMethod,
                            Constant(criteria.TextValue.ToUpper())
                            );
            Expression<Func<T, bool>> lambda = Lambda<Func<T, bool>>(exp, prm);

            return source.Where(lambda);
        }

        public  override IQueryable<T> Equal<T>(IQueryable<T> query)
        {
            if (string.IsNullOrEmpty(criteria.TextValue)) { return query; }
            Type elementType = typeof(T);
            var targetField = elementType.GetProperties().Where(x => x.Name == criteria.FieldName).FirstOrDefault();
            if (targetField == null) return query;
            ParameterExpression prm = Parameter(elementType);
            Expression exp = Expression.Equal(
                                Call( // <=== this one is new
                                    Property(prm, targetField),
                                    "ToUpper", null
                                ),
                                Constant(criteria.TextValue.ToUpper())
                            );
            Expression<Func<T, bool>> lambda = Lambda<Func<T, bool>>(exp, prm);
            return query.Where(lambda);

        }

        public IQueryable<T> AllFieldTextFilter<T>(IQueryable<T> source, string term)
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

        public override IQueryable<T> GreaterThan<T>(IQueryable<T> source)
        {
            throw new NotImplementedException();
        }

        public override IQueryable<T> GreaterThanOrEqual<T>(IQueryable<T> source)
        {
            throw new NotImplementedException();
        }

        public override IQueryable<T> LessThan<T>(IQueryable<T> source)
        {
            throw new NotImplementedException();
        }

        public override IQueryable<T> LessThanOrEqual<T>(IQueryable<T> source)
        {
            throw new NotImplementedException();
        }
    }
}
