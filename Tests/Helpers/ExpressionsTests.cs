using System.Linq.Expressions;
using Dma.DatasourceLoader.Helpers;
using Dma.DatasourceLoader.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Tests.Helpers
{
    public class ExpressionsTests
    {
        [Fact]
        public void ConstantForMember_WhenExpressionIsNullable_Int_ReturnsNullableIntConstantExpression()
        {
            int? nullableValue = 10;
            ParameterExpression parameter = Expression.Parameter(typeof(SampleData));
            MemberExpression property = Expression.Property(parameter, "IntProperty");
            var constantExpression = (ConstantExpression)property.ConstantForMember(nullableValue);

            Assert.Equal(typeof(int?), constantExpression.Type);
            Assert.Equal(nullableValue, constantExpression.Value);
        }

        [Fact]
        public void ConstantForMember_WhenExpressionIsNotNullable_Int_ReturnsIntConstantExpression()
        {
            int intValue = 5;
            ParameterExpression parameter = Expression.Parameter(typeof(SampleData));
            MemberExpression property = Expression.Property(parameter, "Id");

            var constantExpression = (ConstantExpression)property.ConstantForMember(intValue);

            Assert.Equal(typeof(int), constantExpression.Type);
            Assert.Equal(intValue, constantExpression.Value);
        }
    }
}
