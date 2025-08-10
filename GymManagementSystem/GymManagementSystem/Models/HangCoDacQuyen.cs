using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GymManagementSystem.Models
{
    public class HangCoDacQuyen
    {
        [Key, Column(Order = 0)]
        public int HangHoiVienId { get; set; }

        [Key, Column(Order = 1)]
        public int DacQuyenId { get; set; }

        [ForeignKey("HangHoiVienId")]
        public virtual HangHoiVien HangHoiVien { get; set; }

        [ForeignKey("DacQuyenId")]
        public virtual DacQuyen DacQuyen { get; set; }
    }
}