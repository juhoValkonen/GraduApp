using GraduApp.DataAccess.GraduModels;

using Npgsql;
using System.Data;
using System.Text;

namespace GraduApp.DataAccess.GraduDBOperations
{
    internal class PostgreSQLAdoNetOperations : IGraduDBOperations
    {
        private readonly string connectionString;

        public PostgreSQLAdoNetOperations(string _connectionString)
        {
            connectionString = _connectionString;
        }
        public int CountProductsByCategoryId(int categoryId)
        {

            NpgsqlParameter[] parameters =
              {
                  new NpgsqlParameter()
                  {
                      Value = categoryId,
                      ParameterName = "@p0",
                      DbType = DbType.Int32
                  }
              };

            using (NpgsqlConnection conn = new(connectionString))
            {

                List<Product> products = new();
                using (NpgsqlCommand cmd = makeSqlCommand(@"SELECT COUNT(""ID"") as c
                                                        FROM public.""Product"" AS p WHERE p.""ProductCategoryID""=@p0", parameters, conn))
                {
                    conn.Open();
                    return (int)(long)cmd.ExecuteScalar();
                }

            }

        }

        public void DeleteCustomerDataByCustomerId(int customerId)
        {
            NpgsqlParameter[] parameters =
          {
              new NpgsqlParameter()
              {
                  Value = customerId,
                  ParameterName = "@p0",
                  DbType = DbType.Int32
              }

          };

            HashSet<int> salesOrderIDs = new();
            HashSet<int> salesOrderDetailIDs = new();
            HashSet<int> customerAddressIds = new();

            using (NpgsqlConnection conn = new(connectionString))
            {


                conn.Open();
                using (NpgsqlTransaction tx = conn.BeginTransaction())
                {


                    // Haetaan SalesOrders
                    using (NpgsqlCommand cmd = makeSqlCommand(@"select  h.""ID"", d.""ID"", ca.""ID"" from public.""SalesOrderHeader"" h join public.""SalesOrderDetail"" d on h.""ID"" = d.""SalesOrderID""
                                                                join public.""CustomerAddress"" ca on h.""CustomerID""=ca.""CustomerID"" where ca.""CustomerID"" = @p0", parameters, conn, tx))
                    {
                        NpgsqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            _ = salesOrderIDs.Add(reader.GetInt32(0));
                            _ = salesOrderDetailIDs.Add(reader.GetInt32(1));
                            _ = customerAddressIds.Add(reader.GetInt32(2));
                        }
                        reader.Close();
                        cmd.Parameters.Clear();
                    }

                    List<NpgsqlParameter> paramList = new List<NpgsqlParameter>();
                    paramList.Add(
                         new NpgsqlParameter()
                         {
                             Value = customerId,
                             ParameterName = "@p0",
                             DbType = DbType.Int32
                         });
                    string statement = GetDeleteCustomerStatement(salesOrderIDs, salesOrderDetailIDs, customerAddressIds, paramList);

                    // Poistetaan Customer
                    using (NpgsqlCommand cmd = makeSqlCommand(statement, paramList.ToArray(), conn, tx))
                    {
                        _ = cmd.ExecuteNonQuery();
                    }
                    tx.Commit();
                }
            }
        }

        public void DeleteProductCategoryByID(int productCategoryID)
        {
            NpgsqlParameter[] parameters =
          {
              new NpgsqlParameter()
              {
                  Value = productCategoryID,
                  ParameterName = "@p0",
                  DbType = DbType.Int32
              }

          };
            List<int> productIDs = new();
            List<int> productModelIDs = new();
            List<int> productDescriptionIDs = new();
            string productModelIDsCommaSeparated = "";

            using (NpgsqlConnection conn = new(connectionString))
            {
                conn.Open();
                using (NpgsqlTransaction tx = conn.BeginTransaction())
                {

                    using (NpgsqlCommand cmd = makeSqlCommand(@"select ""ID"", ""ProductModelID"" from public.""Product"" where ""ProductCategoryID"" = @p0", parameters, conn, tx))
                    {
                        NpgsqlDataReader reader = cmd.ExecuteReader();

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
                        using (NpgsqlCommand cmd = makeSqlCommand(@"select ""ProductDescriptionID"" from public.""ProductModelProductDescription"" where ""ProductModelID"" in (" + productModelIDsCommaSeparated + ")", conn, tx))
                        {
                            NpgsqlDataReader reader = cmd.ExecuteReader();

                            while (reader.Read())
                            {
                                productDescriptionIDs.Add(reader.GetInt32(0));
                            }
                            reader.Close();
                        }
                    }

                    List<NpgsqlParameter> paramList = new List<NpgsqlParameter>();
                    paramList.Add(new NpgsqlParameter()
                    {
                        Value = productCategoryID,
                        ParameterName = "@p0",
                        DbType = DbType.Int32,
                    });

                    string deleteStatement = GetDeleteProductCategoryStatement(productIDs, productModelIDs, productDescriptionIDs, paramList);

                    // Tuotemallit
                    using (NpgsqlCommand cmd = makeSqlCommand(deleteStatement, paramList.ToArray(), conn, tx))
                    {
                        _ = cmd.ExecuteNonQuery();
                    }

                    tx.Commit();
                }
            }
        }

        public void DeleteSalesOrderDetailById(int salesOrderDetailID)
        {
            NpgsqlParameter[] parameters =
          {
              new NpgsqlParameter()
              {
                  Value = salesOrderDetailID,
                  ParameterName = "@p0",

                  DbType = DbType.Int32
              }

          };

            using (NpgsqlConnection conn = new(connectionString))
            {
                using (NpgsqlCommand cmd = makeSqlCommand(@"DELETE from public.""SalesOrderDetail"" where ""ID"" = @p0", parameters, conn))
                {
                    conn.Open();
                    _ = cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Product> GetProductsByCategoryId(int categoryId)
        {
            NpgsqlParameter[] parameters =
          {
              new NpgsqlParameter()
              {
                  Value = categoryId,
                  ParameterName = "@p0",

                  DbType = DbType.Int32
              }

          };
            using (NpgsqlConnection conn = new(connectionString))
            {
                conn.Open();
                List<Product> products = new();
                using (NpgsqlCommand cmd = makeSqlCommand(@"SELECT p.""ID"", p.""Color"", p.""DiscontinuedDate"", p.""ListPrice"", p.""ModifiedDate"", p.""Name"", p.""ProductCategoryID"", p.""ProductModelID"", p.""ProductNumber"", p.""SellEndDate"", 
                                                        p.""SellStartDate"", p.""Size"", p.""StandardCost"", p.""ThumbNailPhoto"", p.""ThumbnailPhotoFileName"", p.""Weight"", p.""rowguid""
                                                        FROM public.""Product"" AS p WHERE p.""ProductCategoryID""=@p0", parameters, conn))
                {


                    NpgsqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        products.Add(PostgreSQLDataHelper.FromDataReader(reader));
                    }

                }
                return products;
            }
        }

        public decimal GetTotalAmountByCustomerId(int customerId)
        {
            decimal ret = 0;
            NpgsqlParameter[] parameters =
          {
              new NpgsqlParameter()
              {
                  Value = customerId,
                  ParameterName = "@p0",

                  DbType = DbType.Int32
              }

          };
            using (NpgsqlConnection conn = new(connectionString))
            {
                conn.Open();
                using (NpgsqlCommand cmd = makeSqlCommand(@"select sum(d.""LineTotal"") from public.""SalesOrderDetail"" d
                                        inner join public.""SalesOrderHeader"" h on d.""SalesOrderID""=h.""ID""
                                        where ""CustomerID"" = @p0", parameters, conn))
                {


                    object? r = cmd.ExecuteScalar();
                    if (r != null && DBNull.Value != r)
                    {
                        ret = (decimal)r;
                    }
                    else
                    {
                        return 0;
                    }

                }

            }
            return ret;
        }

        public int InsertCustomer(Customer customer)
        {
            int customerID = 0;
            using (NpgsqlConnection conn = new(connectionString))
            {
                conn.Open();
                using (NpgsqlTransaction tx = conn.BeginTransaction())
                {


                    using (NpgsqlCommand cmd = makeSqlCommand(@"INSERT INTO  public.""Customer""
                   (""NameStyle""
                   ,""Title""
                   ,""FirstName""
                   ,""MiddleName""
                   ,""LastName""
                   ,""Suffix""
                   ,""CompanyName""
                   ,""SalesPerson""
                   ,""EmailAddress""
                   ,""Phone""
                   ,""PasswordHash""
                   ,""PasswordSalt""
                   ,""rowguid""
                   ,""ModifiedDate"")
                 VALUES
               (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13) RETURNING ""ID""", PostgreSQLDataHelper.GetNpgsqlParameters(customer), conn, tx))
                    {

                        customerID = (int)cmd.ExecuteScalar();


                    }

                    foreach (CustomerAddress a in customer.CustomerAddress)
                    {
                        int addressID = 0;
                        using (NpgsqlCommand cmd = makeSqlCommand(@"INSERT INTO  public.""Address""
                                                       (""AddressLine1""
                                                       ,""AddressLine2""
                                                       ,""City""
                                                       ,""StateProvince""
                                                       ,""CountryRegion""
                                                       ,""PostalCode""
                                                       ,""rowguid""
                                                       ,""ModifiedDate"")
                                                 VALUES
                                                       (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7) RETURNING ""ID""", PostgreSQLDataHelper.GetNpgsqlParameters(a.Address), conn, tx))
                        {
                            addressID = (int)cmd.ExecuteScalar();

                        }


                        using (NpgsqlCommand cmd = makeSqlCommand(@"INSERT INTO public.""CustomerAddress""
                                                           (""CustomerID""
                                                           ,""AddressID""
                                                           ,""AddressType""
                                                           ,""rowguid""
                                                           ,""ModifiedDate"") 
                                                          VALUES
                                                                (@p0, @p1, @p2, @p3, @p4)", PostgreSQLDataHelper.GetNpgsqlParameters(a, customerID, addressID), conn, tx))
                        {
                            _ = cmd.ExecuteNonQuery();
                        }
                    }
                    tx.Commit();
                }

            }
            return customerID;
        }

        public int InsertProduct(Product p)
        {

            using (NpgsqlConnection conn = new(connectionString))
            {
                conn.Open();
                int productID = 0;
                using (NpgsqlCommand cmd = makeSqlCommand(@"INSERT INTO public.""Product""
                                                       (""Name""
                                                       ,""ProductNumber""
                                                       ,""Color""
                                                       ,""StandardCost""
                                                       ,""ListPrice""
                                                       ,""Size""
                                                       ,""Weight""
                                                       ,""ProductCategoryID""
                                                       ,""ProductModelID""
                                                       ,""SellStartDate""
                                                       ,""SellEndDate""
                                                       ,""DiscontinuedDate""
                                                       ,""ThumbNailPhoto""
                                                       ,""ThumbnailPhotoFileName""
                                                       ,""rowguid""
                                                       ,""ModifiedDate"")
                                                 VALUES
                                                       (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15) RETURNING ""ID""", PostgreSQLDataHelper.GetNpgsqlParameters(p), conn))
                {
                    productID = (int)cmd.ExecuteScalar();

                }
                return productID;
            }
        }

        public int InsertSalesOrderHeader(SalesOrderHeader order)
        {
            int headerID = 0;
            using (NpgsqlConnection conn = new(connectionString))
            {
                conn.Open();
                using (NpgsqlTransaction tx = conn.BeginTransaction())
                {


                    using (NpgsqlCommand cmd = makeSqlCommand(@"INSERT INTO public.""SalesOrderHeader""
                                                       (""RevisionNumber""
                                                       ,""OrderDate""
                                                       ,""DueDate""
                                                       ,""ShipDate""
                                                       ,""Status""
                                                       ,""OnlineOrderFlag""
                                                       ,""SalesOrderNumber""
                                                       ,""PurchaseOrderNumber""
                                                       ,""AccountNumber""
                                                       ,""CustomerID""
                                                       ,""ShipToAddressID""
                                                       ,""BillToAddressID""
                                                       ,""ShipMethod""
                                                       ,""SubTotal""
                                                       ,""TaxAmt""
                                                       ,""Freight""
                                                       ,""TotalDue""
                                                       ,""rowguid""
                                                       ,""Comment""
                                                       ,""ModifiedDate""
                                                       ,""CreditCardApprovalCode"") 
                                                 VALUES
                                                       (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15, @p16, @p17, @p18, @p19, @p20) RETURNING ""ID""", PostgreSQLDataHelper.GetNpgsqlParameters(order), conn, tx))
                    {
                        headerID = (int)cmd.ExecuteScalar();

                    }
                    foreach (SalesOrderDetail d in order.SalesOrderDetail)
                    {

                        using (NpgsqlCommand cmd = makeSqlCommand(@"INSERT INTO public.""SalesOrderDetail""
                       (""OrderQty""
                       ,""ProductID""
                       ,""UnitPrice""
                       ,""UnitPriceDiscount""
                       ,""LineTotal""
                       ,""rowguid""
                       ,""ModifiedDate""
                       ,""SalesOrderID"")
                        VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7)", PostgreSQLDataHelper.GetNpgsqlParameters(d, headerID), conn, tx))
                        {
                            _ = cmd.ExecuteNonQuery();
                        }
                    }
                    tx.Commit();
                }

            }
            return headerID;
        }

        public void MultiplyPricesByCategoryId(int categoryId, decimal priceChange)
        {
            NpgsqlParameter[] parameters =
          {
              new NpgsqlParameter()
              {
                  Value = priceChange,
                  ParameterName = "@p0",

                  DbType = DbType.Decimal
              },
               new NpgsqlParameter()
              {
                  Value = categoryId,
                  ParameterName = "@p1",

                  DbType = DbType.Int32
              }

          };

            using (NpgsqlConnection conn = new(connectionString))
            {

                using (NpgsqlCommand cmd = makeSqlCommand(@"UPDATE public.""Product"" SET ""ListPrice""=""ListPrice""*@p0 where ""ProductCategoryID""=@p1", parameters, conn))
                {
                    conn.Open();
                    _ = cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Product> SearchProducts(string keyword)
        {

            NpgsqlParameter[] parameters =
          {
              new NpgsqlParameter()
              {
                  Value = "%" + keyword + "%",
                  ParameterName = "@p0",
                  DbType = DbType.String
              }
          };
            using (NpgsqlConnection conn = new(connectionString))
            {
                List<Product> products = new();
                using (NpgsqlCommand cmd = makeSqlCommand(@"SELECT p.""ID"", p.""Color"", p.""DiscontinuedDate"", p.""ListPrice"", p.""ModifiedDate"", p.""Name"", p.""ProductCategoryID"", p.""ProductModelID"", p.""ProductNumber"", p.""SellEndDate"", 
                                                        p.""SellStartDate"", p.""Size"", p.""StandardCost"", p.""ThumbNailPhoto"", p.""ThumbnailPhotoFileName"", p.""Weight"", p.""rowguid""
                                                        FROM public.""Product"" AS p WHERE  p.""Name"" like @p0", parameters, conn))
                {
                    conn.Open();
                    NpgsqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        products.Add(PostgreSQLDataHelper.FromDataReader(reader));
                    }

                }
                return products;
            }
        }

        public void UpdateLastName(int customerId, string newLastName)
        {
            NpgsqlParameter[] parameters =
          {
              new NpgsqlParameter()
              {
                  Value = newLastName,
                  ParameterName = "@p0",

                  DbType = DbType.String
              },
               new NpgsqlParameter()
              {
                  Value = customerId,
                  ParameterName = "@p1",

                  DbType = DbType.Int32
              }

          };

            using (NpgsqlConnection conn = new(connectionString))
            {

                using (NpgsqlCommand cmd = makeSqlCommand(@"UPDATE public.""Customer"" SET ""LastName""=@p0 where ""ID""=@p1", parameters, conn))
                {
                    conn.Open();
                    _ = cmd.ExecuteNonQuery();

                }

            }
        }

        public void UpdateProductCategoryByCategoryId(int categoryId, int newCategoryId)
        {
            NpgsqlParameter[] parameters =
          {
              new NpgsqlParameter()
              {
                  Value = newCategoryId,
                  ParameterName = "@p0",
                  DbType = DbType.Int32
              },
               new NpgsqlParameter()
              {
                  Value = categoryId,
                  ParameterName = "@p1",
                  DbType = DbType.Int32
              }

          };

            using (NpgsqlConnection conn = new(connectionString))
            {

                using (NpgsqlCommand cmd = makeSqlCommand(@"UPDATE public.""Product"" SET ""ProductCategoryID""=@p0 where ""ProductCategoryID""=@p1", parameters, conn))
                {
                    conn.Open();
                    _ = cmd.ExecuteNonQuery();

                }

            }
        }

        private NpgsqlCommand makeSqlCommand(string command, NpgsqlParameter[] parameters, NpgsqlConnection conn, NpgsqlTransaction tx)
        {
            NpgsqlCommand cmd = makeSqlCommand(command, parameters, conn);


            cmd.Transaction = tx;


            return cmd;
        }
        private NpgsqlCommand makeSqlCommand(string command, NpgsqlParameter[] parameters, NpgsqlConnection conn)
        {
            NpgsqlCommand cmd = new(command, conn);

            for (int i = 0; i < parameters.Count(); i++)
            {
                _ = cmd.Parameters.Add(parameters[i]);
            }

            return cmd;
        }

        private NpgsqlCommand makeSqlCommand(string command, NpgsqlConnection conn, NpgsqlTransaction tx = null)
        {
            return makeSqlCommand(command, new NpgsqlParameter[] { }, conn, tx);
        }

        private string GetDeleteProductCategoryStatement(List<int> productIDs, List<int> productModelIDs, List<int> productDescriptionIDs, List<NpgsqlParameter> paramList)
        {
            int countOfParams = paramList.Count;
            StringBuilder deleteStatement = new("");

            foreach (int productID in productIDs)
            {
                _ = deleteStatement.AppendFormat(@"DELETE from public.""SalesOrderDetail"" where ""ProductID"" = @p{0};", countOfParams);

                paramList.Add(new NpgsqlParameter()
                {
                    ParameterName = String.Format("@p{0}", countOfParams),
                    Value = productID,
                    DbType = DbType.Int32
                });

                countOfParams++;
            }

            foreach (int productModelID in productModelIDs)
            {
                _ = deleteStatement.AppendFormat(@"DELETE from public.""ProductModelProductDescription"" where ""ProductModelID"" = @p{0};", countOfParams);


                paramList.Add(new NpgsqlParameter()
                {
                    ParameterName = String.Format("@p{0}", countOfParams),
                    Value = productModelID,
                    DbType = DbType.Int32
                });

                countOfParams++;
            }

            foreach (int productDescriptionID in productDescriptionIDs)
            {
                _ = deleteStatement.AppendFormat(@"DELETE from public.""ProductDescription"" where ""ID"" =@p{0};", countOfParams);

                paramList.Add(new NpgsqlParameter()
                {
                    ParameterName = String.Format("@p{0}", countOfParams),
                    Value = productDescriptionID,
                    DbType = DbType.Int32
                });

                countOfParams++;
            }

            foreach (int productID in productIDs)
            {
                _ = deleteStatement.AppendFormat(@"DELETE from public.""Product"" where ""ID"" = @p{0};", countOfParams);

                paramList.Add(new NpgsqlParameter()
                {
                    ParameterName = String.Format("@p{0}", countOfParams),
                    Value = productID,
                    DbType = DbType.Int32
                });

                countOfParams++;

            }

            foreach (int productModelID in productModelIDs)
            {

                _ = deleteStatement.AppendFormat(@"DELETE from public.""ProductModel"" where ""ID"" = @p{0};", countOfParams);

                paramList.Add(new NpgsqlParameter()
                {
                    ParameterName = String.Format("@p{0}", countOfParams),
                    Value = productModelID,
                    DbType = DbType.Int32
                });

                countOfParams++;
            }

            _ = deleteStatement.Append(@"DELETE from public.""ProductCategory"" where ""ID"" = @p0;");

            return deleteStatement.ToString();
        }

        private string GetDeleteCustomerStatement(HashSet<int> salesOrderIDs, HashSet<int> salesOrderDetailIDs, HashSet<int> customerAddressIds, List<NpgsqlParameter> paramList)
        {
            int countOfParams = paramList.Count;

            StringBuilder s = new("");

            foreach (int salesOrderDetailID in salesOrderDetailIDs)
            {
                _ = s.AppendFormat(@"DELETE from public.""SalesOrderDetail"" where ""ID"" =@p{0};", countOfParams);

                paramList.Add(new NpgsqlParameter()
                {
                    ParameterName = String.Format("@p{0}", countOfParams),
                    Value = salesOrderDetailID,
                    DbType = DbType.Int32
                });

                countOfParams++;
            }

            foreach (int salesOrderID in salesOrderIDs)
            {
                _ = s.AppendFormat(@"DELETE from public.""SalesOrderHeader"" where ""ID"" ={0};", countOfParams);

                paramList.Add(new NpgsqlParameter()
                {
                    ParameterName = String.Format("@p{0}", countOfParams),
                    Value = salesOrderID,
                    DbType = DbType.Int32
                });

                countOfParams++;
            }

            foreach (int customerAddressId in customerAddressIds)
            {
                _ = s.AppendFormat(@"DELETE from public.""CustomerAddress"" where ""ID"" ={0};", countOfParams);

                paramList.Add(new NpgsqlParameter()
                {
                    ParameterName = String.Format("@p{0}", countOfParams),
                    Value = customerAddressId,
                    DbType = DbType.Int32
                });

                countOfParams++;
            }
            _ = s.Append(@"DELETE from public.""Customer"" where ""ID"" =@p0;");

            return s.ToString();
        }
    }
}
