
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraduApp.DataAccess.GraduModels
{
    /// <summary>
    /// Street address information for customers.
    /// </summary>
    public partial class Address 
    {
        public Address()
        {
            CustomerAddress = new HashSet<CustomerAddress>();
            SalesOrderHeaderBillToAddress = new HashSet<SalesOrderHeader>();
            SalesOrderHeaderShipToAddress = new HashSet<SalesOrderHeader>();
        }

        /// <summary>
        /// Primary key for Address records.
        /// </summary>
        
        public int ID { get; set; }
        /// <summary>
        /// First street address line.
        /// </summary>
        public string AddressLine1 { get; set; } = null!;
        /// <summary>
        /// Second street address line.
        /// </summary>
        public string? AddressLine2 { get; set; }
        /// <summary>
        /// Name of the city.
        /// </summary>
        public string City { get; set; } = null!;
        /// <summary>
        /// Name of state or province.
        /// </summary>
        public string StateProvince { get; set; } = null!;
        public string CountryRegion { get; set; } = null!;
        /// <summary>
        /// Postal code for the street address.
        /// </summary>
        public string PostalCode { get; set; } = null!;
        /// <summary>
        /// ROWGUIDCOL number uniquely identifying the record. Used to support a merge replication sample.
        /// </summary>
        public string rowguid { get; set; }
        /// <summary>
        /// Date and time the record was last updated.
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        public virtual ICollection<CustomerAddress> CustomerAddress { get; set; }
        public virtual ICollection<SalesOrderHeader> SalesOrderHeaderBillToAddress { get; set; }
        public virtual ICollection<SalesOrderHeader> SalesOrderHeaderShipToAddress { get; set; }
    }
}
