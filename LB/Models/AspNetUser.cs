//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LB.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class AspNetUser
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public Nullable<bool> EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public Nullable<bool> PhoneNumberConfirmed { get; set; }
        public Nullable<bool> TwoFactorEnabled { get; set; }
        public Nullable<System.DateTime> LockoutEndDateUtc { get; set; }
        public Nullable<bool> LockoutEnabled { get; set; }
        public Nullable<int> AccessFailedCount { get; set; }
        public string UserName { get; set; }
        public string MaTruong { get; set; }
        public string UserGroup { get; set; }
        public string HoTen { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string UType { get; set; }
        public string AddressInfo { get; set; }
        public string Sex { get; set; }
    }
}
