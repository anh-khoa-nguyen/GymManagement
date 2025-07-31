namespace DAL_Manage.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NhanVien")]
    public partial class NhanVien
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NhanVien()
        {
            HoaDon = new HashSet<HoaDon>();
            LichTap = new HashSet<LichTap>();
            PhieuKiemTra = new HashSet<PhieuKiemTra>();
            TienDoTapLuyen = new HashSet<TienDoTapLuyen>();
        }

        public int ID { get; set; }

        [Required]
        [StringLength(255)]
        public string Ten { get; set; }

        [StringLength(10)]
        public string Gioi_Tinh { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Ngay_Sinh { get; set; }

        [StringLength(255)]
        public string Dia_Chi { get; set; }

        [StringLength(20)]
        public string SDT { get; set; }

        [StringLength(255)]
        public string Email { get; set; }

        [StringLength(100)]
        public string Vai_Tro { get; set; }

        public int? ID_Phong { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HoaDon> HoaDon { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LichTap> LichTap { get; set; }

        public virtual Phong Phong { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PhieuKiemTra> PhieuKiemTra { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TienDoTapLuyen> TienDoTapLuyen { get; set; }
    }
}
