namespace ActualSQL
{
    partial class FormAuth
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
        public void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAuth));
            this.LoginTB = new System.Windows.Forms.TextBox();
            this.PasswordTB = new System.Windows.Forms.TextBox();
            this.loginBtn = new System.Windows.Forms.Button();
            this.UserNameLbl = new System.Windows.Forms.Label();
            this.PasswordLbl = new System.Windows.Forms.Label();
            this.DataBaseTB = new System.Windows.Forms.TextBox();
            this.DataBaseNameLbl = new System.Windows.Forms.Label();
            this.infoLbl = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.depositorBtn = new System.Windows.Forms.Button();
            this.passLbl = new System.Windows.Forms.Label();
            this.passTB = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // LoginTB
            // 
            this.LoginTB.Location = new System.Drawing.Point(12, 96);
            this.LoginTB.Name = "LoginTB";
            this.LoginTB.Size = new System.Drawing.Size(150, 20);
            this.LoginTB.TabIndex = 0;
            this.LoginTB.TextChanged += new System.EventHandler(this.LoginTB_TextChanged);
            // 
            // PasswordTB
            // 
            this.PasswordTB.Location = new System.Drawing.Point(12, 141);
            this.PasswordTB.Name = "PasswordTB";
            this.PasswordTB.PasswordChar = '*';
            this.PasswordTB.Size = new System.Drawing.Size(306, 20);
            this.PasswordTB.TabIndex = 1;
            // 
            // loginBtn
            // 
            this.loginBtn.Location = new System.Drawing.Point(12, 167);
            this.loginBtn.Name = "loginBtn";
            this.loginBtn.Size = new System.Drawing.Size(306, 23);
            this.loginBtn.TabIndex = 3;
            this.loginBtn.Text = "Войти";
            this.loginBtn.UseVisualStyleBackColor = true;
            this.loginBtn.Click += new System.EventHandler(this.EnterButton_Click);
            // 
            // UserNameLbl
            // 
            this.UserNameLbl.AutoSize = true;
            this.UserNameLbl.Location = new System.Drawing.Point(12, 78);
            this.UserNameLbl.Name = "UserNameLbl";
            this.UserNameLbl.Size = new System.Drawing.Size(106, 13);
            this.UserNameLbl.TabIndex = 4;
            this.UserNameLbl.Text = "Имя пользователя:";
            // 
            // PasswordLbl
            // 
            this.PasswordLbl.AutoSize = true;
            this.PasswordLbl.Location = new System.Drawing.Point(12, 125);
            this.PasswordLbl.Name = "PasswordLbl";
            this.PasswordLbl.Size = new System.Drawing.Size(48, 13);
            this.PasswordLbl.TabIndex = 5;
            this.PasswordLbl.Text = "Пароль:";
            // 
            // DataBaseTB
            // 
            this.DataBaseTB.Location = new System.Drawing.Point(168, 96);
            this.DataBaseTB.Name = "DataBaseTB";
            this.DataBaseTB.Size = new System.Drawing.Size(150, 20);
            this.DataBaseTB.TabIndex = 6;
            this.DataBaseTB.TextChanged += new System.EventHandler(this.DataBaseTB_TextChanged);
            // 
            // DataBaseNameLbl
            // 
            this.DataBaseNameLbl.AutoSize = true;
            this.DataBaseNameLbl.Location = new System.Drawing.Point(165, 78);
            this.DataBaseNameLbl.Name = "DataBaseNameLbl";
            this.DataBaseNameLbl.Size = new System.Drawing.Size(129, 13);
            this.DataBaseNameLbl.TabIndex = 7;
            this.DataBaseNameLbl.Text = "Название базы данных:";
            // 
            // infoLbl
            // 
            this.infoLbl.AutoSize = true;
            this.infoLbl.Location = new System.Drawing.Point(12, 23);
            this.infoLbl.Name = "infoLbl";
            this.infoLbl.Size = new System.Drawing.Size(35, 13);
            this.infoLbl.TabIndex = 8;
            this.infoLbl.Text = "label1";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(168, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(147, 52);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 12;
            this.pictureBox1.TabStop = false;
            // 
            // depositorBtn
            // 
            this.depositorBtn.Location = new System.Drawing.Point(12, 275);
            this.depositorBtn.Name = "depositorBtn";
            this.depositorBtn.Size = new System.Drawing.Size(306, 23);
            this.depositorBtn.TabIndex = 13;
            this.depositorBtn.Text = "Войти как вкладчик";
            this.depositorBtn.UseVisualStyleBackColor = true;
            this.depositorBtn.Click += new System.EventHandler(this.depositorBtn_Click);
            // 
            // passLbl
            // 
            this.passLbl.AutoSize = true;
            this.passLbl.Location = new System.Drawing.Point(12, 233);
            this.passLbl.Name = "passLbl";
            this.passLbl.Size = new System.Drawing.Size(135, 13);
            this.passLbl.TabIndex = 14;
            this.passLbl.Text = "Серия и номер паспорта:";
            // 
            // passTB
            // 
            this.passTB.Location = new System.Drawing.Point(12, 249);
            this.passTB.Name = "passTB";
            this.passTB.Size = new System.Drawing.Size(306, 20);
            this.passTB.TabIndex = 15;
            // 
            // FormAuth
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(327, 311);
            this.Controls.Add(this.passTB);
            this.Controls.Add(this.passLbl);
            this.Controls.Add(this.depositorBtn);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.infoLbl);
            this.Controls.Add(this.DataBaseNameLbl);
            this.Controls.Add(this.DataBaseTB);
            this.Controls.Add(this.PasswordLbl);
            this.Controls.Add(this.UserNameLbl);
            this.Controls.Add(this.loginBtn);
            this.Controls.Add(this.PasswordTB);
            this.Controls.Add(this.LoginTB);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.Name = "FormAuth";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Подключение \"ЛайтБанк\"";
            this.TransparencyKey = System.Drawing.SystemColors.ControlDark;
            this.Load += new System.EventHandler(this.FormAuth_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox LoginTB;
        public System.Windows.Forms.TextBox PasswordTB;
        public System.Windows.Forms.Button loginBtn;
        public System.Windows.Forms.Label UserNameLbl;
        public System.Windows.Forms.Label PasswordLbl;
        public System.Windows.Forms.TextBox DataBaseTB;
        public System.Windows.Forms.Label DataBaseNameLbl;
        public System.Windows.Forms.Label infoLbl;
        public System.Windows.Forms.PictureBox pictureBox1;
        public System.Windows.Forms.Button depositorBtn;
        public System.Windows.Forms.Label passLbl;
        public System.Windows.Forms.TextBox passTB;
    }
}