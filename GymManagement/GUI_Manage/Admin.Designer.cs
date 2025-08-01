namespace GUI_Manage
{
    partial class Admin
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.materialTabControl1 = new MaterialSkin.Controls.MaterialTabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label5 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btnXoaPhong = new MaterialSkin.Controls.MaterialButton();
            this.btnSuaPhong = new MaterialSkin.Controls.MaterialButton();
            this.btnThemPhong = new MaterialSkin.Controls.MaterialButton();
            this.dgvPhong = new System.Windows.Forms.DataGridView();
            this.txtDiaChiPhong = new MaterialSkin.Controls.MaterialTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtIDPhong = new MaterialSkin.Controls.MaterialTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtTenPhong = new MaterialSkin.Controls.MaterialTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.materialTabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPhong)).BeginInit();
            this.SuspendLayout();
            // 
            // materialTabControl1
            // 
            this.materialTabControl1.Controls.Add(this.tabPage1);
            this.materialTabControl1.Controls.Add(this.tabPage2);
            this.materialTabControl1.Controls.Add(this.tabPage3);
            this.materialTabControl1.Controls.Add(this.tabPage4);
            this.materialTabControl1.Controls.Add(this.tabPage5);
            this.materialTabControl1.Controls.Add(this.tabPage6);
            this.materialTabControl1.Controls.Add(this.tabPage7);
            this.materialTabControl1.Depth = 0;
            this.materialTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.materialTabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.83186F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.materialTabControl1.Location = new System.Drawing.Point(3, 64);
            this.materialTabControl1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialTabControl1.Multiline = true;
            this.materialTabControl1.Name = "materialTabControl1";
            this.materialTabControl1.SelectedIndex = 0;
            this.materialTabControl1.Size = new System.Drawing.Size(859, 580);
            this.materialTabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(815, 561);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 17.84071F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(34, 38);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(162, 33);
            this.label5.TabIndex = 1;
            this.label5.Text = "Tổng quan";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btnXoaPhong);
            this.tabPage2.Controls.Add(this.btnSuaPhong);
            this.tabPage2.Controls.Add(this.btnThemPhong);
            this.tabPage2.Controls.Add(this.dgvPhong);
            this.tabPage2.Controls.Add(this.txtDiaChiPhong);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.txtIDPhong);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.txtTenPhong);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Location = new System.Drawing.Point(4, 29);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(851, 547);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Chi nhánh";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btnXoaPhong
            // 
            this.btnXoaPhong.AutoSize = false;
            this.btnXoaPhong.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnXoaPhong.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.btnXoaPhong.Depth = 0;
            this.btnXoaPhong.HighEmphasis = true;
            this.btnXoaPhong.Icon = null;
            this.btnXoaPhong.Location = new System.Drawing.Point(588, 341);
            this.btnXoaPhong.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnXoaPhong.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnXoaPhong.Name = "btnXoaPhong";
            this.btnXoaPhong.NoAccentTextColor = System.Drawing.Color.Empty;
            this.btnXoaPhong.Size = new System.Drawing.Size(195, 52);
            this.btnXoaPhong.TabIndex = 4;
            this.btnXoaPhong.Text = "xóa";
            this.btnXoaPhong.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btnXoaPhong.UseAccentColor = false;
            this.btnXoaPhong.UseVisualStyleBackColor = true;
            this.btnXoaPhong.Click += new System.EventHandler(this.btnXoaPhong_Click);
            // 
            // btnSuaPhong
            // 
            this.btnSuaPhong.AutoSize = false;
            this.btnSuaPhong.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnSuaPhong.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.btnSuaPhong.Depth = 0;
            this.btnSuaPhong.HighEmphasis = true;
            this.btnSuaPhong.Icon = null;
            this.btnSuaPhong.Location = new System.Drawing.Point(588, 264);
            this.btnSuaPhong.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnSuaPhong.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnSuaPhong.Name = "btnSuaPhong";
            this.btnSuaPhong.NoAccentTextColor = System.Drawing.Color.Empty;
            this.btnSuaPhong.Size = new System.Drawing.Size(195, 52);
            this.btnSuaPhong.TabIndex = 4;
            this.btnSuaPhong.Text = "SỬA";
            this.btnSuaPhong.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btnSuaPhong.UseAccentColor = false;
            this.btnSuaPhong.UseVisualStyleBackColor = true;
            this.btnSuaPhong.Click += new System.EventHandler(this.btnSuaPhong_Click);
            // 
            // btnThemPhong
            // 
            this.btnThemPhong.AutoSize = false;
            this.btnThemPhong.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnThemPhong.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.btnThemPhong.Depth = 0;
            this.btnThemPhong.HighEmphasis = true;
            this.btnThemPhong.Icon = null;
            this.btnThemPhong.Location = new System.Drawing.Point(588, 187);
            this.btnThemPhong.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnThemPhong.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnThemPhong.Name = "btnThemPhong";
            this.btnThemPhong.NoAccentTextColor = System.Drawing.Color.Empty;
            this.btnThemPhong.Size = new System.Drawing.Size(195, 52);
            this.btnThemPhong.TabIndex = 4;
            this.btnThemPhong.Text = "Thêm";
            this.btnThemPhong.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btnThemPhong.UseAccentColor = false;
            this.btnThemPhong.UseVisualStyleBackColor = true;
            this.btnThemPhong.Click += new System.EventHandler(this.btnThemPhong_Click);
            // 
            // dgvPhong
            // 
            this.dgvPhong.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPhong.Location = new System.Drawing.Point(32, 187);
            this.dgvPhong.Name = "dgvPhong";
            this.dgvPhong.RowHeadersWidth = 48;
            this.dgvPhong.RowTemplate.Height = 24;
            this.dgvPhong.Size = new System.Drawing.Size(527, 324);
            this.dgvPhong.TabIndex = 3;
            this.dgvPhong.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPhong_CellClick);
            // 
            // txtDiaChiPhong
            // 
            this.txtDiaChiPhong.AnimateReadOnly = false;
            this.txtDiaChiPhong.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDiaChiPhong.Depth = 0;
            this.txtDiaChiPhong.Font = new System.Drawing.Font("Roboto", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtDiaChiPhong.LeadingIcon = null;
            this.txtDiaChiPhong.Location = new System.Drawing.Point(359, 101);
            this.txtDiaChiPhong.MaxLength = 50;
            this.txtDiaChiPhong.MouseState = MaterialSkin.MouseState.OUT;
            this.txtDiaChiPhong.Multiline = false;
            this.txtDiaChiPhong.Name = "txtDiaChiPhong";
            this.txtDiaChiPhong.Size = new System.Drawing.Size(398, 50);
            this.txtDiaChiPhong.TabIndex = 2;
            this.txtDiaChiPhong.Text = "";
            this.txtDiaChiPhong.TrailingIcon = null;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.0177F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(197, 110);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 29);
            this.label3.TabIndex = 1;
            this.label3.Text = "Địa chỉ:";
            // 
            // txtIDPhong
            // 
            this.txtIDPhong.AnimateReadOnly = false;
            this.txtIDPhong.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtIDPhong.Depth = 0;
            this.txtIDPhong.Font = new System.Drawing.Font("Roboto", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtIDPhong.LeadingIcon = null;
            this.txtIDPhong.Location = new System.Drawing.Point(670, 26);
            this.txtIDPhong.MaxLength = 50;
            this.txtIDPhong.MouseState = MaterialSkin.MouseState.OUT;
            this.txtIDPhong.Multiline = false;
            this.txtIDPhong.Name = "txtIDPhong";
            this.txtIDPhong.Size = new System.Drawing.Size(87, 50);
            this.txtIDPhong.TabIndex = 2;
            this.txtIDPhong.Text = "";
            this.txtIDPhong.TrailingIcon = null;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.0177F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(609, 39);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(42, 29);
            this.label4.TabIndex = 1;
            this.label4.Text = "ID:";
            // 
            // txtTenPhong
            // 
            this.txtTenPhong.AnimateReadOnly = false;
            this.txtTenPhong.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtTenPhong.Depth = 0;
            this.txtTenPhong.Font = new System.Drawing.Font("Roboto", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtTenPhong.LeadingIcon = null;
            this.txtTenPhong.Location = new System.Drawing.Point(359, 26);
            this.txtTenPhong.MaxLength = 50;
            this.txtTenPhong.MouseState = MaterialSkin.MouseState.OUT;
            this.txtTenPhong.Multiline = false;
            this.txtTenPhong.Name = "txtTenPhong";
            this.txtTenPhong.Size = new System.Drawing.Size(200, 50);
            this.txtTenPhong.TabIndex = 2;
            this.txtTenPhong.Text = "";
            this.txtTenPhong.TrailingIcon = null;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.0177F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(197, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(175, 29);
            this.label2.TabIndex = 1;
            this.label2.Text = "Tên phòng tập:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 17.84071F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(26, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(156, 33);
            this.label1.TabIndex = 0;
            this.label1.Text = "Chi nhánh";
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(4, 29);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(815, 561);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "tabPage3";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Location = new System.Drawing.Point(4, 29);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(815, 561);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "tabPage4";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tabPage5
            // 
            this.tabPage5.Location = new System.Drawing.Point(4, 29);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(815, 561);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "tabPage5";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // tabPage6
            // 
            this.tabPage6.Location = new System.Drawing.Point(4, 29);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(815, 561);
            this.tabPage6.TabIndex = 5;
            this.tabPage6.Text = "tabPage6";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // tabPage7
            // 
            this.tabPage7.Location = new System.Drawing.Point(4, 29);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage7.Size = new System.Drawing.Size(815, 561);
            this.tabPage7.TabIndex = 6;
            this.tabPage7.Text = "tabPage7";
            this.tabPage7.UseVisualStyleBackColor = true;
            // 
            // Admin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(865, 647);
            this.Controls.Add(this.materialTabControl1);
            this.DrawerShowIconsWhenHidden = true;
            this.DrawerTabControl = this.materialTabControl1;
            this.Name = "Admin";
            this.Text = "Admin";
            this.Load += new System.EventHandler(this.Admin_Load);
            this.materialTabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPhong)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private MaterialSkin.Controls.MaterialTabControl materialTabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.TabPage tabPage7;
        private System.Windows.Forms.DataGridView dgvPhong;
        private MaterialSkin.Controls.MaterialTextBox txtDiaChiPhong;
        private System.Windows.Forms.Label label3;
        private MaterialSkin.Controls.MaterialTextBox txtIDPhong;
        private System.Windows.Forms.Label label4;
        private MaterialSkin.Controls.MaterialTextBox txtTenPhong;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private MaterialSkin.Controls.MaterialButton btnXoaPhong;
        private MaterialSkin.Controls.MaterialButton btnSuaPhong;
        private MaterialSkin.Controls.MaterialButton btnThemPhong;
        private System.Windows.Forms.Label label5;
    }
}