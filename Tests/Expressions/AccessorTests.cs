using Dma.DatasourceLoader.Expressions;
using Dma.DatasourceLoader.Models;
using System.Linq.Expressions;

namespace Tests.Expressions
{

    
    public class AccessorTests
    {
        IQueryable<SampleData> data = new List<SampleData>() { new SampleData
        {
            StrProperty= "a",
        } }.AsQueryable();


        [Fact]
        public void ShouldSelect()
        {
            var prm = Expression.Parameter(typeof(SampleData));

            Expression<Func<SampleData, string>> sel = Accessor.SelectExpression<SampleData, string>(prm, "StrProperty");

            var res = data.Select(sel);

            Assert.Collection(res, (r) =>
            {
                Assert.Equal("a", r);
            });


            Expression sel1 = Accessor.SelectExpression(prm, "StrProperty");

            res = data.Select(sel);

            Assert.Collection(res, (r) =>
            {
                Assert.Equal("a", r);
            });
        }
       
    }
}
