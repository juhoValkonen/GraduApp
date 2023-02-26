
using System;
using System.Collections.Generic;

namespace GraduApp.DataAccess.GraduModels
{
    public partial class ProductModel 
    {
        public ProductModel()
        {
            Product = new HashSet<Product>();
            ProductModelProductDescription = new HashSet<ProductModelProductDescription>();
        }

        public int ID { get; set; }
        public string Name { get; set; } = null!;
        public string? CatalogDescription { get; set; }
        public string rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual ICollection<Product> Product { get; set; }
        public virtual ICollection<ProductModelProductDescription> ProductModelProductDescription { get; set; }
    }
}
