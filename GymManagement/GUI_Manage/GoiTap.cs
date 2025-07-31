using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BUS_Manage; // Để dùng lớp BUS
using DTO_Manage; // Để dùng lớp DTO

namespace GUI_Manage
{
    public partial class GoiTap : Form
    {
        private LoaiGoiTapBUS loaiGoiTapBUS = new LoaiGoiTapBUS();

        public GoiTap()
        {
            InitializeComponent();
        }

        private void LoadData()
        {
            // Gọi BUS để lấy danh sách DTO
            dgvLoaiGoiTap.DataSource = loaiGoiTapBUS.GetAll();
        }

        private void GoiTap_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            // 1. Tạo đối tượng DTO từ dữ liệu trên form
            LoaiGoiTapDTO newLoai = new LoaiGoiTapDTO
            {
                Ten = txtTenLoai.Text
            };

            // 2. Gọi BUS để thực hiện nghiệp vụ
            loaiGoiTapBUS.Add(newLoai);

            // 3. Thông báo và tải lại dữ liệu
            MessageBox.Show("Thêm thành công!");
            LoadData();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            // 1. Tạo DTO
            LoaiGoiTapDTO updatedLoai = new LoaiGoiTapDTO
            {
                ID = int.Parse(txtID.Text),
                Ten = txtTenLoai.Text
            };

            // 2. Gọi BUS
            loaiGoiTapBUS.Update(updatedLoai);

            // 3. Thông báo và tải lại
            MessageBox.Show("Sửa thành công!");
            LoadData();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            // 1. Lấy ID
            int id = int.Parse(txtID.Text);

            // 2. Gọi BUS
            loaiGoiTapBUS.Delete(id);

            // 3. Thông báo và tải lại
            MessageBox.Show("Xóa thành công!");
            LoadData();
        }
    }
}
