using GraduApp.DataAccess.Enums;
using GraduApp.DataAccess.GraduDBOperations;
using GraduApp.DataAccess.GraduModels;
using GraduApp.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace GraduApp.IntegrationTests
{
    [TestClass]
    public class UpdateTests : IntegrationTestsBase
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
        public void UpdateCustomerLastName(bool useEntityFramework, GraduDBType dbType)
        {

            IGraduDBOperations operations = dbOperationsFactory.Create(dbType, configuration, useEntityFramework);
            operations.UpdateLastName(294, "Johansson");
            using (GraduDBContext ctx = new(dbType, configuration))
            {
                Assert.AreEqual("Johansson", ctx.Customer.Where(x => x.ID == 294).First().LastName);

            }
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
        public void MultiplyPricesByCategoryId(bool useEntityFramework, GraduDBType dbType)
        {
        
            decimal multiplier = new(1.1);
            decimal[] prices;
            Product[] productsBeforeUpdate;

            IGraduDBOperations operations = dbOperationsFactory.Create(dbType, configuration, useEntityFramework);
            using (GraduDBContext ctx = new(dbType, configuration))
            {
                productsBeforeUpdate = ctx.Product.Where(x => x.ProductCategoryID == 18).OrderBy(x => x.ID).ToArray();
                prices = new decimal[productsBeforeUpdate.Length];
                for (int i = 0; i < productsBeforeUpdate.Length; i++)
                {
                    prices[i] = productsBeforeUpdate[i].ListPrice;
                }
            }
            operations.MultiplyPricesByCategoryId(18, multiplier);

            using (GraduDBContext ctx = new(dbType, configuration))
            {
                Product[] productsAfterUpdate = ctx.Product.Where(x => x.ProductCategoryID == 18).OrderBy(x => x.ID).ToArray();
                for (int i = 0; i < productsBeforeUpdate.Length; i++)
                {
                    Assert.AreEqual(Math.Round(prices[i] * multiplier, 3, MidpointRounding.AwayFromZero), productsAfterUpdate[i].ListPrice);
                }
            }
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
        public void UpdateProductCategoryByCategoryId(bool useEntityFramework, GraduDBType dbType)
        {
  

            IGraduDBOperations operations = dbOperationsFactory.Create(dbType, configuration, useEntityFramework);
            List<Product> products = operations.GetProductsByCategoryId(18);
            //Assert.AreEqual(0, products.Count());
            operations.UpdateProductCategoryByCategoryId(18, 2);

            products = operations.GetProductsByCategoryId(18);
            Assert.AreEqual(0, products.Count());
        }
    }
}
