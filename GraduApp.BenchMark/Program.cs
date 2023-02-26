using BenchmarkDotNet.Running;
using GraduApp.Benchmark.Benchmarks;

namespace GraduApp.BenchMark
{
    public class Program
    {
        public static void Main(string[] args)
        {
              _ = BenchmarkRunner.Run<CountProductsByCategoryId>();
              _ = BenchmarkRunner.Run<GetProductsByCategoryId>();
              _ = BenchmarkRunner.Run<GetTotalAmountByCustomerId>();
              _ = BenchmarkRunner.Run<SearchProductsByName>();
              _ = BenchmarkRunner.Run<UpdateProductCategoryByCategoryId>();
              _ = BenchmarkRunner.Run<MultiplyPricesByCategoryId>();
              _ = BenchmarkRunner.Run<UpdateCustomerLastName>();
              _ = BenchmarkRunner.Run<DeleteCustomerDataByCustomerId>();
              _ = BenchmarkRunner.Run<DeleteProductCategoryByID>();
              _ = BenchmarkRunner.Run<DeleteSalesOrderDetailById>();
              _ = BenchmarkRunner.Run<InsertCustomer>();
              _ = BenchmarkRunner.Run<InsertProduct>();
            _ = BenchmarkRunner.Run<InsertSalesOrderHeader>();
        }

    }
}