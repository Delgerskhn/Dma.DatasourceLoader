using Dma.DatasourceLoader.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tests.DatasourceLoader;

namespace Tests.Expressions
{

    public class Solution
    {
        int minChar = 0;

        public void dfs(int i, int[,] adj, int[] visited, List<int> component)
        {
            if (visited[i] == 1) return;
            visited[i] = 1;

            component.Add(i);

            for (int c = 0; c < 26; c++)
            {
                if (adj[i, c] == 1)
                    dfs(c, adj, visited, component);
                minChar = Math.Min(minChar, i);
            }

        }
        public string SmallestEquivalentString(string s1, string s2, string baseStr)
        {
            int[,] adj = new int[26, 26];
            for (int i = 0; i < s1.Length; i++)
            {
                int a = s1[i] - 'a';
                int b = s2[i] - 'a';
                adj[a, b] = 1;
                adj[b, a] = 1;
            }

            int[] mappingChar = new int[26];
            int[] visited = new int[26];

            for (int i = 0; i < 26; i++)
            {
                mappingChar[i] = i;
            }

            for (int i = 0; i < 26; i++)
            {
                if (visited[i] != 1)
                {
                    minChar = 27;
                    var component = new List<int>();
                    dfs(i, adj, visited, component);

                    foreach (var v in component) mappingChar[v] = minChar;
                }
            }

            StringBuilder sb = new StringBuilder();
            foreach (var c in baseStr)
            {
                sb.Append((char)(mappingChar[c - 'a'] + 'a'));
            }
            return sb.ToString();
        }



    }
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
        [Fact]
        public void TestSolution()
        {
            var sol = new Solution();
            var res = sol.SmallestEquivalentString("parker", "morris", "parser");
            Assert.Equal("makkek", res);
        }

    }
}
