using BenchmarkDotNet.Attributes;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduApp.Benchmark.Benchmarks
{
    public class DefaultBenchmarkBase : BaseBenchMark
    {
        [Params(DBOperationsMode.EFCore, DBOperationsMode.ADONET)]
        public DBOperationsMode dbOperationsMode;

        [GlobalSetup]
        public void GlobalSetup()
        {
            setupDb(dbType);
            operations = dbOperationsFactory.Create(dbType, configuration, !dbOperationsMode.Equals(DBOperationsMode.ADONET), enableLogging: false);
            
        }
    }
}
