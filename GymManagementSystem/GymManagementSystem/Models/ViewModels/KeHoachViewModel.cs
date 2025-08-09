using System.Collections.Generic;
using System.Web.Mvc;

namespace GymManagementSystem.Models.ViewModels
{
    //For Admin
    public class KeHoachViewModel
    {
        public KeHoach KeHoach { get; set; }

        public IEnumerable<SelectListItem> DanhSachBaiTap { get; set; }
        public IEnumerable<SelectListItem> DanhSachKhuyenMai { get; set; }

        public KeHoachViewModel()
        {
            KeHoach = new KeHoach();
            KeHoach.ChiTietKeHoachs = new List<ChiTietKeHoach>();
        }
    }
}