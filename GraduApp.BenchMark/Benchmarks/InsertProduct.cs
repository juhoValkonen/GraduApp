using BenchmarkDotNet.Attributes;
using GraduApp.DataAccess.GraduModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduApp.Benchmark.Benchmarks
{
    public class InsertProduct : DefaultBenchmarkBase
    {
        List<Product> products = new();
        //[Params(500)]
        public int iterationCount = 500;

        [Benchmark]
        public void RunBenchMark()
        {

            for (int i = 1; i < iterationCount - 1; i++)
            {
                Product p = products[i];
                p.ID = 0;
                operations.InsertProduct(p);
            }


        }

        [IterationSetup]
        public void PrepareCustomerList()
        {
            for (int i = 1; i < iterationCount; i++)
            {
                products.Add(BenchmarkDataGenerator.MakeProduct(i));
            }
        }
    }
}
