namespace DAL_Manage.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DangKy")]
    public partial class DangKy
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_HoiVien { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_Goi_Tap { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Ngay_Bat_Dau { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Ngay_Ket_Thuc { get; set; }

        [StringLength(100)]
        public string TrangThai { get; set; }

        public virtual GoiTap GoiTap { get; set; }

        public virtual HoiVien HoiVien { get; set; }
    }
}
