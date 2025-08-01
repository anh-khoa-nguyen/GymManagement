namespace GUI_Manage
{
    partial class DangNhap_DangKy
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
            this.btnDangNhap = new MaterialSkin.Controls.MaterialButton();
            this.txtLoginPassword = new MaterialSkin.Controls.MaterialTextBox2();
            this.label3 = new System.Windows.Forms.Label();
            this.txtLoginSDT = new MaterialSkin.Controls.MaterialTextBox2();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btnDangKy = new MaterialSkin.Controls.MaterialButton();
            this.txtRegisterSDT = new MaterialSkin.Controls.MaterialTextBox2();
            this.txtRegisterPassword = new MaterialSkin.Controls.MaterialTextBox2();
            this.txtRegisterTen = new MaterialSkin.Controls.MaterialTextBox2();
            this.label8 = new System.Windows.Forms.Label();
            this.txtRegisterEmail = new MaterialSkin.Controls.MaterialTextBox2();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.materialSwitch1 = new MaterialSkin.Controls.MaterialSwitch();
            this.label9 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.materialTabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // materialTabControl1
            // 
            this.materialTabControl1.Controls.Add(this.tabPage1);
            this.materialTabControl1.Controls.Add(this.tabPage2);
            this.materialTabControl1.Depth = 0;
            this.materialTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.materialTabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.10619F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.materialTabControl1.Location = new System.Drawing.Point(3, 64);
            this.materialTabControl1.Margin = new System.Windows.Forms.Padding(4);
            this.materialTabControl1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialTabControl1.Multiline = true;
            this.materialTabControl1.Name = "materialTabControl1";
            this.materialTabControl1.SelectedIndex = 0;
            this.materialTabControl1.Size = new System.Drawing.Size(489, 517);
            this.materialTabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.materialTabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.White;
            this.tabPage1.Controls.Add(this.materialSwitch1);
            this.tabPage1.Controls.Add(this.pictureBox1);
            this.tabPage1.Controls.Add(this.btnDangNhap);
            this.tabPage1.Controls.Add(this.txtLoginPassword);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.txtLoginSDT);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 33);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage1.Size = new System.Drawing.Size(481, 480);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Đăng nhập";
            // 
            // btnDangNhap
            // 
            this.btnDangNhap.AutoSize = false;
            this.btnDangNhap.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnDangNhap.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.btnDangNhap.Depth = 0;
            this.btnDangNhap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDangNhap.Font = new System.Drawing.Font("Microsoft Sans Serif", 28.0354F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDangNhap.HighEmphasis = true;
            this.btnDangNhap.Icon = null;
            this.btnDangNhap.Location = new System.Drawing.Point(22, 432);
            this.btnDangNhap.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnDangNhap.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnDangNhap.Name = "btnDangNhap";
            this.btnDangNhap.NoAccentTextColor = System.Drawing.Color.Empty;
            this.btnDangNhap.Size = new System.Drawing.Size(436, 42);
            this.btnDangNhap.TabIndex = 2;
            this.btnDangNhap.Text = "Đăng nhập";
            this.btnDangNhap.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btnDangNhap.UseAccentColor = false;
            this.btnDangNhap.UseVisualStyleBackColor = true;
            this.btnDangNhap.Click += new System.EventHandler(this.btnDangNhap_Click);
            // 
            // txtLoginPassword
            // 
            this.txtLoginPassword.AnimateReadOnly = false;
            this.txtLoginPassword.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.txtLoginPassword.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtLoginPassword.Depth = 0;
            this.txtLoginPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtLoginPassword.HideSelection = true;
            this.txtLoginPassword.LeadingIcon = null;
            this.txtLoginPassword.Location = new System.Drawing.Point(29, 363);
            this.txtLoginPassword.MaxLength = 32767;
            this.txtLoginPassword.MouseState = MaterialSkin.MouseState.OUT;
            this.txtLoginPassword.Name = "txtLoginPassword";
            this.txtLoginPassword.PasswordChar = '\0';
            this.txtLoginPassword.PrefixSuffixText = null;
            this.txtLoginPassword.ReadOnly = false;
            this.txtLoginPassword.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtLoginPassword.SelectedText = "";
            this.txtLoginPassword.SelectionLength = 0;
            this.txtLoginPassword.SelectionStart = 0;
            this.txtLoginPassword.ShortcutsEnabled = true;
            this.txtLoginPassword.Size = new System.Drawing.Size(436, 48);
            this.txtLoginPassword.TabIndex = 2;
            this.txtLoginPassword.TabStop = false;
            this.txtLoginPassword.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtLoginPassword.TrailingIcon = null;
            this.txtLoginPassword.UseSystemPasswordChar = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.0177F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(24, 320);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(115, 29);
            this.label3.TabIndex = 0;
            this.label3.Text = "Mật khẩu:";
            // 
            // txtLoginSDT
            // 
            this.txtLoginSDT.AnimateReadOnly = false;
            this.txtLoginSDT.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.txtLoginSDT.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtLoginSDT.Depth = 0;
            this.txtLoginSDT.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.19469F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLoginSDT.HideSelection = true;
            this.txtLoginSDT.LeadingIcon = null;
            this.txtLoginSDT.Location = new System.Drawing.Point(29, 258);
            this.txtLoginSDT.MaxLength = 32767;
            this.txtLoginSDT.MouseState = MaterialSkin.MouseState.OUT;
            this.txtLoginSDT.Name = "txtLoginSDT";
            this.txtLoginSDT.PasswordChar = '\0';
            this.txtLoginSDT.PrefixSuffixText = null;
            this.txtLoginSDT.ReadOnly = false;
            this.txtLoginSDT.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtLoginSDT.SelectedText = "";
            this.txtLoginSDT.SelectionLength = 0;
            this.txtLoginSDT.SelectionStart = 0;
            this.txtLoginSDT.ShortcutsEnabled = true;
            this.txtLoginSDT.Size = new System.Drawing.Size(426, 48);
            this.txtLoginSDT.TabIndex = 1;
            this.txtLoginSDT.TabStop = false;
            this.txtLoginSDT.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtLoginSDT.TrailingIcon = null;
            this.txtLoginSDT.UseSystemPasswordChar = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.0177F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(24, 216);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(154, 29);
            this.label2.TabIndex = 0;
            this.label2.Text = "Số điện thoại";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 17.84071F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(154, 173);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(166, 32);
            this.label1.TabIndex = 0;
            this.label1.Text = "Đăng Nhập";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.pictureBox2);
            this.tabPage2.Controls.Add(this.label9);
            this.tabPage2.Controls.Add(this.btnDangKy);
            this.tabPage2.Controls.Add(this.txtRegisterSDT);
            this.tabPage2.Controls.Add(this.txtRegisterPassword);
            this.tabPage2.Controls.Add(this.txtRegisterTen);
            this.tabPage2.Controls.Add(this.label8);
            this.tabPage2.Controls.Add(this.txtRegisterEmail);
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Location = new System.Drawing.Point(4, 33);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage2.Size = new System.Drawing.Size(481, 480);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Đăng ký";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btnDangKy
            // 
            this.btnDangKy.AutoSize = false;
            this.btnDangKy.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnDangKy.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.btnDangKy.Depth = 0;
            this.btnDangKy.Font = new System.Drawing.Font("Microsoft Sans Serif", 22.30089F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDangKy.HighEmphasis = true;
            this.btnDangKy.Icon = null;
            this.btnDangKy.Location = new System.Drawing.Point(28, 429);
            this.btnDangKy.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnDangKy.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnDangKy.Name = "btnDangKy";
            this.btnDangKy.NoAccentTextColor = System.Drawing.Color.Empty;
            this.btnDangKy.Size = new System.Drawing.Size(421, 41);
            this.btnDangKy.TabIndex = 8;
            this.btnDangKy.Text = "Đăng ký";
            this.btnDangKy.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btnDangKy.UseAccentColor = false;
            this.btnDangKy.UseVisualStyleBackColor = true;
            this.btnDangKy.Click += new System.EventHandler(this.btnDangKy_Click);
            // 
            // txtRegisterSDT
            // 
            this.txtRegisterSDT.AnimateReadOnly = false;
            this.txtRegisterSDT.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.txtRegisterSDT.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtRegisterSDT.Depth = 0;
            this.txtRegisterSDT.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtRegisterSDT.HideSelection = true;
            this.txtRegisterSDT.LeadingIcon = null;
            this.txtRegisterSDT.Location = new System.Drawing.Point(28, 207);
            this.txtRegisterSDT.MaxLength = 32767;
            this.txtRegisterSDT.MouseState = MaterialSkin.MouseState.OUT;
            this.txtRegisterSDT.Name = "txtRegisterSDT";
            this.txtRegisterSDT.PasswordChar = '\0';
            this.txtRegisterSDT.PrefixSuffixText = null;
            this.txtRegisterSDT.ReadOnly = false;
            this.txtRegisterSDT.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtRegisterSDT.SelectedText = "";
            this.txtRegisterSDT.SelectionLength = 0;
            this.txtRegisterSDT.SelectionStart = 0;
            this.txtRegisterSDT.ShortcutsEnabled = true;
            this.txtRegisterSDT.Size = new System.Drawing.Size(421, 48);
            this.txtRegisterSDT.TabIndex = 2;
            this.txtRegisterSDT.TabStop = false;
            this.txtRegisterSDT.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtRegisterSDT.TrailingIcon = null;
            this.txtRegisterSDT.UseSystemPasswordChar = false;
            // 
            // txtRegisterPassword
            // 
            this.txtRegisterPassword.AnimateReadOnly = false;
            this.txtRegisterPassword.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.txtRegisterPassword.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtRegisterPassword.Depth = 0;
            this.txtRegisterPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtRegisterPassword.HideSelection = true;
            this.txtRegisterPassword.LeadingIcon = null;
            this.txtRegisterPassword.Location = new System.Drawing.Point(28, 384);
            this.txtRegisterPassword.MaxLength = 32767;
            this.txtRegisterPassword.MouseState = MaterialSkin.MouseState.OUT;
            this.txtRegisterPassword.Name = "txtRegisterPassword";
            this.txtRegisterPassword.PasswordChar = '\0';
            this.txtRegisterPassword.PrefixSuffixText = null;
            this.txtRegisterPassword.ReadOnly = false;
            this.txtRegisterPassword.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtRegisterPassword.SelectedText = "";
            this.txtRegisterPassword.SelectionLength = 0;
            this.txtRegisterPassword.SelectionStart = 0;
            this.txtRegisterPassword.ShortcutsEnabled = true;
            this.txtRegisterPassword.Size = new System.Drawing.Size(421, 48);
            this.txtRegisterPassword.TabIndex = 4;
            this.txtRegisterPassword.TabStop = false;
            this.txtRegisterPassword.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtRegisterPassword.TrailingIcon = null;
            this.txtRegisterPassword.UseSystemPasswordChar = false;
            // 
            // txtRegisterTen
            // 
            this.txtRegisterTen.AnimateReadOnly = false;
            this.txtRegisterTen.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.txtRegisterTen.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtRegisterTen.Depth = 0;
            this.txtRegisterTen.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtRegisterTen.HideSelection = true;
            this.txtRegisterTen.LeadingIcon = null;
            this.txtRegisterTen.Location = new System.Drawing.Point(28, 122);
            this.txtRegisterTen.MaxLength = 32767;
            this.txtRegisterTen.MouseState = MaterialSkin.MouseState.OUT;
            this.txtRegisterTen.Name = "txtRegisterTen";
            this.txtRegisterTen.PasswordChar = '\0';
            this.txtRegisterTen.PrefixSuffixText = null;
            this.txtRegisterTen.ReadOnly = false;
            this.txtRegisterTen.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtRegisterTen.SelectedText = "";
            this.txtRegisterTen.SelectionLength = 0;
            this.txtRegisterTen.SelectionStart = 0;
            this.txtRegisterTen.ShortcutsEnabled = true;
            this.txtRegisterTen.Size = new System.Drawing.Size(421, 48);
            this.txtRegisterTen.TabIndex = 1;
            this.txtRegisterTen.TabStop = false;
            this.txtRegisterTen.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtRegisterTen.TrailingIcon = null;
            this.txtRegisterTen.UseSystemPasswordChar = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.0177F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(23, 170);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(68, 29);
            this.label8.TabIndex = 3;
            this.label8.Text = "SDT:";
            // 
            // txtRegisterEmail
            // 
            this.txtRegisterEmail.AnimateReadOnly = false;
            this.txtRegisterEmail.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.txtRegisterEmail.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtRegisterEmail.Depth = 0;
            this.txtRegisterEmail.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtRegisterEmail.HideSelection = true;
            this.txtRegisterEmail.LeadingIcon = null;
            this.txtRegisterEmail.Location = new System.Drawing.Point(28, 294);
            this.txtRegisterEmail.MaxLength = 32767;
            this.txtRegisterEmail.MouseState = MaterialSkin.MouseState.OUT;
            this.txtRegisterEmail.Name = "txtRegisterEmail";
            this.txtRegisterEmail.PasswordChar = '\0';
            this.txtRegisterEmail.PrefixSuffixText = null;
            this.txtRegisterEmail.ReadOnly = false;
            this.txtRegisterEmail.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtRegisterEmail.SelectedText = "";
            this.txtRegisterEmail.SelectionLength = 0;
            this.txtRegisterEmail.SelectionStart = 0;
            this.txtRegisterEmail.ShortcutsEnabled = true;
            this.txtRegisterEmail.Size = new System.Drawing.Size(421, 48);
            this.txtRegisterEmail.TabIndex = 3;
            this.txtRegisterEmail.TabStop = false;
            this.txtRegisterEmail.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtRegisterEmail.TrailingIcon = null;
            this.txtRegisterEmail.UseSystemPasswordChar = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.0177F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(23, 88);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(119, 29);
            this.label7.TabIndex = 4;
            this.label7.Text = "Họ và tên:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.0177F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(23, 340);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(115, 29);
            this.label4.TabIndex = 3;
            this.label4.Text = "Mật khẩu:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.0177F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(23, 257);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 29);
            this.label5.TabIndex = 4;
            this.label5.Text = "Email:";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::GUI_Manage.Properties.Resources.user;
            this.pictureBox1.Location = new System.Drawing.Point(173, 28);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(125, 125);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // materialSwitch1
            // 
            this.materialSwitch1.AutoSize = true;
            this.materialSwitch1.Depth = 0;
            this.materialSwitch1.Location = new System.Drawing.Point(336, 17);
            this.materialSwitch1.Margin = new System.Windows.Forms.Padding(0);
            this.materialSwitch1.MouseLocation = new System.Drawing.Point(-1, -1);
            this.materialSwitch1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialSwitch1.Name = "materialSwitch1";
            this.materialSwitch1.Ripple = true;
            this.materialSwitch1.Size = new System.Drawing.Size(129, 37);
            this.materialSwitch1.TabIndex = 4;
            this.materialSwitch1.Text = "Nhân viên";
            this.materialSwitch1.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 17.84071F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(97, 30);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(129, 32);
            this.label9.TabIndex = 9;
            this.label9.Text = "Đăng Ký";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::GUI_Manage.Properties.Resources.user;
            this.pictureBox2.Location = new System.Drawing.Point(28, 15);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(63, 58);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 10;
            this.pictureBox2.TabStop = false;
            // 
            // DangNhap_DangKy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(495, 584);
            this.Controls.Add(this.materialTabControl1);
            this.DrawerTabControl = this.materialTabControl1;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.10619F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "DangNhap_DangKy";
            this.Sizable = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Đăng nhập";
            this.materialTabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private MaterialSkin.Controls.MaterialTabControl materialTabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabPage2;
        private MaterialSkin.Controls.MaterialButton btnDangNhap;
        private MaterialSkin.Controls.MaterialTextBox2 txtLoginPassword;
        private System.Windows.Forms.Label label3;
        private MaterialSkin.Controls.MaterialTextBox2 txtLoginSDT;
        private MaterialSkin.Controls.MaterialButton btnDangKy;
        private MaterialSkin.Controls.MaterialTextBox2 txtRegisterSDT;
        private MaterialSkin.Controls.MaterialTextBox2 txtRegisterPassword;
        private MaterialSkin.Controls.MaterialTextBox2 txtRegisterTen;
        private System.Windows.Forms.Label label8;
        private MaterialSkin.Controls.MaterialTextBox2 txtRegisterEmail;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.PictureBox pictureBox1;
        private MaterialSkin.Controls.MaterialSwitch materialSwitch1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.PictureBox pictureBox2;
    }
}