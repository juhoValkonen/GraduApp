using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduApp.Benchmark.Benchmarks
{
    public class DeleteProductCategoryByID : DefaultBenchmarkBase
    {
        //[Params(15)]
        public int iterationCount = 50;

        int counter = 0;
        [Benchmark]
        public void RunBenchMark()
        {

            for (int i = counter; i < counter + iterationCount; i++)
            {
                operations.DeleteProductCategoryByID(i);
            }


        }

        [IterationCleanup]
        public void setup()
        {
            counter += iterationCount;
            if (counter >= 500)
            {
                counter = 0;
                setupDb(dbType);
            }
            
        }
    }
}
