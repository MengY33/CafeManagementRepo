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
    
    public partial class Reservation
    {
        public string ReservationID { get; set; }
        public string MemberID { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<System.DateTime> Time { get; set; }
        public Nullable<int> NumberofAdult { get; set; }
        public Nullable<int> NumberofChild { get; set; }
    }
}
