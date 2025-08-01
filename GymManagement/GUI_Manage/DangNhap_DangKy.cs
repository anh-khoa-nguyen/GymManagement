using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BUS_Manage;
using DTO_Manage;
using MaterialSkin;
using MaterialSkin.Controls;

namespace GUI_Manage
{
    public partial class DangNhap_DangKy : MaterialForm
    {
        private readonly MaterialSkinManager materialSkinManager;

        private HoiVienBUS hoiVienBUS = new HoiVienBUS();
        public HoiVienDTO LoggedInUser { get; private set; }

        public DangNhap_DangKy()
        {
            InitializeComponent();
            // Thiết lập MaterialSkin cho form
            //materialSkinManager = MaterialSkinManager.Instance;
            //materialSkinManager.AddFormToManage(this);
            ////materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            ////materialSkinManager.ColorScheme = new ColorScheme(
            ////    Primary.Blue400, Primary.Blue500,
            ////    Primary.Blue500, Accent.LightBlue200,
            ////    TextShade.WHITE);
        }

        #region Xử lý sự kiện (UI Events)
        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            HandleLogin();
        }

        private void btnDangKy_Click(object sender, EventArgs e)
        {
            HandleRegistration();

        }

        #endregion

      
    }
}
