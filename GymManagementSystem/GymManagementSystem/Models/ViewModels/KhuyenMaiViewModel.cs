using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GymManagementSystem.Models.ViewModels
{
    public class KhuyenMaiViewModel
    {
        public KhuyenMai KhuyenMai { get; set; }
        public List<int> SelectedGoiTapIds { get; set; }
        public IEnumerable<SelectListItem> DanhSachGoiTap { get; set; }
        public string ApplyType { get; set; }

    }
}