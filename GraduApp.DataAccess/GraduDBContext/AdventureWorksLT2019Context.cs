using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using GraduApp.DataAccess.GraduModels;
using GraduApp.DataAccess.Enums;
using System.Data;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using MySql.EntityFrameworkCore.Extensions;
using Org.BouncyCastle.Asn1.X509;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace GraduApp.DataAccess
{
    public partial class GraduDBContext : DbContext
    {
        public readonly GraduDBType DbType;
        bool sourceDataContext = false;
        bool enableLogging = false;
        private readonly IConfiguration configuration;

        public GraduDBContext()
        {
        }
        
        public GraduDBContext(GraduDBType _dbType, 
                              IConfiguration _configuration, 
                              bool _sourceDataContext = false, 
                              bool _enableLogging = false, 
                              bool _useChangeTrackingOnSelects = true)
        {
            if(sourceDataContext == true &&
                _dbType != GraduDBType.SQLServer)
            {
                throw new Exception("The source Data context must use SQL-Server");
            }

            DbType = _dbType;
            sourceDataContext = _sourceDataContext;
            enableLogging = _enableLogging;
            configuration = _configuration;
            if (!_useChangeTrackingOnSelects)
            {
                base.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            } 
            

        }





        public virtual DbSet<Address> Address { get; set; } = null!;
        public virtual DbSet<BuildVersion> BuildVersion { get; set; } = null!;
        public virtual DbSet<Customer> Customer { get; set; } = null!;
        public virtual DbSet<CustomerAddress> CustomerAddress { get; set; } = null!;
        public virtual DbSet<ErrorLog> ErrorLog { get; set; } = null!;
        public virtual DbSet<Product> Product { get; set; } = null!;
        public virtual DbSet<ProductCategory> ProductCategory { get; set; } = null!;
        public virtual DbSet<ProductDescription> ProductDescription { get; set; } = null!;
        public virtual DbSet<ProductModel> ProductModel { get; set; } = null!;
        public virtual DbSet<ProductModelProductDescription> ProductModelProductDescription { get; set; } = null!;
        public virtual DbSet<SalesOrderDetail> SalesOrderDetail { get; set; } = null!;
        public virtual DbSet<SalesOrderHeader> SalesOrderHeader { get; set; } = null!;
        public virtual DbSet<vGetAllCategories> vGetAllCategories { get; set; } = null!;
        public virtual DbSet<vProductAndDescription> vProductAndDescription { get; set; } = null!;
        public virtual DbSet<vProductModelCatalogDescription> vProductModelCatalogDescription { get; set; } = null!;
        
       
        protected override void ConfigureConventions(
            ModelConfigurationBuilder configurationBuilder)
        {
           if(DbType != GraduDBType.Oracle) { 
            configurationBuilder.Properties<decimal>()
            .HavePrecision(12, 3);
            }
            if (DbType == GraduDBType.Oracle)
            {
                configurationBuilder.Properties<int>()
                .HavePrecision(9,0);
                configurationBuilder.Properties<Guid>()
               .HaveColumnType("NVARCHAR2(32)");
                configurationBuilder.Properties<decimal>()
                .HavePrecision(18, 3);
            }

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (enableLogging)
            {
                optionsBuilder.LogTo(Console.WriteLine);
                optionsBuilder.EnableSensitiveDataLogging();
            }
            
            
            if (sourceDataContext)
            {
                _ = optionsBuilder.UseSqlServer(configuration.GetConnectionString("SeedDBConnection"));
                return;
            }

            switch (DbType)
            {
                case GraduDBType.SQLServer:
                    _ = optionsBuilder.UseSqlServer(configuration.GetConnectionString("SqlServerConnection"));
                    break;
                case GraduDBType.MYSQL:
                    _ = optionsBuilder.UseMySQL(configuration.GetConnectionString("MySQLConnection"));
                    break;
                case GraduDBType.PostgreSQL:
                    _ = optionsBuilder.UseNpgsql(configuration.GetConnectionString("PostgreSQLrConnection"));
                    break;
                case GraduDBType.Mongo:
                    _ = optionsBuilder.UseMongoDB(configuration.GetConnectionString("MongoDBConnection"));
                    break;
                case GraduDBType.Oracle:
                    _ = optionsBuilder.UseOracle(configuration.GetConnectionString("OracleConnection"));
                    break;
                default:
                    break;
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            

            if (DbType == GraduDBType.PostgreSQL)
            {
                modelBuilder.HasSequence<int>("CustomerIDs")
                .StartsAt(100000)
                .IncrementsBy(1);

                modelBuilder.HasSequence<int>("ProductIDs")
              .StartsAt(100000)
              .IncrementsBy(1);

                modelBuilder.HasSequence<int>("CustomerAddressIDs")
                 .StartsAt(100000)
                 .IncrementsBy(1);

                modelBuilder.HasSequence<int>("AddressIDs")
               .StartsAt(100000)
               .IncrementsBy(1);

                modelBuilder.HasSequence<int>("SalesOrderHeaderIDs")
           .StartsAt(100000)
           .IncrementsBy(1);


                modelBuilder.HasSequence<int>("SalesOrderDetailIDs")
           .StartsAt(130000)
           .IncrementsBy(1);


                modelBuilder.Entity<Customer>()
                       .Property(o => o.ID)
                       .HasDefaultValueSql("nextval('\"CustomerIDs\"')");

                modelBuilder.Entity<CustomerAddress>()
                      .Property(o => o.ID)
                      .HasDefaultValueSql("nextval('\"CustomerAddressIDs\"')");

                modelBuilder.Entity<Address>()
                  .Property(o => o.ID)
                  .HasDefaultValueSql("nextval('\"AddressIDs\"')");

                modelBuilder.Entity<Product>()
            .Property(o => o.ID)
            .HasDefaultValueSql("nextval('\"ProductIDs\"')");

                modelBuilder.Entity<SalesOrderHeader>()
 .Property(o => o.ID)
 .HasDefaultValueSql("nextval('\"SalesOrderHeaderIDs\"')");

                modelBuilder.Entity<SalesOrderDetail>()
 .Property(o => o.ID)
 .HasDefaultValueSql("nextval('\"SalesOrderDetailIDs\"')");
            }
            



            modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("Address");
            });

            modelBuilder.Entity<BuildVersion>(entity =>
            {
                entity.HasNoKey();
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");
                  


            });

            modelBuilder.Entity<CustomerAddress>(entity =>
            {
                
                entity.ToTable("CustomerAddress");
                
            });

            modelBuilder.Entity<ErrorLog>(entity =>
            {
                
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");
               
                if (DbType == GraduDBType.Oracle)
                {
                    entity.Property(a => a.ThumbNailPhoto).HasColumnType("BLOB");
                  //  entity.Property(a => a.ID).HasColumnType("NUMBER(9,0)");
                  //  entity.Property(a => a.ProductModelID).HasColumnType("NUMBER(9,0)");
                  //  entity.Property(a => a.ProductCategoryID).HasColumnType("NUMBER(9,0)");
                }
                
            });

            modelBuilder.Entity<ProductCategory>(entity =>
            {
                entity.ToTable("ProductCategory");
                });

            modelBuilder.Entity<ProductDescription>(entity =>
            {
                entity.ToTable("ProductDescription");
                 });

            modelBuilder.Entity<ProductModel>(entity =>
            {
                entity.ToTable("ProductModel");
                
                if(DbType == GraduDBType.Oracle)
                {
                    entity.Property(a => a.CatalogDescription).HasColumnType("CLOB");
                }
                
            });

            modelBuilder.Entity<ProductModelProductDescription>(entity =>
            {
              
                entity.ToTable("ProductModelProductDescription");
                
            });

            modelBuilder.Entity<SalesOrderDetail>(entity =>
            {
              
                entity.ToTable("SalesOrderDetail");

            });

            modelBuilder.Entity<SalesOrderHeader>(entity =>
            {
            

                entity.ToTable("SalesOrderHeader");
               
                entity.HasOne(d => d.BillToAddress)
                    .WithMany(p => p.SalesOrderHeaderBillToAddress)
                    .HasForeignKey(d => d.BillToAddressID)
                    .HasConstraintName("FK_SalesOrderHeader_Address_BillTo_AddressID");
                
               
            });

            modelBuilder.Entity<vGetAllCategories>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vGetAllCategories");

                entity.Property(e => e.ParentProductCategoryName).HasMaxLength(50);

                entity.Property(e => e.ProductCategoryName).HasMaxLength(50);
            });

            modelBuilder.Entity<vProductAndDescription>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vProductAndDescription");

                entity.HasComment("Product names and descriptions. Product descriptions are provided in multiple languages.");

                entity.Property(e => e.Culture)
                    .HasMaxLength(6)
                    .IsFixedLength();

                entity.Property(e => e.Description).HasMaxLength(400);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.ProductModel).HasMaxLength(50);
            });

            modelBuilder.Entity<vProductModelCatalogDescription>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vProductModelCatalogDescription");

                entity.HasComment("Displays the content from each element in the xml column CatalogDescription for each product in the Sales.ProductModel table that has catalog data.");

                entity.Property(e => e.Color).HasMaxLength(256);

                entity.Property(e => e.Copyright).HasMaxLength(30);

                entity.Property(e => e.Crankset).HasMaxLength(256);

                entity.Property(e => e.MaintenanceDescription).HasMaxLength(256);

                entity.Property(e => e.Material).HasMaxLength(256);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.NoOfYears).HasMaxLength(256);

                entity.Property(e => e.Pedal).HasMaxLength(256);

                entity.Property(e => e.PictureAngle).HasMaxLength(256);

                entity.Property(e => e.PictureSize).HasMaxLength(256);

                entity.Property(e => e.ProductLine).HasMaxLength(256);

                entity.Property(e => e.ProductModelID).ValueGeneratedOnAdd();

                entity.Property(e => e.ProductPhotoID).HasMaxLength(256);

                entity.Property(e => e.ProductURL).HasMaxLength(256);

                entity.Property(e => e.RiderExperience).HasMaxLength(1024);

                entity.Property(e => e.Saddle).HasMaxLength(256);

                entity.Property(e => e.Style).HasMaxLength(256);

                entity.Property(e => e.WarrantyDescription).HasMaxLength(256);

                entity.Property(e => e.WarrantyPeriod).HasMaxLength(256);

                entity.Property(e => e.Wheel).HasMaxLength(256);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }

   
}
