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
            this.ChangeValueBtn = new System.Windows.Forms.Button();
            this.AddStringBtn = new System.Windows.Forms.Button();
            this.DeleteStringBtn = new System.Windows.Forms.Button();
            this.updateBtn = new System.Windows.Forms.Button();
            this.addBtn = new System.Windows.Forms.Button();
            this.delBtn = new System.Windows.Forms.Button();
            this.backupBtn = new System.Windows.Forms.Button();
            this.restoreBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Location = new System.Drawing.Point(9, 12);
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
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(699, 551);
            this.Controls.Add(this.restoreBtn);
            this.Controls.Add(this.backupBtn);
            this.Controls.Add(this.delBtn);
            this.Controls.Add(this.addBtn);
            this.Controls.Add(this.updateBtn);
            this.Controls.Add(this.DeleteStringBtn);
            this.Controls.Add(this.AddStringBtn);
            this.Controls.Add(this.ChangeValueBtn);
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
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.Button UpdateRightsBtn;
        private System.Windows.Forms.Button ChangeValueBtn;
        private System.Windows.Forms.Button AddStringBtn;
        private System.Windows.Forms.Button DeleteStringBtn;
        private System.Windows.Forms.Button updateBtn;
        private System.Windows.Forms.Button addBtn;
        private System.Windows.Forms.Button delBtn;
        private System.Windows.Forms.Button backupBtn;
        private System.Windows.Forms.Button restoreBtn;
    }
}

