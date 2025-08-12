using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymManagementSystem.Models { 
    public class Phong
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100), Display(Name = "Tên Phòng")]
        public string TenPhong { get; set; }

        [StringLength(50), Display(Name = "Mã Phòng")]
        public string MaPhong { get; set; } // Ví dụ: "GYM-01", "SAUNA-M"

        public virtual ICollection<ThietBi> ThietBis { get; set; }
    }
}