namespace DAL_Manage.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GoiTap")]
    public partial class GoiTap
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GoiTap()
        {
            DangKy = new HashSet<DangKy>();
            HoaDon = new HashSet<HoaDon>();
        }

        public int ID { get; set; }

        [Required]
        [StringLength(255)]
        public string Ten { get; set; }

        public decimal Gia { get; set; }

        public int? So_Buoi_Tap_PT { get; set; }

        public int? ID_Loai_Goi_Tap { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DangKy> DangKy { get; set; }

        public virtual LoaiGoiTap LoaiGoiTap { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HoaDon> HoaDon { get; set; }
    }
}
