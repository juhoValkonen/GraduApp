using GraduApp.DataAccess.GraduModels;

using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduApp.DataAccess
{
    public static class PostgreSQLDataHelper
    {
        public static Product FromDataReader(NpgsqlDataReader reader)
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

        public static NpgsqlParameter[] GetNpgsqlParameters(Customer customer)
        {

            NpgsqlParameter[] parameters =
                {
              new NpgsqlParameter()
              {
                  Value = customer.NameStyle,
                  ParameterName = "@p0",
                  NpgsqlDbType = NpgsqlDbType.Boolean
              },
               new NpgsqlParameter()
              {
                  Value = customer.Title,
                  ParameterName = "@p1",
                  NpgsqlDbType = NpgsqlDbType.Varchar
              },
               new NpgsqlParameter()
              {
                  Value = customer.FirstName,
                  ParameterName = "@p2",
                  NpgsqlDbType = NpgsqlDbType.Varchar
              },
               new NpgsqlParameter()
              {
                  Value = customer.MiddleName,
                  ParameterName = "@p3",
                  NpgsqlDbType = NpgsqlDbType.Varchar
              },
               new NpgsqlParameter()
              {
                  Value = customer.LastName,
                  ParameterName = "@p4",
                  NpgsqlDbType = NpgsqlDbType.Varchar
              },
               new NpgsqlParameter()
              {
                  Value = customer.Suffix,
                  ParameterName = "@p5",
                  NpgsqlDbType = NpgsqlDbType.Varchar
              },
               new NpgsqlParameter()
              {
                  Value = customer.CompanyName,
                  ParameterName = "@p6",
                  NpgsqlDbType = NpgsqlDbType.Varchar
              },
               new NpgsqlParameter()
              {
                  Value = customer.SalesPerson,
                  ParameterName = "@p7",
                  NpgsqlDbType = NpgsqlDbType.Varchar
              },
               new NpgsqlParameter()
              {
                  Value = customer.EmailAddress,
                  ParameterName = "@p8",
                  NpgsqlDbType = NpgsqlDbType.Varchar
              },
               new NpgsqlParameter()
              {
                  Value = customer.Phone,
                  ParameterName = "@p9",
                  NpgsqlDbType = NpgsqlDbType.Varchar
              },
               new NpgsqlParameter() {
                  Value = customer.PasswordHash,
                  ParameterName = "@p10",
                  NpgsqlDbType = NpgsqlDbType.Varchar
              },
               new NpgsqlParameter()
              {
                  Value = customer.PasswordSalt,
                  ParameterName = "@p11",
                  NpgsqlDbType = NpgsqlDbType.Varchar
              },
               new NpgsqlParameter()
              {
                  Value = customer.rowguid,
                  ParameterName = "@p12",
                  NpgsqlDbType = NpgsqlDbType.Varchar
              },
               new NpgsqlParameter()
              {
                  Value = customer.ModifiedDate,
                  ParameterName = "@p13",
                  NpgsqlDbType = NpgsqlDbType.Date
              }
            };
            return parameters;
        }

        public static NpgsqlParameter[] GetNpgsqlParameters(Address address)
        {
            NpgsqlParameter[] parameters =
                        {
              new NpgsqlParameter()
              {
                  Value = address.AddressLine1,
                  ParameterName = "@p0",
                  NpgsqlDbType = NpgsqlDbType.Varchar
              },
               new NpgsqlParameter()
              {
                  Value = address.AddressLine2,
                  ParameterName = "@p1",
                  NpgsqlDbType = NpgsqlDbType.Varchar
              },
                new NpgsqlParameter()
              {
                  Value = address.City,
                  ParameterName = "@p2",
                  NpgsqlDbType = NpgsqlDbType.Varchar
              },
               new NpgsqlParameter()
              {
                  Value = address.StateProvince,
                  ParameterName = "@p3",
                  NpgsqlDbType = NpgsqlDbType.Varchar
              },
                new NpgsqlParameter()
              {
                  Value = address.CountryRegion,
                  ParameterName = "@p4",
                  NpgsqlDbType = NpgsqlDbType.Varchar
              },
               new NpgsqlParameter()
              {
                  Value = address.PostalCode,
                  ParameterName = "@p5",
                  NpgsqlDbType = NpgsqlDbType.Varchar
              },
                   new NpgsqlParameter()
              {
                  Value = address.rowguid,
                  ParameterName = "@p6",
                  NpgsqlDbType = NpgsqlDbType.Varchar
              },
               new NpgsqlParameter()
              {
                  Value = address.ModifiedDate,
                  ParameterName = "@p7",
                  NpgsqlDbType = NpgsqlDbType.Date
              }
            };
            return parameters;

        }

        public static NpgsqlParameter[] GetNpgsqlParameters(CustomerAddress a, int customerID, int addressID)
        {
            NpgsqlParameter[] parameters =
                        {
                              new NpgsqlParameter()
                              {
                                  Value = customerID,
                                  ParameterName = "@p0",
                                  NpgsqlDbType = NpgsqlDbType.Integer
                              },
                               new NpgsqlParameter()
                              {
                                  Value = addressID,
                                  ParameterName = "@p1",
                                  NpgsqlDbType = NpgsqlDbType.Integer
                              },
                                new NpgsqlParameter()
                              {
                                  Value = a.AddressType,
                                  ParameterName = "@p2",
                                  NpgsqlDbType = NpgsqlDbType.Varchar
                              },
                                       new NpgsqlParameter()
                              {
                                  Value = a.rowguid,
                                  ParameterName = "@p3",
                                  NpgsqlDbType = NpgsqlDbType.Varchar
                              },
                               new NpgsqlParameter()
                              {
                                  Value = a.ModifiedDate,
                                  ParameterName = "@p4",
                                  NpgsqlDbType = NpgsqlDbType.Date
                              }
                    };
            return parameters;
        }

        public static NpgsqlParameter[] GetNpgsqlParameters(Product p)
        {
            NpgsqlParameter[] parameters =
               {
              new NpgsqlParameter()
              {
                  Value = p.Name,
                  ParameterName = "@p0",
                  NpgsqlDbType = NpgsqlDbType.Varchar
              },
               new NpgsqlParameter()
              {
                  Value = p.ProductNumber,
                  ParameterName = "@p1",
                  NpgsqlDbType = NpgsqlDbType.Varchar
              },
               new NpgsqlParameter()
              {
                  Value = p.Color,
                  ParameterName = "@p2",
                  NpgsqlDbType = NpgsqlDbType.Varchar
              },
               new NpgsqlParameter()
              {
                  Value = p.StandardCost,
                  ParameterName = "@p3",
                  NpgsqlDbType = NpgsqlDbType.Numeric
              },
               new NpgsqlParameter()
              {
                  Value = p.ListPrice,
                  ParameterName = "@p4",
                  NpgsqlDbType = NpgsqlDbType.Numeric
              },
               new NpgsqlParameter()
              {
                  Value = p.Size,
                  ParameterName = "@p5",
                  NpgsqlDbType = NpgsqlDbType.Varchar
              },
               new NpgsqlParameter()
              {
                  Value = p.Weight,
                  ParameterName = "@p6",
                  NpgsqlDbType = NpgsqlDbType.Numeric
              },
               new NpgsqlParameter()
              {
                  Value = p.ProductCategoryID,
                  ParameterName = "@p7",
                  NpgsqlDbType = NpgsqlDbType.Integer
              },
               new NpgsqlParameter()
              {
                  Value = p.ProductModelID,
                  ParameterName = "@p8",
                  NpgsqlDbType = NpgsqlDbType.Integer
              },
               new NpgsqlParameter()
              {
                  Value = p.SellStartDate,
                  ParameterName = "@p9",
                  NpgsqlDbType = NpgsqlDbType.Date
              },
               new NpgsqlParameter() {
                  Value = p.SellEndDate,
                  ParameterName = "@p10",
                  NpgsqlDbType = NpgsqlDbType.Date
              },
               new NpgsqlParameter()
              {
                  Value = p.DiscontinuedDate,
                  ParameterName = "@p11",
                  NpgsqlDbType = NpgsqlDbType.Date
              },
               new NpgsqlParameter()
              {
                  Value = p.ThumbNailPhoto,
                  ParameterName = "@p12",
              },
               new NpgsqlParameter()
              {
                  Value = p.ThumbnailPhotoFileName,
                  ParameterName = "@p13",
                  NpgsqlDbType = NpgsqlDbType.Varchar
              },
                  new NpgsqlParameter()
              {
                  Value = p.rowguid,
                  ParameterName = "@p14",
                  NpgsqlDbType = NpgsqlDbType.Varchar
              },
               new NpgsqlParameter()
              {
                  Value = p.ModifiedDate,
                  ParameterName = "@p15",
                  NpgsqlDbType = NpgsqlDbType.Date
              }
            };

            return parameters;
        }

        public static NpgsqlParameter[] GetNpgsqlParameters(SalesOrderHeader h) // TODO: Korjaa järjestys muihinkin helpereihin
        {
            NpgsqlParameter[] parameters =
               {
              new NpgsqlParameter()
              {
                  Value = h.RevisionNumber,
                  ParameterName = "@p0",
                  
              },
               new NpgsqlParameter()
              {
                  Value = h.OrderDate,
                  ParameterName = "@p1",
                  NpgsqlDbType = NpgsqlDbType.Date
              },
               new NpgsqlParameter()
              {
                  Value = h.DueDate,
                  ParameterName = "@p2",
                  NpgsqlDbType = NpgsqlDbType.Date
              },
               new NpgsqlParameter()
              {
                  Value = h.ShipDate,
                  ParameterName = "@p3",
                  NpgsqlDbType = NpgsqlDbType.Date
              },
               new NpgsqlParameter()
              {
                  Value = h.Status,
                  ParameterName = "@p4",
              },
               new NpgsqlParameter()
              {
                  Value = h.OnlineOrderFlag,
                  ParameterName = "@p5",
                  NpgsqlDbType = NpgsqlDbType.Boolean
              },
               new NpgsqlParameter()
              {
                  Value = h.SalesOrderNumber,
                  ParameterName = "@p6",
                  NpgsqlDbType = NpgsqlDbType.Varchar
              },
               new NpgsqlParameter()
              {
                  Value = h.PurchaseOrderNumber,
                  ParameterName = "@p7",
                  NpgsqlDbType = NpgsqlDbType.Varchar
              },
               new NpgsqlParameter()
              {
                  Value = h.AccountNumber,
                  ParameterName = "@p8",
                  NpgsqlDbType = NpgsqlDbType.Varchar
              },
               new NpgsqlParameter()
              {
                  Value = h.CustomerID,
                  ParameterName = "@p9",
                  NpgsqlDbType = NpgsqlDbType.Integer
              },
               new NpgsqlParameter() {
                  Value = h.ShipToAddressID,
                  ParameterName = "@p10",
                  NpgsqlDbType = NpgsqlDbType.Integer
              },
               new NpgsqlParameter()
              {
                  Value = h.BillToAddressID,
                  ParameterName = "@p11",
                  NpgsqlDbType = NpgsqlDbType.Integer
              },
               new NpgsqlParameter()
              {
                  Value = h.ShipMethod,
                  ParameterName = "@p12",
                  NpgsqlDbType = NpgsqlDbType.Varchar
              },
                 new NpgsqlParameter()
              {
                  Value = h.SubTotal,
                  ParameterName = "@p13",
                  NpgsqlDbType = NpgsqlDbType.Numeric
              },
               new NpgsqlParameter()
              {
                  Value = h.TaxAmt,
                  ParameterName = "@p14",
                  NpgsqlDbType = NpgsqlDbType.Numeric
              },
               new NpgsqlParameter()
              {
                  Value = h.Freight,
                  ParameterName = "@p15",
                  NpgsqlDbType = NpgsqlDbType.Numeric
              },
               new NpgsqlParameter()
              {
                  Value = h.TotalDue,
                  ParameterName = "@p16",
                  NpgsqlDbType = NpgsqlDbType.Numeric
              },
               new NpgsqlParameter()
              {
                  Value = h.Comment,
                  ParameterName = "@p17",
                  NpgsqlDbType = NpgsqlDbType.Varchar
              },
               new NpgsqlParameter()
              {
                  Value = h.rowguid,
                  ParameterName = "@p18",
                  NpgsqlDbType = NpgsqlDbType.Varchar
              },
               new NpgsqlParameter()
              {
                  Value = h.ModifiedDate,
                  ParameterName = "@p19",
                  NpgsqlDbType = NpgsqlDbType.Date
              }, new NpgsqlParameter()
              {
                  Value = h.CreditCardApprovalCode,
                  ParameterName = "@p20",
                  NpgsqlDbType = NpgsqlDbType.Varchar
              },
            };

            return parameters;
        }

        public static NpgsqlParameter[] GetNpgsqlParameters(SalesOrderDetail detail, int salesOrderID)
        {

            NpgsqlParameter[] parameters =
               {

                    new NpgsqlParameter()
                    {
                        Value = detail.OrderQty,
                        ParameterName = "@p0",
                    },
                    new NpgsqlParameter()
                    {
                        Value = detail.ProductID,
                        ParameterName = "@p1",
                        NpgsqlDbType = NpgsqlDbType.Integer
                    },
                    new NpgsqlParameter()
                    {
                        Value = detail.UnitPrice,
                        ParameterName = "@p2",
                        NpgsqlDbType = NpgsqlDbType.Numeric
                    },
                    new NpgsqlParameter()
                    {
                        Value = detail.UnitPriceDiscount,
                        ParameterName = "@p3",
                        NpgsqlDbType = NpgsqlDbType.Numeric
                    },
                        new NpgsqlParameter()
                    {
                        Value = detail.LineTotal,
                        ParameterName = "@p4",
                        NpgsqlDbType = NpgsqlDbType.Numeric
                    },
                    new NpgsqlParameter()
                    {
                        Value = detail.rowguid,
                        ParameterName = "@p5",
                        NpgsqlDbType = NpgsqlDbType.Varchar
                    },
                    new NpgsqlParameter()
                    {
                        Value = detail.ModifiedDate,
                        ParameterName = "@p6",
                        NpgsqlDbType = NpgsqlDbType.Date
                    }
                    ,
                    new NpgsqlParameter()
                    {
                        Value = salesOrderID,
                        ParameterName = "@p7",
                        NpgsqlDbType = NpgsqlDbType.Integer
                    }
            };
            return parameters;
        }

    }
}
