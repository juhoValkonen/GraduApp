using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduApp.Benchmark.Benchmarks
{
    public class CountProductsByCategoryId : NotrackingBenchmarkBase
    {
        //[Params(2000)]
        public int iterationCount = 2000;

        [Benchmark]
        public int RunBenchMark()
        {
            int total = 0;
            for (int i = 0; i < iterationCount; i++)
            {
                total += operations.CountProductsByCategoryId((i%500)+1);
            }
            //Console.WriteLine(total.ToString());
            return total;
        }
    }
}
