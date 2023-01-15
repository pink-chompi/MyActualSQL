using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ActualSQL
{
    public partial class FormBox : Form
    {
        FormMain formMain;
        string globalMode;
        public FormBox(FormMain fm, string Mode, string rText)
        {
            InitializeComponent();
            formMain = fm;
            globalMode = Mode;
            switch (Mode)
            {
                case "error":
                    Text = "Ошибка";
                    textLbl.Text = rText;
                    Icon = SystemIcons.Error;
                    break;

                case "success":
                    Text = "Успешно";
                    textLbl.Text = rText;
                    Icon = SystemIcons.Information;
                    break;

                case "warning":
                    Text = "Предупреждение";
                    textLbl.Text = rText;
                    Icon = SystemIcons.Warning;
                    break;

                case "input":
                    Text = "Ввод пароля";
                    textLbl.Text = rText;
                    passwordTB.Visible = true;
                    Icon = SystemIcons.Information;
                    closeBtn.Text = "Назначить пароль";
                    break;
            }
        }

        private void FormBox_Load(object sender, EventArgs e)
        {
            Height = textLbl.Height + 80;
            closeBtn.Top = textLbl.Height + 10;
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            if (globalMode == "input")
            {
                string userName = formMain.DeleteSpaces(formMain.dataGridViews[formMain.selectedTab][0, formMain.dataGridViews[formMain.selectedTab].CurrentCell.RowIndex].Value.ToString());
                string password = passwordTB.Text;
                string fields = formMain.GetFields(formMain.currenttableName);
                string fvalues = $"'{userName}'";
                formMain.sp_addUser(userName, password, fields, fvalues);
            }
            Close();
        }
    }
}
