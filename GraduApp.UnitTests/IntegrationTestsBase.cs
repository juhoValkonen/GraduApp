using GraduApp.DataAccess;
using GraduApp.DataAccess.Enums;
using GraduApp.DataAccess.GraduDBOperations;
using GraduApp.DataAccess.GraduDBUtils;
using GraduApp.DataAccess.GraduModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Data;
using System.Text;

namespace GraduApp.IntegrationTests
{
    [TestClass]
    public class IntegrationTestsBase
    {
        protected readonly GraduDBOperationsFactory dbOperationsFactory;
        protected static IConfiguration configuration;

        public IntegrationTestsBase()
        {
            dbOperationsFactory = new GraduDBOperationsFactory();
            Configure();
        }

        protected static void setupDb(GraduDBType dbType)
        {
            if(configuration == null)
            {
                Configure();
            }

            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);

            using (GraduDBContext ctx = new(dbType, configuration, false, false))
            {
                GraduDBCreator c = new(ctx);
                c.CreateDatabase();  
            }
            using (GraduDBContext ctx = new(dbType, configuration))
            {
                GraduDBSeeder s = new(ctx, configuration);
                s.Seed();
            }
        }

        protected static void Configure()
        {

            var builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true)
                .AddJsonFile($"appsettings.Development.json", true, true);

            configuration = builder.Build();

        }
    }
}