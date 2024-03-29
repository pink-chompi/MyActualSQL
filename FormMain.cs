﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ActualSQL
{
    public partial class FormMain : Form
    {
        FormAuth Auth;

        public string userName, password;

        public int selectedTab = 0, codeCall = -1;
        string dataBaseName = "";
        string connectionString = "";
        public string currenttableName = "";

        SqlDataAdapter dataAdapter = new SqlDataAdapter();
        BindingSource[] bs = new BindingSource[0];
        public DataGridView[] dataGridViews = new DataGridView[0];
        TabPage currentTab = null;

        // ============== Вспомогательные методы ====================
        /* Метод удаления пробелов, появляющихся от значений ячеек с типом nvarchar(n) из БД */
        public string DeleteSpaces(string value)
        {
            if (value.Length != 0)
            {
                int index = 0;
                for (int q = value.Length - 1; q >= 0; q--)
                    if (value[q] != ' ') { index = q; break; }

                value = value.Remove(index + 1, value.Length - index - 1);
            }
            return value;
        }

        /* Метод получения названия столбцов таблицы tableName */
        public string GetFields(string tableName)
        {
            string result = "";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand($"SELECT COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{tableName}'", con))
                {
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read()) result += '[' + dr[0].ToString() + ']' + ",";
                    }
                }
            }
            result = result.Remove(result.Length - 1, 1);
            return result;
        }

        /* Метод получения содержимого последней строки в таблице с индексом index */
        public string GetFieldsValues(int index)
        {
            string result = "", cellValue = "";
            for (int i = 0; i < dataGridViews[index].Columns.Count; i++)
            {
                cellValue = dataGridViews[index][i, dataGridViews[index].RowCount - 2].Value.ToString();
                result += $"'{DeleteSpaces(cellValue)}'" + ",";
            }
            result = result.Remove(result.Length - 1, 1);
            return result;
        }
        string GetCurrentFieldsValues(int index, int rowindex)
        {
            string result = "", cellValue = "";
            for (int i = 0; i < dataGridViews[index].Columns.Count; i++)
            {
                cellValue = dataGridViews[index][i, rowindex].Value.ToString();
                result += $"'{DeleteSpaces(cellValue)}'" + ",";
            }
            result = result.Remove(result.Length - 1, 1);
            return result;
        }
        // =====================================================

        public FormMain(FormAuth f)
        {
            InitializeComponent();
            Auth = f;
            dataBaseName = "AutoService";
            userName = Auth.LoginTB.Text;
            password = Auth.PasswordTB.Text; ;
            connectionString = $"Server=DESKTOP-4KUDERO\\SQLEXPRESS;Database={dataBaseName};Integrated Security=false;User Id={userName};Password={password};";
        }  
        
        /* Метод записи в БД через binding (не используется) только для обновления прав пользователям
        / все остальные операции, как добавление строки, изменение ячейки осуществляется через вызов ХП */
        void WriteDataBinding()
        {
            TabPage currentTab = tabControl.SelectedTab;
            string tName = currentTab.AccessibilityObject.Name;

            dataAdapter = new SqlDataAdapter($"SELECT * FROM {tName}", connectionString);
            SqlCommandBuilder cmdbl = new SqlCommandBuilder(dataAdapter);

            try
            {
                dataAdapter.UpdateCommand = cmdbl.GetUpdateCommand();
                dataAdapter.Update((DataTable)bs[tabControl.SelectedIndex].DataSource);
            }
            catch (SqlException ex)
            {
                //FormBox a = new FormBox(this, "error", "Ошибка обновления прав пользователей");
                //a.ShowDialog();
            }
        }

        /* Метод обновления содержимого всех таблиц 
         * Данный метод осуществляет повторный запрос на получение данных от SQL-сервера */
        public void UpdateAllTables()
        {
            for (int i = 0; i < tabControl.TabPages.Count; i++)
            {
                bs[i] = new BindingSource();
                GetData($"SELECT * FROM {tabControl.TabPages[i].Text}", bs[i]);
                try { dataGridViews[i].DataSource = bs[i]; }
                catch { }
                dataGridViews[i].Width = tabControl.TabPages[i].Width;
                dataGridViews[i].Height = tabControl.TabPages[i].Height;
                dataGridViews[i].Update();
            }
        }

        /* Метод получения списка таблиц базы данных
         * Данный метод осуществляет запрос к INFORMATION_SCHEMA.TABLES базы данных, в которой
         * содержатся все таблицы и возвращает список названий таблиц */
        public List<string> GetListDataFromSQL(string query)
        {
            List<string> list = new List<string>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read()) 
                            list.Add(dr[0].ToString());
                    }
                }
            }
            return list;
        }

        /* Метод получения данных с БД SQL-сервера
         * Данный момент осуществляет подключение к серверу SQL и запрос на возврат данных
         * в табличном виде (тип DataTable) в соответствии с SQL-запросом selectCommand (передается,
         * как формальный параметр) */
        private void GetData(string selectCommand, BindingSource bs)
        {
            try
            {
                dataAdapter = new SqlDataAdapter(selectCommand, connectionString);
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
                DataTable table = new DataTable
                {
                    Locale = CultureInfo.InvariantCulture
                };
                dataAdapter.Fill(table);
                bs.DataSource = table;
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /* Метод распределения прав пользователям, в соответствии с матрицей доступов (таблицей)
         * Данный метод осуществляет вызов хранимой процедуры GrantRights в соответствии с содержащимся
         * значением R, RWE, X в матрице доступов */
        public void GrantRights(string user, string table, string Right, string action)
        {
            connectionString = $"Server=DESKTOP-4KUDERO\\SQLEXPRESS;Database={dataBaseName};Integrated Security=false;User Id=sec_admin;Password=123;";
            string command = "";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                switch (Right)
                {
                    case "R":
                        command = $"EXEC GrantRights {user}, {table}, '{action}', 'SELECT'";
                        break;
                    case "RW":
                        if (table.Contains("View"))
                        {
                            command = $"EXEC GrantRights {user}, {table}, '{action}', 'SELECT'" +
                                  $" EXEC GrantRights {user}, {table}, '{action}', 'INSERT'" +
                                  $" EXEC GrantRights {user}, {table}, '{action}', 'UPDATE'" +
                                  $" EXEC GrantRights {user}, {table}, '{action}', 'DELETE'" +
                                  $" USE {dataBaseName}; {action} EXECUTE ON OBJECT::dbo.ChangeValue TO {user}";
                        }
                        else
                        {
                            command = $"EXEC GrantRights {user}, {table}, '{action}', 'SELECT'" +
                                  $" EXEC GrantRights {user}, {table}, '{action}', 'INSERT'" +
                                  $" EXEC GrantRights {user}, {table}, '{action}', 'UPDATE'" +
                                  $" EXEC GrantRights {user}, {table}, '{action}', 'DELETE'" +
                                  $" USE {dataBaseName}; {action} EXECUTE ON OBJECT::dbo.AddNew{table} TO {user}" +
                                  $" USE {dataBaseName}; {action} EXECUTE ON OBJECT::dbo.Delete{table} TO {user}" +
                                  $" USE {dataBaseName}; {action} EXECUTE ON OBJECT::dbo.InsertValue TO {user} " +
                                  $" USE {dataBaseName}; {action} EXECUTE ON OBJECT::dbo.ChangeValue TO {user} ";
                        }
                        break;
                    case "X":
                        action = "REVOKE";
                        if (table.Contains("View"))
                        {
                            command = $"EXEC GrantRights {user}, {table}, '{action}', 'SELECT'" +
                                  $" EXEC GrantRights {user}, {table}, '{action}', 'INSERT'" +
                                  $" EXEC GrantRights {user}, {table}, '{action}', 'UPDATE'" +
                                  $" EXEC GrantRights {user}, {table}, '{action}', 'DELETE'";
                        }
                        else
                        {
                            command = $"EXEC GrantRights {user}, {table}, '{action}', 'SELECT'" +
                                  $" EXEC GrantRights {user}, {table}, '{action}', 'INSERT'" +
                                  $" EXEC GrantRights {user}, {table}, '{action}', 'UPDATE'" +
                                  $" EXEC GrantRights {user}, {table}, '{action}', 'DELETE'" +
                                  $" USE {dataBaseName}; {action} EXECUTE ON OBJECT::dbo.AddNew{table} TO {user}" +
                                  $" USE {dataBaseName}; {action} EXECUTE ON OBJECT::dbo.Delete{table} TO {user}" +
                                  $" USE {dataBaseName}; {action} EXECUTE ON OBJECT::dbo.InsertValue TO {user} " +
                                  $" USE {dataBaseName}; {action} EXECUTE ON OBJECT::dbo.ChangeValue TO {user} ";
                        }
                        break;
                }
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(command, con);
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show($"Ошибка назначения прав пользователю {user}");
                }
            }
            connectionString = $"Server=DESKTOP-4KUDERO\\SQLEXPRESS;Database={dataBaseName};Integrated Security=false;User Id={userName};Password={password};";
        }

        /* Метод загрузки формы (окна со всем содержимым: кнопки, поля ввода, таблицы, вкладки */
        private void FormMain_Load(object sender, EventArgs e)
        {
            if (!Auth.asDepositor)
            {
                Text = "Пользователь: " + userName;

                string[] tableNames = File.ReadAllLines("tables.txt");

                GrantRights(userName, "LevelAccess", "R", "GRANT"); GrantRights(userName, "LevelConf", "R", "GRANT");

                List<string> s_levelAccess = GetListDataFromSQL($"SELECT [Уровень доступа] FROM LevelAccess WHERE [Пользователь] = '{userName}'");
                int levelAccess = Convert.ToInt32(s_levelAccess[0]);


                if (userName != "sec_admin")
                {
                    List<string> tmp = tableNames.ToList();
                    tmp.Remove("LevelAccess"); 
                    tmp.Remove("LevelConf");
                    tableNames = tmp.ToArray();
                }

                foreach (string table in tableNames)
                {
                    List<string> s_levelConf = GetListDataFromSQL($"SELECT [Уровень конфиденциальности] FROM LevelConf WHERE [Таблица] = '{table}'");
                    int levelConf = Convert.ToInt32(s_levelConf[0]);

                    if (table != "LevelConf")
                    {
                        if (levelAccess == levelConf) { GrantRights(userName, table, "X", "REVOKE"); GrantRights(userName, table, "RW", "GRANT"); }
                        else if (levelAccess < levelConf) { continue; }
                        else { GrantRights(userName, table, "RW", "REVOKE"); GrantRights(userName, table, "R", "GRANT"); ; }
                    }

                    Array.Resize(ref bs, bs.Length + 1);

                    bs[bs.Length - 1] = new BindingSource();

                    GetData($"SELECT * FROM {table}", bs[bs.Length - 1]);

                    Array.Resize(ref dataGridViews, dataGridViews.Length + 1);

                    dataGridViews[dataGridViews.Length - 1] = new DataGridView();
                    dataGridViews[dataGridViews.Length - 1].Location = new Point(0, 0);
                    dataGridViews[dataGridViews.Length - 1].DataSource = bs[bs.Length - 1];

                    tabControl.TabPages.Add(table);
                    tabControl.TabPages[tabControl.TabCount - 1].Controls.Add(dataGridViews[dataGridViews.Length - 1]);

                    dataGridViews[dataGridViews.Length - 1].Width = tabControl.TabPages[tabControl.TabCount - 1].Width;
                    dataGridViews[dataGridViews.Length - 1].BackgroundColor = Color.White;
                    dataGridViews[dataGridViews.Length - 1].AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dataGridViews[dataGridViews.Length - 1].Height = tabControl.TabPages[tabControl.TabCount - 1].Height;
                    dataGridViews[dataGridViews.Length - 1].ScrollBars = ScrollBars.Both;

                    // установка своего обработчика 
                    //dataGridViews[dataGridViews.Length - 1].SelectionChanged += DG_SelectionChanged;
                    dataGridViews[dataGridViews.Length - 1].CellClick += DG_CellClick;
                    dataGridViews[dataGridViews.Length - 1].CellEndEdit += DG_CellEndEdit;
                    dataGridViews[dataGridViews.Length - 1].ContextMenuStrip = contextMenuStrip;
                }
                tabControl_Selecting(dataGridViews[dataGridViews.Length - 1], null);
            }
            else
            {
                Text = "Клиент: " + Auth.passTB.Text;

                string[] queries = { $"SELECT [Дата заказа], [Государственный номер], Марка, Фамилия, Имя, Отчество, Название, Стоимость FROM dbo.DViewOrders WHERE dbo.DViewOrders.[Телефон] = '{Auth.passTB.Text}'" };

                string[] tableNames = { "Заказы на ремонт" };

                for (int i = 0; i < queries.Length; i++)
                {
                    Array.Resize(ref bs, bs.Length + 1);

                    bs[bs.Length - 1] = new BindingSource();

                    GetData(queries[i], bs[bs.Length - 1]);

                    Array.Resize(ref dataGridViews, dataGridViews.Length + 1);

                    dataGridViews[dataGridViews.Length - 1] = new DataGridView();
                    dataGridViews[dataGridViews.Length - 1].Location = new Point(0, 0);
                    dataGridViews[dataGridViews.Length - 1].DataSource = bs[bs.Length - 1];

                    tabControl.TabPages.Add(tableNames[i]);
                    tabControl.TabPages[tabControl.TabCount - 1].Controls.Add(dataGridViews[dataGridViews.Length - 1]);

                    dataGridViews[dataGridViews.Length - 1].Width = tabControl.TabPages[tabControl.TabCount - 1].Width;
                    dataGridViews[dataGridViews.Length - 1].BackgroundColor = Color.White;
                    dataGridViews[dataGridViews.Length - 1].AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dataGridViews[dataGridViews.Length - 1].Height = tabControl.TabPages[tabControl.TabCount - 1].Height;

                    dataGridViews[dataGridViews.Length - 1].ReadOnly = true;
                }
            }
        }

        /* Метод закрытия формы
         * Данный метод вызывается при закрытии формы и завершает процесс программы
        */
        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        /* Метод изменения значения в ячейке таблицы
         * Данный момент осуществляет вызов хранимой процедуры изменения значения ячейки */
        void ChangeValueCell(string tableName, string uniField, string uniFieldValue, string changeField, string changeFieldValue)
        {
            string command = "";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    command = $"EXEC ChangeValue {tableName}, {uniField}, {uniFieldValue}, {changeField}, {changeFieldValue}";
                    SqlCommand cmd = new SqlCommand(command, con);
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Ошибка изменения значения ячейки");
                }
            }
        }

        /* Метод добавления строки в таблицу 
         * Данный момент осуществляет вызов хранимой процедуры добавления строки */
        void AddString(string tableName, string fields, string values)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    string command = $@"EXEC InsertValue {tableName}, {string.Format("'{0}'", fields)}, ""{values}""";
                    SqlCommand cmd = new SqlCommand(command, con);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show($"Строка успешно добавлена в таблицу \"{tableName}\"!");

                    UpdateAllTables();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Ошибка добавления строки");
                }
            }
        }

        /* Метод удаления строки из таблицы
         * Данный момент осуществляет вызов хранимой процедуры удаления строки */
        void DeleteString(string tableName, string uniField, string uniFieldValue)
        {
            string command = "";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    command = $"EXEC DeleteValue {tableName}, {string.Format("[{0}]", uniField)}, {uniFieldValue}";
                    SqlCommand cmd = new SqlCommand(command, con);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Строка из таблицы \"{tableName}\" успешно удалена!");
                }
                catch (SqlException ex) 
                {
                    MessageBox.Show("Ошибка удаления строки"); 
                }
            }
        }

        /* Метод добавления пользователя */
        public void sp_addUser(string userName, string password, string fields, string values)
        {
            string command;
            if (userName != "")
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    try
                    {
                        con.Open();
                        command = $"EXEC AddNewLevelAccess {userName}, '{password}'";
                        SqlCommand cmd = new SqlCommand(command, con);
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = $"EXEC InsertValue {currenttableName}, '{fields}', \"{values}\"";
                        cmd.ExecuteNonQuery();

                        MessageBox.Show($"Пользователь \"{userName}\" успешно добавлен!");
                        UpdateAllTables();
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Ошибка добавления пользователя");
                    }
                }
            }
            else MessageBox.Show("Указано некорректное имя пользователя", "Ошибка");
        }

        /* Метод удаления пользователя */
        void DeleteUser(string userName, string uniField, string uniFieldValue)
        {
            string command = "";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    command = $"EXEC DeleteLevelAccess {userName}";
                    SqlCommand cmd = new SqlCommand(command, con);
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = $"EXEC DeleteValue {currenttableName}, {uniField}, {uniFieldValue}";
                    cmd.ExecuteNonQuery();

                    MessageBox.Show($"Пользователь \"{userName}\" успешно удален!");
                    UpdateAllTables();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Ошибка удаления пользователя");
                }
            }
        }

        private void DG_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridViews[selectedTab].Columns[0].ReadOnly = true;
        }

        private void DG_SelectionChanged(object sender, EventArgs e)
        {
            string uniField = '[' + dataGridViews[selectedTab].Columns[0].Name + ']';
            string uniFieldValue = "'" + dataGridViews[selectedTab][0, dataGridViews[selectedTab].CurrentCell.RowIndex].Value.ToString() + "'";
            string changeField = '[' + dataGridViews[selectedTab].Columns[dataGridViews[selectedTab].CurrentCell.ColumnIndex].Name + ']';

            string changeFieldValue = "'" + dataGridViews[selectedTab].CurrentCell.Value + "'";

            ChangeValueCell(currenttableName, uniField, uniFieldValue, changeField, changeFieldValue);
            UpdateAllTables();
        }

        private void DG_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string uniField = '[' + dataGridViews[selectedTab].Columns[0].Name + ']';
            string uniFieldValue = "'" + dataGridViews[selectedTab][0, dataGridViews[selectedTab].CurrentCell.RowIndex].Value.ToString() + "'";
            string changeField = '[' + dataGridViews[selectedTab].Columns[dataGridViews[selectedTab].CurrentCell.ColumnIndex].Name + ']';         
            string changeFieldValue = "'" + dataGridViews[selectedTab].CurrentCell.Value + "'";

            ChangeValueCell(currenttableName, uniField, uniFieldValue, changeField, changeFieldValue);
            UpdateAllTables();
        }

        // обработчик выбора вкладки
        private void tabControl_Selecting(object sender, TabControlCancelEventArgs e)
        {
            currentTab = tabControl.SelectedTab;
            currenttableName = currentTab.AccessibilityObject.Name;
            selectedTab = tabControl.TabPages.IndexOf(tabControl.SelectedTab);

            switch (currenttableName)
            {
                case "Clients":
                    AddToolStripMenuItem.Text = "Добавить клиента в таблицу"; AddToolStripMenuItem.Enabled = true;
                    DelToolStripMenuItem.Text = "Удалить клиента из таблицы"; DelToolStripMenuItem.Enabled = true;
                    codeCall = 1;
                    break;
                
                case "Masters":
                    AddToolStripMenuItem.Text = "Добавить мастера в таблицу"; AddToolStripMenuItem.Enabled = true;
                    DelToolStripMenuItem.Text = "Удалить мастера из таблицы"; DelToolStripMenuItem.Enabled = true;
                    codeCall = 2;
                    break;

                case "Orders":
                    AddToolStripMenuItem.Text = "Добавить заказ в таблицу"; AddToolStripMenuItem.Enabled = true;
                    DelToolStripMenuItem.Text = "Удалить заказ из таблицы"; DelToolStripMenuItem.Enabled = true;
                    codeCall = 3;
                    break;
                
                case "Serv_Orders":
                    AddToolStripMenuItem.Text = "Добавить соответствие в таблицу"; AddToolStripMenuItem.Enabled = true;
                    DelToolStripMenuItem.Text = "Удалить соответствие из таблицы"; DelToolStripMenuItem.Enabled = true;
                    codeCall = 4;
                    break;

                case "Services":
                    AddToolStripMenuItem.Text = "Добавить услугу в таблицу"; AddToolStripMenuItem.Enabled = true;
                    DelToolStripMenuItem.Text = "Удалить услугу из таблицы"; DelToolStripMenuItem.Enabled = true;
                    codeCall = 5;
                    break;

                case "Vehicles":
                    AddToolStripMenuItem.Text = "Добавить автомобиль в таблицу"; AddToolStripMenuItem.Enabled = true;
                    DelToolStripMenuItem.Text = "Удалить автомобиль из таблицы"; DelToolStripMenuItem.Enabled = true;
                    codeCall = 6;
                    break;

                case "LevelAccess":
                    AddToolStripMenuItem.Text = "Добавить пользователя в таблицу"; AddToolStripMenuItem.Enabled = true;
                    DelToolStripMenuItem.Text = "Удалить пользователя из таблицы"; DelToolStripMenuItem.Enabled = true;
                    codeCall = 7;
                    break;

                default:
                    AddToolStripMenuItem.Text = "Операция не предусмотрена"; AddToolStripMenuItem.Enabled = false;
                    DelToolStripMenuItem.Text = "Операция не предусмотрена"; DelToolStripMenuItem.Enabled = false;
                    codeCall = -1;
                    break;
            }
        }

        private void updateBtn_Click(object sender, EventArgs e)
        {
            UpdateAllTables();
        }

        /* Метод вызова хранимых процедур из БД */
        public void sp_Call(string nameSP, string[] parameters, string[] values)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(nameSP, con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    for (int i = 0; i < parameters.Length; i++)
                    {
                        parameters[i] = parameters[i].Remove(0, 1);
                        parameters[i] = parameters[i].Remove(parameters[i].Length - 1, 1);
                        parameters[i] = parameters[i].Insert(0, "@");
                        parameters[i] = parameters[i].Replace('-', '_');
                        parameters[i] = parameters[i].Replace(' ', '_');

                        values[i] = values[i].Remove(0, 1);
                        values[i] = values[i].Remove(values[i].Length - 1, 1);

                        cmd.Parameters.AddWithValue(parameters[i], values[i]);
                    }
                    SqlParameter outParameter = new SqlParameter();
                    outParameter.ParameterName = "@Out";
                    outParameter.SqlDbType = System.Data.SqlDbType.NVarChar;
                    outParameter.Size = 100;
                    outParameter.Value = "0";
                    outParameter.Direction = System.Data.ParameterDirection.Output;
                    cmd.Parameters.Add(outParameter);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show(cmd.Parameters["@Out"].Value.ToString());
                    UpdateAllTables();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Данная операция запрещена политикой безопасности");
                }
            }
        }

        private void UpdStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateAllTables();
        }

        private void AddToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormInput f = new FormInput(this);
            f.ShowDialog();
        }

        private void DelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string userName, uniField, uniFieldValue;
            string[] parameters = null;
            string[] values = null;
            switch (codeCall)
            {
                // AccessMatrix
                case 7:
                    userName = DeleteSpaces(dataGridViews[selectedTab][0, dataGridViews[selectedTab].CurrentCell.RowIndex].Value.ToString());
                    uniField = dataGridViews[selectedTab].Columns[0].Name;
                    uniFieldValue = dataGridViews[selectedTab][0, dataGridViews[selectedTab].CurrentCell.RowIndex].Value.ToString();
                    DeleteUser(userName, uniField, uniFieldValue);
                    break;

                // Clients
                case 1:

                    uniField = string.Format("[{0}]", dataGridViews[selectedTab].Columns[0].Name);
                    uniFieldValue = string.Format("'{0}'", dataGridViews[selectedTab][0, dataGridViews[selectedTab].CurrentCell.RowIndex].Value.ToString());
                    Array.Resize(ref parameters, 1); parameters[parameters.Length - 1] = uniField;
                    Array.Resize(ref values, 1); values[values.Length - 1] = uniFieldValue;
                    sp_Call("DeleteClients", parameters, values);
                    break;

                // Masters
                case 2:
                    uniField = string.Format("[{0}]", dataGridViews[selectedTab].Columns[0].Name);
                    uniFieldValue = string.Format("'{0}'", dataGridViews[selectedTab][0, dataGridViews[selectedTab].CurrentCell.RowIndex].Value.ToString());
                    Array.Resize(ref parameters, 1); parameters[parameters.Length - 1] = uniField;
                    Array.Resize(ref values, 1); values[values.Length - 1] = uniFieldValue;
                    sp_Call("DeleteMasters", parameters, values);
                    break;

                // Orders
                case 3:
                    uniField = string.Format("[{0}]", dataGridViews[selectedTab].Columns[0].Name);
                    uniFieldValue = string.Format("'{0}'", dataGridViews[selectedTab][0, dataGridViews[selectedTab].CurrentCell.RowIndex].Value.ToString());
                    Array.Resize(ref parameters, 1); parameters[parameters.Length - 1] = uniField;
                    Array.Resize(ref values, 1); values[values.Length - 1] = uniFieldValue;
                    sp_Call("DeleteOrders", parameters, values);
                    break;

                // Serv_Orders
                case 4:
                    uniField = string.Format("[{0}]", dataGridViews[selectedTab].Columns[0].Name);
                    uniFieldValue = string.Format("'{0}'", dataGridViews[selectedTab][0, dataGridViews[selectedTab].CurrentCell.RowIndex].Value.ToString());
                    Array.Resize(ref parameters, 1); parameters[parameters.Length - 1] = uniField;
                    Array.Resize(ref values, 1); values[values.Length - 1] = uniFieldValue;
                    sp_Call("DeleteServ_Orders", parameters, values);
                    break;

                // Services
                case 5:
                    uniField = string.Format("[{0}]", dataGridViews[selectedTab].Columns[0].Name);
                    uniFieldValue = string.Format("'{0}'", dataGridViews[selectedTab][0, dataGridViews[selectedTab].CurrentCell.RowIndex].Value.ToString());
                    Array.Resize(ref parameters, 1); parameters[parameters.Length - 1] = uniField;
                    Array.Resize(ref values, 1); values[values.Length - 1] = uniFieldValue;
                    sp_Call("DeleteServices", parameters, values);
                    break;

                // Vehicles
                case 6:
                    uniField = string.Format("[{0}]", dataGridViews[selectedTab].Columns[0].Name);
                    uniFieldValue = string.Format("'{0}'", dataGridViews[selectedTab][0, dataGridViews[selectedTab].CurrentCell.RowIndex].Value.ToString());
                    Array.Resize(ref parameters, 1); parameters[parameters.Length - 1] = uniField;
                    Array.Resize(ref values, 1); values[values.Length - 1] = uniFieldValue;
                    sp_Call("DeleteVehicles", parameters, values);
                    break;
            }
        }

        private void BackupStripMenuItem_Click(object sender, EventArgs e)
        {
            string command;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    command = $"BACKUP DATABASE {dataBaseName} TO DISK = '{dataBaseName}_actual.bak' WITH FORMAT";
                    SqlCommand cmd = new SqlCommand(command, con);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Резервное копирование базы данных успешно завершено!");
                    UpdateAllTables();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("error", ex.Message);
                }
            }
        }

        private void RestoreStripMenuItem_Click(object sender, EventArgs e)
        {
            string command;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    command = $"USE master ALTER DATABASE {dataBaseName} SET SINGLE_USER WITH ROLLBACK IMMEDIATE " +
                        $"USE master RESTORE DATABASE {dataBaseName} from Disk = '{dataBaseName}_actual.bak' " +
                        $"WITH REPLACE USE master ALTER DATABASE {dataBaseName} SET MULTI_USER";
                    SqlCommand cmd = new SqlCommand(command, con);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Восстановление базы данных успешно завершено! Требуется повторная авторизация");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormInput a = new FormInput(this);
            a.ShowDialog();
        }

        private void addBtn_Click(object sender, EventArgs e)
        {
            
        }
    }
}
