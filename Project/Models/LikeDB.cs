//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Project.Models
{
    using System;
    using System.Collections.Generic;

    public partial class LikeDB
    {
        public int AId { get; set; }
        public Nullable<int> PaperId { get; set; }
        public Nullable<int> AuthorId { get; set; }
        public Nullable<System.DateTime> ActionDate { get; set; }
        public Nullable<bool> LikeP { get; set; }

        public virtual Author Author { get; set; }
        public virtual Paper Paper { get; set; }
    }
}