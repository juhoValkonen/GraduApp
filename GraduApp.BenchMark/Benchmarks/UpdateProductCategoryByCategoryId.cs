using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduApp.Benchmark.Benchmarks
{
    public class UpdateProductCategoryByCategoryId : DefaultBenchmarkBase
    {
        //[Params(2000)]
        public int iterationCount = 2000;

        [Benchmark]
        public void RunBenchMark()
        {
            for (int i = 0; i < iterationCount; i++)
            {
                int oldCategoryID = i%500 +2 ;
                operations.UpdateProductCategoryByCategoryId((oldCategoryID), oldCategoryID - 1);
            }
        }


    }
}
