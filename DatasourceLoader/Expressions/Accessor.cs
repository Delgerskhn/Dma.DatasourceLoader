using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DatasourceLoader.Expressions
{
    public static class Accessor
    {
        public static Expression<Func<TSrc, TDest>> SelectExpression<TSrc, TDest>(ParameterExpression source, string fieldName)
        {
            return Expression.Lambda<Func<TSrc, TDest>>(Expression.Property(source, fieldName), source);
        }
    }
}
