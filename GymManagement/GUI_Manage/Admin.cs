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
using MaterialSkin;
using MaterialSkin.Controls;
using DTO_Manage;

namespace GUI_Manage
{
    public partial class Admin : MaterialForm
    {
        public Admin()
        {
            InitializeComponent();
        }

        private void Admin_Load(object sender, EventArgs e)
        {
            LoadDataIntoGrid(dgvPhong);

        }

        //Phòng tập
        private void btnThemPhong_Click(object sender, EventArgs e)
        {
            string tenPhong = txtTenPhong.Text;
            string diaChi = txtDiaChiPhong.Text;

            // Gọi hàm logic và truyền dữ liệu vào
            HandleAddPhong(tenPhong, diaChi);

            // Sau khi xử lý, tải lại dữ liệu
            LoadDataIntoGrid(dgvPhong);
        }

        private void btnSuaPhong_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtIDPhong.Text, out int id))
            {
                MessageBox.Show("Mã phòng không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string tenPhong = txtTenPhong.Text;
            string diaChi = txtDiaChiPhong.Text;

            // Gọi hàm logic để cập nhật
            HandleUpdatePhong(id, tenPhong, diaChi);

            // Tải lại dữ liệu
            LoadDataIntoGrid(dgvPhong);
        }

        private void btnXoaPhong_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtIDPhong.Text, out int id))
            {
                MessageBox.Show("Mã phòng không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Gọi hàm logic để xóa
            HandleDeletePhong(id);

            // Tải lại dữ liệu
            LoadDataIntoGrid(dgvPhong);
        }

        private void dgvPhong_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Lấy đối tượng PhongDTO từ dòng được chọn
            PhongDTO selectedPhong = GetSelectedPhongFromGrid(dgvPhong);

            // Hiển thị dữ liệu lên các control
            if (selectedPhong != null)
            {
                DisplayPhongData(selectedPhong);
            }

        }
    }
}
