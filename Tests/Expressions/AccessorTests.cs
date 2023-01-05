using DatasourceLoader.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tests.DatasourceLoader;

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
                Assert.Equal("a",r);
            });
        }

        
    }
}
