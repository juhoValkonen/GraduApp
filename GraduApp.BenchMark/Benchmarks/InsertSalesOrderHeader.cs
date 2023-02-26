using BenchmarkDotNet.Attributes;
using GraduApp.DataAccess.GraduModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduApp.Benchmark.Benchmarks
{
    public class InsertSalesOrderHeader : DefaultBenchmarkBase
    {
        List<SalesOrderHeader> _salesOrders = new();
        //[Params(500)]
        public int iterationCount = 500;

        [Benchmark]
        public void RunBenchMark()
        {

            for (int i = 1; i < iterationCount - 1; i++)
            {
                Console.WriteLine(i.ToString());
                SalesOrderHeader s = _salesOrders[i];
                s.ID = 0;
                foreach(SalesOrderDetail d in s.SalesOrderDetail)
                {
                    d.ID = 0;
                }

                operations.InsertSalesOrderHeader(s);
            }


        }

        [IterationSetup]
        public void PrepareSalesOrderList()
        {
            for (int i = 1; i < iterationCount; i++)
            {
                _salesOrders.Add(BenchmarkDataGenerator.MakeSalesOrderHeader(i));
            }
        }
    }
}
