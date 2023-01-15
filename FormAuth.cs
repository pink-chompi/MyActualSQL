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
            DataBaseTB.Text = "AutoService";
        }

        private void EnterButton_Click(object sender, EventArgs e)
        {
            connectionString = $"Server=DESKTOP-4KUDERO\\SQLEXPRESS;Database={DataBaseTB.Text};User Id={LoginTB.Text};Password={PasswordTB.Text};";
            if (LoginTB.Text == "nopriv_user")
            {
                FormBox a = new FormBox(null, "error", "Ошибка! Данный пользователь не имеет прямого доступа к БД");
                a.ShowDialog();
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

                    /*if (tableNames.Count == 0)
                    {
                        FormBox a = new FormBox(null, "error", "Ошибка! У вас нет прав для просмотра таблиц");
                        a.ShowDialog();
                        return;
                    }*/

                    Hide();
                    f.ShowDialog();
                }
                catch (SqlException ex)
                {
                    FormBox a = new FormBox(null, "error", ex.Message);
                    a.ShowDialog();
                }

            }
        }

        private void FormAuth_Load(object sender, EventArgs e)
        {
            LoginTB.Text = "sec_admin"; PasswordTB.Text = "123";
            infoLbl.Text = $"Подключение к БД - {DataBaseTB.Text}\nПользователь: " + LoginTB.Text;
        }

        private void LoginTB_TextChanged(object sender, EventArgs e)
        {
            infoLbl.Text = $"Подключение к БД - {DataBaseTB.Text}\nПользователь: " + LoginTB.Text;
        }

        private void DataBaseTB_TextChanged(object sender, EventArgs e)
        {
            infoLbl.Text = $"Подключение к БД - {DataBaseTB.Text}\nПользователь: " + LoginTB.Text;
        }

        string[] GetPassportList(string query)
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

        private void depositorBtn_Click(object sender, EventArgs e)
        {
            f = new FormMain(this);
            asDepositor = true;
            LoginTB.Text = "nopriv_user";
            PasswordTB.Text = "";
            connectionString = $"Server=DESKTOP-4KUDERO\\SQLEXPRESS;Database={DataBaseTB.Text};User Id={LoginTB.Text};Password={PasswordTB.Text};";

            string[] depositors = GetPassportList($"SELECT [Паспорт] from dbo.DView");

            if (depositors.Contains(f.DeleteSpaces(passTB.Text)))
            {
                string[] deposits = GetPassportList($"SELECT [Паспорт] from dbo.DViewAll");
                if (deposits.Contains(f.DeleteSpaces(passTB.Text)))
                {
                    f = new FormMain(this);
                    Hide();
                    f.ShowDialog();
                }
                else
                {
                    FormBox a = new FormBox(null, "error", "У вкладчика с введенными паспортными данными нет вкладов");
                    a.ShowDialog();
                    return;
                }
                
            }
            else
            {
                FormBox a = new FormBox(null, "error", "Вкладчика с введенными паспортными данными не существует!");
                a.ShowDialog();
            }
        }
    }
}
