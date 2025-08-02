using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO_Manage
{
    public class ThietBiDTO
    {
        public int ID { get; set; }
        public string Ten { get; set; }
        public TinhTrangThietBi Tinh_Trang { get; set; }
        public string TinhTrangHienThi { get; set; }

        public int? ID_Phong { get; set; }
        public string TenPhong { get; set; }
    }
}
