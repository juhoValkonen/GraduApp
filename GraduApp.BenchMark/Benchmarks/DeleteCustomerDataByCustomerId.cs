using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduApp.Benchmark.Benchmarks
{
    public class DeleteCustomerDataByCustomerId : DefaultBenchmarkBase
    {
        int counter = 1;
        //[Params(100)]
        public int iterationCount = 150;

        [Benchmark]
        public void RunBenchMark()
        {
            
            for (int i = counter; i < counter + iterationCount; i++)
            {
                 operations.DeleteCustomerDataByCustomerId(i);
            }
           
            
        }

        [IterationCleanup]
        public void setup()
        {
            counter += iterationCount;
            if (counter + iterationCount >= 5000)
            {
                counter = 1;
                setupDb(dbType);
            }

        }
    }
}
