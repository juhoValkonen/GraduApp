// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Logging;

using GraduApp.DataAccess.Enums;
using GraduApp.DataAccess.GraduDBUtils;
using Microsoft.Extensions.Logging;
using GraduApp.DataAccess.GraduDBOperations;
using GraduApp.DataAccess;
using GraduApp.DataAccess.GraduModels;
using Org.BouncyCastle.Tls;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Text;
//using GraduApp.DataAccess.GraduDBContext;

namespace GraduApp
{
    public class Program
    {
        
        private static ILogger<Program> _logger;
        private static IConfiguration configuration;
        public static void Main(string[] args)
        {
            Configure();
            GraduDBOperationsFactory factory =new GraduDBOperationsFactory();
            //setupLogging();
            
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
            
            using (GraduDBContext ctx = new(GraduDBType.SQLServer, configuration, false, false))
            {
                setupDb(ctx);
                
            }
            //IGraduDBOperations operations = factory.Create(GraduDBType.SQLServer, configuration, true );
            //operations.CountProductsByCategoryId(18);

            IGraduDBOperations operations = factory.Create(GraduDBType.SQLServer, configuration, true, true);

          //  for (int i = 0; i < 2000; i++)
           // {
             //   Console.WriteLine(i);
                operations.DeleteCustomerDataByCustomerId(1);

            //}
        }


    

        private static void setupDb(GraduDBContext ctx)
        {
            GraduDBCreator c = new GraduDBCreator(ctx);
            c.CreateDatabase();

            GraduDBSeeder s = new GraduDBSeeder(ctx, configuration);
            s.Seed();
        }

