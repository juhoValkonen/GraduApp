﻿
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraduApp.DataAccess.GraduModels
{
    /// <summary>
    /// Customer information.
    /// </summary>
    public partial class Customer 
    {
        public Customer()
        {
            CustomerAddress = new HashSet<CustomerAddress>();
            SalesOrderHeader = new HashSet<SalesOrderHeader>();
        }

        /// <summary>
        /// Primary key for Customer records.
        /// </summary>
        
        public int ID { get; set; }
        /// <summary>
        /// 0 = The data in FirstName and LastName are stored in western style (first name, last name) order.  1 = Eastern style (last name, first name) order.
        /// </summary>
        public bool NameStyle { get; set; }
        /// <summary>
        /// A courtesy title. For example, Mr. or Ms.
        /// </summary>
        public string? Title { get; set; }
        /// <summary>
        /// First name of the person.
        /// </summary>
        public string FirstName { get; set; } = null!;
        /// <summary>
        /// Middle name or middle initial of the person.
        /// </summary>
        public string? MiddleName { get; set; }
        /// <summary>
        /// Last name of the person.
        /// </summary>
        public string LastName { get; set; } = null!;
        /// <summary>
        /// Surname suffix. For example, Sr. or Jr.
        /// </summary>
        public string? Suffix { get; set; }
        /// <summary>
        /// The customer&apos;s organization.
        /// </summary>
        public string? CompanyName { get; set; }
        /// <summary>
        /// The customer&apos;s sales person, an employee of AdventureWorks Cycles.
        /// </summary>
        public string? SalesPerson { get; set; }
        /// <summary>
        /// E-mail address for the person.
        /// </summary>
        public string? EmailAddress { get; set; }
        /// <summary>
        /// Phone number associated with the person.
        /// </summary>
        public string? Phone { get; set; }
        /// <summary>
        /// Password for the e-mail account.
        /// </summary>
        public string PasswordHash { get; set; } = null!;
        /// <summary>
        /// Random value concatenated with the password string before the password is hashed.
        /// </summary>
        public string PasswordSalt { get; set; } = null!;
        /// <summary>
        /// ROWGUIDCOL number uniquely identifying the record. Used to support a merge replication sample.
        /// </summary>
        public string rowguid { get; set; }
        /// <summary>
        /// Date and time the record was last updated.
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        public virtual ICollection<CustomerAddress> CustomerAddress { get; set; }
        public virtual ICollection<SalesOrderHeader> SalesOrderHeader { get; set; }
    }
}
