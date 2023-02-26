using GraduApp.DataAccess.GraduModels;
using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using Npgsql;
using Oracle.EntityFrameworkCore.Storage.Internal;
using System.Data;
using System.Text;

namespace GraduApp.DataAccess.GraduDBOperations
{
    internal class MySQLAdoNetDBOperations : IGraduDBOperations
    {
        private readonly string connectionString;

        public MySQLAdoNetDBOperations(string _connectionString)
        {
            connectionString = _connectionString;
        }

        public int CountProductsByCategoryId(int categoryId)
        {

            MySqlParameter[] parameters =
          {
              new MySqlParameter()
              {
                  Value = categoryId,
                  ParameterName = "@p0",
                  
                  MySqlDbType = MySqlDbType.Int32,
              }

          };
            using (MySqlConnection conn = new(connectionString))
            {

                List<Product> products = new();
                using (MySqlCommand cmd = makeSqlCommand(@"SELECT COUNT(*) as c
                                                        FROM Product AS p WHERE p.ProductCategoryID=@p0", parameters, conn))
                {
                    conn.Open();
                    return (int)(long)cmd.ExecuteScalar();
                }
            }
        }

        public void DeleteCustomerDataByCustomerId(int customerId)
        {
            MySqlParameter[] parameters =
           {
              new MySqlParameter()
              {
                  Value = customerId,
                  ParameterName = "@p0",
                  
                  MySqlDbType = MySqlDbType.Int32,
              }

          };

            HashSet<int> salesOrderIDs = new();
            HashSet<int> salesOrderDetailIDs = new();
            HashSet<int> customerAddressIds = new();

            using (MySqlConnection conn = new(connectionString))
            {
                conn.Open();
                using (MySqlTransaction tx = conn.BeginTransaction())
                {
                    // Haetaan SalesOrders
                    using (MySqlCommand cmd = makeSqlCommand(@"select h.ID, d.ID, ca.ID from SalesOrderHeader h join SalesOrderDetail d on h.ID = d.SalesOrderID 
                                                    join CustomerAddress ca on h.CustomerID=ca.CustomerID where ca.CustomerID= @p0", parameters, conn, tx))
                    {
                        MySqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            _ = salesOrderIDs.Add(reader.GetInt32(0));
                            _ = salesOrderDetailIDs.Add(reader.GetInt32(1));
                            _ = customerAddressIds.Add(reader.GetInt32(2));
                        }
                        reader.Close();
                        cmd.Parameters.Clear();
                    }
                    List<MySqlParameter> paramList = new List<MySqlParameter>();
                    paramList.Add(
                    new MySqlParameter()
                    {
                        Value = customerId,
                        ParameterName = "@p0",
                        MySqlDbType = MySqlDbType.Int32,
                    });

                    string statement = GetDeleteCustomerStatement(salesOrderIDs, salesOrderDetailIDs, customerAddressIds, paramList);

                    // Poistetaan Customer
                    using (MySqlCommand cmd = makeSqlCommand(statement, paramList.ToArray(), conn, tx))
                    {
                        _ = cmd.ExecuteNonQuery();
                    }
                    tx.Commit();
                }
            }
        }

        public void DeleteProductCategoryByID(int productCategoryID)
        {
            MySqlParameter[] parameters =
           {
              new MySqlParameter()
              {
                  Value = productCategoryID,
                  ParameterName = "@p0",
                  
                  MySqlDbType = MySqlDbType.Int32,
              }

          };
            
            List<int> productIDs = new();
            List<int> productModelIDs = new();
            List<int> productDescriptionIDs = new();

            using (MySqlConnection conn = new(connectionString))
            {
              
                
                conn.Open();
                using (MySqlTransaction tx = conn.BeginTransaction())
                {
                    // Haetaan SalesOrders
                    using (MySqlCommand cmd = makeSqlCommand(@"select ID, ProductModelID from Product where ProductCategoryID = @p0", parameters, conn, tx))
                    {
                        MySqlDataReader reader = cmd.ExecuteReader();

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
                       string productModelIDsCommaSeparated = string.Join(",", productModelIDs.Select(n => n.ToString()).ToArray());
                        using (MySqlCommand cmd = makeSqlCommand(@"select ProductDescriptionID from ProductModelProductDescription where productModelID in (" + productModelIDsCommaSeparated + ")", conn, tx))
                        {
                            MySqlDataReader reader = cmd.ExecuteReader();

                            while (reader.Read())
                            {
                                productDescriptionIDs.Add(reader.GetInt32(0));
                            }
                            reader.Close();
                        }

                    }

                    List<MySqlParameter> paramList = new List<MySqlParameter>();
                    paramList.Add(
                    new MySqlParameter()
                    {
                        Value = productCategoryID,
                        ParameterName = "@p0",
                        MySqlDbType = MySqlDbType.Int32,
                    });
                    string deleteStatement = GetDeleteProductCategoryStatement(productIDs, productModelIDs, productDescriptionIDs, paramList);

                    // Tuotemallit
                    using (MySqlCommand cmd = makeSqlCommand(deleteStatement, paramList.ToArray(), conn, tx))
                    {
                        _ = cmd.ExecuteNonQuery();
                    }
                    tx.Commit();
                }
            }
        }

        public void DeleteSalesOrderDetailById(int salesOrderDetailID)
        {
            MySqlParameter[] parameters =
           {
              new MySqlParameter()
              {
                  Value = salesOrderDetailID,
                  ParameterName = "@p0",
                  MySqlDbType = MySqlDbType.Int32,
              }

          };

            using (MySqlConnection conn = new(connectionString))
            {
                using (MySqlCommand cmd = makeSqlCommand(@"DELETE FROM `SalesOrderDetail`  WHERE `ID` = @p0;", parameters, conn))
                {
                    conn.Open();
                    _ = cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Product> GetProductsByCategoryId(int categoryId)
        {
            MySqlParameter[] parameters =
           {
              new MySqlParameter()
              {
                  Value = categoryId,
                  ParameterName = "@p0",
                  
                  MySqlDbType = MySqlDbType.Int32,
              }

          };
            using (MySqlConnection conn = new(connectionString))
            {
                List<Product> products = new();
                using (MySqlCommand cmd = makeSqlCommand(@"SELECT p.ID, p.Color, p.DiscontinuedDate, p.ListPrice, p.ModifiedDate, p.Name, p.ProductCategoryID, p.ProductModelID, p.ProductNumber, p.SellEndDate, 
                                                        p.SellStartDate, p.Size, p.StandardCost, p.ThumbNailPhoto, p.ThumbnailPhotoFileName, p.Weight, p.rowguid
                                                        FROM Product AS p WHERE p.ProductCategoryID=@p0", parameters, conn))
                {

                    conn.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        products.Add(MySQLDataHelper.FromDataReader(reader));
                    }

                }
                return products;
            }
        }

        public decimal GetTotalAmountByCustomerId(int customerId)
        {
            decimal ret = 0;
            MySqlParameter[] parameters =
           {
              new MySqlParameter()
              {
                  Value = customerId,
                  ParameterName = "@p0",
                  MySqlDbType = MySqlDbType.Int32,
              }

          };
            using (MySqlConnection conn = new(connectionString))
            {

                List<Product> products = new();
                using (MySqlCommand cmd = makeSqlCommand(@"select COALESCE(sum(salesOrderDetail.LineTotal),0) from salesOrderDetail
                                                            inner join SalesOrderHeader  on salesOrderDetail.SalesOrderID=SalesOrderHeader.Id
                                                            where CustomerID = @p0", parameters, conn))
                {
                    conn.Open();

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
            using (MySqlConnection conn = new(connectionString))
            {
                conn.Open();
                using (MySqlTransaction tx = conn.BeginTransaction())
                {
                   
                    using (MySqlCommand cmd = makeSqlCommand(@"INSERT INTO Customer
                   (NameStyle
                   ,Title
                   ,FirstName
                   ,MiddleName
                   ,LastName
                   ,Suffix
                   ,CompanyName
                   ,SalesPerson
                   ,EmailAddress
                   ,Phone
                   ,PasswordHash
                   ,PasswordSalt
                   ,rowguid
                   ,ModifiedDate)
                 VALUES
               (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13)", MySQLDataHelper.GetMySqlParameters(customer), conn, tx))
                    {
                        _ = cmd.ExecuteNonQuery();
                        customerID = (int)cmd.LastInsertedId;

                    }

                    foreach (CustomerAddress a in customer.CustomerAddress)
                    {
                        int addressID = 0;


                        using (MySqlCommand cmd = makeSqlCommand(@"INSERT INTO Address
                                                       (AddressLine1
                                                       ,AddressLine2
                                                       ,City
                                                       ,StateProvince
                                                       ,CountryRegion
                                                       ,PostalCode
                                                       ,rowguid
                                                       ,ModifiedDate)
                                                 VALUES
                                                       (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7)", MySQLDataHelper.GetMySqlParameters(a.Address), conn, tx))
                        {
                            _ = cmd.ExecuteNonQuery();
                            addressID = (int)cmd.LastInsertedId;

                        }


                        using (MySqlCommand cmd = makeSqlCommand(@"INSERT INTO CustomerAddress
                                                           (CustomerID
                                                           ,AddressID
                                                           ,AddressType
                                                           ,rowguid
                                                           ,ModifiedDate) 
                                                          VALUES
                                                                (@p0, @p1, @p2, @p3, @p4)", MySQLDataHelper.GetMySqlParameters(a, customerID, addressID), conn, tx))
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
            using (MySqlConnection conn = new(connectionString))
            {

                
                using (MySqlCommand cmd = makeSqlCommand(@"INSERT INTO Product
                                                       (Name
                                                       ,ProductNumber
                                                       ,Color
                                                       ,StandardCost
                                                       ,ListPrice
                                                       ,Size
                                                       ,Weight
                                                       ,ProductCategoryID
                                                       ,ProductModelID
                                                       ,SellStartDate
                                                       ,SellEndDate
                                                       ,DiscontinuedDate
                                                       ,ThumbNailPhoto
                                                       ,ThumbnailPhotoFileName
                                                       ,rowguid
                                                       ,ModifiedDate)
                                                 VALUES
                                                       (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15)", MySQLDataHelper.GetMySqlParameters(p), conn))
                {
                    conn.Open();
                    _ = cmd.ExecuteNonQuery();
                    productID = (int)cmd.LastInsertedId;

                }
                return productID;
            }
        }

        public int InsertSalesOrderHeader(SalesOrderHeader order)
        {
            int headerID = 0;
            using (MySqlConnection conn = new(connectionString))
            {
                conn.Open();
                using (MySqlTransaction tx = conn.BeginTransaction())
                {

                    using (MySqlCommand cmd = makeSqlCommand(@"INSERT INTO SalesOrderHeader
                                                       (RevisionNumber
                                                       ,OrderDate
                                                       ,DueDate
                                                       ,ShipDate
                                                       ,Status
                                                       ,OnlineOrderFlag
                                                       ,SalesOrderNumber
                                                       ,PurchaseOrderNumber
                                                       ,AccountNumber
                                                       ,CustomerID
                                                       ,ShipToAddressID
                                                       ,BillToAddressID
                                                       ,ShipMethod
                                                       ,SubTotal
                                                       ,TaxAmt
                                                       ,Freight
                                                       ,TotalDue
                                                       ,rowguid
                                                       ,Comment
                                                       ,ModifiedDate
                                                       ,CreditCardApprovalCode)
                                                 VALUES
                                                       (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15, @p16, @p17, @p18, @p19, @p20)", MySQLDataHelper.GetMySqlParameters(order), conn, tx))
                    {
                        _ = cmd.ExecuteNonQuery();
                        headerID = (int)cmd.LastInsertedId;

                    }
                    foreach (SalesOrderDetail d in order.SalesOrderDetail)
                    {

                        using (MySqlCommand cmd = makeSqlCommand(@"INSERT INTO SalesOrderDetail
                       (OrderQty
                       ,ProductID
                       ,UnitPrice
                       ,UnitPriceDiscount
                       ,LineTotal
                       ,rowguid
                       ,ModifiedDate
                       ,SalesOrderID)
                        VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7)", MySQLDataHelper.GetMySqlParameters(d, headerID), conn, tx))
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
            MySqlParameter[] parameters =
           {
              new MySqlParameter()
              {
                  Value = priceChange,
                  ParameterName = "@p0",
                  
                  MySqlDbType = MySqlDbType.Decimal,
              },
               new MySqlParameter()
              {
                  Value = categoryId,
                  ParameterName = "@p1",
                  
                  MySqlDbType = MySqlDbType.Int32,
              }

          };

            using (MySqlConnection conn = new(connectionString))
            {

                using (MySqlCommand cmd = makeSqlCommand(@"UPDATE Product SET ListPrice=ListPrice*@p0 where ProductCategoryID=@p1", parameters, conn))
                {
                    conn.Open();
                    _ = cmd.ExecuteNonQuery();

                }

            }
        }

        public List<Product> SearchProducts(string keyword)
        {

            MySqlParameter[] parameters =
           {
              new MySqlParameter()
              {
                  Value = "%"+keyword+"%",
                  ParameterName = "@p0",
                  
                  MySqlDbType = MySqlDbType.Text,
              },

          };
            using (MySqlConnection conn = new(connectionString))
            {
                List<Product> products = new();
                using (MySqlCommand cmd = makeSqlCommand(@"SELECT p.ID, p.Color, p.DiscontinuedDate, p.ListPrice, p.ModifiedDate, p.Name, p.ProductCategoryID, p.ProductModelID, p.ProductNumber, p.SellEndDate, 
                                                        p.SellStartDate, p.Size, p.StandardCost, p.ThumbNailPhoto, p.ThumbnailPhotoFileName, p.Weight, p.rowguid
                                                        FROM Product AS p WHERE  p.Name like @p0", parameters, conn))
                {
                    conn.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        products.Add(MySQLDataHelper.FromDataReader(reader));
                    }

                }
                return products;
            }
        }

        public void UpdateLastName(int customerId, string newLastName)
        {
            MySqlParameter[] parameters =
           {
              new MySqlParameter()
              {
                  Value = newLastName,
                  ParameterName = "@p0",
                  
                  MySqlDbType = MySqlDbType.Text,
              },
               new MySqlParameter()
              {
                  Value = customerId,
                  ParameterName = "@p1",
                  
                  MySqlDbType = MySqlDbType.Int32,
              }

          };

            using (MySqlConnection conn = new(connectionString))
            {

                using (MySqlCommand cmd = makeSqlCommand(@"UPDATE Customer SET LastName=@p0 where id=@p1", parameters, conn))
                {
                    conn.Open();
                    _ = cmd.ExecuteNonQuery();

                }

            }
        }

        public void UpdateProductCategoryByCategoryId(int categoryId, int newCategoryId)
        {
            MySqlParameter[] parameters =
           {
              new MySqlParameter()
              {
                  Value = newCategoryId,
                  ParameterName = "@p0",
                  
                  MySqlDbType = MySqlDbType.Int32,
              },
               new MySqlParameter()
              {
                  Value = categoryId,
                  ParameterName = "@p1",
                  
                  MySqlDbType = MySqlDbType.Int32,
              }

          };

            using (MySqlConnection conn = new(connectionString))
            {

                using (MySqlCommand cmd = makeSqlCommand(@"UPDATE Product SET ProductCategoryID=@p0 where ProductCategoryID=@p1", parameters, conn))
                {
                    conn.Open();
                    _ = cmd.ExecuteNonQuery();

                }

            }
        }

        private MySqlCommand makeSqlCommand(string command, MySqlParameter[] parameters, MySqlConnection conn, MySqlTransaction tx)
        {
            MySqlCommand cmd = makeSqlCommand(command, parameters, conn);


            cmd.Transaction = tx;


            return cmd;
        }
        private MySqlCommand makeSqlCommand(string command, MySqlParameter[] parameters, MySqlConnection conn)
        {
            MySqlCommand cmd = new(command, conn);

            for (int i = 0; i < parameters.Count(); i++)
            {
                _ = cmd.Parameters.Add(parameters[i]);
            }

            return cmd;
        }

        private MySqlCommand makeSqlCommand(string command, MySqlConnection conn, MySqlTransaction tx = null)
        {
            return makeSqlCommand(command, new MySqlParameter[] { }, conn, tx);
        }

        private string GetDeleteProductCategoryStatement(List<int> productIDs, List<int> productModelIDs, List<int> productDescriptionIDs, List<MySqlParameter> paramList)
        {
            int countOfParams = paramList.Count;
            StringBuilder deleteStatement = new("");

            foreach (int productID in productIDs)
            {
                _ = deleteStatement.AppendFormat(@"DELETE from SalesOrderDetail where ProductID = @p{0};", countOfParams);

                paramList.Add(new MySqlParameter()
                {
                    ParameterName = String.Format("@p{0}", countOfParams),
                    Value = productID,
                    DbType = DbType.Int32
                });

                countOfParams++;
            }

            foreach (int productModelID in productModelIDs)
            {
                _ = deleteStatement.AppendFormat(@"DELETE from ProductModelProductDescription where ProductModelID = @p{0};", countOfParams);


                paramList.Add(new MySqlParameter()
                {
                    ParameterName = String.Format("@p{0}", countOfParams),
                    Value = productModelID,
                    DbType = DbType.Int32
                });

                countOfParams++;
            }

            foreach (int productDescriptionID in productDescriptionIDs)
            {
                _ = deleteStatement.AppendFormat(@"DELETE from ProductDescription where ID =@p{0};", countOfParams);

                paramList.Add(new MySqlParameter()
                {
                    ParameterName = String.Format("@p{0}", countOfParams),
                    Value = productDescriptionID,
                    DbType = DbType.Int32
                });

                countOfParams++;
            }

            foreach (int productID in productIDs)
            {
                _ = deleteStatement.AppendFormat(@"DELETE from Product where ID = @p{0};", countOfParams);

                paramList.Add(new MySqlParameter()
                {
                    ParameterName = String.Format("@p{0}", countOfParams),
                    Value = productID,
                    DbType = DbType.Int32
                });

                countOfParams++;

            }

            foreach (int productModelID in productModelIDs)
            {

                _ = deleteStatement.AppendFormat(@"DELETE from ProductModel where ID = @p{0};", countOfParams);

                paramList.Add(new MySqlParameter()
                {
                    ParameterName = String.Format("@p{0}", countOfParams),
                    Value = productModelID,
                    DbType = DbType.Int32
                });

                countOfParams++;
            }

            _ = deleteStatement.Append(@"DELETE from ProductCategory where ID = @p0;");

            return deleteStatement.ToString();
        }

        private string GetDeleteCustomerStatement(HashSet<int> salesOrderIDs, HashSet<int> salesOrderDetailIDs, HashSet<int> customerAddressIds, List<MySqlParameter> paramList)
        {
            int countOfParams = paramList.Count;

            StringBuilder s = new("");

            foreach (int salesOrderDetailID in salesOrderDetailIDs)
            {
                _ = s.AppendFormat(@"DELETE from SalesOrderDetail where ID =@p{0};", countOfParams);

                paramList.Add(new MySqlParameter()
                {
                    ParameterName = String.Format("@p{0}", countOfParams),
                    Value = salesOrderDetailID,
                    DbType = DbType.Int32
                });

                countOfParams++;
            }

            foreach (int salesOrderID in salesOrderIDs)
            {
                _ = s.AppendFormat(@"DELETE from SalesOrderHeader where ID ={0};", countOfParams);

                paramList.Add(new MySqlParameter()
                {
                    ParameterName = String.Format("@p{0}", countOfParams),
                    Value = salesOrderID,
                    DbType = DbType.Int32
                });

                countOfParams++;
            }

            foreach (int customerAddressId in customerAddressIds)
            {
                _ = s.AppendFormat(@"DELETE from CustomerAddress where ID ={0};", countOfParams);

                paramList.Add(new MySqlParameter()
                {
                    ParameterName = String.Format("@p{0}", countOfParams),
                    Value = customerAddressId,
                    DbType = DbType.Int32
                });

                countOfParams++;
            }
            _ = s.Append(@"DELETE from Customer where ID =@p0;");

            return s.ToString();
        }
    }
}