        private static void setupLogging()
        {
            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning)
                    .AddFilter("GraduApp.Program", LogLevel.Debug).AddConsole();
            });
            _logger = loggerFactory.CreateLogger<Program>();
           
        }

        private static void Configure()
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true)
                .AddJsonFile($"appsettings.{environmentName}.json", true, true);

            configuration = builder.Build();
            
        }
    }

    public static class BenchmarkDataGenerator
    {
        public static Customer MakeCustomer(int seed)
        {
            Random randomNumberGenerator = new Random();
            return new Customer()
            {
                NameStyle = false,
                Title = CreateString(3, randomNumberGenerator),
                FirstName = CreateString(6, randomNumberGenerator),
                MiddleName = CreateString(5, randomNumberGenerator),
                LastName = CreateString(19, randomNumberGenerator),
                Suffix = CreateString(3, randomNumberGenerator),
                CompanyName = CreateString(15, randomNumberGenerator),
                SalesPerson = CreateString(15, randomNumberGenerator),
                EmailAddress = CreateString(15, randomNumberGenerator),
                Phone = CreateString(10, randomNumberGenerator),
                PasswordHash = CreateString(20, randomNumberGenerator),
                PasswordSalt = CreateString(15, randomNumberGenerator),
                rowguid = Guid.NewGuid().ToString(),
                ModifiedDate = DateTime.Now,
                CustomerAddress = {new CustomerAddress()
               {
                   AddressType = CreateString(10, randomNumberGenerator),
                   rowguid = Guid.NewGuid().ToString(),
                   ModifiedDate = DateTime.Now,
                   Address = new Address()
                   {
                        AddressLine1 = CreateString(10, randomNumberGenerator),
                        AddressLine2 = CreateString(5, randomNumberGenerator),
                        City = CreateString(10, randomNumberGenerator),
                        StateProvince = CreateString(10, randomNumberGenerator),
                        CountryRegion = CreateString(10, randomNumberGenerator),
                        PostalCode = CreateString(5, randomNumberGenerator),
                        rowguid = Guid.NewGuid().ToString(),
                        ModifiedDate = DateTime.Now,
                   }
               }
               }
            };

        }

        public static Product MakeProduct(int seed)
        {
            Random randomNumberGenerator = new Random();
            return new Product()
            {
                Name = CreateString(15, randomNumberGenerator),
                ProductNumber = CreateString(8, randomNumberGenerator),
                Color = CreateString(5, randomNumberGenerator),
                StandardCost = CreateDecimal(randomNumberGenerator),
                ListPrice = CreateDecimal(randomNumberGenerator),
                Size = CreateString(1, randomNumberGenerator),
                Weight = CreateDecimal(randomNumberGenerator),
                ProductCategoryID = (seed % 49) + 1,
                ProductModelID = seed % 5000,
                SellStartDate = CreateDateTime(randomNumberGenerator),
                SellEndDate = CreateDateTime(randomNumberGenerator),
                DiscontinuedDate = CreateDateTime(randomNumberGenerator),
                ThumbNailPhoto = Encoding.ASCII.GetBytes(CreateString(10, randomNumberGenerator)),
                ThumbnailPhotoFileName = CreateString(10, randomNumberGenerator) + ".png",
                rowguid = Guid.NewGuid().ToString(),
                ModifiedDate = DateTime.Now,
            };
        }

        public static SalesOrderHeader MakeSalesOrderHeader(int seed)
        {
            Random randomNumberGenerator = new Random();
            return new SalesOrderHeader()
            {
                RevisionNumber = 1,
                OrderDate = CreateDateTime(randomNumberGenerator),
                DueDate = CreateDateTime(randomNumberGenerator),
                ShipDate = CreateDateTime(randomNumberGenerator),
                Status = 5,
                OnlineOrderFlag = true,
                SalesOrderNumber = CreateString(10, randomNumberGenerator),
                PurchaseOrderNumber = CreateString(15, randomNumberGenerator),
                AccountNumber = CreateString(15, randomNumberGenerator),
                CustomerID = seed, // Olemassaoleva asiakas
                ShipToAddressID = seed,
                BillToAddressID = seed,
                ShipMethod = CreateString(15, randomNumberGenerator),
                SubTotal = CreateDecimal(randomNumberGenerator),
                TaxAmt = CreateDecimal(randomNumberGenerator),
                Freight = CreateDecimal(randomNumberGenerator),
                TotalDue = CreateDecimal(randomNumberGenerator),
                rowguid = Guid.NewGuid().ToString(),
                Comment = CreateString(15, randomNumberGenerator),
                CreditCardApprovalCode = CreateString(4, randomNumberGenerator),
                ModifiedDate = DateTime.Now,
                SalesOrderDetail =
                {
                    new SalesOrderDetail()
                    {
                        OrderQty = 1,
                        ProductID = seed + 1,
                        UnitPrice =  CreateDecimal(randomNumberGenerator),
                        UnitPriceDiscount = 0,
                        LineTotal =  CreateDecimal(randomNumberGenerator),
                        rowguid = Guid.NewGuid().ToString(),
                        ModifiedDate= DateTime.Now,
                    },
                     new SalesOrderDetail()
                    {
                        OrderQty = 1,
                        ProductID = seed + 2,
                        UnitPrice =  CreateDecimal(randomNumberGenerator),
                        UnitPriceDiscount = 0,
                        LineTotal =  CreateDecimal(randomNumberGenerator),
                        rowguid = Guid.NewGuid().ToString(),
                        ModifiedDate= DateTime.Now,
                    },
                      new SalesOrderDetail()
                    {
                        OrderQty = 1,
                        ProductID = seed + 3,
                        UnitPrice =  CreateDecimal(randomNumberGenerator),
                        UnitPriceDiscount = 0,
                        LineTotal =  CreateDecimal(randomNumberGenerator),
                        rowguid = Guid.NewGuid().ToString(),
                        ModifiedDate= DateTime.Now,
                    }
                }
            };
        }
        private static string CreateString(int stringLength, Random randomNumberGenerator)
        {
            const string allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@$?_-";
            char[] chars = new char[stringLength];

            for (int i = 0; i < stringLength; i++)
            {
                chars[i] = allowedChars[randomNumberGenerator.Next(0, allowedChars.Length)];
            }

            return new string(chars);
        }

        private static decimal CreateDecimal(Random randomNumberGenerator)
        {
            int precision = randomNumberGenerator.Next(2, 3);
            int scale = randomNumberGenerator.Next(2, precision);

            if (randomNumberGenerator == null)
                throw new ArgumentNullException("randomNumberGenerator");
            if (!(precision >= 1 && precision <= 28))
                throw new ArgumentOutOfRangeException("precision", precision, "Precision must be between 1 and 28.");
            if (!(scale >= 0 && scale <= precision))
                throw new ArgumentOutOfRangeException("scale", precision, "Scale must be between 0 and precision.");

            Decimal d = 0m;
            for (int i = 0; i < precision; i++)
            {
                int r = randomNumberGenerator.Next(0, 10);
                d = d * 100m + r;
            }
            for (int s = 0; s < scale; s++)
            {
                d /= 10m;
            }
            if (randomNumberGenerator.Next(2) == 1)
                d = decimal.Negate(d);
            return d;
        }

        private static DateTime CreateDateTime(Random randomNumberGenerator)
        {
            DateTime start = new DateTime(1995, 1, 1);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(randomNumberGenerator.Next(range));
        }
    }
}
