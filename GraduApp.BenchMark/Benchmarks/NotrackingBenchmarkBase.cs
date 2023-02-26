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
    public class NotrackingBenchmarkBase : BaseBenchMark
    {
        [Params(DBOperationsMode.EFCore_NoTracking, DBOperationsMode.EFCore,  DBOperationsMode.ADONET)]
        public DBOperationsMode dbOperationsMode;

        [GlobalSetup]
        public void GlobalSetup()
        {
            setupDb(dbType);
            if (dbOperationsMode == DBOperationsMode.ADONET)
            {
                operations = dbOperationsFactory.Create(dbType, configuration, !dbOperationsMode.Equals(DBOperationsMode.ADONET), enableLogging: false);
            }
            else
            {
                operations = dbOperationsFactory.Create(dbType, configuration, !dbOperationsMode.Equals(DBOperationsMode.ADONET), enableLogging: false, disableChangeTracking: (dbOperationsMode == DBOperationsMode.EFCore_NoTracking));
            }

        }
    }
}
