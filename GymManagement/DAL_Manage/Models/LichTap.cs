namespace DAL_Manage.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LichTap")]
    public partial class LichTap
    {
        public int ID { get; set; }

        public int? ID_HoiVien { get; set; }

        public int? ID_Nhan_Vien { get; set; }

        public DateTime? Ngay_Tap { get; set; }

        [StringLength(100)]
        public string Trang_Thai { get; set; }

        public TimeSpan? Gio_Tap { get; set; }

        public virtual HoiVien HoiVien { get; set; }

        public virtual NhanVien NhanVien { get; set; }
    }
}
