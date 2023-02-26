using GraduApp.DataAccess;
using GraduApp.DataAccess.GraduModels;
using System.Configuration;
using System.Data;

namespace GraduApp.DataAccess.GraduDBOperations
{
    public interface IGraduDBOperations
    {
        // Select
        int CountProductsByCategoryId(int categoryId);
        List<Product> GetProductsByCategoryId(int categoryId);
        decimal GetTotalAmountByCustomerId(int customerId);
        List<Product> SearchProducts(string keyword);

        // Update
        void MultiplyPricesByCategoryId(int categoryId, decimal priceChange);
        void UpdateLastName(int customerId, string newLastName);
        void UpdateProductCategoryByCategoryId(int categoryId, int newCategoryId);

        // Delete
        void DeleteCustomerDataByCustomerId(int customerId);
        void DeleteSalesOrderDetailById(int salesOrderDetailID);
        void DeleteProductCategoryByID(int productCategoryID);

        // Insert
        int InsertProduct(Product p);
        int InsertSalesOrderHeader(SalesOrderHeader order);
        int InsertCustomer(Customer customer);
    }
}
