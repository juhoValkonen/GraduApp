using GraduApp.DataAccess.Enums;
using GraduApp.DataAccess.GraduDBOperations;
using GraduApp.DataAccess.GraduModels;
using GraduApp.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduApp.IntegrationTests
{
    [TestClass]
    public class DeleteTests : IntegrationTestsBase
    {
        [TestMethod]
        [DataRow(true, GraduDBType.SQLServer)]
        [DataRow(false, GraduDBType.SQLServer)]
        [DataRow(true, GraduDBType.MYSQL)]
        [DataRow(false, GraduDBType.MYSQL)]
        [DataRow(true, GraduDBType.PostgreSQL)]
        [DataRow(false, GraduDBType.PostgreSQL)]
        [DataRow(true, GraduDBType.Oracle)]
        [DataRow(false, GraduDBType.Oracle)]
        public void DeleteCustomerDataByCustomerId(bool useEntityFramework, GraduDBType dbType)
        {
            int customerID = 294;
            setupDb(dbType);

            IGraduDBOperations operations = dbOperationsFactory.Create(dbType, configuration, useEntityFramework);
            operations.DeleteCustomerDataByCustomerId(customerID);
            using (GraduDBContext ctx = new(dbType, configuration))
            {
                Assert.IsNull(ctx.Customer.Where(x => x.ID == customerID).FirstOrDefault());
                Assert.IsNull(ctx.CustomerAddress.Where(x => x.CustomerID == customerID).FirstOrDefault());
                Assert.IsNull(ctx.SalesOrderHeader.Where(x => x.CustomerID == customerID).FirstOrDefault());

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
        public void DeleteProductCategoryByID(bool useEntityFramework, GraduDBType dbType)
        {
            int categoryID = 18;
            setupDb(dbType);

            List<int> productModelIDs = new();

            IGraduDBOperations operations = dbOperationsFactory.Create(dbType, configuration, useEntityFramework);
            operations.DeleteProductCategoryByID(categoryID);

            using (GraduDBContext ctx = new(dbType, configuration))
            {
                Assert.IsNull(ctx.Product.Where(x => x.ProductCategoryID == categoryID).FirstOrDefault());
                Assert.IsNull(ctx.ProductCategory.Where(x => x.ID == categoryID).FirstOrDefault());
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
        public void DeleteSalesOrderDetailByID(bool useEntityFramework, GraduDBType dbType)
        {
            int salesOrderDetailID = 1000;
            setupDb(dbType);

            IGraduDBOperations operations = dbOperationsFactory.Create(dbType, configuration, useEntityFramework);

            operations.DeleteSalesOrderDetailById(salesOrderDetailID);
            using (GraduDBContext ctx = new(dbType, configuration, false, false))
            {
                List<SalesOrderDetail> assertList = ctx.SalesOrderDetail.Where(x => x.ID == salesOrderDetailID).ToList();
                Assert.AreEqual(0, assertList.Count());
            }

        }
    }
}
