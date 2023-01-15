namespace ActualSQL
{
    partial class FormBox
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
            this.textLbl = new System.Windows.Forms.Label();
            this.closeBtn = new System.Windows.Forms.Button();
            this.passwordTB = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textLbl
            // 
            this.textLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textLbl.Location = new System.Drawing.Point(12, 9);
            this.textLbl.Name = "textLbl";
            this.textLbl.Size = new System.Drawing.Size(387, 104);
            this.textLbl.TabIndex = 0;
            this.textLbl.Text = "error text";
            this.textLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // closeBtn
            // 
            this.closeBtn.Location = new System.Drawing.Point(149, 124);
            this.closeBtn.Name = "closeBtn";
            this.closeBtn.Size = new System.Drawing.Size(119, 23);
            this.closeBtn.TabIndex = 1;
            this.closeBtn.Text = "Закрыть ";
            this.closeBtn.UseVisualStyleBackColor = true;
            this.closeBtn.Click += new System.EventHandler(this.closeBtn_Click);
            // 
            // passwordTB
            // 
            this.passwordTB.Location = new System.Drawing.Point(79, 83);
            this.passwordTB.Name = "passwordTB";
            this.passwordTB.Size = new System.Drawing.Size(258, 20);
            this.passwordTB.TabIndex = 2;
            this.passwordTB.Visible = false;
            // 
            // FormBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(411, 155);
            this.Controls.Add(this.passwordTB);
            this.Controls.Add(this.closeBtn);
            this.Controls.Add(this.textLbl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FormBox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormBox";
            this.Load += new System.EventHandler(this.FormBox_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label textLbl;
        public System.Windows.Forms.Button closeBtn;
        private System.Windows.Forms.TextBox passwordTB;
    }
}