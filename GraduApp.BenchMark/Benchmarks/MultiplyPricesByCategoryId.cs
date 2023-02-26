using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduApp.Benchmark.Benchmarks
{
    public class MultiplyPricesByCategoryId : DefaultBenchmarkBase
    {
        //[Params(2000)]
        public int iterationCount = 2000;

        [Benchmark]
        public void RunBenchMark()
        {
            for (int i = 0; i < iterationCount; i++)
            {
                operations.MultiplyPricesByCategoryId((i % 500) + 1, new decimal(1.01));
            }
        }
    }
}
