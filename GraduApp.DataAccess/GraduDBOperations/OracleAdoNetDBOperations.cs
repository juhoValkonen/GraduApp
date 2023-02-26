using GraduApp.DataAccess.GraduModels;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Text;

namespace GraduApp.DataAccess.GraduDBOperations
{
    internal class OracleAdoNetDBOperations : IGraduDBOperations
    {
        private readonly string connectionString;

        public OracleAdoNetDBOperations(string _connectionString)
        {
            connectionString = _connectionString;
        }
        public int CountProductsByCategoryId(int categoryId)
        {
            OracleParameter[] parameters =
          {
              new OracleParameter()
              {
                  Value = categoryId,
                  ParameterName = "p0",
                  OracleDbType = OracleDbType.Int32

              }
          };

            using (OracleConnection conn = new(connectionString))
            {

                List<Product> products = new();
                using (OracleCommand cmd = makeSqlCommand(@"SELECT COUNT(ID) as c
                                                        FROM ""Product"" WHERE ""ProductCategoryID""=:p0", parameters, conn))
                {
                    conn.Open();
                    return (int)(decimal)cmd.ExecuteScalar();
                }

            }
        }

        public void DeleteCustomerDataByCustomerId(int customerId)
        {
            OracleParameter[] parameters =
          {
              new OracleParameter()
              {
                  Value = customerId,
                  ParameterName = "p0",

                  OracleDbType = OracleDbType.Int32

              }
          };

            HashSet<int> salesOrderIDs = new();
            HashSet<int> salesOrderDetailIDs = new();
            HashSet<int> customerAddressIds = new();

            using (OracleConnection conn = new(connectionString))
            {


                conn.Open();
                using (OracleTransaction tx = conn.BeginTransaction())
                {

                    // Haetaan SalesOrders
                    using (OracleCommand cmd = makeSqlCommand(@"select h.""ID"", d.""ID"", ca.""ID""  from ""SalesOrderHeader"" h join ""SalesOrderDetail"" d on h.""ID"" = d.""SalesOrderID""
                                                                  join ""CustomerAddress"" ca on h.""CustomerID""=ca.""CustomerID"" where ca.""CustomerID"" = :p0", parameters, conn, tx))
                    {
                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            _ = salesOrderIDs.Add(reader.GetInt32(0));
                            _ = salesOrderDetailIDs.Add(reader.GetInt32(1));
                            _ = customerAddressIds.Add(reader.GetInt32(2));
                        }
                        reader.Close();
                        cmd.Parameters.Clear();
                    }
                    List<OracleParameter> paramList = new();


                    string statement = GetDeleteCustomerStatement(salesOrderIDs, salesOrderDetailIDs, customerAddressIds, paramList, customerId);

                    using (OracleCommand cmd = makeSqlCommand(statement, paramList.ToArray(), conn, tx))
                    {
                        _ = cmd.ExecuteNonQuery();

                    }
                    tx.Commit();
                }

            }

        }

        public void DeleteProductCategoryByID(int productCategoryID)
        {
            OracleParameter[] parameters =
          {
              new OracleParameter()
              {
                  Value = productCategoryID,
                  ParameterName = "p0",

                  OracleDbType = OracleDbType.Int32

              }
          };

            List<int> productIDs = new();
            List<int> productModelIDs = new();
            List<int> productDescriptionIDs = new();
            string productModelIDsCommaSeparated = "";

            using (OracleConnection conn = new(connectionString))
            {

                conn.Open();
                using (OracleTransaction tx = conn.BeginTransaction())
                {

                    // Haetaan SalesOrders
                    using (OracleCommand cmd = makeSqlCommand(@"select ""ID"", ""ProductModelID"" from ""Product"" where ""ProductCategoryID"" = :p0", parameters, conn, tx))
                    {
                        OracleDataReader reader = cmd.ExecuteReader();

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
                        using (OracleCommand cmd = makeSqlCommand(@"select ""ProductDescriptionID"" from ""ProductModelProductDescription"" where ""ProductModelID"" in (" + productModelIDsCommaSeparated + ")", conn, tx))
                        {
                            OracleDataReader reader = cmd.ExecuteReader();

                            while (reader.Read())
                            {
                                productDescriptionIDs.Add(reader.GetInt32(0));
                            }
                            reader.Close();
                        }
                    }


                    List<OracleParameter> paramList = new();

                    string deleteStatement = GetDeleteProductCategoryStatement(productIDs, productModelIDs, productDescriptionIDs, paramList, productCategoryID);

                    using (OracleCommand cmd = makeSqlCommand(deleteStatement, paramList.ToArray(), conn, tx))
                    {
                        _ = cmd.ExecuteNonQuery();
                    }
                    tx.Commit();
                }
            }
        }


        public void DeleteSalesOrderDetailById(int salesOrderDetailID)
        {
            OracleParameter[] parameters =
          {
              new OracleParameter()
              {
                  Value = salesOrderDetailID,
                  ParameterName = "p0",
                  OracleDbType = OracleDbType.Int32

              }
          };

            using (OracleConnection conn = new(connectionString))
            {
                conn.Open();
                using (OracleCommand cmd = makeSqlCommand(@"DELETE from ""SalesOrderDetail"" where ""ID"" = :p0", parameters, conn))
                {
                    _ = cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Product> GetProductsByCategoryId(int categoryId)
        {
            OracleParameter[] parameters =
          {
              new OracleParameter()
              {
                  Value = categoryId,
                  ParameterName = "p0",
                  OracleDbType = OracleDbType.Int32

              }
          };
            using (OracleConnection conn = new(connectionString))
            {
                conn.Open();
                List<Product> products = new();
                using (OracleCommand cmd = makeSqlCommand(@"SELECT p.""ID"", p.""Color"", p.""DiscontinuedDate"", p.""ListPrice"", p.""ModifiedDate"", p.""Name"", p.""ProductCategoryID"", p.""ProductModelID"", p.""ProductNumber"", p.""SellEndDate"", 
                                                        p.""SellStartDate"", p.""Size"", p.""StandardCost"", p.""ThumbNailPhoto"", p.""ThumbnailPhotoFileName"", p.""Weight"", p.""rowguid""
                                                        FROM ""Product"" p WHERE ""ProductCategoryID""=:p0", parameters, conn))
                {

                    OracleDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        products.Add(OracleDataHelper.FromDataReader(reader));
                    }
                    reader.Close();
                }
                return products;
            }
        }

        public decimal GetTotalAmountByCustomerId(int customerId)
        {
            decimal ret = 0;
            OracleParameter[] parameters =
          {
              new OracleParameter()
              {
                  Value = customerId,
                  ParameterName = "p0",
                  OracleDbType = OracleDbType.Int32

              }
          };
            using (OracleConnection conn = new(connectionString))
            {
                conn.Open();
                List<Product> products = new();
                using (OracleCommand cmd = makeSqlCommand(@"select coalesce(sum(d.""LineTotal""),0) from ""SalesOrderDetail"" d
                                        inner join ""SalesOrderHeader"" h on d.""SalesOrderID""=h.Id
                                        where ""CustomerID"" = :p0", parameters, conn))
                {


                    object r = cmd.ExecuteScalar();
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
            using (OracleConnection conn = new(connectionString))
            {
                conn.Open();
                using (OracleTransaction tx = conn.BeginTransaction())
                {


                    using (OracleCommand cmd = makeSqlCommand(@"INSERT INTO ""Customer""
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
               (:p0, :p1, :p2, :p3, :p4, :p5, :p6, :p7, :p8, :p9, :p10, :p11, :p12, :p13) RETURNING ID INTO :my_id_param", OracleDataHelper.GetOracleParameters(customer), conn, tx))
                    {
                        OracleParameter outputParameter = new()
                        {
                            ParameterName = "my_id_param",
                            DbType = DbType.Int32,
                            Direction = ParameterDirection.Output
                        };
                        _ = cmd.Parameters.Add(outputParameter);
                        _ = cmd.ExecuteNonQuery();
                        customerID = (int)outputParameter.Value;

                    }

                    foreach (CustomerAddress a in customer.CustomerAddress)
                    {
                        int addressID = 0;


                        using (OracleCommand cmd = makeSqlCommand(@"INSERT INTO ""Address""
                                                       (""AddressLine1""
                                                       ,""AddressLine2""
                                                       ,""City""
                                                       ,""StateProvince""
                                                       ,""CountryRegion""
                                                       ,""PostalCode""
                                                       ,""rowguid""
                                                       ,""ModifiedDate"")
                                                 VALUES
                                                       (:p0, :p1, :p2, :p3, :p4, :p5, :p6, :p7) RETURNING ID INTO :my_id_param", OracleDataHelper.GetOracleParameters(a.Address), conn, tx))
                        {
                            OracleParameter outputParameter = new()
                            {
                                ParameterName = "my_id_param",
                                DbType = DbType.Int32,
                                Direction = ParameterDirection.Output
                            };
                            _ = cmd.Parameters.Add(outputParameter);
                            _ = cmd.ExecuteNonQuery();
                            addressID = (int)outputParameter.Value;

                        }

                        using (OracleCommand cmd = makeSqlCommand(@"INSERT INTO ""CustomerAddress""
                                                           (""CustomerID""
                                                           ,""AddressID""
                                                           ,""AddressType""
                                                           ,""rowguid""
                                                           ,""ModifiedDate"") 
                                                          VALUES
                                                                (:p0, :p1, :p2, :p3, :p4)", OracleDataHelper.GetOracleParameters(a, customerID, addressID), conn, tx))
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

            using (OracleConnection conn = new(connectionString))
            {
                conn.Open();
                int productID = 0;
                using (OracleCommand cmd = makeSqlCommand(@"INSERT INTO ""Product""
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
                                                       (:p0, :p1, :p2, :p3, :p4, :p5, :p6, :p7, :p8, :p9, :p10, :p11, :p12, :p13, :p14, :p15) RETURNING ID INTO :my_id_param", OracleDataHelper.GetOracleParameters(p), conn))
                {
                    OracleParameter outputParameter = new()
                    {
                        ParameterName = "my_id_param",
                        DbType = DbType.Int32,
                        Direction = ParameterDirection.Output
                    };
                    _ = cmd.Parameters.Add(outputParameter);
                    _ = cmd.ExecuteNonQuery();
                    productID = (int)outputParameter.Value;

                }
                return productID;
            }
        }

        public int InsertSalesOrderHeader(SalesOrderHeader order)
        {
            int headerID = 0;
            using (OracleConnection conn = new(connectionString))
            {
                conn.Open();
                using (OracleTransaction tx = conn.BeginTransaction())
                {


                    using (OracleCommand cmd = makeSqlCommand(@"INSERT INTO ""SalesOrderHeader""
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
                                                       (:p0, :p1, :p2, :p3, :p4, :p5, :p6, :p7, :p8, :p9, :p10, :p11, :p12, :p13, :p14, :p15, :p16, :p17, :p18, :p19, :p20) RETURNING ID INTO :my_id_param", OracleDataHelper.GetOracleParameters(order), conn, tx))
                    {
                        OracleParameter outputParameter = new()
                        {
                            ParameterName = "my_id_param",
                            DbType = DbType.Int32,
                            Direction = ParameterDirection.Output
                        };
                        _ = cmd.Parameters.Add(outputParameter);
                        _ = cmd.ExecuteNonQuery();

                        headerID = (int)outputParameter.Value;

                    }
                    foreach (SalesOrderDetail d in order.SalesOrderDetail)
                    {

                        using (OracleCommand cmd = makeSqlCommand(@"INSERT INTO ""SalesOrderDetail""
                       (""OrderQty""
                       ,""ProductID""
                       ,""UnitPrice""
                       ,""UnitPriceDiscount""
                       ,""LineTotal""
                       ,""rowguid""
                       ,""ModifiedDate""
                       ,""SalesOrderID"")
                        VALUES (:p0, :p1, :p2, :p3, :p4, :p5, :p6, :p7)", OracleDataHelper.GetOracleParameters(d, headerID), conn, tx))
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
            OracleParameter[] parameters =
          {
              new OracleParameter()
              {
                  Value = priceChange,
                  ParameterName = "p0",

                  OracleDbType = OracleDbType.Decimal

              },
               new OracleParameter()
              {
                  Value = categoryId,
                  ParameterName = "p1",

                  OracleDbType = OracleDbType.Int32

              }
          };

            using (OracleConnection conn = new(connectionString))
            {

                using (OracleCommand cmd = makeSqlCommand(@"UPDATE ""Product"" SET ""ListPrice""=""ListPrice""*:p0 where ""ProductCategoryID""=:p1", parameters, conn))
                {
                    conn.Open();
                    _ = cmd.ExecuteNonQuery();

                }

            }
        }

        public List<Product> SearchProducts(string keyword)
        {
            OracleParameter[] parameters =
          {
              new OracleParameter()
              {
                  Value =  keyword,
                  ParameterName = "p0",

                  OracleDbType = OracleDbType.Varchar2

              }
          };
            using (OracleConnection conn = new(connectionString))
            {

                List<Product> products = new();
                using (OracleCommand cmd = makeSqlCommand(@"SELECT ""p"".""ID"", ""p"".""Color"", ""p"".""DiscontinuedDate"", ""p"".""ListPrice"", ""p"".""ModifiedDate"", ""p"".""Name"", ""p"".""ProductCategoryID"", ""p"".""ProductModelID"", ""p"".""ProductNumber"", ""p"".""SellEndDate"", ""p"".""SellStartDate"", ""p"".""Size"", ""p"".""StandardCost"", ""p"".""ThumbNailPhoto"", ""p"".""ThumbnailPhotoFileName"", ""p"".""Weight"", ""p"".""rowguid""
      FROM ""Product"" ""p""
      WHERE ((:p0 IS NULL ) OR (INSTR(""p"".""Name"", :p0) > 0))", parameters, conn))
                {
                    conn.Open();
                    OracleDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        products.Add(OracleDataHelper.FromDataReader(reader));
                    }

                }
                return products;
            }
        }

        public void UpdateLastName(int customerId, string newLastName)
        {
            OracleParameter[] parameters =
          {
              new OracleParameter()
              {
                  Value = newLastName,
                  ParameterName = "p0",

                  OracleDbType = OracleDbType.Varchar2

              },
               new OracleParameter()
              {
                  Value = customerId,
                  ParameterName = "p1",

                  OracleDbType = OracleDbType.Int32

              }
          };

            using (OracleConnection conn = new(connectionString))
            {
                using (OracleCommand cmd = makeSqlCommand(@"UPDATE ""Customer"" SET ""LastName""=:p0 where ""ID""=:p1", parameters, conn))
                {
                    conn.Open();
                    _ = cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateProductCategoryByCategoryId(int categoryId, int newCategoryId)
        {
            OracleParameter[] parameters =
          {
              new OracleParameter()
              {
                  Value = newCategoryId,
                  ParameterName = "p0",

                  OracleDbType = OracleDbType.Int32

              },
               new OracleParameter()
              {
                  Value = categoryId,
                  ParameterName = "p1",

                  OracleDbType = OracleDbType.Int32

              }
          };

            using (OracleConnection conn = new(connectionString))
            {

                using (OracleCommand cmd = makeSqlCommand(@"UPDATE ""Product"" SET ""ProductCategoryID""=:p0 where ""ProductCategoryID""=:p1", parameters, conn))
                {
                    conn.Open();
                    _ = cmd.ExecuteNonQuery();

                }

            }
        }
        private OracleCommand makeSqlCommand(string command, OracleParameter[] parameters, OracleConnection conn, OracleTransaction tx)
        {
            OracleCommand cmd = makeSqlCommand(command, parameters, conn);
            cmd.Transaction = tx;
            return cmd;
        }

        private OracleCommand makeSqlCommand(string command, OracleParameter[] parameters, OracleConnection conn)
        {
            OracleCommand cmd = new(command, conn);

            for (int i = 0; i < parameters.Count(); i++)
            {
                _ = cmd.Parameters.Add(parameters[i]);
            }

            return cmd;
        }

        private OracleCommand makeSqlCommand(string command, OracleConnection conn, OracleTransaction tx = null)
        {
            return makeSqlCommand(command, new OracleParameter[] { }, conn, tx);
        }

        private string GetDeleteProductCategoryStatement(List<int> productIDs, List<int> productModelIDs, List<int> productDescriptionIDs, List<OracleParameter> paramList, int productCategoryId)
        {
            int countOfParams = paramList.Count;
            StringBuilder deleteStatement = new("begin");

            foreach (int productID in productIDs)
            {
                _ = deleteStatement.AppendFormat(Environment.NewLine + @"DELETE from ""SalesOrderDetail"" where ""ProductID"" = :p{0};", countOfParams);

                paramList.Add(new OracleParameter()
                {
                    ParameterName = string.Format("p{0}", countOfParams),
                    Value = productID,
                    OracleDbType = OracleDbType.Int32
                });

                countOfParams++;
            }

            foreach (int productModelID in productModelIDs)
            {
                _ = deleteStatement.AppendFormat(Environment.NewLine + @"DELETE from ""ProductModelProductDescription"" where ""ProductModelID"" = :p{0};", countOfParams);


                paramList.Add(new OracleParameter()
                {
                    ParameterName = string.Format("p{0}", countOfParams),
                    Value = productModelID,
                    OracleDbType = OracleDbType.Int32
                });

                countOfParams++;
            }

            foreach (int productDescriptionID in productDescriptionIDs)
            {
                _ = deleteStatement.AppendFormat(Environment.NewLine + @"DELETE from ""ProductDescription"" where ""ID"" =:p{0};", countOfParams);

                paramList.Add(new OracleParameter()
                {
                    ParameterName = string.Format("p{0}", countOfParams),
                    Value = productDescriptionID,
                    OracleDbType = OracleDbType.Int32
                });

                countOfParams++;
            }


            foreach (int productID in productIDs)
            {
                _ = deleteStatement.AppendFormat(Environment.NewLine + @"DELETE from ""Product"" where ""ID"" = :p{0};", countOfParams);

                paramList.Add(new OracleParameter()
                {
                    ParameterName = string.Format("p{0}", countOfParams),
                    Value = productID,
                    OracleDbType = OracleDbType.Int32
                });

                countOfParams++;

            }

            foreach (int productModelID in productModelIDs)
            {

                _ = deleteStatement.AppendFormat(Environment.NewLine + @"DELETE from ""ProductModel"" where ""ID"" = :p{0};", countOfParams);

                paramList.Add(new OracleParameter()
                {
                    ParameterName = string.Format("p{0}", countOfParams),
                    Value = productModelID,
                    OracleDbType = OracleDbType.Int32
                });

                countOfParams++;
            }

           

            

           

           

            _ = deleteStatement.AppendFormat(Environment.NewLine + @"DELETE from ""ProductCategory"" where ""ID"" = :p{0};", countOfParams);
            _ = deleteStatement.Append(Environment.NewLine + @"end;");

            paramList.Add(new OracleParameter()
            {
                Value = productCategoryId,
                ParameterName = string.Format("p{0}", countOfParams),
                DbType = DbType.Int32,
            });
            return deleteStatement.ToString();
        }

        private string GetDeleteCustomerStatement(HashSet<int> salesOrderIDs, HashSet<int> salesOrderDetailIDs, HashSet<int> customerAddressIds, List<OracleParameter> paramList, int customerId)
        {
            int countOfParams = paramList.Count;

            StringBuilder s = new("begin");

            foreach (int salesOrderDetailID in salesOrderDetailIDs)
            {
                _ = s.AppendFormat(Environment.NewLine + @"DELETE from ""SalesOrderDetail"" where ""ID"" =:p{0};", countOfParams);

                paramList.Add(new OracleParameter()
                {
                    ParameterName = string.Format("p{0}", countOfParams),
                    Value = salesOrderDetailID,
                    OracleDbType = OracleDbType.Int32
                });

                countOfParams++;
            }

            foreach (int salesOrderID in salesOrderIDs)
            {
                _ = s.AppendFormat(Environment.NewLine + @"DELETE from ""SalesOrderHeader"" where ""ID"" =:p{0};", countOfParams);

                paramList.Add(new OracleParameter()
                {
                    ParameterName = string.Format("p{0}", countOfParams),
                    Value = salesOrderID,
                    OracleDbType = OracleDbType.Int32
                });

                countOfParams++;
            }

            foreach (int customerAddressId in customerAddressIds)
            {
                _ = s.AppendFormat(Environment.NewLine + @"DELETE from ""CustomerAddress"" where ""ID"" =:p{0};", countOfParams);

                paramList.Add(new OracleParameter()
                {
                    ParameterName = string.Format("p{0}", countOfParams),
                    Value = customerAddressId,
                    OracleDbType = OracleDbType.Int32
                });

                countOfParams++;
            }
            _ = s.AppendFormat(Environment.NewLine + @"DELETE from ""Customer"" where ""ID"" =:p{0};", countOfParams);
            _ = s.Append(Environment.NewLine + @"end;");

            paramList.Add(new OracleParameter()
            {
                Value = customerId,
                ParameterName = string.Format("p{0}", countOfParams),
                OracleDbType = OracleDbType.Int32

            });

            return s.ToString();
        }

    }
}
