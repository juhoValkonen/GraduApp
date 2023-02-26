
using System;
using System.Collections.Generic;

namespace GraduApp.DataAccess.GraduModels
{
    /// <summary>
    /// Cross-reference table mapping product descriptions and the language the description is written in.
    /// </summary>
    public partial class ProductModelProductDescription 
    {
        /// <summary>
        /// Primary key. Foreign key to ProductModel.ProductModelID.
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Primary key. Foreign key to ProductDescription.ProductDescriptionID.
        /// </summary>
        public int ProductDescriptionID { get; set; }
        /// <summary>
        /// The culture for which the description is written
        /// </summary>
        /// 
        public int ProductModelID { get; set; }
        /// <summary>
        /// The culture for which the description is written
        /// </summary>
        public string Culture { get; set; } = null!;
        public string rowguid { get; set; }
        /// <summary>
        /// Date and time the record was last updated.
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        public virtual ProductDescription ProductDescription { get; set; } = null!;
        public virtual ProductModel ProductModel { get; set; } = null!;
    }
}
