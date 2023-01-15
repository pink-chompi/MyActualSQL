namespace ActualSQL
{
    partial class FormMain
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        public void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.tabControl = new System.Windows.Forms.TabControl();
            this.UpdateRightsBtn = new System.Windows.Forms.Button();
            this.CellValueTB = new System.Windows.Forms.TextBox();
            this.ChangeValueBtn = new System.Windows.Forms.Button();
            this.AddStringBtn = new System.Windows.Forms.Button();
            this.DeleteStringBtn = new System.Windows.Forms.Button();
            this.tableInfoLbl = new System.Windows.Forms.Label();
            this.CellValueLbl = new System.Windows.Forms.Label();
            this.updateBtn = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.addBtn = new System.Windows.Forms.Button();
            this.delBtn = new System.Windows.Forms.Button();
            this.backupBtn = new System.Windows.Forms.Button();
            this.restoreBtn = new System.Windows.Forms.Button();
            this.infoLbl = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Location = new System.Drawing.Point(13, 96);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(678, 336);
            this.tabControl.TabIndex = 5;
            this.tabControl.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabControl_Selecting);
            // 
            // UpdateRightsBtn
            // 
            this.UpdateRightsBtn.Location = new System.Drawing.Point(195, 516);
            this.UpdateRightsBtn.Name = "UpdateRightsBtn";
            this.UpdateRightsBtn.Size = new System.Drawing.Size(231, 23);
            this.UpdateRightsBtn.TabIndex = 6;
            this.UpdateRightsBtn.Text = "Обновить права доступа";
            this.UpdateRightsBtn.UseVisualStyleBackColor = true;
            this.UpdateRightsBtn.Click += new System.EventHandler(this.UpdateRights_Click);
            // 
            // CellValueTB
            // 
            this.CellValueTB.Location = new System.Drawing.Point(18, 459);
            this.CellValueTB.Name = "CellValueTB";
            this.CellValueTB.Size = new System.Drawing.Size(168, 20);
            this.CellValueTB.TabIndex = 8;
            // 
            // ChangeValueBtn
            // 
            this.ChangeValueBtn.Location = new System.Drawing.Point(18, 487);
            this.ChangeValueBtn.Name = "ChangeValueBtn";
            this.ChangeValueBtn.Size = new System.Drawing.Size(168, 23);
            this.ChangeValueBtn.TabIndex = 9;
            this.ChangeValueBtn.Text = "Изменить значение";
            this.ChangeValueBtn.UseVisualStyleBackColor = true;
            this.ChangeValueBtn.Click += new System.EventHandler(this.ChangeValueBtn_Click);
            // 
            // AddStringBtn
            // 
            this.AddStringBtn.Location = new System.Drawing.Point(195, 458);
            this.AddStringBtn.Name = "AddStringBtn";
            this.AddStringBtn.Size = new System.Drawing.Size(231, 23);
            this.AddStringBtn.TabIndex = 10;
            this.AddStringBtn.Text = "Добавить строку";
            this.AddStringBtn.UseVisualStyleBackColor = true;
            this.AddStringBtn.Visible = false;
            this.AddStringBtn.Click += new System.EventHandler(this.AddStringBtn_Click);
            // 
            // DeleteStringBtn
            // 
            this.DeleteStringBtn.Location = new System.Drawing.Point(195, 487);
            this.DeleteStringBtn.Name = "DeleteStringBtn";
            this.DeleteStringBtn.Size = new System.Drawing.Size(231, 23);
            this.DeleteStringBtn.TabIndex = 12;
            this.DeleteStringBtn.Text = "Удалить строку";
            this.DeleteStringBtn.UseVisualStyleBackColor = true;
            this.DeleteStringBtn.Visible = false;
            this.DeleteStringBtn.Click += new System.EventHandler(this.DeleteStringBtn_Click);
            // 
            // tableInfoLbl
            // 
            this.tableInfoLbl.AutoSize = true;
            this.tableInfoLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tableInfoLbl.Location = new System.Drawing.Point(12, 54);
            this.tableInfoLbl.Name = "tableInfoLbl";
            this.tableInfoLbl.Size = new System.Drawing.Size(171, 17);
            this.tableInfoLbl.TabIndex = 13;
            this.tableInfoLbl.Text = "Доступные объекты БД:";
            // 
            // CellValueLbl
            // 
            this.CellValueLbl.AutoSize = true;
            this.CellValueLbl.Location = new System.Drawing.Point(15, 438);
            this.CellValueLbl.Name = "CellValueLbl";
            this.CellValueLbl.Size = new System.Drawing.Size(96, 13);
            this.CellValueLbl.TabIndex = 16;
            this.CellValueLbl.Text = "Значение ячейки:";
            // 
            // updateBtn
            // 
            this.updateBtn.Location = new System.Drawing.Point(18, 516);
            this.updateBtn.Name = "updateBtn";
            this.updateBtn.Size = new System.Drawing.Size(168, 23);
            this.updateBtn.TabIndex = 19;
            this.updateBtn.Text = "Обновить таблицы";
            this.updateBtn.UseVisualStyleBackColor = true;
            this.updateBtn.Click += new System.EventHandler(this.updateBtn_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(540, 19);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(147, 52);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 20;
            this.pictureBox1.TabStop = false;
            // 
            // addBtn
            // 
            this.addBtn.Location = new System.Drawing.Point(195, 458);
            this.addBtn.Name = "addBtn";
            this.addBtn.Size = new System.Drawing.Size(495, 23);
            this.addBtn.TabIndex = 22;
            this.addBtn.UseVisualStyleBackColor = true;
            this.addBtn.Click += new System.EventHandler(this.addBtn_Click);
            // 
            // delBtn
            // 
            this.delBtn.Location = new System.Drawing.Point(195, 487);
            this.delBtn.Name = "delBtn";
            this.delBtn.Size = new System.Drawing.Size(495, 23);
            this.delBtn.TabIndex = 23;
            this.delBtn.UseVisualStyleBackColor = true;
            this.delBtn.Click += new System.EventHandler(this.delBtn_Click);
            // 
            // backupBtn
            // 
            this.backupBtn.Location = new System.Drawing.Point(433, 516);
            this.backupBtn.Name = "backupBtn";
            this.backupBtn.Size = new System.Drawing.Size(141, 23);
            this.backupBtn.TabIndex = 24;
            this.backupBtn.Text = "Резервное копирование";
            this.backupBtn.UseVisualStyleBackColor = true;
            this.backupBtn.Click += new System.EventHandler(this.backupBtn_Click);
            // 
            // restoreBtn
            // 
            this.restoreBtn.Location = new System.Drawing.Point(580, 516);
            this.restoreBtn.Name = "restoreBtn";
            this.restoreBtn.Size = new System.Drawing.Size(107, 23);
            this.restoreBtn.TabIndex = 25;
            this.restoreBtn.Text = "Восстановить";
            this.restoreBtn.UseVisualStyleBackColor = true;
            this.restoreBtn.Click += new System.EventHandler(this.restoreBtn_Click);
            // 
            // infoLbl
            // 
            this.infoLbl.AutoSize = true;
            this.infoLbl.Location = new System.Drawing.Point(192, 438);
            this.infoLbl.Name = "infoLbl";
            this.infoLbl.Size = new System.Drawing.Size(176, 13);
            this.infoLbl.TabIndex = 26;
            this.infoLbl.Text = "Операции взаимодействия с БД:";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(699, 551);
            this.Controls.Add(this.infoLbl);
            this.Controls.Add(this.restoreBtn);
            this.Controls.Add(this.backupBtn);
            this.Controls.Add(this.delBtn);
            this.Controls.Add(this.addBtn);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.updateBtn);
            this.Controls.Add(this.CellValueLbl);
            this.Controls.Add(this.tableInfoLbl);
            this.Controls.Add(this.DeleteStringBtn);
            this.Controls.Add(this.AddStringBtn);
            this.Controls.Add(this.ChangeValueBtn);
            this.Controls.Add(this.CellValueTB);
            this.Controls.Add(this.UpdateRightsBtn);
            this.Controls.Add(this.tabControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormMain_FormClosed);
            this.Load += new System.EventHandler(this.FormMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.Button UpdateRightsBtn;
        private System.Windows.Forms.TextBox CellValueTB;
        private System.Windows.Forms.Button ChangeValueBtn;
        private System.Windows.Forms.Button AddStringBtn;
        private System.Windows.Forms.Button DeleteStringBtn;
        private System.Windows.Forms.Label tableInfoLbl;
        private System.Windows.Forms.Label CellValueLbl;
        private System.Windows.Forms.Button updateBtn;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button addBtn;
        private System.Windows.Forms.Button delBtn;
        private System.Windows.Forms.Button backupBtn;
        private System.Windows.Forms.Button restoreBtn;
        private System.Windows.Forms.Label infoLbl;
    }
}

