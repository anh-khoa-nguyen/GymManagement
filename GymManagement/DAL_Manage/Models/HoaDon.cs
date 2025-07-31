namespace DAL_Manage.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HoaDon")]
    public partial class HoaDon
    {
        public int ID { get; set; }

        public DateTime Ngay_Tao { get; set; }

        [StringLength(100)]
        public string Tinh_Trang { get; set; }

        [StringLength(100)]
        public string Phuong_Thuc_Thanh_Toan { get; set; }

        public int? ID_Nhan_Vien { get; set; }

        public int? ID_HoiVien { get; set; }

        public int? ID_GoiTap { get; set; }

        public virtual GoiTap GoiTap { get; set; }

        public virtual HoiVien HoiVien { get; set; }

        public virtual NhanVien NhanVien { get; set; }
    }
}
