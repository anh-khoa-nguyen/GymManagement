using System;
using System.Collections.Generic;
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
	public partial class Admin : MaterialForm
	{
        #region Business Logic
        private readonly PhongBUS phongBUS = new PhongBUS();

        private void LoadDataIntoGrid(DataGridView dgv)
        {
            dgv.DataSource = this.phongBUS.GetAll();
            // Bạn vẫn có thể cấu hình cột ở đây
            dgv.Columns["ID"].HeaderText = "Mã Phòng";
            dgv.Columns["Ten"].HeaderText = "Tên Phòng";
            dgv.Columns["Dia_Chi"].HeaderText = "Địa Chỉ";
        }

        private PhongDTO GetSelectedPhongFromGrid(DataGridView dgv)
        {
            if (dgv.CurrentRow != null && dgv.CurrentRow.DataBoundItem is PhongDTO selectedDto)
            {
                return selectedDto;
            }
            return null;
        }
        private void DisplayPhongData(PhongDTO phong)
        {
            txtIDPhong.Text = phong.ID.ToString();
            txtTenPhong.Text = phong.Ten;
            txtDiaChiPhong.Text = phong.Dia_Chi;
        }

        private void HandleAddPhong(string tenPhong, string diaChi)
        {
            var dto = new PhongDTO { Ten = tenPhong, Dia_Chi = diaChi };

            ResultDTO result = this.phongBUS.Add(dto);
            if (result.IsSuccess)
            {
                MessageBox.Show("Thêm phòng thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(result.ErrorMessage, "Thêm thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void HandleUpdatePhong(int id, string tenPhong, string diaChi)
        {
            var dto = new PhongDTO { ID = id, Ten = tenPhong, Dia_Chi = diaChi };

            ResultDTO result = this.phongBUS.Update(dto);
            if (result.IsSuccess)
            {
                MessageBox.Show("Cập nhật thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(result.ErrorMessage, "Cập nhật thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void HandleDeletePhong(int id)
        {
            var confirmResult = MessageBox.Show("Bạn có chắc chắn muốn xóa phòng này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirmResult == DialogResult.Yes)
            {
                ResultDTO result = this.phongBUS.Delete(id);
                if (result.IsSuccess)
                {
                    MessageBox.Show("Xóa thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(result.ErrorMessage, "Xóa thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        #endregion

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // Admin
            // 
            this.ClientSize = new System.Drawing.Size(300, 300);
            this.Name = "Admin";
            this.ResumeLayout(false);

        }
    }
}