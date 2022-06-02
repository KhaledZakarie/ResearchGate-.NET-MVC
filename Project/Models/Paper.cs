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
    using System.ComponentModel;
    using System.Web;

    public partial class Paper
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Paper()
        {
            this.Participates = new HashSet<Participate>();
            this.LikeDBs = new HashSet<LikeDB>();
            this.CommentDBs = new HashSet<CommentDB>();
        }

        public int PaperId { get; set; }
        [DisplayName("Paper Name")]
        public string PaperName { get; set; }
        public Nullable<int> PublisherId { get; set; }
        [DisplayName("Publish Date")]
        public Nullable<System.DateTime> PublishDate { get; set; }
        [DisplayName("Upload Paper")]
        public string PaperFile { get; set; }
        public HttpPostedFileBase FilePost { get; set; }

        public virtual Author Author { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Participate> Participates { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LikeDB> LikeDBs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CommentDB> CommentDBs { get; set; }
    }
}
