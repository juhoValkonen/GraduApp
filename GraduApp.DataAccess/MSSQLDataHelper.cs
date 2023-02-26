using GraduApp.DataAccess.GraduModels;
using Microsoft.Data.SqlClient;

using MySql.Data.MySqlClient;
using Npgsql;
using Oracle.ManagedDataAccess.Client;

using System.Data;


namespace GraduApp.DataAccess
{
    public static class MSSQLDataHelper
    {
        public static Product FromDataReader(SqlDataReader reader)
        {
            return new Product()
            {
                ID = reader.GetInt32(0),
                Color = !reader.IsDBNull(1) ? reader.GetString(1) : null,
                DiscontinuedDate = !reader.IsDBNull(2) ? reader.GetDateTime(2) : null,
                ListPrice = reader.GetDecimal(3),
                ModifiedDate = reader.GetDateTime(4),
                Name = reader.GetString(5),
                ProductCategoryID = !reader.IsDBNull(6) ? reader.GetInt32(6) : null,
                ProductModelID = !reader.IsDBNull(7) ? reader.GetInt32(7) : null,
                ProductNumber = reader.GetString(8),
                SellEndDate = !reader.IsDBNull(9) ? reader.GetDateTime(9) : null,
                SellStartDate = reader.GetDateTime(10),
                Size = !reader.IsDBNull(11) ? reader.GetString(11) : null,
                StandardCost = reader.GetDecimal(12),
                ThumbNailPhoto = !reader.IsDBNull(13) ? (byte[])reader.GetValue(13) : null,
                ThumbnailPhotoFileName = !reader.IsDBNull(14) ? reader.GetString(14) : null,
                Weight = !reader.IsDBNull(15) ? reader.GetDecimal(15) : null,
                rowguid = reader.GetString(16),
            };
        }

        public static SqlParameter[] GetSqlParameters(Customer customer)
        {

            SqlParameter[] parameters =
                {
              new SqlParameter()
              {
                  Value = customer.NameStyle,
                  ParameterName = "@p0",
                  SqlDbType = SqlDbType.Bit
              },
               new SqlParameter()
              {
                  Value = customer.Title,
                  ParameterName = "@p1",
                  SqlDbType = SqlDbType.NVarChar
              },
               new SqlParameter()
              {
                  Value = customer.FirstName,
                  ParameterName = "@p2",
                  SqlDbType = SqlDbType.NVarChar
              },
               new SqlParameter()
              {
                  Value = customer.MiddleName,
                  ParameterName = "@p3",
                  SqlDbType = SqlDbType.NVarChar
              },
               new SqlParameter()
              {
                  Value = customer.LastName,
                  ParameterName = "@p4",
                  SqlDbType = SqlDbType.NVarChar
              },
               new SqlParameter()
              {
                  Value = customer.Suffix,
                  ParameterName = "@p5",
                  SqlDbType = SqlDbType.NVarChar
              },
               new SqlParameter()
              {
                  Value = customer.CompanyName,
                  ParameterName = "@p6",
                  SqlDbType = SqlDbType.NVarChar
              },
               new SqlParameter()
              {
                  Value = customer.SalesPerson,
                  ParameterName = "@p7",
                  SqlDbType = SqlDbType.NVarChar
              },
               new SqlParameter()
              {
                  Value = customer.EmailAddress,
                  ParameterName = "@p8",
                  SqlDbType = SqlDbType.NVarChar
              },
               new SqlParameter()
              {
                  Value = customer.Phone,
                  ParameterName = "@p9",
                  SqlDbType = SqlDbType.NVarChar
              },
               new SqlParameter() {
                  Value = customer.PasswordHash,
                  ParameterName = "@p10",
                  SqlDbType = SqlDbType.NVarChar
              },
               new SqlParameter()
              {
                  Value = customer.PasswordSalt,
                  ParameterName = "@p11",
                  SqlDbType = SqlDbType.NVarChar
              },
               new SqlParameter()
              {
                  Value = customer.rowguid,
                  ParameterName = "@p12",
                  SqlDbType = SqlDbType.NVarChar
              },
               new SqlParameter()
              {
                  Value = customer.ModifiedDate,
                  ParameterName = "@p13",
                  SqlDbType = SqlDbType.DateTime2
              }
            };
            return parameters;
        }

