using BenchmarkDotNet.Attributes;
using GraduApp.DataAccess.GraduModels;

namespace GraduApp.Benchmark.Benchmarks
{
    public class GetProductsByCategoryId : NotrackingBenchmarkBase
    {

        //[Params(2000)]
        public int iterationCount = 2000;

        [Benchmark]
        public List<Product> RunBenchMark()
        {
            List<Product> ret = new();
            for (int i = 0; i < iterationCount; i++)
            {
                ret = operations.GetProductsByCategoryId((i % 500) + 1);
            }
            return ret;

        }
    }
}
