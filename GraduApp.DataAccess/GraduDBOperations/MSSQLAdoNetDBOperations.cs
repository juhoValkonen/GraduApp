using GraduApp.DataAccess.GraduModels;
using Microsoft.Data.SqlClient;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Text;

namespace GraduApp.DataAccess.GraduDBOperations
{
    internal class MSSQLAdoNetDBOperations : IGraduDBOperations
    {
        private readonly string connectionString;

        public MSSQLAdoNetDBOperations(string _connectionString)
        {
            connectionString = _connectionString;
        }

        public int CountProductsByCategoryId(int categoryId)
        {
            SqlParameter[] parameters =
          {
              new SqlParameter()
              {
                  Value = categoryId,
                  ParameterName = "@p0",
                  SqlDbType = SqlDbType.Int,
              }

          };

            using (SqlConnection conn = new(connectionString))
            {
                using (SqlCommand cmd = makeSqlCommand(@"SELECT COUNT(*) FROM [Product] AS [p] WHERE [p].[ProductCategoryID]=@p0", parameters, conn))
                {
                    conn.Open();
                    int ret = (int)cmd.ExecuteScalar();
                    return ret;
                }
            }
        }

        public void DeleteCustomerDataByCustomerId(int customerId)
        {
            SqlParameter[] parameters =
          {
              new SqlParameter()
              {
                  Value = customerId,
                  ParameterName = "@p0",
                  SqlDbType = SqlDbType.Int,
              }

          };

            HashSet<int> salesOrderIDs = new();
            HashSet<int> salesOrderDetailIDs = new();
            HashSet<int> customerAddressIds = new();


            using (SqlConnection conn = new(connectionString))
            {
                conn.Open();
                using (SqlTransaction tx = conn.BeginTransaction())
                {
                    // Haetaan SalesOrders
                    using (SqlCommand cmd = makeSqlCommand(@"SET NOCOUNT ON; select distinct h.ID, d.ID, ca.ID from SalesOrderHeader h join SalesOrderDetail d on h.ID = d.SalesOrderID 
                                                    join CustomerAddress ca on h.CustomerID=ca.CustomerID where ca.CustomerID= @p0", parameters, conn, tx))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            _ = salesOrderIDs.Add(reader.GetInt32(0));
                            _ = salesOrderDetailIDs.Add(reader.GetInt32(1));
                            _ = customerAddressIds.Add(reader.GetInt32(2));
                        }
                        reader.Close();
                        cmd.Parameters.Clear();
                    }

                    List<SqlParameter> paramList = new List<SqlParameter>();
                    
                    paramList.Add(new SqlParameter()
                    {
                        Value = customerId,
                        ParameterName = "@p0",
                        SqlDbType = SqlDbType.Int,
                    });

                    string statement = GetDeleteCustomerStatement(salesOrderIDs, salesOrderDetailIDs, customerAddressIds, paramList);

                    using (SqlCommand cmd = makeSqlCommand(statement, paramList.ToArray(), conn, tx))
                    {
                        _ = cmd.ExecuteNonQuery();
                    }
                    tx.Commit();
                }

            }

        }

