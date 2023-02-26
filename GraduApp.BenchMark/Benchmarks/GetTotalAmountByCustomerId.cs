using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using GraduApp.DataAccess;
using GraduApp.DataAccess.Enums;
using GraduApp.DataAccess.GraduDBOperations;
using GraduApp.DataAccess.GraduDBUtils;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduApp.Benchmark.Benchmarks
{
   
    public class GetTotalAmountByCustomerId : NotrackingBenchmarkBase
    {
        //[Params(2000)]
        public int iterationCount = 2000;

        [Benchmark]
        public decimal RunBenchMark()
        {
            decimal totalAmount = 0;
            for(int i = 0; i < iterationCount; i++)
            {
                totalAmount += operations.GetTotalAmountByCustomerId(i);
            }
            return totalAmount;
        }
    }
}
