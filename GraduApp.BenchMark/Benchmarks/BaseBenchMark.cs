using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Xlsx;
using GraduApp.DataAccess;
using GraduApp.DataAccess.Enums;
using GraduApp.DataAccess.GraduDBOperations;
using GraduApp.DataAccess.GraduDBUtils;
using Microsoft.Extensions.Configuration;


namespace GraduApp.Benchmark.Benchmarks
{
   public enum DBOperationsMode
    {
        
        EFCore,
        EFCore_NoTracking,
        ADONET
    }

    [CsvMeasurementsExporter]
    [RPlotExporter]
     //[XlsxExporter]
     [SimpleJob(RunStrategy.Throughput, launchCount: 1, warmupCount: 10, iterationCount: 30)]
     [MinColumn, Q1Column, Q3Column, MaxColumn, MedianColumn]
    public class BaseBenchMark
    {
        protected IGraduDBOperations operations;
        protected GraduDBOperationsFactory dbOperationsFactory = new GraduDBOperationsFactory();
        protected IConfiguration configuration;

        public BaseBenchMark()
        {
            configuration = Common.GetConfiguration();
        }

        [Params(
                GraduDBType.MYSQL,
                GraduDBType.SQLServer,
                GraduDBType.PostgreSQL,
                GraduDBType.Oracle
            )]
        public GraduDBType dbType;


        [GlobalCleanup]
        public void GlobalCleanup()
        {
            // Disposing logic
        }

        protected void setupDb(GraduDBType dbType)
        {
            Console.WriteLine("Setting up db..");
            if(dbType == GraduDBType.PostgreSQL)
            {
                AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
                AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
            }
            

            using (GraduDBContext ctx = new(dbType, configuration))
            {
                GraduDBCreator c = new(ctx);
                c.CreateDatabase();

                GraduDBSeeder s = new(ctx, configuration);
                s.Seed();
            }
            Thread.Sleep(1000);
            //Console.WriteLine("Done setting up db..");

        }
    }


}
