using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ActualSQL
{
    public partial class FormAuth : Form
    {
        FormMain f;
        string connectionString;
        public bool asDepositor = false;
        
        public FormAuth()
        {
            InitializeComponent();
        }

        private void EnterButton_Click(object sender, EventArgs e)
        {
            connectionString = $"Server=DESKTOP-4KUDERO\\SQLEXPRESS;Database=AutoService;User Id={LoginTB.Text};Password={PasswordTB.Text};";
            if (LoginTB.Text == "nopriv_user")
            {
                MessageBox.Show("Ошибка! Данный пользователь не имеет прямого доступа к БД");
                return;
            }

            using (SqlConnection tryConnect = new SqlConnection(connectionString))
            {
                try
                {
                    tryConnect.Open();
                    asDepositor = false;

                    FormMain f = new FormMain(this);
                    string[] tableNames = File.ReadAllLines("tables.txt");

                    Hide();
                    MessageBox.Show($"Добро пожаловать {LoginTB.Text}!");
                    f.ShowDialog();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        private void FormAuth_Load(object sender, EventArgs e)
        {
            LoginTB.Text = "sec_admin"; PasswordTB.Text = "123";
        }

        private void LoginTB_TextChanged(object sender, EventArgs e)
        {

        }

        private void DataBaseTB_TextChanged(object sender, EventArgs e)
        {

        }

        string[] GetPhonetList(string query)
        {
            f = new FormMain(this);
            string[] result = new string[0];
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Array.Resize(ref result, result.Length + 1);
                            result[result.Length - 1] = f.DeleteSpaces(dr[0].ToString());
                        }
                    }
                }
            }
            return result;
        }

        private void clientBtn_Click(object sender, EventArgs e)
        {
            f = new FormMain(this);
            asDepositor = true;
            LoginTB.Text = "nopriv_user";
            PasswordTB.Text = "";
            connectionString = $"Server=DESKTOP-4KUDERO\\SQLEXPRESS;Database=AutoService;User Id={LoginTB.Text};Password={PasswordTB.Text};";

            string[] clients = GetPhonetList($"SELECT [Телефон] from dbo.DViewOrders");

            if (clients.Contains(f.DeleteSpaces(passTB.Text)))
            {
                MessageBox.Show("Добро пожаловать!");
                f = new FormMain(this);
                Hide();
                f.ShowDialog();    
            }
            else
            {
                MessageBox.Show("Клиента с введенным номером телефона не существует!");
            }
        }

        private void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox.Checked) clientBtn.Enabled = true;
            else clientBtn.Enabled = false;
        }
    }
}
