using GraduApp.DataAccess.Enums;
using GraduApp.DataAccess.GraduDBOperations;
using GraduApp.DataAccess.GraduModels;
using GraduApp.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GraduApp.IntegrationTests
{
    [TestClass]
    public class InsertTests : IntegrationTestsBase
    {

        [ClassInitialize]
        public static void SetupDb(TestContext context)
        {
            setupDb(GraduDBType.SQLServer);
            setupDb(GraduDBType.MYSQL);
            setupDb(GraduDBType.Oracle);
            setupDb(GraduDBType.PostgreSQL);
        }

        [TestMethod]
        [DataRow(true, GraduDBType.SQLServer)]
        [DataRow(false, GraduDBType.SQLServer)]
        [DataRow(true, GraduDBType.MYSQL)]
        [DataRow(false, GraduDBType.MYSQL)]
        [DataRow(true, GraduDBType.PostgreSQL)]
        [DataRow(false, GraduDBType.PostgreSQL)]
        [DataRow(true, GraduDBType.Oracle)]
        [DataRow(false, GraduDBType.Oracle)]
        public void InsertSalesOrderHeader(bool useEntityFramework, GraduDBType dbType)
        {
            SalesOrderHeader salesOrder = getTestSalesOrder();

            IGraduDBOperations operations = dbOperationsFactory.Create(dbType, configuration, useEntityFramework);

            int newID = operations.InsertSalesOrderHeader(salesOrder);
            using (GraduDBContext ctx = new(dbType, configuration, false, false))
            {
                List<SalesOrderHeader> assertList = ctx.SalesOrderHeader
                    .Include(x => x.SalesOrderDetail).Where(x => x.ID == newID)
                    .ToList();
                Assert.AreEqual(1, assertList.Count());
                Assert.AreEqual(1, assertList[0].SalesOrderDetail.Count());
            }

        }

        private static SalesOrderHeader getTestSalesOrder()
        {
            SalesOrderHeader salesOrder = new SalesOrderHeader()
            {
                RevisionNumber = 1,
                OrderDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(10),
                ShipDate = DateTime.Now.AddDays(30),
                Status = 5,
                OnlineOrderFlag = true,
                SalesOrderNumber = "SO99999",
                PurchaseOrderNumber = "PO9999999",
                AccountNumber = "10-4020-000999",
                CustomerID = 2948, // Olemassaoleva asiakas
                ShipToAddressID = 1086,
                BillToAddressID = 1086,
                ShipMethod = "CARGO TRANSPORT 5",
                SubTotal = new decimal(880.348),
                TaxAmt = new decimal(70.428),
                Freight = new decimal(22.009),
                TotalDue = new decimal(972.785),
                rowguid = Guid.NewGuid().ToString(),
                Comment = "TestComment",
                CreditCardApprovalCode = "897",
                ModifiedDate = DateTime.Now,
                SalesOrderDetail =
                {
                    new SalesOrderDetail()
                    {
                        OrderQty = 1,
                        ProductID = 907,
                        UnitPrice = new decimal(63.900),
                        UnitPriceDiscount = 0,
                        LineTotal = new decimal(63.900),
                        rowguid = Guid.NewGuid().ToString(),
                        ModifiedDate= DateTime.Now,
                    }
                }
            };
            return salesOrder;
        }

        [TestMethod]
        [DataRow(true, GraduDBType.SQLServer)]
        [DataRow(false, GraduDBType.SQLServer)]
        [DataRow(true, GraduDBType.MYSQL)]
        [DataRow(false, GraduDBType.MYSQL)]
        [DataRow(true, GraduDBType.PostgreSQL)]
        [DataRow(false, GraduDBType.PostgreSQL)]
        [DataRow(true, GraduDBType.Oracle)]
        [DataRow(false, GraduDBType.Oracle)]
        public void InsertProduct(bool useEntityFramework, GraduDBType dbType)
        {
            Product p = getTestProduct();

            IGraduDBOperations operations = dbOperationsFactory.Create(dbType, configuration, useEntityFramework);

            int newID = operations.InsertProduct(p);
            using (GraduDBContext ctx = new(dbType, configuration, false, false))
            {
                List<Product> assertList = ctx.Product.Where(x => x.ID == newID).ToList();
                Assert.AreEqual(1, assertList.Count());

            }

        }

        private static Product getTestProduct()
        {
            return new Product()
            {
                Name = "Test product",
                ProductNumber = "TEST-999",
                Color = "Green",
                StandardCost = new decimal(87.93),
                ListPrice = new decimal(117.93),
                Size = "L",
                Weight = new decimal(2307),
                ProductCategoryID = 2,
                ProductModelID = 6,
                SellStartDate = DateTime.Today,
                SellEndDate = DateTime.Today.AddDays(365),
                DiscontinuedDate = DateTime.Today.AddDays(365),
                ThumbNailPhoto = Encoding.ASCII.GetBytes("test"),
                ThumbnailPhotoFileName = "test.png",
                rowguid = Guid.NewGuid().ToString(),
                ModifiedDate = DateTime.Now,


            };
        }

        [TestMethod]
        [DataRow(true, GraduDBType.SQLServer)]
        [DataRow(false, GraduDBType.SQLServer)]
        [DataRow(true, GraduDBType.MYSQL)]
        [DataRow(false, GraduDBType.MYSQL)]
        [DataRow(true, GraduDBType.PostgreSQL)]
        [DataRow(false, GraduDBType.PostgreSQL)]
        [DataRow(true, GraduDBType.Oracle)]
        [DataRow(false, GraduDBType.Oracle)]
        public void InsertCustomer(bool useEntityFramework, GraduDBType dbType)
        {
            Customer c = getTestCustomer();
            

            IGraduDBOperations operations = dbOperationsFactory.Create(dbType, configuration, useEntityFramework);

            int newID = operations.InsertCustomer(c);
            using (GraduDBContext ctx = new(dbType, configuration, false, false))
            {
                List<Customer> assertList = ctx.Customer
                        .Include(x => x.CustomerAddress).Where(x => x.ID == newID).ToList();
                Assert.AreEqual(1, assertList.Count());
                Assert.AreEqual(1, assertList[0].CustomerAddress.Count());

            }
        }

        private static Customer getTestCustomer()
        {
            return new Customer()
            {
                NameStyle = false,
                Title = "Mr.",
                FirstName = "Timo",
                MiddleName = "T",
                LastName = "Testaaja",
                Suffix = "Jr.",
                CompanyName = "Adventureworks Client",
                SalesPerson = "adventure-works\\david8",
                EmailAddress = "Timo.testaaja@example.com",
                Phone = "123-456-789",
                PasswordHash = "ElzTpSNbUW1Ut+L5cWlfR7MF6nBZia8WpmGaQPjLOJA=",
                PasswordSalt = "nm7D5e4=",
                rowguid = Guid.NewGuid().ToString(),
                ModifiedDate = DateTime.Now,
                CustomerAddress = {new CustomerAddress()
               {
                   AddressType = "Main office",
                   rowguid = Guid.NewGuid().ToString(),
                   ModifiedDate = DateTime.Now,
                   Address = new Address()
                   {
                        AddressLine1 = "Testikatu 1",
                        AddressLine2 = "",
                        City = "Testikaupunki",
                        StateProvince = "Testikunta",
                        CountryRegion = "Finland",
                        PostalCode = "33870",
                        rowguid = Guid.NewGuid().ToString(),
                        ModifiedDate = DateTime.Now,
                   }
               }
               }

            };
        }
    }
}
