using Dma.DatasourceLoader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Fixture
{
    public static class SampleDataCollection
    {
        public static List<SampleData> CreateCollection()
        {
            return new List<SampleData> {
                new SampleData { StrProperty = "Text1", IntProperty = 1, DateProperty = new DateTime(2023, 1, 10), NullableStringProperty = "nullable"},
                new SampleData { IntProperty = 0, DateProperty = new DateTime(2023, 1, 10)},
                new SampleData { IntProperty = -20, DateProperty = new DateTime(2023, 1, 10)},
                new SampleData { StrProperty = "Text2" , IntProperty = 2},
                new SampleData {
                    StrProperty = "TextText3435Text",
                    NestedData = new(){

                        StrProperty = "Text1"
                    }
                },
                new SampleData { NestedCollection = new List<SampleNestedData>(){
                    new()
                    {
                        DeepNestedData = new()
                        {
                            StrProperty = "DeepNestedText"
                        },
                        StrProperty = "Text1"
                    }
                } },

            };
        }
    }
}
