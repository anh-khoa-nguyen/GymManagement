using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_Manage.Models
{
    public enum TinhTrangThietBi
    {
        [Description("Tốt")]
        Tot = 0,

        [Description("Đang bảo trì")]
        DangBaoTri = 1,

        [Description("Hỏng")]
        Hong = 2
    }
}
