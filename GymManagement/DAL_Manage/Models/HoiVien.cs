namespace DAL_Manage.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HoiVien")]
    public partial class HoiVien
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HoiVien()
        {
            DangKy = new HashSet<DangKy>();
            HoaDon = new HashSet<HoaDon>();
            LichTap = new HashSet<LichTap>();
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

        [StringLength(255)]
        public string Email { get; set; }

        [StringLength(20)]
        [Index(IsUnique = true)]
        public string SDT { get; set; }

        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DangKy> DangKy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HoaDon> HoaDon { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LichTap> LichTap { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TienDoTapLuyen> TienDoTapLuyen { get; set; }
    }
}
