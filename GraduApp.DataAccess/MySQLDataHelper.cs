using GraduApp.DataAccess.GraduModels;
using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduApp.DataAccess
{
    public static class MySQLDataHelper
    {
        public static Product FromDataReader(MySqlDataReader reader)
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

        public static MySqlParameter[] GetMySqlParameters(Customer customer)
        {

            MySqlParameter[] parameters =
                {
              new MySqlParameter()
              {
                  Value = customer.NameStyle,
                  ParameterName = "@p0",
                  MySqlDbType = MySqlDbType.Bit
              },
               new MySqlParameter()
              {
                  Value = customer.Title,
                  ParameterName = "@p1",
                  MySqlDbType = MySqlDbType.Text
              },
               new MySqlParameter()
              {
                  Value = customer.FirstName,
                  ParameterName = "@p2",
                  MySqlDbType = MySqlDbType.Text
              },
               new MySqlParameter()
              {
                  Value = customer.MiddleName,
                  ParameterName = "@p3",
                  MySqlDbType = MySqlDbType.Text
              },
               new MySqlParameter()
              {
                  Value = customer.LastName,
                  ParameterName = "@p4",
                  MySqlDbType = MySqlDbType.Text
              },
               new MySqlParameter()
              {
                  Value = customer.Suffix,
                  ParameterName = "@p5",
                  MySqlDbType = MySqlDbType.Text
              },
               new MySqlParameter()
              {
                  Value = customer.CompanyName,
                  ParameterName = "@p6",
                  MySqlDbType = MySqlDbType.Text
              },
               new MySqlParameter()
              {
                  Value = customer.SalesPerson,
                  ParameterName = "@p7",
                  MySqlDbType = MySqlDbType.Text
              },
               new MySqlParameter()
              {
                  Value = customer.EmailAddress,
                  ParameterName = "@p8",
                  MySqlDbType = MySqlDbType.Text
              },
               new MySqlParameter()
              {
                  Value = customer.Phone,
                  ParameterName = "@p9",
                  MySqlDbType = MySqlDbType.Text
              },
               new MySqlParameter() {
                  Value = customer.PasswordHash,
                  ParameterName = "@p10",
                  MySqlDbType = MySqlDbType.Text
              },
               new MySqlParameter()
              {
                  Value = customer.PasswordSalt,
                  ParameterName = "@p11",
                  MySqlDbType = MySqlDbType.Text
              },
               new MySqlParameter()
              {
                  Value = customer.rowguid,
                  ParameterName = "@p12",
                  MySqlDbType = MySqlDbType.Text
              },
               new MySqlParameter()
              {
                  Value = customer.ModifiedDate,
                  ParameterName = "@p13",
                  MySqlDbType = MySqlDbType.DateTime
              }
            };
            return parameters;
        }

        public static MySqlParameter[] GetMySqlParameters(Address address)
        {
            MySqlParameter[] parameters =
                        {
              new MySqlParameter()
              {
                  Value = address.AddressLine1,
                  ParameterName = "@p0",
                  MySqlDbType = MySqlDbType.VarString
              },
               new MySqlParameter()
              {
                  Value = address.AddressLine2,
                  ParameterName = "@p1",
                  MySqlDbType = MySqlDbType.VarString
              },
                new MySqlParameter()
              {
                  Value = address.City,
                  ParameterName = "@p2",
                  MySqlDbType = MySqlDbType.VarString
              },
               new MySqlParameter()
              {
                  Value = address.StateProvince,
                  ParameterName = "@p3",
                  MySqlDbType = MySqlDbType.VarString
              },
                new MySqlParameter()
              {
                  Value = address.CountryRegion,
                  ParameterName = "@p4",
                  MySqlDbType = MySqlDbType.VarString
              },
               new MySqlParameter()
              {
                  Value = address.PostalCode,
                  ParameterName = "@p5",
                  MySqlDbType = MySqlDbType.VarString
              },
                   new MySqlParameter()
              {
                  Value = address.rowguid,
                  ParameterName = "@p6",
                  MySqlDbType = MySqlDbType.VarString
              },
               new MySqlParameter()
              {
                  Value = address.ModifiedDate,
                  ParameterName = "@p7",
                  MySqlDbType = MySqlDbType.DateTime
              }
            };
            return parameters;

        }

        public static MySqlParameter[] GetMySqlParameters(CustomerAddress a, int customerID, int addressID)
        {
            MySqlParameter[] parameters =
                        {
                              new MySqlParameter()
                              {
                                  Value = customerID,
                                  ParameterName = "@p0",
                                  MySqlDbType = MySqlDbType.Int32
                              },
                               new MySqlParameter()
                              {
                                  Value = addressID,
                                  ParameterName = "@p1",
                                  MySqlDbType = MySqlDbType.Int32
                              },
                                new MySqlParameter()
                              {
                                  Value = a.AddressType,
                                  ParameterName = "@p2",
                                  MySqlDbType = MySqlDbType.VarString
                              },
                                       new MySqlParameter()
                              {
                                  Value = a.rowguid,
                                  ParameterName = "@p3",
                                  MySqlDbType = MySqlDbType.VarString
                              },
                               new MySqlParameter()
                              {
                                  Value = a.ModifiedDate,
                                  ParameterName = "@p4",
                                  MySqlDbType = MySqlDbType.DateTime
                              }
                    };
            return parameters;
        }

        public static MySqlParameter[] GetMySqlParameters(Product p)
        {
            MySqlParameter[] parameters =
               {
              new MySqlParameter()
              {
                  Value = p.Name,
                  ParameterName = "@p0",
                  MySqlDbType = MySqlDbType.VarString
              },
               new MySqlParameter()
              {
                  Value = p.ProductNumber,
                  ParameterName = "@p1",
                  MySqlDbType = MySqlDbType.VarString
              },
               new MySqlParameter()
              {
                  Value = p.Color,
                  ParameterName = "@p2",
                  MySqlDbType = MySqlDbType.VarString
              },
               new MySqlParameter()
              {
                  Value = p.StandardCost,
                  ParameterName = "@p3",
                  MySqlDbType = MySqlDbType.Decimal
              },
               new MySqlParameter()
              {
                  Value = p.ListPrice,
                  ParameterName = "@p4",
                  MySqlDbType = MySqlDbType.Decimal
              },
               new MySqlParameter()
              {
                  Value = p.Size,
                  ParameterName = "@p5",
                  MySqlDbType = MySqlDbType.VarString
              },
               new MySqlParameter()
              {
                  Value = p.Weight,
                  ParameterName = "@p6",
                  MySqlDbType = MySqlDbType.Decimal
              },
               new MySqlParameter()
              {
                  Value = p.ProductCategoryID,
                  ParameterName = "@p7",
                  MySqlDbType = MySqlDbType.Int32
              },
               new MySqlParameter()
              {
                  Value = p.ProductModelID,
                  ParameterName = "@p8",
                  MySqlDbType = MySqlDbType.Int32
              },
               new MySqlParameter()
              {
                  Value = p.SellStartDate,
                  ParameterName = "@p9",
                  MySqlDbType = MySqlDbType.DateTime
              },
               new MySqlParameter() {
                  Value = p.SellEndDate,
                  ParameterName = "@p10",
                  MySqlDbType = MySqlDbType.DateTime
              },
               new MySqlParameter()
              {
                  Value = p.DiscontinuedDate,
                  ParameterName = "@p11",
                  MySqlDbType = MySqlDbType.DateTime
              },
               new MySqlParameter()
              {
                  Value = p.ThumbNailPhoto,
                  ParameterName = "@p12",
                  MySqlDbType = MySqlDbType.VarBinary
              },
               new MySqlParameter()
              {
                  Value = p.ThumbnailPhotoFileName,
                  ParameterName = "@p13",
                  MySqlDbType = MySqlDbType.VarString
              },
                  new MySqlParameter()
              {
                  Value = p.rowguid,
                  ParameterName = "@p14",
                  MySqlDbType = MySqlDbType.VarString
              },
               new MySqlParameter()
              {
                  Value = p.ModifiedDate,
                  ParameterName = "@p15",
                  MySqlDbType = MySqlDbType.DateTime
              }
            };

            return parameters;
        }

        public static MySqlParameter[] GetMySqlParameters(SalesOrderHeader h)
        {
            MySqlParameter[] parameters =
               {
              new MySqlParameter()
              {
                  Value = h.RevisionNumber,
                  ParameterName = "@p0",
              },
               new MySqlParameter()
              {
                  Value = h.OrderDate,
                  ParameterName = "@p1",
                  MySqlDbType = MySqlDbType.DateTime
              },
               new MySqlParameter()
              {
                  Value = h.DueDate,
                  ParameterName = "@p2",
                  MySqlDbType = MySqlDbType.DateTime
              },
               new MySqlParameter()
              {
                  Value = h.ShipDate,
                  ParameterName = "@p3",
                  MySqlDbType = MySqlDbType.DateTime
              },
               new MySqlParameter()
              {
                  Value = h.Status,
                  ParameterName = "@p4",
              },
               new MySqlParameter()
              {
                  Value = h.OnlineOrderFlag,
                  ParameterName = "@p5",
                  MySqlDbType = MySqlDbType.Bit
              },
               new MySqlParameter()
              {
                  Value = h.SalesOrderNumber,
                  ParameterName = "@p6",
                  MySqlDbType = MySqlDbType.VarString
              },
               new MySqlParameter()
              {
                  Value = h.PurchaseOrderNumber,
                  ParameterName = "@p7",
                  MySqlDbType = MySqlDbType.VarString
              },
               new MySqlParameter()
              {
                  Value = h.AccountNumber,
                  ParameterName = "@p8",
                  MySqlDbType = MySqlDbType.VarString
              },
               new MySqlParameter()
              {
                  Value = h.CustomerID,
                  ParameterName = "@p9",
                  MySqlDbType = MySqlDbType.Int32
              },
               new MySqlParameter() {
                  Value = h.ShipToAddressID,
                  ParameterName = "@p10",
                  MySqlDbType = MySqlDbType.Int32
              },
               new MySqlParameter()
              {
                  Value = h.BillToAddressID,
                  ParameterName = "@p11",
                  MySqlDbType = MySqlDbType.Int32
              },
               new MySqlParameter()
              {
                  Value = h.ShipMethod,
                  ParameterName = "@p12",
                  MySqlDbType = MySqlDbType.VarString
              },
       
               new MySqlParameter()
              {
                  Value = h.SubTotal,
                  ParameterName = "@p13",
                  MySqlDbType = MySqlDbType.Decimal
              },
               new MySqlParameter()
              {
                  Value = h.TaxAmt,
                  ParameterName = "@p14",
                  MySqlDbType = MySqlDbType.Decimal
              },
               new MySqlParameter()
              {
                  Value = h.Freight,
                  ParameterName = "@p15",
                  MySqlDbType = MySqlDbType.Decimal
              },
               new MySqlParameter()
              {
                  Value = h.TotalDue,
                  ParameterName = "@p16",
                  MySqlDbType = MySqlDbType.Decimal
              },
               new MySqlParameter()
              {
                  Value = h.Comment,
                  ParameterName = "@p17",
                  MySqlDbType = MySqlDbType.VarString
              },
               new MySqlParameter()
              {
                  Value = h.rowguid,
                  ParameterName = "@p18",
                  MySqlDbType = MySqlDbType.VarString
              },
               new MySqlParameter()
              {
                  Value = h.ModifiedDate,
                  ParameterName = "@p19",
                  MySqlDbType = MySqlDbType.DateTime
              },        new MySqlParameter()
              {
                  Value = h.CreditCardApprovalCode,
                  ParameterName = "@p20",
                  MySqlDbType = MySqlDbType.VarString
              },
            };

            return parameters;
        }

        public static MySqlParameter[] GetMySqlParameters(SalesOrderDetail detail, int salesOrderID)
        {

            MySqlParameter[] parameters =
               {

                    new MySqlParameter()
                    {
                        Value = detail.OrderQty,
                        ParameterName = "@p0",
                    },
                    new MySqlParameter()
                    {
                        Value = detail.ProductID,
                        ParameterName = "@p1",
                        MySqlDbType = MySqlDbType.Int32
                    },
                    new MySqlParameter()
                    {
                        Value = detail.UnitPrice,
                        ParameterName = "@p2",
                        MySqlDbType = MySqlDbType.Decimal
                    },
                    new MySqlParameter()
                    {
                        Value = detail.UnitPriceDiscount,
                        ParameterName = "@p3",
                        MySqlDbType = MySqlDbType.Decimal
                    },
                        new MySqlParameter()
                    {
                        Value = detail.LineTotal,
                        ParameterName = "@p4",
                        MySqlDbType = MySqlDbType.Decimal
                    },
                    new MySqlParameter()
                    {
                        Value = detail.rowguid,
                        ParameterName = "@p5",
                        MySqlDbType = MySqlDbType.VarString
                    },
                    new MySqlParameter()
                    {
                        Value = detail.ModifiedDate,
                        ParameterName = "@p6",
                        MySqlDbType = MySqlDbType.DateTime
                    }
                    ,
                    new MySqlParameter()
                    {
                        Value = salesOrderID,
                        ParameterName = "@p7",
                        MySqlDbType = MySqlDbType.Int32
                    }
            };
            return parameters;
        }
    }
}
