
using System;
using System.Collections.Generic;

namespace GraduApp.DataAccess.GraduModels
{
    /// <summary>
    /// Individual products associated with a specific sales order. See SalesOrderHeader.
    /// </summary>
    public partial class SalesOrderDetail 
    {
        /// <summary>
        /// Primary key. Foreign key to SalesOrderHeader.SalesOrderID.
        /// </summary>
        public int ID { get; set; }
    
        /// <summary>
        /// Quantity ordered per product.
        /// </summary>
        public short OrderQty { get; set; }
        /// <summary>
        /// Product sold to customer. Foreign key to Product.ProductID.
        /// </summary>
        public int ProductID { get; set; }
        /// <summary>
        /// Selling price of a single product.
        /// </summary>
        public decimal UnitPrice { get; set; }
        /// <summary>
        /// Discount amount.
        /// </summary>
        public decimal UnitPriceDiscount { get; set; }
        /// <summary>
        /// Per product subtotal. Computed as UnitPrice * (1 - UnitPriceDiscount) * OrderQty.
        /// </summary>
        public decimal LineTotal { get; set; }
        /// <summary>
        /// ROWGUIDCOL number uniquely identifying the record. Used to support a merge replication sample.
        /// </summary>
        public string rowguid { get; set; }
        /// <summary>
        /// Date and time the record was last updated.
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        public virtual Product Product { get; set; } = null!;
        public virtual SalesOrderHeader SalesOrder { get; set; } = null!;
    }
}
