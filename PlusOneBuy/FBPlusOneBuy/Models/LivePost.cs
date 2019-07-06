namespace FBPlusOneBuy.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class LivePost
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LivePost()
        {
            Orders = new HashSet<Order>();
        }

        [Key]
        [StringLength(50)]
        public string LiveID { get; set; }

        public int FanPageID { get; set; }

        [StringLength(50)]
        public string LiveName { get; set; }

        public virtual FanPage FanPage { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }
    }
}