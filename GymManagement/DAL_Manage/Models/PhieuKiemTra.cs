namespace DAL_Manage.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PhieuKiemTra")]
    public partial class PhieuKiemTra
    {
        public int ID { get; set; }

        public int? ID_Nhan_Vien { get; set; }

        public int? ID_Thiet_Bi { get; set; }

        public DateTime? Ngay_Kiem_Tra { get; set; }

        [Column(TypeName = "ntext")]
        public string Mo_Ta { get; set; }

        public virtual NhanVien NhanVien { get; set; }

        public virtual ThietBi ThietBi { get; set; }
    }
}
