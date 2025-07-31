namespace DAL_Manage.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TienDoTapLuyen")]
    public partial class TienDoTapLuyen
    {
        public int ID { get; set; }

        public int? ID_HoiVien { get; set; }

        public int? ID_Nhan_Vien { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Ngay_Ghi_Nhan { get; set; }

        public decimal? Can_Nang { get; set; }

        public decimal? Ti_Le_Mo { get; set; }

        public virtual HoiVien HoiVien { get; set; }

        public virtual NhanVien NhanVien { get; set; }
    }
}
