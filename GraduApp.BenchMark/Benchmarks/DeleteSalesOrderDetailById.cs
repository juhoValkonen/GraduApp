using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduApp.Benchmark.Benchmarks
{
    public class DeleteSalesOrderDetailById : DefaultBenchmarkBase
    {
        int counter = 1;
        //[Params(1000)]
        public int iterationCount = 1000;

        [Benchmark]
        public void RunBenchMark()
        {

            for (int i = counter; i < counter + iterationCount; i++)
            {
                operations.DeleteSalesOrderDetailById(i);
            }


        }

        [IterationCleanup]
        public void setup()
        {
            counter += iterationCount;
            if (counter + iterationCount >= 3000)
            {
                counter = 1;
                setupDb(dbType);
            }

        }
    }
}
