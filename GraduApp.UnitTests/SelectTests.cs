using GraduApp.DataAccess.Enums;
using GraduApp.DataAccess.GraduDBOperations;
using GraduApp.DataAccess.GraduModels;

namespace GraduApp.IntegrationTests
{
    [TestClass]
    public class SelectTests : IntegrationTestsBase
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
        [DataRow(true, GraduDBType.SQLServer, true)]
        [DataRow(false, GraduDBType.SQLServer)]
        [DataRow(true, GraduDBType.MYSQL)]
        [DataRow(true, GraduDBType.MYSQL, true)]
        [DataRow(false, GraduDBType.MYSQL)]
       [DataRow(true, GraduDBType.PostgreSQL)]
       [DataRow(true, GraduDBType.PostgreSQL, true)]
       [DataRow(false, GraduDBType.PostgreSQL)]
       [DataRow(true, GraduDBType.Oracle)]
       [DataRow(true, GraduDBType.Oracle, true)]
       [DataRow(false, GraduDBType.Oracle)]
        public void GetProductsByCategoryId(bool useEntityFramework, GraduDBType dbType, bool disableChangeTracking = false)
        {
            int categoryID = 18;
            int ExpectedCount = 5;
            

            IGraduDBOperations operations = dbOperationsFactory.Create(dbType, configuration, useEntityFramework, disableChangeTracking:disableChangeTracking);
            List<Product> products = operations.GetProductsByCategoryId(categoryID);
            Assert.AreEqual(ExpectedCount, products.Count());
        }

        [TestMethod]
        [DataRow(true, GraduDBType.SQLServer)]
        [DataRow(true, GraduDBType.SQLServer, true)]
        [DataRow(false, GraduDBType.SQLServer)]
        [DataRow(true, GraduDBType.MYSQL)]
        [DataRow(true, GraduDBType.MYSQL, true)]
        [DataRow(false, GraduDBType.MYSQL)]
       [DataRow(true, GraduDBType.PostgreSQL)]
       [DataRow(true, GraduDBType.PostgreSQL, true)]
       [DataRow(false, GraduDBType.PostgreSQL)]
       [DataRow(true, GraduDBType.Oracle)]
       [DataRow(true, GraduDBType.Oracle, true)]
       [DataRow(false, GraduDBType.Oracle)]
        public void CountProductsByCategoryId(bool useEntityFramework, GraduDBType dbType, bool disableChangeTracking = false)
        {
            int categoryID = 18;
            int ExpectedCount = 5;
            

            IGraduDBOperations operations = dbOperationsFactory.Create(dbType, configuration, useEntityFramework, disableChangeTracking: disableChangeTracking);
            Assert.AreEqual(ExpectedCount, operations.CountProductsByCategoryId(categoryID));
        }

        [TestMethod]
        [DataRow(true, GraduDBType.SQLServer)]
        [DataRow(true, GraduDBType.SQLServer, true)]
        [DataRow(false, GraduDBType.SQLServer)]
        [DataRow(true, GraduDBType.MYSQL)]
        [DataRow(true, GraduDBType.MYSQL, true)]
        [DataRow(false, GraduDBType.MYSQL)]
        [DataRow(true, GraduDBType.PostgreSQL)]
        [DataRow(true, GraduDBType.PostgreSQL, true)]
        [DataRow(false, GraduDBType.PostgreSQL)]
        [DataRow(true, GraduDBType.Oracle)]
        [DataRow(true, GraduDBType.Oracle, true)]
        [DataRow(false, GraduDBType.Oracle)]
        public void GetTotalAmountByCustomerId(bool useEntityFramework, GraduDBType dbType, bool disableChangeTracking = false)
        {
            int customerID = 294;
            decimal ExpectedAmount = new decimal(1853.025);
            

            IGraduDBOperations operations = dbOperationsFactory.Create(dbType, configuration, useEntityFramework, disableChangeTracking: disableChangeTracking);
            decimal totalAmount = operations.GetTotalAmountByCustomerId(customerID);
            Assert.AreEqual(ExpectedAmount, totalAmount);
        }

        [TestMethod]
        [DataRow(true, GraduDBType.SQLServer)]
        [DataRow(true, GraduDBType.SQLServer, true)]
        [DataRow(false, GraduDBType.SQLServer)]
        [DataRow(true, GraduDBType.MYSQL)]
        [DataRow(true, GraduDBType.MYSQL, true)]
        [DataRow(false, GraduDBType.MYSQL)]
        [DataRow(true, GraduDBType.PostgreSQL)]
        [DataRow(true, GraduDBType.PostgreSQL, true)]
        [DataRow(false, GraduDBType.PostgreSQL)]
        [DataRow(true, GraduDBType.Oracle)]
        [DataRow(true, GraduDBType.Oracle, true)]
        [DataRow(false, GraduDBType.Oracle)]
        public void SearchProductsByName(bool useEntityFramework, GraduDBType dbType, bool disableChangeTracking = false)
        {
            
            IGraduDBOperations operations = dbOperationsFactory.Create(dbType, configuration, useEntityFramework, disableChangeTracking: disableChangeTracking);
            Assert.AreEqual(23, operations.SearchProducts("Varsi").Count());
        }
    }
}
