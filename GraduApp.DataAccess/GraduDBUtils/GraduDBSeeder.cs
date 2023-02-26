using GraduApp.DataAccess.Enums;

using GraduApp.DataAccess.GraduModels;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using Org.BouncyCastle.Crypto.Signers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduApp.DataAccess.GraduDBUtils
{
    public class GraduDBSeeder
    {
        GraduDBContext targetContext;
        SeedDBContextFactory seedDBContextFactory = new SeedDBContextFactory();
        private IConfiguration configuration;
        public GraduDBSeeder(GraduDBContext targetContext, IConfiguration _configuration)
        {
            this.targetContext = targetContext;
            configuration = _configuration;
        }

        private void setIdentityInsert( string tableName, bool enable)
        {
            string onOff = enable ? "ON" : "OFF";
            if(targetContext.DbType == GraduDBType.SQLServer)
            {
                targetContext.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT [dbo].[{tableName}] {onOff}");
            }
            
        }

        public void Seed()
        {
            using (GraduDBContext sourceContext = seedDBContextFactory.MakeSeedDBContext(configuration))
            {

                using (targetContext)
                {
                    using (var transaction = targetContext.Database.BeginTransaction())
                    {
                        setIdentityInsert("Customer", true);
                        targetContext.Customer.AddRange(sourceContext.Customer);
                        targetContext.SaveChanges();
                        setIdentityInsert("Customer", false);
                        transaction.Commit();
                    }

                    using (var transaction = targetContext.Database.BeginTransaction())
                    {
                        setIdentityInsert("Address", true);
                        targetContext.Address.AddRange(sourceContext.Address);
                        targetContext.SaveChanges();
                        setIdentityInsert("Address", false);
                        transaction.Commit();
                    }

                    using (var transaction = targetContext.Database.BeginTransaction())
                    {
                        setIdentityInsert("CustomerAddress", true);
                        targetContext.CustomerAddress.AddRange(sourceContext.CustomerAddress);
                        targetContext.SaveChanges();
                        setIdentityInsert("CustomerAddress", false);
                        transaction.Commit();
                    }

                    using (var transaction = targetContext.Database.BeginTransaction())
                    {
                        setIdentityInsert("ProductModel", true);
                        targetContext.ProductModel.AddRange(sourceContext.ProductModel);
                        targetContext.SaveChanges();
                        setIdentityInsert("ProductModel", false);
                        transaction.Commit();
                    }

                    using (var transaction = targetContext.Database.BeginTransaction())
                    {
                        setIdentityInsert("ProductCategory", true);
                        targetContext.ProductCategory.AddRange(sourceContext.ProductCategory);
                        targetContext.SaveChanges();
                        setIdentityInsert("ProductCategory", false);
                        transaction.Commit();
                    }

                    using (var transaction = targetContext.Database.BeginTransaction())
                    {
                        setIdentityInsert("Product", true);
                        targetContext.Product.AddRange(sourceContext.Product);
                        targetContext.SaveChanges();
                        setIdentityInsert("Product", false);
                        transaction.Commit();
                    }

                    using (var transaction = targetContext.Database.BeginTransaction())
                    {
                        setIdentityInsert("ProductDescription", true);
                        targetContext.ProductDescription.AddRange(sourceContext.ProductDescription);
                        targetContext.SaveChanges();
                        setIdentityInsert("ProductDescription", false);
                        transaction.Commit();
                    }

                    using (var transaction = targetContext.Database.BeginTransaction())
                    {
                        setIdentityInsert("ProductModelProductDescription", true);
                        targetContext.ProductModelProductDescription.AddRange(sourceContext.ProductModelProductDescription);
                        targetContext.SaveChanges();
                        setIdentityInsert("ProductModelProductDescription", false);
                        transaction.Commit();
                    }

                    using (var transaction = targetContext.Database.BeginTransaction())
                    {
                        setIdentityInsert("SalesOrderHeader", true);
                        targetContext.SalesOrderHeader.AddRange(sourceContext.SalesOrderHeader);
                        targetContext.SaveChanges();
                        setIdentityInsert("SalesOrderHeader", false);
                        transaction.Commit();
                    }

                    using (var transaction = targetContext.Database.BeginTransaction())
                    {
                        setIdentityInsert("SalesOrderDetail", true);
                        targetContext.SalesOrderDetail.AddRange(sourceContext.SalesOrderDetail);
                        targetContext.SaveChanges();
                        setIdentityInsert("SalesOrderDetail", false);
                        transaction.Commit();
                    }


                }
            }

            //For oracle, fix the sequences that are out of sync...
            if (targetContext.DbType == GraduDBType.Oracle)
                using (var conn = new OracleConnection(configuration.GetConnectionString("OracleConnection")))
                {
                    conn.Open();
                    Dictionary<string, string> tablesAndSequences = new Dictionary<string, string>();
                    using (OracleCommand cmd = new OracleCommand(@"select table_name, data_default from user_tab_cols where identity_column = 'YES' AND TABLE_NAME NOT LIKE 'ErrorLog'", conn))
                    {
                        cmd.InitialLONGFetchSize = -1;
                        OracleDataReader reader = cmd.ExecuteReader();
                       
                        while (reader.Read())
                        {
                            tablesAndSequences.Add(reader.GetString(0), reader.GetString(1));
                        }
                    }

                    foreach(KeyValuePair<string, string> kvp in tablesAndSequences)
                    {
                        string tableName = "\"" + kvp.Key + "\"";
                        string sequenceName = "\""+kvp.Value.Replace(".nextval", "")+"\"";
                        using (OracleCommand cmd = new OracleCommand(@"declare  maxval number;
                                  begin
                                  select max(""ID"") into maxval from GRADUAPP." + tableName + @";
                                  execute immediate 'alter table GRADUAPP." + tableName + @" modify ""ID"" generated always as identity (INCREMENT BY 1 START WITH  '|| (maxval + 1)||')';
                                  END;
                        ", conn))
                        {
                            cmd.ExecuteNonQuery();
                        }
                            

                    }


                }
        }


        public void SeedCustomersAfterBenchmark()
        {
            using (GraduDBContext sourceContext = seedDBContextFactory.MakeSeedDBContext(configuration))
            {

                using (targetContext)
                {
                    using (var transaction = targetContext.Database.BeginTransaction())
                    {
                        setIdentityInsert("Customer", true);
                        targetContext.Customer.AddRange(sourceContext.Customer);
                        targetContext.SaveChanges();
                        setIdentityInsert("Customer", false);
                        transaction.Commit();
                    }

                    using (var transaction = targetContext.Database.BeginTransaction())
                    {
                        setIdentityInsert("Address", true);
                        targetContext.Address.AddRange(sourceContext.Address);
                        targetContext.SaveChanges();
                        setIdentityInsert("Address", false);
                        transaction.Commit();
                    }

                    using (var transaction = targetContext.Database.BeginTransaction())
                    {
                        setIdentityInsert("CustomerAddress", true);
                        targetContext.CustomerAddress.AddRange(sourceContext.CustomerAddress);
                        targetContext.SaveChanges();
                        setIdentityInsert("CustomerAddress", false);
                        transaction.Commit();
                    }

                    using (var transaction = targetContext.Database.BeginTransaction())
                    {
                        setIdentityInsert("ProductModel", true);
                        targetContext.ProductModel.AddRange(sourceContext.ProductModel);
                        targetContext.SaveChanges();
                        setIdentityInsert("ProductModel", false);
                        transaction.Commit();
                    }

                    using (var transaction = targetContext.Database.BeginTransaction())
                    {
                        setIdentityInsert("ProductCategory", true);
                        targetContext.ProductCategory.AddRange(sourceContext.ProductCategory);
                        targetContext.SaveChanges();
                        setIdentityInsert("ProductCategory", false);
                        transaction.Commit();
                    }

                    using (var transaction = targetContext.Database.BeginTransaction())
                    {
                        setIdentityInsert("Product", true);
                        targetContext.Product.AddRange(sourceContext.Product);
                        targetContext.SaveChanges();
                        setIdentityInsert("Product", false);
                        transaction.Commit();
                    }

                    using (var transaction = targetContext.Database.BeginTransaction())
                    {
                        setIdentityInsert("ProductDescription", true);
                        targetContext.ProductDescription.AddRange(sourceContext.ProductDescription);
                        targetContext.SaveChanges();
                        setIdentityInsert("ProductDescription", false);
                        transaction.Commit();
                    }

                    using (var transaction = targetContext.Database.BeginTransaction())
                    {
                        setIdentityInsert("ProductModelProductDescription", true);
                        targetContext.ProductModelProductDescription.AddRange(sourceContext.ProductModelProductDescription);
                        targetContext.SaveChanges();
                        setIdentityInsert("ProductModelProductDescription", false);
                        transaction.Commit();
                    }

                    using (var transaction = targetContext.Database.BeginTransaction())
                    {
                        setIdentityInsert("SalesOrderHeader", true);
                        targetContext.SalesOrderHeader.AddRange(sourceContext.SalesOrderHeader);
                        targetContext.SaveChanges();
                        setIdentityInsert("SalesOrderHeader", false);
                        transaction.Commit();
                    }

                    using (var transaction = targetContext.Database.BeginTransaction())
                    {
                        setIdentityInsert("SalesOrderDetail", true);
                        targetContext.SalesOrderDetail.AddRange(sourceContext.SalesOrderDetail);
                        targetContext.SaveChanges();
                        setIdentityInsert("SalesOrderDetail", false);
                        transaction.Commit();
                    }


                }
            }

            //For oracle, fix the sequences that are out of sync...
            if (targetContext.DbType == GraduDBType.Oracle)
                using (var conn = new OracleConnection(configuration.GetConnectionString("OracleConnection")))
                {
                    conn.Open();
                    Dictionary<string, string> tablesAndSequences = new Dictionary<string, string>();
                    using (OracleCommand cmd = new OracleCommand(@"select table_name, data_default from user_tab_cols where identity_column = 'YES' AND TABLE_NAME NOT LIKE 'ErrorLog'", conn))
                    {
                        cmd.InitialLONGFetchSize = -1;
                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            tablesAndSequences.Add(reader.GetString(0), reader.GetString(1));
                        }
                    }

                    foreach (KeyValuePair<string, string> kvp in tablesAndSequences)
                    {
                        string tableName = "\"" + kvp.Key + "\"";
                        string sequenceName = "\"" + kvp.Value.Replace(".nextval", "") + "\"";
                        using (OracleCommand cmd = new OracleCommand(@"declare  maxval number;
                                  begin
                                  select max(""ID"") into maxval from GRADUAPP." + tableName + @";
                                  execute immediate 'alter table GRADUAPP." + tableName + @" modify ""ID"" generated always as identity (INCREMENT BY 1 START WITH  '|| (maxval + 1)||')';
                                  END;
                        ", conn))
                        {
                            cmd.ExecuteNonQuery();
                        }


                    }


                }
        }



    }
}
