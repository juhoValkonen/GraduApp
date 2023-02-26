using System;
using System.Collections.Generic;

namespace GraduApp.DataAccess.GraduModels
{
    public partial class vGetAllCategories
    {
        public string ParentProductCategoryName { get; set; } = null!;
        public string? ProductCategoryName { get; set; }
        public int? ProductCategoryID { get; set; }
    }
}