        public static SqlParameter[] GetSqlParameters(Address address)
        {
            SqlParameter[] parameters =
                        {
              new SqlParameter()
              {
                  Value = address.AddressLine1,
                  ParameterName = "@p0",
                  SqlDbType = SqlDbType.NVarChar
              },
               new SqlParameter()
              {
                  Value = address.AddressLine2,
                  ParameterName = "@p1",
                  SqlDbType = SqlDbType.NVarChar
              },
                new SqlParameter()
              {
                  Value = address.City,
                  ParameterName = "@p2",
                  SqlDbType = SqlDbType.NVarChar
              },
               new SqlParameter()
              {
                  Value = address.StateProvince,
                  ParameterName = "@p3",
                  SqlDbType = SqlDbType.NVarChar
              },
                new SqlParameter()
              {
                  Value = address.CountryRegion,
                  ParameterName = "@p4",
                  SqlDbType = SqlDbType.NVarChar
              },
               new SqlParameter()
              {
                  Value = address.PostalCode,
                  ParameterName = "@p5",
                  SqlDbType = SqlDbType.NVarChar
              },
                   new SqlParameter()
              {
                  Value = address.rowguid,
                  ParameterName = "@p6",
                  SqlDbType = SqlDbType.NVarChar
              },
               new SqlParameter()
              {
                  Value = address.ModifiedDate,
                  ParameterName = "@p7",
                  SqlDbType = SqlDbType.DateTime2
              }
            };
            return parameters;

        }

        public static SqlParameter[] GetSqlParameters(CustomerAddress a, int customerID, int addressID)
        {
            SqlParameter[] parameters =
                        {
                              new SqlParameter()
                              {
                                  Value = customerID,
                                  ParameterName = "@p0",
                                  SqlDbType = SqlDbType.Int
                              },
                               new SqlParameter()
                              {
                                  Value = addressID,
                                  ParameterName = "@p1",
                                  SqlDbType = SqlDbType.Int
                              },
                                new SqlParameter()
                              {
                                  Value = a.AddressType,
                                  ParameterName = "@p2",
                                  SqlDbType = SqlDbType.NVarChar
                              },
                                       new SqlParameter()
                              {
                                  Value = a.rowguid,
                                  ParameterName = "@p3",
                                  SqlDbType = SqlDbType.NVarChar
                              },
                               new SqlParameter()
                              {
                                  Value = a.ModifiedDate,
                                  ParameterName = "@p4",
                                  SqlDbType = SqlDbType.DateTime2
                              }
                    };
            return parameters;
        }

        public static SqlParameter[] GetSqlParameters(Product p)
        {
            SqlParameter[] parameters =
               {
              new SqlParameter()
              {
                  Value = p.Name,
                  ParameterName = "@p0",
                  SqlDbType = SqlDbType.NVarChar
              },
               new SqlParameter()
              {
                  Value = p.ProductNumber,
                  ParameterName = "@p1",
                  SqlDbType = SqlDbType.NVarChar
              },
               new SqlParameter()
              {
                  Value = p.Color,
                  ParameterName = "@p2",
                  SqlDbType = SqlDbType.NVarChar
              },
               new SqlParameter()
              {
                  Value = p.StandardCost,
                  ParameterName = "@p3",
                  SqlDbType = SqlDbType.Decimal
              },
               new SqlParameter()
              {
                  Value = p.ListPrice,
                  ParameterName = "@p4",
                  SqlDbType = SqlDbType.Decimal
              },
               new SqlParameter()
              {
                  Value = p.Size,
                  ParameterName = "@p5",
                  SqlDbType = SqlDbType.NVarChar
              },
               new SqlParameter()
              {
                  Value = p.Weight,
                  ParameterName = "@p6",
                  SqlDbType = SqlDbType.Decimal 
              },
               new SqlParameter()
              {
                  Value = p.ProductCategoryID,
                  ParameterName = "@p7",
                  SqlDbType = SqlDbType.Int
              },
               new SqlParameter()
              {
                  Value = p.ProductModelID,
                  ParameterName = "@p8",
                  SqlDbType = SqlDbType.Int
              },
               new SqlParameter()
              {
                  Value = p.SellStartDate,
                  ParameterName = "@p9",
                  SqlDbType = SqlDbType.DateTime2
              },
               new SqlParameter() {
                  Value = p.SellEndDate,
                  ParameterName = "@p10",
                  SqlDbType = SqlDbType.DateTime2
              },
               new SqlParameter()
              {
                  Value = p.DiscontinuedDate,
                  ParameterName = "@p11",
                  SqlDbType = SqlDbType.DateTime2
              },
               new SqlParameter()
              {
                  Value = p.ThumbNailPhoto,
                  ParameterName = "@p12",
                  SqlDbType = SqlDbType.VarBinary
              },
               new SqlParameter()
              {
                  Value = p.ThumbnailPhotoFileName,
                  ParameterName = "@p13",
                  SqlDbType = SqlDbType.NVarChar
              },
                  new SqlParameter()
              {
                  Value = p.rowguid,
                  ParameterName = "@p14",
                  SqlDbType = SqlDbType.NVarChar
              },
               new SqlParameter()
              {
                  Value = p.ModifiedDate,
                  ParameterName = "@p15",
                  SqlDbType = SqlDbType.DateTime2
              }
            };

            return parameters;
        }

