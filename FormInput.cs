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
    public partial class FormInput : Form
    {
        Label[] labels = new Label[0];
        TextBox[] textBoxes = new TextBox[0];
        Button subBtn = new Button();

        FormMain formMain;

        string GetValues()
        {
            string values = "";
            for (int i = 0; i < textBoxes.Length; i++) values += $"'{textBoxes[i].Text}'" + ",";

            values = values.Remove(values.Length - 1, 1);
            return values;
        }

        private void subBtn_Click(object sender, EventArgs e)
        {
            string[] parameters;
            string[] values;
            switch (formMain.codeCall)
            {
                // LevelAccess
                case 7:
                    string userName = textBoxes[0].Text;
                    string password = "123";
                    string fields = formMain.GetFields(formMain.currenttableName);
                    string fvalues = $"'{userName}','{textBoxes[1].Text}'";
                    formMain.sp_addUser(userName, password, fields, fvalues);
                    break;

                // Clients
                case 1:
                    parameters = formMain.GetFields(formMain.currenttableName).Split(',');
                    values = GetValues().Split(',');
                    formMain.sp_Call("AddNewClients", parameters, values);
                    break;

                // Masters
                case 2:
                    parameters = formMain.GetFields(formMain.currenttableName).Split(',');
                    values = GetValues().Split(',');
                    formMain.sp_Call("AddNewMasters", parameters, values);
                    break;

                // Orders
                case 3:
                    parameters = formMain.GetFields(formMain.currenttableName).Split(',');
                    values = GetValues().Split(',');
                    formMain.sp_Call("AddNewOrders", parameters, values);
                    break;

                // Serv_Orders
                case 4:
                    parameters = formMain.GetFields(formMain.currenttableName).Split(',');
                    values = GetValues().Split(',');
                    formMain.sp_Call("AddNewServ_Orders", parameters, values);
                    break;

                // Services
                case 5:
                    parameters = formMain.GetFields(formMain.currenttableName).Split(',');
                    values = GetValues().Split(',');
                    formMain.sp_Call("AddNewServices", parameters, values);
                    break;

                // Vehicles
                case 6:
                    parameters = formMain.GetFields(formMain.currenttableName).Split(',');
                    values = GetValues().Split(',');
                    formMain.sp_Call("AddNewVehicles", parameters, values);
                    break;
            }
        }


        public FormInput(FormMain f)
        {
            InitializeComponent();  
            formMain = f;

            int formHeight = 0;
            for (int i = 0; i < formMain.dataGridViews[formMain.selectedTab].Columns.Count; i++)
            {
                Array.Resize(ref labels, labels.Length + 1); Array.Resize(ref textBoxes, textBoxes.Length + 1);
                labels[labels.Length - 1] = new Label(); textBoxes[textBoxes.Length - 1] = new TextBox();
                labels[labels.Length - 1].Location = new Point(15, 15 + (i * 50)); textBoxes[textBoxes.Length - 1].Location = new Point(15, labels[labels.Length - 1].Height + 15 + (i * 50));


                labels[labels.Length - 1].Text = formMain.dataGridViews[formMain.selectedTab].Columns[i].HeaderText;
                

                textBoxes[textBoxes.Length - 1].Width += 50;

                //labels[labels.Length - 1].ForeColor = Color.Black;

                this.Controls.Add(labels[labels.Length - 1]); this.Controls.Add(textBoxes[textBoxes.Length - 1]);
                //MessageBox.Show(formMain.dataGridViews[formMain.formMain.selectedTab].Columns[i].HeaderText);
                formHeight += labels[labels.Length - 1].Height + textBoxes[textBoxes.Length - 1].Height;
            }
            Height = 130 + formHeight;
            Width = 50 + textBoxes[0].Width;
            StartPosition = FormStartPosition.CenterParent;

            subBtn.Width = textBoxes[textBoxes.Length - 1].Width;
            subBtn.Text = "Ввод";
            subBtn.Location = new Point(15, 10 + textBoxes[textBoxes.Length - 1].Top + textBoxes[textBoxes.Length - 1].Height);

            this.subBtn.Click += new System.EventHandler(this.subBtn_Click);

            this.Controls.Add(subBtn);
        }
    }
}
