//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CafeManagementWeb.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class EmployeeOrder
    {
        public string PurchaseOrderID { get; set; }
        public string EmployeeID { get; set; }
        public string IngredientID { get; set; }
        public Nullable<int> PurchaseQuantity { get; set; }
    }
}
