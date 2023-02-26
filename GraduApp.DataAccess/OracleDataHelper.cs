using GraduApp.DataAccess.GraduModels;
using Microsoft.Data.SqlClient;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduApp.DataAccess
{
    public static class OracleDataHelper
    {
        public static Product FromDataReader(OracleDataReader reader)
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

        public static OracleParameter[] GetOracleParameters(Customer customer)
        {

            OracleParameter[] parameters =
                {
              new OracleParameter()
              {
                  Value = customer.NameStyle ? 1: 0,
                  ParameterName = "@p0",
              },
               new OracleParameter()
              {
                  Value = customer.Title,
                  ParameterName = "@p1",
                  OracleDbType = OracleDbType.NVarchar2
              },
               new OracleParameter()
              {
                  Value = customer.FirstName,
                  ParameterName = "@p2",
                  OracleDbType = OracleDbType.NVarchar2
              },
               new OracleParameter()
              {
                  Value = customer.MiddleName,
                  ParameterName = "@p3",
                  OracleDbType = OracleDbType.NVarchar2
              },
               new OracleParameter()
              {
                  Value = customer.LastName,
                  ParameterName = "@p4",
                  OracleDbType = OracleDbType.NVarchar2
              },
               new OracleParameter()
              {
                  Value = customer.Suffix,
                  ParameterName = "@p5",
                  OracleDbType = OracleDbType.NVarchar2
              },
               new OracleParameter()
              {
                  Value = customer.CompanyName,
                  ParameterName = "@p6",
                  OracleDbType = OracleDbType.NVarchar2
              },
               new OracleParameter()
              {
                  Value = customer.SalesPerson,
                  ParameterName = "@p7",
                  OracleDbType = OracleDbType.NVarchar2
              },
               new OracleParameter()
              {
                  Value = customer.EmailAddress,
                  ParameterName = "@p8",
                  OracleDbType = OracleDbType.NVarchar2
              },
               new OracleParameter()
              {
                  Value = customer.Phone,
                  ParameterName = "@p9",
                  OracleDbType = OracleDbType.NVarchar2
              },
               new OracleParameter() {
                  Value = customer.PasswordHash,
                  ParameterName = "@p10",
                  OracleDbType = OracleDbType.NVarchar2
              },
               new OracleParameter()
              {
                  Value = customer.PasswordSalt,
                  ParameterName = "@p11",
                  OracleDbType = OracleDbType.NVarchar2
              },
               new OracleParameter()
              {
                  Value = customer.rowguid,
                  ParameterName = "@p12",
                  OracleDbType = OracleDbType.NVarchar2
              },
               new OracleParameter()
              {
                  Value = customer.ModifiedDate,
                  ParameterName = "@p13",
                  OracleDbType = OracleDbType.Date
              }
            };
            return parameters;
        }

        public static OracleParameter[] GetOracleParameters(Address address)
        {
            OracleParameter[] parameters =
                        {
              new OracleParameter()
              {
                  Value = address.AddressLine1,
                  ParameterName = "@p0",
                  OracleDbType = OracleDbType.NVarchar2
              },
               new OracleParameter()
              {
                  Value = address.AddressLine2,
                  ParameterName = "@p1",
                  OracleDbType = OracleDbType.NVarchar2
              },
                new OracleParameter()
              {
                  Value = address.City,
                  ParameterName = "@p2",
                  OracleDbType = OracleDbType.NVarchar2
              },
               new OracleParameter()
              {
                  Value = address.StateProvince,
                  ParameterName = "@p3",
                  OracleDbType = OracleDbType.NVarchar2
              },
                new OracleParameter()
              {
                  Value = address.CountryRegion,
                  ParameterName = "@p4",
                  OracleDbType = OracleDbType.NVarchar2
              },
               new OracleParameter()
              {
                  Value = address.PostalCode,
                  ParameterName = "@p5",
                  OracleDbType = OracleDbType.NVarchar2
              },
                   new OracleParameter()
              {
                  Value = address.rowguid,
                  ParameterName = "@p6",
                  OracleDbType = OracleDbType.NVarchar2
              },
               new OracleParameter()
              {
                  Value = address.ModifiedDate,
                  ParameterName = "@p7",
                  OracleDbType = OracleDbType.Date
              }
            };
            return parameters;

        }

        public static OracleParameter[] GetOracleParameters(CustomerAddress a, int customerID, int addressID)
        {
            OracleParameter[] parameters =
                        {
                              new OracleParameter()
                              {
                                  Value = customerID,
                                  ParameterName = "@p0",
                                  OracleDbType = OracleDbType.Int32
                              },
                               new OracleParameter()
                              {
                                  Value = addressID,
                                  ParameterName = "@p1",
                                  OracleDbType = OracleDbType.Int32
                              },
                                new OracleParameter()
                              {
                                  Value = a.AddressType,
                                  ParameterName = "@p2",
                                  OracleDbType = OracleDbType.NVarchar2
                              },
                                       new OracleParameter()
                              {
                                  Value = a.rowguid,
                                  ParameterName = "@p3",
                                  OracleDbType = OracleDbType.NVarchar2
                              },
                               new OracleParameter()
                              {
                                  Value = a.ModifiedDate,
                                  ParameterName = "@p4",
                                  OracleDbType = OracleDbType.Date
                              }
                    };
            return parameters;
        }

        public static OracleParameter[] GetOracleParameters(Product p)
        {
            OracleParameter[] parameters =
               {
              new OracleParameter()
              {
                  Value = p.Name,
                  ParameterName = "@p0",
                  OracleDbType = OracleDbType.NVarchar2
              },
               new OracleParameter()
              {
                  Value = p.ProductNumber,
                  ParameterName = "@p1",
                  OracleDbType = OracleDbType.NVarchar2
              },
               new OracleParameter()
              {
                  Value = p.Color,
                  ParameterName = "@p2",
                  OracleDbType = OracleDbType.NVarchar2
              },
               new OracleParameter()
              {
                  Value = p.StandardCost,
                  ParameterName = "@p3",
                  OracleDbType = OracleDbType.Decimal
              },
               new OracleParameter()
              {
                  Value = p.ListPrice,
                  ParameterName = "@p4",
                  OracleDbType = OracleDbType.Decimal
              },
               new OracleParameter()
              {
                  Value = p.Size,
                  ParameterName = "@p5",
                  OracleDbType = OracleDbType.NVarchar2
              },
               new OracleParameter()
              {
                  Value = p.Weight,
                  ParameterName = "@p6",
                  OracleDbType = OracleDbType.Decimal
              },
               new OracleParameter()
              {
                  Value = p.ProductCategoryID,
                  ParameterName = "@p7",
                  OracleDbType = OracleDbType.Int32
              },
               new OracleParameter()
              {
                  Value = p.ProductModelID,
                  ParameterName = "@p8",
                  OracleDbType = OracleDbType.Int32
              },
               new OracleParameter()
              {
                  Value = p.SellStartDate,
                  ParameterName = "@p9",
                  OracleDbType = OracleDbType.Date
              },
               new OracleParameter() {
                  Value = p.SellEndDate,
                  ParameterName = "@p10",
                  OracleDbType = OracleDbType.Date
              },
               new OracleParameter()
              {
                  Value = p.DiscontinuedDate,
                  ParameterName = "@p11",
                  OracleDbType = OracleDbType.Date
              },
               new OracleParameter()
              {
                  Value = p.ThumbNailPhoto,
                  ParameterName = "@p12",
              },
               new OracleParameter()
              {
                  Value = p.ThumbnailPhotoFileName,
                  ParameterName = "@p13",
                  OracleDbType = OracleDbType.NVarchar2
              },
                  new OracleParameter()
              {
                  Value = p.rowguid,
                  ParameterName = "@p14",
                  OracleDbType = OracleDbType.NVarchar2
              },
               new OracleParameter()
              {
                  Value = p.ModifiedDate,
                  ParameterName = "@p15",
                  OracleDbType = OracleDbType.Date
              }
            };

            return parameters;
        }

        public static OracleParameter[] GetOracleParameters(SalesOrderHeader h)
        {
            OracleParameter[] parameters =
               {
              new OracleParameter()
              {
                  Value = h.RevisionNumber,
                  ParameterName = "@p0",
              },
               new OracleParameter()
              {
                  Value = h.OrderDate,
                  ParameterName = "@p1",
                  OracleDbType = OracleDbType.Date
              },
               new OracleParameter()
              {
                  Value = h.DueDate,
                  ParameterName = "@p2",
                  OracleDbType = OracleDbType.Date
              },
               new OracleParameter()
              {
                  Value = h.ShipDate,
                  ParameterName = "@p3",
                  OracleDbType = OracleDbType.Date
              },
               new OracleParameter()
              {
                  Value = h.Status,
                  ParameterName = "@p4",
              },
               new OracleParameter()
              {
                  Value = (h.OnlineOrderFlag is null || !(bool)h.OnlineOrderFlag) ? 1: 0,
                  ParameterName = "@p5",
              },
               new OracleParameter()
              {
                  Value = h.SalesOrderNumber,
                  ParameterName = "@p6",
                  OracleDbType = OracleDbType.NVarchar2
              },
               new OracleParameter()
              {
                  Value = h.PurchaseOrderNumber,
                  ParameterName = "@p7",
                  OracleDbType = OracleDbType.NVarchar2
              },
               new OracleParameter()
              {
                  Value = h.AccountNumber,
                  ParameterName = "@p8",
                  OracleDbType = OracleDbType.NVarchar2
              },
               new OracleParameter()
              {
                  Value = h.CustomerID,
                  ParameterName = "@p9",
                  OracleDbType = OracleDbType.Int32
              },
               new OracleParameter() {
                  Value = h.ShipToAddressID,
                  ParameterName = "@p10",
                  OracleDbType = OracleDbType.Int32
              },
               new OracleParameter()
              {
                  Value = h.BillToAddressID,
                  ParameterName = "@p11",
                  OracleDbType = OracleDbType.Int32
              },
               new OracleParameter()
              {
                  Value = h.ShipMethod,
                  ParameterName = "@p12",
                  OracleDbType = OracleDbType.NVarchar2
              },
              
               new OracleParameter()
              {
                  Value = h.SubTotal,
                  ParameterName = "@p13",
                  OracleDbType = OracleDbType.Decimal
              },
               new OracleParameter()
              {
                  Value = h.TaxAmt,
                  ParameterName = "@p14",
                  OracleDbType = OracleDbType.Decimal
              },
               new OracleParameter()
              {
                  Value = h.Freight,
                  ParameterName = "@p15",
                  OracleDbType = OracleDbType.Decimal
              },
               new OracleParameter()
              {
                  Value = h.TotalDue,
                  ParameterName = "@p16",
                  OracleDbType = OracleDbType.Decimal
              },
               new OracleParameter()
              {
                  Value = h.Comment,
                  ParameterName = "@p17",
                  OracleDbType = OracleDbType.NVarchar2
              },
               new OracleParameter()
              {
                  Value = h.rowguid,
                  ParameterName = "@p18",
                  OracleDbType = OracleDbType.NVarchar2
              },
               new OracleParameter()
              {
                  Value = h.ModifiedDate,
                  ParameterName = "@p19",
                  OracleDbType = OracleDbType.Date
              },
                new OracleParameter()
              {
                  Value = h.CreditCardApprovalCode,
                  ParameterName = "@p20",
                  OracleDbType = OracleDbType.NVarchar2
              },
            };

            return parameters;
        }

        public static OracleParameter[] GetOracleParameters(SalesOrderDetail detail, int salesOrderID)
        {

            OracleParameter[] parameters =
               {

                    new OracleParameter()
                    {
                        Value = detail.OrderQty,
                        ParameterName = "@p0",
                    },
                    new OracleParameter()
                    {
                        Value = detail.ProductID,
                        ParameterName = "@p1",
                        OracleDbType = OracleDbType.Int32
                    },
                    new OracleParameter()
                    {
                        Value = detail.UnitPrice,
                        ParameterName = "@p2",
                        OracleDbType = OracleDbType.Decimal
                    },
                    new OracleParameter()
                    {
                        Value = detail.UnitPriceDiscount,
                        ParameterName = "@p3",
                        OracleDbType = OracleDbType.Decimal
                    },
                        new OracleParameter()
                    {
                        Value = detail.LineTotal,
                        ParameterName = "@p4",
                        OracleDbType = OracleDbType.Decimal
                    },
                    new OracleParameter()
                    {
                        Value = detail.rowguid,
                        ParameterName = "@p5",
                        OracleDbType = OracleDbType.NVarchar2
                    },
                    new OracleParameter()
                    {
                        Value = detail.ModifiedDate,
                        ParameterName = "@p6",
                        OracleDbType = OracleDbType.Date
                    }
                    ,
                    new OracleParameter()
                    {
                        Value = salesOrderID,
                        ParameterName = "@p7",
                        OracleDbType = OracleDbType.Int32
                    }
            };
            return parameters;
        }
    }
}
