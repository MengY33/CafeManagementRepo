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
    
    public partial class Menu
    {
        public string MenuID { get; set; }
        public string MenuName { get; set; }
        public string MenuDesc { get; set; }
        public string MenuCategory { get; set; }
        public string MenuStatus { get; set; }
        public Nullable<decimal> MenuPrice { get; set; }
        public Nullable<decimal> DiscountPrice { get; set; }
    }
}
