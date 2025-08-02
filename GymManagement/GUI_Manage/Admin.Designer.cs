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
            this.cmbPhongTapThietBi = new System.Windows.Forms.ComboBox();
            this.cmbTinhTrang = new System.Windows.Forms.ComboBox();
            this.btnXoaThietBi = new MaterialSkin.Controls.MaterialButton();
            this.btnSuaThietBi = new MaterialSkin.Controls.MaterialButton();
            this.btnThemThietBi = new MaterialSkin.Controls.MaterialButton();
            this.dgvThietBi = new System.Windows.Forms.DataGridView();
            this.label10 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtIDThietBi = new MaterialSkin.Controls.MaterialTextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtTenThietBi = new MaterialSkin.Controls.MaterialTextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.materialTabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPhong)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvThietBi)).BeginInit();
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
            this.tabPage1.Size = new System.Drawing.Size(851, 547);
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
            this.tabPage3.Controls.Add(this.cmbPhongTapThietBi);
            this.tabPage3.Controls.Add(this.cmbTinhTrang);
            this.tabPage3.Controls.Add(this.btnXoaThietBi);
            this.tabPage3.Controls.Add(this.btnSuaThietBi);
            this.tabPage3.Controls.Add(this.btnThemThietBi);
            this.tabPage3.Controls.Add(this.dgvThietBi);
            this.tabPage3.Controls.Add(this.label10);
            this.tabPage3.Controls.Add(this.label6);
            this.tabPage3.Controls.Add(this.txtIDThietBi);
            this.tabPage3.Controls.Add(this.label7);
            this.tabPage3.Controls.Add(this.txtTenThietBi);
            this.tabPage3.Controls.Add(this.label8);
            this.tabPage3.Controls.Add(this.label9);
            this.tabPage3.Location = new System.Drawing.Point(4, 29);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(851, 547);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Thiết bị";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // cmbPhongTapThietBi
            // 
            this.cmbPhongTapThietBi.FormattingEnabled = true;
            this.cmbPhongTapThietBi.Location = new System.Drawing.Point(564, 122);
            this.cmbPhongTapThietBi.Name = "cmbPhongTapThietBi";
            this.cmbPhongTapThietBi.Size = new System.Drawing.Size(181, 28);
            this.cmbPhongTapThietBi.TabIndex = 16;
            // 
            // cmbTinhTrang
            // 
            this.cmbTinhTrang.FormattingEnabled = true;
            this.cmbTinhTrang.Location = new System.Drawing.Point(210, 122);
            this.cmbTinhTrang.Name = "cmbTinhTrang";
            this.cmbTinhTrang.Size = new System.Drawing.Size(181, 28);
            this.cmbTinhTrang.TabIndex = 16;
            // 
            // btnXoaThietBi
            // 
            this.btnXoaThietBi.AutoSize = false;
            this.btnXoaThietBi.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnXoaThietBi.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.btnXoaThietBi.Depth = 0;
            this.btnXoaThietBi.HighEmphasis = true;
            this.btnXoaThietBi.Icon = null;
            this.btnXoaThietBi.Location = new System.Drawing.Point(609, 346);
            this.btnXoaThietBi.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnXoaThietBi.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnXoaThietBi.Name = "btnXoaThietBi";
            this.btnXoaThietBi.NoAccentTextColor = System.Drawing.Color.Empty;
            this.btnXoaThietBi.Size = new System.Drawing.Size(195, 52);
            this.btnXoaThietBi.TabIndex = 13;
            this.btnXoaThietBi.Text = "xóa";
            this.btnXoaThietBi.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btnXoaThietBi.UseAccentColor = false;
            this.btnXoaThietBi.UseVisualStyleBackColor = true;
            this.btnXoaThietBi.Click += new System.EventHandler(this.btnXoaThietBi_Click);
            // 
            // btnSuaThietBi
            // 
            this.btnSuaThietBi.AutoSize = false;
            this.btnSuaThietBi.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnSuaThietBi.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.btnSuaThietBi.Depth = 0;
            this.btnSuaThietBi.HighEmphasis = true;
            this.btnSuaThietBi.Icon = null;
            this.btnSuaThietBi.Location = new System.Drawing.Point(609, 269);
            this.btnSuaThietBi.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnSuaThietBi.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnSuaThietBi.Name = "btnSuaThietBi";
            this.btnSuaThietBi.NoAccentTextColor = System.Drawing.Color.Empty;
            this.btnSuaThietBi.Size = new System.Drawing.Size(195, 52);
            this.btnSuaThietBi.TabIndex = 14;
            this.btnSuaThietBi.Text = "SỬA";
            this.btnSuaThietBi.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btnSuaThietBi.UseAccentColor = false;
            this.btnSuaThietBi.UseVisualStyleBackColor = true;
            this.btnSuaThietBi.Click += new System.EventHandler(this.btnSuaThietBi_Click);
            // 
            // btnThemThietBi
            // 
            this.btnThemThietBi.AutoSize = false;
            this.btnThemThietBi.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnThemThietBi.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.btnThemThietBi.Depth = 0;
            this.btnThemThietBi.HighEmphasis = true;
            this.btnThemThietBi.Icon = null;
            this.btnThemThietBi.Location = new System.Drawing.Point(609, 192);
            this.btnThemThietBi.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnThemThietBi.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnThemThietBi.Name = "btnThemThietBi";
            this.btnThemThietBi.NoAccentTextColor = System.Drawing.Color.Empty;
            this.btnThemThietBi.Size = new System.Drawing.Size(195, 52);
            this.btnThemThietBi.TabIndex = 15;
            this.btnThemThietBi.Text = "Thêm";
            this.btnThemThietBi.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btnThemThietBi.UseAccentColor = false;
            this.btnThemThietBi.UseVisualStyleBackColor = true;
            this.btnThemThietBi.Click += new System.EventHandler(this.btnThemThietBi_Click);
            // 
            // dgvThietBi
            // 
            this.dgvThietBi.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvThietBi.Location = new System.Drawing.Point(53, 192);
            this.dgvThietBi.Name = "dgvThietBi";
            this.dgvThietBi.RowHeadersWidth = 48;
            this.dgvThietBi.RowTemplate.Height = 24;
            this.dgvThietBi.Size = new System.Drawing.Size(527, 324);
            this.dgvThietBi.TabIndex = 12;
            this.dgvThietBi.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvThietBi_CellContentClick);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.0177F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(68, 120);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(121, 29);
            this.label10.TabIndex = 6;
            this.label10.Text = "Tình trạng";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.0177F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(436, 120);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(122, 29);
            this.label6.TabIndex = 6;
            this.label6.Text = "Phòng tập";
            // 
            // txtIDThietBi
            // 
            this.txtIDThietBi.AnimateReadOnly = false;
            this.txtIDThietBi.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtIDThietBi.Depth = 0;
            this.txtIDThietBi.Font = new System.Drawing.Font("Roboto", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtIDThietBi.LeadingIcon = null;
            this.txtIDThietBi.Location = new System.Drawing.Point(691, 31);
            this.txtIDThietBi.MaxLength = 50;
            this.txtIDThietBi.MouseState = MaterialSkin.MouseState.OUT;
            this.txtIDThietBi.Multiline = false;
            this.txtIDThietBi.Name = "txtIDThietBi";
            this.txtIDThietBi.Size = new System.Drawing.Size(87, 50);
            this.txtIDThietBi.TabIndex = 10;
            this.txtIDThietBi.Text = "";
            this.txtIDThietBi.TrailingIcon = null;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.0177F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(630, 44);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(42, 29);
            this.label7.TabIndex = 7;
            this.label7.Text = "ID:";
            // 
            // txtTenThietBi
            // 
            this.txtTenThietBi.AnimateReadOnly = false;
            this.txtTenThietBi.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtTenThietBi.Depth = 0;
            this.txtTenThietBi.Font = new System.Drawing.Font("Roboto", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtTenThietBi.LeadingIcon = null;
            this.txtTenThietBi.Location = new System.Drawing.Point(380, 31);
            this.txtTenThietBi.MaxLength = 50;
            this.txtTenThietBi.MouseState = MaterialSkin.MouseState.OUT;
            this.txtTenThietBi.Multiline = false;
            this.txtTenThietBi.Name = "txtTenThietBi";
            this.txtTenThietBi.Size = new System.Drawing.Size(200, 50);
            this.txtTenThietBi.TabIndex = 11;
            this.txtTenThietBi.Text = "";
            this.txtTenThietBi.TrailingIcon = null;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.0177F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(218, 41);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(133, 29);
            this.label8.TabIndex = 8;
            this.label8.Text = "Tên thiết bị";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 17.84071F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(47, 38);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(119, 33);
            this.label9.TabIndex = 5;
            this.label9.Text = "Thiết bị";
            // 
            // tabPage4
            // 
            this.tabPage4.Location = new System.Drawing.Point(4, 29);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(851, 547);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "tabPage4";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tabPage5
            // 
            this.tabPage5.Location = new System.Drawing.Point(4, 29);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(851, 547);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "tabPage5";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // tabPage6
            // 
            this.tabPage6.Location = new System.Drawing.Point(4, 29);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(851, 547);
            this.tabPage6.TabIndex = 5;
            this.tabPage6.Text = "Hóa đơn";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // tabPage7
            // 
            this.tabPage7.Location = new System.Drawing.Point(4, 29);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage7.Size = new System.Drawing.Size(851, 547);
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
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvThietBi)).EndInit();
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
        private MaterialSkin.Controls.MaterialButton btnXoaThietBi;
        private MaterialSkin.Controls.MaterialButton btnSuaThietBi;
        private MaterialSkin.Controls.MaterialButton btnThemThietBi;
        private System.Windows.Forms.DataGridView dgvThietBi;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label6;
        private MaterialSkin.Controls.MaterialTextBox txtIDThietBi;
        private System.Windows.Forms.Label label7;
        private MaterialSkin.Controls.MaterialTextBox txtTenThietBi;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cmbPhongTapThietBi;
        private System.Windows.Forms.ComboBox cmbTinhTrang;
    }
}