        public void DeleteProductCategoryByID(int productCategoryID)
        {
            SqlParameter[] parameters =
          {
              new SqlParameter()
              {
                  Value = productCategoryID,
                  ParameterName = "@p0",
                  SqlDbType = SqlDbType.Int,
              }

          };


            List<int> productIDs = new();
            List<int> productModelIDs = new();
            List<int> productDescriptionIDs = new();
            string productModelIDsCommaSeparated = "";

            using (SqlConnection conn = new(connectionString))
            {
                conn.Open();
                using (SqlTransaction tx = conn.BeginTransaction())
                {

                    using (SqlCommand cmd = makeSqlCommand(@"SET NOCOUNT ON; select [p].[ID], [p].[ProductModelID] from [Product] [p]  WHERE [p].[ProductCategoryID]=@p0", parameters, conn, tx))
                    {

                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            productIDs.Add(reader.GetInt32(0));
                            productModelIDs.Add(reader.GetInt32(1));
                        }
                        reader.Close();
                        cmd.Parameters.Clear();
                    }


                    if (productModelIDs.Count > 0)
                    {

                        productModelIDsCommaSeparated = string.Join(",", productModelIDs.Select(n => n.ToString()).ToArray());
                        using SqlCommand cmd = makeSqlCommand(@"SET NOCOUNT ON; select [ProductDescriptionID] from [ProductModelProductDescription] where [productModelID] in (" + productModelIDsCommaSeparated + ")", conn, tx);
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            productDescriptionIDs.Add(reader.GetInt32(0));
                        }
                        reader.Close();

                    }
                    List<SqlParameter> paramList = new List<SqlParameter>()
                    {
                        new SqlParameter()
                          {
                              Value = productCategoryID,
                              ParameterName = "@p0",
                              SqlDbType = SqlDbType.Int,
                          }
                    };

                    string deleteStatement = GetDeleteProductCategoryStatement(productIDs, productModelIDs, productDescriptionIDs, paramList);


                    // Tuotemallit
                    using (SqlCommand cmd = makeSqlCommand(deleteStatement, paramList.ToArray(), conn, tx))
                    {
                        _ = cmd.ExecuteNonQuery();
                    }
                    tx.Commit();
                }
            }

        }

        public void DeleteSalesOrderDetailById(int salesOrderDetailID)
        {
            SqlParameter[] parameters =
          {
              new SqlParameter()
              {
                  Value = salesOrderDetailID,
                  ParameterName = "@p0",
                  SqlDbType = SqlDbType.Int,
              }

          };


            using (SqlConnection conn = new(connectionString))
            {
                conn.Open();
                using SqlCommand cmd = makeSqlCommand(@"SET NOCOUNT ON; DELETE FROM [SalesOrderDetail] WHERE [ID] = @p0", parameters, conn);
                _ = cmd.ExecuteNonQuery();
            }
        }



        public List<Product> GetProductsByCategoryId(int categoryId)
        {
            SqlParameter[] parameters =
          {
              new SqlParameter()
              {
                  Value = categoryId,
                  ParameterName = "@p0",
                  SqlDbType = SqlDbType.Int,
              }

          };

            List<Product> products = new();
            using (SqlConnection conn = new(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = makeSqlCommand(@" SELECT [p].[ID], [p].[Color], [p].[DiscontinuedDate], [p].[ListPrice], [p].[ModifiedDate], [p].[Name], [p].[ProductCategoryID], [p].[ProductModelID], [p].[ProductNumber], [p].[SellEndDate], [p].[SellStartDate], [p].[Size], [p].[StandardCost], [p].[ThumbNailPhoto], [p].[ThumbnailPhotoFileName], [p].[Weight], [p].[rowguid]
                                                          FROM [Product] AS [p]
                                                          WHERE [p].[ProductCategoryID]=@p0", parameters, conn))
                {


                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        products.Add(MSSQLDataHelper.FromDataReader(reader));
                    }
                    reader.Close();
                }
            }
            return products;
        }

        public decimal GetTotalAmountByCustomerId(int customerId)
        {
            SqlParameter[] parameters =
          {
              new SqlParameter()
              {
                  Value = customerId,
                  ParameterName = "@p0",
                  SqlDbType = SqlDbType.Int,
              }

          };
            object r;
            using (SqlConnection conn = new(connectionString))
            {
                conn.Open();

                using SqlCommand cmd = makeSqlCommand(@" SELECT COALESCE(SUM([s].[LineTotal]), 0.0)
                                                                      FROM [SalesOrderDetail] AS [s]
                                                                      INNER JOIN [SalesOrderHeader] AS [s1] ON [s].[SalesOrderID] = [s1].[ID]
                                                                      WHERE [s1].[CustomerID] = @p0", parameters, conn);


                r = cmd.ExecuteScalar();
            }
            decimal ret;
            if (r != null && DBNull.Value != r)
            {
                ret = (decimal)r;
            }
            else
            {
                return 0;
            }

            return ret;
        }

        public int InsertCustomer(Customer customer)
        {
            using (SqlConnection conn = new(connectionString))
            {
                conn.Open();
                int customerID = 0;
                using (SqlTransaction tx = conn.BeginTransaction())
                {

                    using (SqlCommand cmd = makeSqlCommand(@"INSERT INTO [dbo].[Customer]
                   ([NameStyle]
                   ,[Title]
                   ,[FirstName]
                   ,[MiddleName]
                   ,[LastName]
                   ,[Suffix]
                   ,[CompanyName]
                   ,[SalesPerson]
                   ,[EmailAddress]
                   ,[Phone]
                   ,[PasswordHash]
                   ,[PasswordSalt]
                   ,[rowguid]
                   ,[ModifiedDate]) output INSERTED.ID
                 VALUES
               (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13)", MSSQLDataHelper.GetSqlParameters(customer), conn, tx))
                    {
                        customerID = (int)cmd.ExecuteScalar();
                    }

                    foreach (CustomerAddress a in customer.CustomerAddress)
                    {
                        int addressID = 0;
                        using (SqlCommand cmd = makeSqlCommand(@"INSERT INTO [dbo].[Address]
                                                       ([AddressLine1]
                                                       ,[AddressLine2]
                                                       ,[City]
                                                       ,[StateProvince]
                                                       ,[CountryRegion]
                                                       ,[PostalCode]
                                                       ,[rowguid]
                                                       ,[ModifiedDate]) output INSERTED.ID
                                                 VALUES
                                                       (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7)", MSSQLDataHelper.GetSqlParameters(a.Address), conn, tx))
                        {
                            addressID = (int)cmd.ExecuteScalar();
                        }


                        using (SqlCommand cmd = makeSqlCommand(@"INSERT INTO [dbo].[CustomerAddress]
                                                           ([CustomerID]
                                                           ,[AddressID]
                                                           ,[AddressType]
                                                           ,[rowguid]
                                                           ,[ModifiedDate]) 
                                                          VALUES
                                                                (@p0, @p1, @p2, @p3, @p4)", MSSQLDataHelper.GetSqlParameters(a, customerID, addressID), conn, tx))
                        {
                            _ = cmd.ExecuteNonQuery();
                        }
                    }
                    tx.Commit();
                }
                return customerID;
            }

        }

        public int InsertProduct(Product p)
        {
            int productID = 0;
            using (SqlConnection conn = new(connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = makeSqlCommand(@"INSERT INTO [dbo].[Product]
                                                       ([Name]
                                                       ,[ProductNumber]
                                                       ,[Color]
                                                       ,[StandardCost]
                                                       ,[ListPrice]
                                                       ,[Size]
                                                       ,[Weight]
                                                       ,[ProductCategoryID]
                                                       ,[ProductModelID]
                                                       ,[SellStartDate]
                                                       ,[SellEndDate]
                                                       ,[DiscontinuedDate]
                                                       ,[ThumbNailPhoto]
                                                       ,[ThumbnailPhotoFileName]
                                                       ,[rowguid]
                                                       ,[ModifiedDate])  output INSERTED.ID
                                                 VALUES
                                                       (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15)", MSSQLDataHelper.GetSqlParameters(p), conn))
                {

                    productID = (int)cmd.ExecuteScalar();
                }
            }
            return productID;
        }


        public int InsertSalesOrderHeader(SalesOrderHeader order)
        {
            int headerID = 0;
            using (SqlConnection conn = new(connectionString))
            {
                conn.Open();
                using (SqlTransaction tx = conn.BeginTransaction())
                {

                    using (SqlCommand cmd = makeSqlCommand(@"INSERT INTO [SalesOrderHeader]
                                                       ([RevisionNumber]
                                                       ,[OrderDate]
                                                       ,[DueDate]
                                                       ,[ShipDate]
                                                       ,[Status]
                                                       ,[OnlineOrderFlag]
                                                       ,[SalesOrderNumber]
                                                       ,[PurchaseOrderNumber]
                                                       ,[AccountNumber]
                                                       ,[CustomerID]
                                                       ,[ShipToAddressID]
                                                       ,[BillToAddressID]
                                                       ,[ShipMethod]
                                                       ,[SubTotal]
                                                       ,[TaxAmt]
                                                       ,[Freight]
                                                       ,[TotalDue]
                                                       ,[rowguid]
                                                       ,[Comment]
                                                       ,[ModifiedDate]
                                                       ,[CreditCardApprovalCode]) output INSERTED.ID
                                                 VALUES
                                                       (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15, @p16, @p17, @p18, @p19, @p20)", MSSQLDataHelper.GetSqlParameters(order), conn, tx))
                    {

                        headerID = (int)cmd.ExecuteScalar();

                    }
                    foreach (SalesOrderDetail d in order.SalesOrderDetail)
                    {
                        using SqlCommand cmd = makeSqlCommand(@"INSERT INTO [SalesOrderDetail]
                       ([OrderQty]
                       ,[ProductID]
                       ,[UnitPrice]
                       ,[UnitPriceDiscount]
                       ,[LineTotal]
                       ,[rowguid]
                       ,[ModifiedDate]
                       ,[SalesOrderID])
                        VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7)", MSSQLDataHelper.GetSqlParameters(d, headerID), conn, tx);
                        _ = cmd.ExecuteNonQuery();
                    }
                    tx.Commit();
                }
                return headerID;
            }
        }

        public void MultiplyPricesByCategoryId(int categoryId, decimal priceChange)
        {
            SqlParameter[] parameters =
          {
              new SqlParameter()
              {
                  Value = priceChange,
                  ParameterName = "@p0",

                  SqlDbType = SqlDbType.Decimal,
              },
               new SqlParameter()
              {
                  Value = categoryId,
                  ParameterName = "@p1",

                  SqlDbType = SqlDbType.Int,
              }

          };

            using (SqlConnection conn = new(connectionString))
            {
                conn.Open();
                using SqlCommand cmd = makeSqlCommand(@"UPDATE [Product] SET [ListPrice]=[ListPrice]*@p0 where ProductCategoryID=@p1", parameters, conn);
                _ = cmd.ExecuteNonQuery();
            }
        }

        public List<Product> SearchProducts(string keyword)
        {
            SqlParameter[] parameters =
          {
              new SqlParameter()
              {
                  Value = "%" + keyword + "%",
                  ParameterName = "@p0",

                  SqlDbType = SqlDbType.NVarChar,
              },
          };
            List<Product> products = new();
            using (SqlConnection conn = new(connectionString))
            {
                
                using (SqlCommand cmd = makeSqlCommand(@"SET NOCOUNT ON;SELECT [p].[ID], [p].[Color], [p].[DiscontinuedDate], [p].[ListPrice], [p].[ModifiedDate], [p].[Name], [p].[ProductCategoryID], [p].[ProductModelID], [p].[ProductNumber], [p].[SellEndDate], [p].[SellStartDate], [p].[Size], [p].[StandardCost], [p].[ThumbNailPhoto], [p].[ThumbnailPhotoFileName], [p].[Weight], [p].[rowguid]
                                                          FROM [Product] AS [p]
                                                          WHERE [p].[Name] like @p0", parameters, conn))
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        products.Add(MSSQLDataHelper.FromDataReader(reader));
                    }
                    reader.Close();
                }
            }
            return products;
        }

        public void UpdateLastName(int customerId, string newLastName)
        {
            SqlParameter[] parameters =
          {
              new SqlParameter()
              {
                  Value = newLastName,
                  ParameterName = "@p0",

                  SqlDbType = SqlDbType.Text,
              },
               new SqlParameter()
              {
                  Value = customerId,
                  ParameterName = "@p1",

                  SqlDbType = SqlDbType.Int,
              }
          };

            using (SqlConnection conn = new(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = makeSqlCommand(@"SET NOCOUNT ON;UPDATE [Customer] SET [LastName] = @p0
                                                          WHERE [ID] = @p1;", parameters, conn))
                {
                    _ = cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateProductCategoryByCategoryId(int categoryId, int newCategoryId)
        {
            SqlParameter[] parameters =
          {
              new SqlParameter()
              {
                  Value = newCategoryId,
                  ParameterName = "@p0",
                  SqlDbType = SqlDbType.Int,
              },
               new SqlParameter()
              {
                  Value = categoryId,
                  ParameterName = "@p1",
                  SqlDbType = SqlDbType.Int,
              }
          };


            using (SqlConnection conn = new(connectionString))
            {
                conn.Open();
                using SqlCommand cmd = makeSqlCommand(@"UPDATE [Product] SET [ProductCategoryID]=@p0 where [ProductCategoryID]=@p1", parameters, conn);

                _ = cmd.ExecuteNonQuery();
            }
        }

        private SqlCommand makeSqlCommand(string command, SqlParameter[] parameters, SqlConnection conn, SqlTransaction tx)
        {
            SqlCommand cmd = makeSqlCommand(command, parameters, conn);


            cmd.Transaction = tx;


            return cmd;
        }
        private SqlCommand makeSqlCommand(string command, SqlParameter[] parameters, SqlConnection conn)
        {
            SqlCommand cmd = new(command, conn);

            for (int i = 0; i < parameters.Count(); i++)
            {
                _ = cmd.Parameters.Add(parameters[i]);
            }

            return cmd;
        }

        private SqlCommand makeSqlCommand(string command, SqlConnection conn, SqlTransaction tx = null)
        {
            return makeSqlCommand(command, new SqlParameter[] { }, conn, tx);
        }

        private string GetDeleteProductCategoryStatement(List<int> productIDs, List<int> productModelIDs, List<int> productDescriptionIDs, List<SqlParameter> paramList)
        {
            int countOfParams = paramList.Count;
            StringBuilder deleteStatement = new("SET NOCOUNT ON; ");

           foreach(int productID in productIDs)
            {
                _ = deleteStatement.AppendFormat("DELETE from [SalesOrderDetail] where [ProductID] = @p{0};", countOfParams);

                paramList.Add(new SqlParameter()
                {
                    ParameterName = String.Format("@p{0}", countOfParams),
                    Value = productID,
                    SqlDbType = SqlDbType.Int,
                });

                countOfParams++;
            }

           foreach(int productModelID in productModelIDs)
            {
                _ = deleteStatement.AppendFormat(@"DELETE from ProductModelProductDescription where ProductModelID = @p{0};", countOfParams);


                paramList.Add(new SqlParameter()
                {
                    ParameterName = string.Format("p{0}", countOfParams),
                    Value = productModelID,
                    SqlDbType = SqlDbType.Int
                });

                countOfParams++;
            }

            foreach (int productDescriptionID in productDescriptionIDs)
            {
                _ = deleteStatement.AppendFormat(Environment.NewLine + @"DELETE from ProductDescription where ID =@p{0};", countOfParams);

                paramList.Add(new SqlParameter()
                {
                    ParameterName = string.Format("p{0}", countOfParams),
                    Value = productDescriptionID,
                    SqlDbType = SqlDbType.Int
                });

                countOfParams++;
            }

            foreach (int productID in productIDs)
            {
                _ = deleteStatement.AppendFormat("DELETE from [Product] where ID = @p{0};", countOfParams);

                paramList.Add(new SqlParameter()
                {
                    ParameterName = String.Format("@p{0}", countOfParams),
                    Value = productID,
                    SqlDbType = SqlDbType.Int,
                });

                countOfParams++;

            }

            foreach (int productModelID in productModelIDs)
            {

                _ = deleteStatement.AppendFormat(Environment.NewLine + @"DELETE from ProductModel where ID = @p{0};", countOfParams);

                paramList.Add(new SqlParameter()
                {
                    ParameterName = string.Format("p{0}", countOfParams),
                    Value = productModelID,
                    SqlDbType = SqlDbType.Int
                });

                countOfParams++;
            }
            _ = deleteStatement.Append("DELETE from [ProductCategory] where ID = @p0;");

            return deleteStatement.ToString();
        }

        private string GetDeleteCustomerStatement(HashSet<int> salesOrderIDs, HashSet<int> salesOrderDetailIDs, HashSet<int> customerAddressIds, List<SqlParameter> paramList)
        {
            int countOfParams = paramList.Count;

            StringBuilder s = new("SET NOCOUNT ON;");

            foreach (int salesOrderDetailID in salesOrderDetailIDs)
            {
                _ = s.AppendFormat("DELETE from [SalesOrderDetail] where ID =@p{0};", countOfParams);
                
                paramList.Add(new SqlParameter()
                {
                    ParameterName = String.Format("@p{0}", countOfParams),
                    Value = salesOrderDetailID,
                    SqlDbType = SqlDbType.Int,
                });

                countOfParams++;
            }

            foreach (int salesOrderID in salesOrderIDs)
            {
                _ = s.AppendFormat("DELETE from [SalesOrderHeader] where ID ={0};", countOfParams);

                paramList.Add(new SqlParameter()
                {
                    ParameterName = String.Format("@p{0}", countOfParams),
                    Value = salesOrderID,
                    SqlDbType = SqlDbType.Int,
                });

                countOfParams++;
            }

            foreach (int customerAddressId in customerAddressIds)
            {
                _ = s.AppendFormat("DELETE from [CustomerAddress] where ID ={0};", countOfParams);
                
                paramList.Add(new SqlParameter()
                {
                    ParameterName = String.Format("@p{0}", countOfParams),
                    Value = customerAddressId,
                    SqlDbType = SqlDbType.Int,
                });

                countOfParams++;
            }
            _ = s.Append("DELETE from [Customer] where ID =@p0;");

            return s.ToString();
        }
    }
}
