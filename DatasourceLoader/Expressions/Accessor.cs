using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Dma.DatasourceLoader.Expressions
{
    public static class Accessor
    {
        public static Expression<Func<TSrc, TDest>> SelectExpression<TSrc, TDest>(ParameterExpression source, string fieldName)
        {
            return Expression.Lambda<Func<TSrc, TDest>>(Expression.Property(source, fieldName), source);
        }

        public static LambdaExpression SelectExpression(ParameterExpression source, string fieldName)
        {
            return Expression.Lambda(Expression.Property(source, fieldName), source);
        }
    }
}
