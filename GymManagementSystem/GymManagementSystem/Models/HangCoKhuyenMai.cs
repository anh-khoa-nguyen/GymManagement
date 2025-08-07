using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GymManagementSystem.Models
{
    public class HangCoKhuyenMai
    {
        [Key, Column(Order = 0)]
        public int HangHoiVienId { get; set; }

        [Key, Column(Order = 1)]
        public int KhuyenMaiId { get; set; }

        [ForeignKey("HangHoiVienId")]
        public virtual HangHoiVien HangHoiVien { get; set; }

        [ForeignKey("KhuyenMaiId")]
        public virtual KhuyenMai KhuyenMai { get; set; }
    }
}