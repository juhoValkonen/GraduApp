﻿using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduApp.Benchmark.Benchmarks
{
    public class UpdateCustomerLastName : DefaultBenchmarkBase
    {
        //[Params(2000)]
        public int iterationCount = 2000;

        [Benchmark]
        public void RunBenchMark()
        {
            for (int i = 1; i < iterationCount; i++)
            {
                operations.UpdateLastName(i+1, "Johansson");
               
            }
        }
    }
    
}
