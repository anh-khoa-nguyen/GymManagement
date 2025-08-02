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
        private readonly ThietBiBUS thietBiBUS = new ThietBiBUS();
        private void LoadInitialThietBiData()
        {
            LoadThietBiDataGrid();
            LoadPhongDataToComboBox(cmbPhongTapThietBi);
            LoadTinhTrangToComboBox();
        }

        private void LoadPhongDataToComboBox(ComboBox cmb)
        {
            cmb.DataSource = this.phongBUS.GetAll();
            cmb.DisplayMember = "Ten";
            cmb.ValueMember = "ID";
        }

        private void LoadThietBiDataGrid()
        {
            dgvThietBi.DataSource = thietBiBUS.GetAll();
            dgvThietBi.Columns["ID"].HeaderText = "Mã TB";
            dgvThietBi.Columns["Ten"].HeaderText = "Tên Thiết Bị";
            dgvThietBi.Columns["TinhTrangHienThi"].HeaderText = "Tình Trạng";
            dgvThietBi.Columns["TenPhong"].HeaderText = "Phòng Tập";
            dgvThietBi.Columns["ID_Phong"].Visible = false;
            dgvThietBi.Columns["Tinh_Trang"].Visible = false;

        }

        private void LoadTinhTrangToComboBox()
        {
            cmbTinhTrang.DataSource = thietBiBUS.GetTinhTrangDataSource();
            cmbTinhTrang.DisplayMember = "Description";
            cmbTinhTrang.ValueMember = "Value";
        }

        private ThietBiDTO GetSelectedThietBiFromGrid()
        {
            if (dgvThietBi.CurrentRow != null && dgvThietBi.CurrentRow.DataBoundItem is ThietBiDTO selectedDto)
            {
                return selectedDto;
            }
            return null;
        }

        private void DisplayThietBiData(ThietBiDTO thietBi)
        {
            txtIDThietBi.Text = thietBi.ID.ToString();
            txtTenThietBi.Text = thietBi.Ten;
            cmbTinhTrang.SelectedValue = thietBi.Tinh_Trang;
            cmbPhongTapThietBi.SelectedValue = thietBi.ID_Phong ?? -1;
        }

        private void HandleAddThietBi(string ten, TinhTrangThietBi tinhTrang, int? idPhong)
        {
            var dto = new ThietBiDTO
            {
                Ten = ten,
                Tinh_Trang = tinhTrang,
                ID_Phong = idPhong
            };

            ResultDTO result = this.thietBiBUS.Add(dto);
            if (result.IsSuccess)
            {
                MessageBox.Show("Thêm thiết bị thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(result.ErrorMessage, "Thêm thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void HandleUpdateThietBi(int id, string ten, TinhTrangThietBi tinhTrang, int? idPhong)
        {
            var dto = new ThietBiDTO
            {
                ID = id,
                Ten = ten,
                Tinh_Trang = tinhTrang,
                ID_Phong = idPhong
            };

            ResultDTO result = this.thietBiBUS.Update(dto);
            if (result.IsSuccess)
            {
                MessageBox.Show("Cập nhật thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(result.ErrorMessage, "Cập nhật thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void HandleDeleteThietBi(int id)
        {
            var confirmResult = MessageBox.Show("Bạn có chắc chắn muốn xóa thiết bị này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirmResult == DialogResult.Yes)
            {
                ResultDTO result = this.thietBiBUS.Delete(id);
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
    }
}