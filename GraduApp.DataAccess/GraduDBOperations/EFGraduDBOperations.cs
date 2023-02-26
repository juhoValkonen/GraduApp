using GraduApp.DataAccess.Enums;
using GraduApp.DataAccess.GraduModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;


namespace GraduApp.DataAccess.GraduDBOperations
{
 public class EFGraduDBOperations : IGraduDBOperations
 {
    private GraduDBType _dbType;
    private bool _enableLogging;
    private IConfiguration configuration;
    private bool useChangeTrackingOnSelects = true;
    public EFGraduDBOperations(GraduDBType dbType, 
                                bool enableLogging,
                                IConfiguration _configuration,
                                bool _useChangeTrackingOnSelects)
    {
        _dbType = dbType;
        _enableLogging = enableLogging;
        configuration = _configuration;
        useChangeTrackingOnSelects = _useChangeTrackingOnSelects;
        }
    public List<Product> GetProductsByCategoryId(int categoryId)
    {
        using (GraduDBContext ctx = new(_dbType, configuration, 
            false, _enableLogging, useChangeTrackingOnSelects))
        {
            return ctx.Product
                .Where(x => x.ProductCategoryID == categoryId)
                .ToList();
        }
    }

    public int CountProductsByCategoryId(int categoryId)
    {
        using (GraduDBContext ctx = new(_dbType, configuration, 
            false, _enableLogging, useChangeTrackingOnSelects))
        {
            return ctx.Product
                .Count(w => w.ProductCategoryID == categoryId);
        }
    }

    public List<Product> SearchProducts(string keyword)
    {
        using (GraduDBContext ctx = new(_dbType, configuration, 
            false, _enableLogging, useChangeTrackingOnSelects))
        {
            return ctx.Product
                .Where(x => x.Name.Contains(keyword))
                .ToList();
        }
    }

    public decimal GetTotalAmountByCustomerId(int customerId)
    {
        using (GraduDBContext ctx = new(_dbType, configuration
            , false, _enableLogging, useChangeTrackingOnSelects))
        {
            var sum = (from d in ctx.SalesOrderDetail
                        join h in ctx.SalesOrderHeader
                        on d.SalesOrder.ID equals h.ID
                        select new
                        {
                            h.CustomerID,
                            d.LineTotal
                        })
                    .Where(x => x.CustomerID == customerId)
                    .Sum(x => x.LineTotal);

            return sum;
        }
    }

    public void UpdateLastName(int customerId, string newLastName)
    {
        using (GraduDBContext ctx = new(_dbType, configuration,
            false, _enableLogging))
        {
            Customer c = ctx.Customer.First(x => x.ID == customerId);
            c.LastName = newLastName;
            ctx.SaveChanges();
        }
    }

    public void MultiplyPricesByCategoryId(int categoryId, 
                                            decimal priceChange)
    {
        using (GraduDBContext ctx = new(_dbType, configuration,
            false, _enableLogging))
        {
            List<Product> products = ctx.Product
                .Where(x => x.ProductCategoryID == categoryId)
                .ToList();

            foreach (Product product in products)
            {
                product.ListPrice = product.ListPrice * priceChange;
            }
            ctx.SaveChanges();
        }
    }

    public void UpdateProductCategoryByCategoryId(int categoryId,
                                                    int newCategoryId)
    {
        using (GraduDBContext ctx = new(_dbType, configuration,
            false, _enableLogging))
        {
            List<Product> products = ctx.Product
                .Where(x => x.ProductCategoryID == categoryId).ToList();
            foreach (Product product in products)
            {
                product.ProductCategoryID = newCategoryId;
            }
            ctx.SaveChanges();
        }
    }
    public void DeleteCustomerDataByCustomerId(int customerId)
    {
        using (GraduDBContext ctx = new(_dbType, configuration,
                                        false, _enableLogging))
        {
                
                var customer = ctx.Customer
                                    .Include(x => x.CustomerAddress)
                                    .Include(x => x.SalesOrderHeader)
                                    .ThenInclude(x => x.SalesOrderDetail)
                                    .Where(x => x.ID == customerId).First();


                customer.SalesOrderHeader.ToList().ForEach(x =>
                {
                    x.SalesOrderDetail.ToList().ForEach(x => ctx.Remove(x));
                    ctx.Remove(x);
                });

                customer.CustomerAddress.ToList().ForEach(x => ctx.Remove(x));
                ctx.Remove(customer);
                    
                
            ctx.SaveChanges();

        }
    }

    public void DeleteSalesOrderDetailById(int salesOrderDetailID)
    {
        using (GraduDBContext ctx = new(_dbType, configuration, 
                                        false, _enableLogging))
        {
            var customer = ctx.SalesOrderDetail
                            .Where(x => x.ID == salesOrderDetailID)
                            .First();

            ctx.Remove(customer);
            ctx.SaveChanges();
        }
    }

    public void DeleteProductCategoryByID(int productCategoryID)
    {
           
            using (GraduDBContext ctx = new(_dbType, configuration,
                                            false, _enableLogging))
            {
                var products = ctx.Product
                                .Where(x =>
                                    x.ProductCategoryID == productCategoryID
                                    ).ToList();
                var productIdList = products.Select(x => x.ID).ToList();
                    

                var salesOrderDetails = ctx.SalesOrderDetail
                                .Where(x => productIdList.Contains(x.ProductID))
                                .ToList();

                ctx.RemoveRange(salesOrderDetails);

                var category = ctx.ProductCategory
                                .Where(x => x.ID == productCategoryID)
                                .ToList();

                ctx.RemoveRange(category);
                ctx.RemoveRange(products);

                var productModelIdList = products.Select(x => x.ProductModelID)
                                        .ToList();
                var productModelProductDescriptions = ctx.ProductModelProductDescription
                                                    .Where(
                                                        x => productModelIdList.Contains(x.ProductModelID)
                                                        ).ToList();

                ctx.RemoveRange(productModelProductDescriptions);

                var productDescriptionIdList = productModelProductDescriptions
                                                .Select(x => x.ProductDescriptionID)
                                                    .ToList();
                var productModels = ctx.ProductModel
                                        .Where(x => productModelIdList
                                            .Contains(x.ID))
                                            .ToList();

                ctx.RemoveRange(productModels);

                var productDescriptions = ctx.ProductDescription.
                                                Where(x => 
                                                    productDescriptionIdList.Contains(x.ID)
                                                ).ToList();
                ctx.RemoveRange(productDescriptions);

                ctx.SaveChanges();
                
        }
            
    }

    public int InsertProduct(Product p)
    {
        using (GraduDBContext ctx = new(_dbType, configuration, false, _enableLogging))
        {
            ctx.Product.Add(p);
            ctx.SaveChanges();
        }
        return p.ID;
    }



    public int InsertSalesOrderHeader(SalesOrderHeader order)
    {
        using (GraduDBContext ctx = new(_dbType, configuration, false, _enableLogging))
        {
            ctx.SalesOrderHeader.Add(order);
            ctx.SaveChanges();
        }
        return order.ID;
    }

    public int InsertCustomer(Customer customer)
    {
        using (GraduDBContext ctx = new(_dbType, configuration, false, _enableLogging))
        {
            ctx.Add(customer);
            ctx.SaveChanges();
        }
        return customer.ID;
    }
 }
}
