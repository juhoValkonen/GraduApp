using BenchmarkDotNet.Attributes;
using GraduApp.DataAccess.GraduModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduApp.Benchmark.Benchmarks
{
    public class InsertCustomer : DefaultBenchmarkBase
    {
        List<Customer> customers = new();
        //[Params(500)]
        public int iterationCount = 500;

        [Benchmark]
        public void RunBenchMark()
        {

            for (int i = 1; i < iterationCount-1; i++)
            {
                Customer c = customers[i];
                c.ID = 0;
                c.CustomerAddress.First().ID = 0;
                c.CustomerAddress.First().Address.ID = 0;

               operations.InsertCustomer(c);
            }
        }
        
        [IterationSetup]
        public void PrepareCustomerList()
        {
            for (int i = 1; i < iterationCount; i++)
            {
                customers.Add(BenchmarkDataGenerator.MakeCustomer(i));
            }
        }
    }
}