        public static SqlParameter[] GetSqlParameters(SalesOrderHeader h) // TODO: Korjaa järjestys muihinkin helpereihin
        {
            SqlParameter[] parameters =
               {
              new SqlParameter()
              {
                  Value = h.RevisionNumber,
                  ParameterName = "@p0",
                  SqlDbType = SqlDbType.TinyInt
              },
               new SqlParameter()
              {
                  Value = h.OrderDate,
                  ParameterName = "@p1",
                  SqlDbType = SqlDbType.DateTime2
              },
               new SqlParameter()
              {
                  Value = h.DueDate,
                  ParameterName = "@p2",
                  SqlDbType = SqlDbType.DateTime2
              },
               new SqlParameter()
              {
                  Value = h.ShipDate,
                  ParameterName = "@p3",
                  SqlDbType = SqlDbType.DateTime2
              },
               new SqlParameter()
              {
                  Value = h.Status,
                  ParameterName = "@p4",
                  SqlDbType = SqlDbType.TinyInt
              },
               new SqlParameter()
              {
                  Value = h.OnlineOrderFlag,
                  ParameterName = "@p5",
                  SqlDbType = SqlDbType.Bit
              },
               new SqlParameter()
              {
                  Value = h.SalesOrderNumber,
                  ParameterName = "@p6",
                  SqlDbType = SqlDbType.NVarChar
              },
               new SqlParameter()
              {
                  Value = h.PurchaseOrderNumber,
                  ParameterName = "@p7",
                  SqlDbType = SqlDbType.NVarChar
              },
               new SqlParameter()
              {
                  Value = h.AccountNumber,
                  ParameterName = "@p8",
                  SqlDbType = SqlDbType.NVarChar
              },
               new SqlParameter()
              {
                  Value = h.CustomerID,
                  ParameterName = "@p9",
                  SqlDbType = SqlDbType.Int
              },
               new SqlParameter() {
                  Value = h.ShipToAddressID,
                  ParameterName = "@p10",
                  SqlDbType = SqlDbType.Int
              },
               new SqlParameter()
              {
                  Value = h.BillToAddressID,
                  ParameterName = "@p11",
                  SqlDbType = SqlDbType.Int
              },
               new SqlParameter()
              {
                  Value = h.ShipMethod,
                  ParameterName = "@p12",
                  SqlDbType = SqlDbType.NVarChar
              },
                 new SqlParameter()
              {
                  Value = h.SubTotal,
                  ParameterName = "@p13",
                  SqlDbType = SqlDbType.Decimal
              },
               new SqlParameter()
              {
                  Value = h.TaxAmt,
                  ParameterName = "@p14",
                  SqlDbType = SqlDbType.Decimal
              },
               new SqlParameter()
              {
                  Value = h.Freight,
                  ParameterName = "@p15",
                  SqlDbType = SqlDbType.Decimal
              },
               new SqlParameter()
              {
                  Value = h.TotalDue,
                  ParameterName = "@p16",
                  SqlDbType = SqlDbType.Decimal
              },
               new SqlParameter()
              {
                  Value = h.Comment,
                  ParameterName = "@p17",
                  SqlDbType = SqlDbType.NVarChar
              },
               new SqlParameter()
              {
                  Value = h.rowguid,
                  ParameterName = "@p18",
                  SqlDbType = SqlDbType.NVarChar
              },
               new SqlParameter()
              {
                  Value = h.ModifiedDate,
                  ParameterName = "@p19",
                  SqlDbType = SqlDbType.DateTime2
              }, new SqlParameter()
              {
                  Value = h.CreditCardApprovalCode,
                  ParameterName = "@p20",
                  SqlDbType = SqlDbType.NVarChar
              },
            };

            return parameters;
        }

        public static SqlParameter[] GetSqlParameters(SalesOrderDetail detail, int salesOrderID)
        {

            SqlParameter[] parameters =
               {

                    new SqlParameter()
                    {
                        Value = detail.OrderQty,
                        ParameterName = "@p0",
                        SqlDbType = SqlDbType.SmallInt
                    },
                    new SqlParameter()
                    {
                        Value = detail.ProductID,
                        ParameterName = "@p1",
                        SqlDbType = SqlDbType.Int
                    },
                    new SqlParameter()
                    {
                        Value = detail.UnitPrice,
                        ParameterName = "@p2",
                        SqlDbType = SqlDbType.Decimal
                    },
                    new SqlParameter()
                    {
                        Value = detail.UnitPriceDiscount,
                        ParameterName = "@p3",
                        SqlDbType = SqlDbType.Decimal
                    },
                        new SqlParameter()
                    {
                        Value = detail.LineTotal,
                        ParameterName = "@p4",
                        SqlDbType = SqlDbType.Decimal
                    },
                    new SqlParameter()
                    {
                        Value = detail.rowguid,
                        ParameterName = "@p5",
                        SqlDbType = SqlDbType.NVarChar
                    },
                    new SqlParameter()
                    {
                        Value = detail.ModifiedDate,
                        ParameterName = "@p6",
                        SqlDbType = SqlDbType.DateTime2
                    }
                    ,
                    new SqlParameter()
                    {
                        Value = salesOrderID,
                        ParameterName = "@p7",
                        SqlDbType = SqlDbType.Int
                    }
            };
            return parameters;
        }
    }
}
