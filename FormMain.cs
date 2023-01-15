using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
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
        string GetFieldsValues(int index)
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
            dataBaseName = Auth.DataBaseTB.Text;
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

                FormBox a = new FormBox(this, "success", $"Права пользователей успешно обновлены!");
                a.ShowDialog();
            }
            catch (SqlException ex)
            {
                FormBox a = new FormBox(this, "error", "Ошибка обновления прав пользователей");
                a.ShowDialog();
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
                dataGridViews[i].DataSource = bs[i];
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
                                  //$" USE {dataBaseName}; {action} EXECUTE ON OBJECT::dbo.AddNew{table} TO {user}" +
                                  //$" USE {dataBaseName}; {action} EXECUTE ON OBJECT::dbo.Delete{table} TO {user}" +
                                  $" USE {dataBaseName}; {action} EXECUTE ON OBJECT::dbo.ChangeValue TO {user}";
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
                                  $" USE {dataBaseName}; {action} EXECUTE ON OBJECT::dbo.ChangeValue TO {user}";
                            // $" USE {dataBaseName}; {action} EXECUTE ON OBJECT::dbo.AddNew{table} TO {user}" +
                            //$" USE {dataBaseName}; {action} EXECUTE ON OBJECT::dbo.Delete{table} TO {user}";
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
                    FormBox a = new FormBox(this, "error", $"Ошибка назначения прав пользователю {user}");
                    a.ShowDialog();
                }
            }
            connectionString = $"Server=DESKTOP-4KUDERO\\SQLEXPRESS;Database={dataBaseName};Integrated Security=false;User Id={userName};Password={password};";
        }

        /* Метод загрузки формы (окна со всем содержимым: кнопки, поля ввода, таблицы, вкладки */
        private void FormMain_Load(object sender, EventArgs e)
        {
            if (!Auth.asDepositor)
            {
                Text = "Клиент взаимодействия с БД - " + dataBaseName + " | Пользователь: " + userName;

                string[] tableNames = File.ReadAllLines("tables.txt");
                List<string> s_levelAccess = GetListDataFromSQL($"SELECT [Уровень доступа] FROM LevelAccess WHERE [Пользователь] = '{userName}'");
                int levelAccess = Convert.ToInt32(s_levelAccess[0]);


                if (userName != "sec_admin")
                {
                    UpdateRightsBtn.Enabled = false;          
                    backupBtn.Enabled = false;
                    restoreBtn.Enabled = false;

                }
                foreach (string table in tableNames)
                {
                    List<string> s_levelConf = GetListDataFromSQL($"SELECT [Уровень конфиденциальности] FROM LevelConf WHERE [Таблица] = '{table}'");
                    int levelConf = Convert.ToInt32(s_levelConf[0]);

                    if (levelAccess == levelConf) { GrantRights(userName, table, "X", "REVOKE"); GrantRights(userName, table, "RW", "GRANT"); }
                    else if (levelAccess < levelConf) { continue; }
                    else { GrantRights(userName, table, "RW", "REVOKE"); GrantRights(userName, table, "R", "GRANT"); ; }


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
                    dataGridViews[dataGridViews.Length - 1].CellClick += DG_CellClick;
                    dataGridViews[dataGridViews.Length - 1].CellEndEdit += DG_CellEndEdit;
                }
                tabControl_Selecting(null, null);
            }
            else
            {
                Text = "Клиент взаимодействия с БД - " + dataBaseName + " | Вкладчик с паспортом: " + Auth.passTB.Text;

                ChangeValueBtn.Hide(); updateBtn.Hide(); AddStringBtn.Hide(); DeleteStringBtn.Hide();
                UpdateRightsBtn.Hide(); addBtn.Hide(); delBtn.Hide(); backupBtn.Hide();
                Height -= 100;

                string[] queries = { $"SELECT dbo.DViewInWork.Тип, dbo.DViewInWork.[Год открытия], dbo.DViewInWork.Сумма, dbo.DViewInWork.Наименование, dbo.DViewInWork.Консультант FROM DViewInWork WHERE dbo.DViewInWork.[Паспорт владельца] = '{Auth.passTB.Text}'",
                $"SELECT [Тип], [Год открытия], [Сумма] FROM dbo.DViewAll WHERE [Паспорт] =  '{Auth.passTB.Text}'"};

                string[] tableNames = { "Вклады в очереди", "Все вклады" };

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

        /* Метод обновления прав в таблице с матрицей доступов
         * Данный метод осуществляет вызов метода распределения прав пользователей в соответствии
         * со значениями R, W, RW в таблице с матрицей доступов */
        private void UpdateRights_Click(object sender, EventArgs e)
        {
            int cc = dataGridViews[selectedTab].ColumnCount;
            int rw = dataGridViews[selectedTab].RowCount;

            string tableName = "", userName = "", Right = "";

            for (int i = 1; i < cc; i++)
            {
                tableName = dataGridViews[selectedTab].Columns[i].Name; tableName = tableName.Replace("table_", "");

                for (int j = 0; j < rw - 1; j++)
                {
                    userName = dataGridViews[selectedTab][0, j].Value.ToString(); userName = userName.Replace(" ", ""); 
                    Right = dataGridViews[selectedTab][i, j].Value.ToString(); Right = Right.Replace(" ", "");
                    GrantRights(userName, tableName, Right, "GRANT");
                }
            }
            WriteDataBinding();
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
                    UpdateAllTables();
                }
                catch (SqlException ex)
                {
                    FormBox a = new FormBox(this, "error", "Ошибка изменения значения ячейки");
                    a.ShowDialog();
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

                    FormBox a = new FormBox(this, "success", $"Строка успешно добавлена в таблицу \"{tableName}\"!");
                    a.ShowDialog();

                    UpdateAllTables();
                }
                catch (SqlException ex)
                {
                    FormBox a = new FormBox(this, "error", "Ошибка добавления строки");
                    a.ShowDialog();
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

                    FormBox a = new FormBox(this, "success", $"Строка из таблицы \"{tableName}\" успешно удалена!");
                    a.ShowDialog();
                    UpdateAllTables();
                }
                catch (SqlException ex) 
                {
                    FormBox a = new FormBox(this, "error", "Ошибка удаления строки"); 
                    a.ShowDialog(); 
                }
            }
        }

        /* Метод добавления пользователя */
        public void sp_addUser(string userName, string password, string fields, string values)
        {
            string command;
            if (userName != "")
            {
                for (int i = 1; i <= dataGridViews[selectedTab].Columns.Count - 1; i++)
                    values += ", 'X'";

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    try
                    {
                        con.Open();
                        command = $"EXEC AddNewAccessMatrix {userName}, '{password}'";
                        SqlCommand cmd = new SqlCommand(command, con);
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = $"EXEC InsertValue {currenttableName}, '{fields}', \"{values}\"";
                        cmd.ExecuteNonQuery();

                        FormBox a = new FormBox(this, "success", $"Пользователь \"{userName}\" успешно добавлен!");
                        a.ShowDialog();
                        UpdateAllTables();
                    }
                    catch (SqlException ex)
                    {
                        FormBox a = new FormBox(this, "error", "Ошибка добавления пользователя");
                        a.ShowDialog();
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
                    command = $"EXEC DeleteAccessMatrix {userName}";
                    SqlCommand cmd = new SqlCommand(command, con);
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = $"EXEC DeleteValue {currenttableName}, {uniField}, {uniFieldValue}";
                    cmd.ExecuteNonQuery();

                    FormBox a = new FormBox(this, "success", $"Пользователь \"{userName}\" успешно удален!");
                    a.ShowDialog();
                    UpdateAllTables();
                }
                catch (SqlException ex)
                {
                    FormBox a = new FormBox(this, "error", "Ошибка удаления пользователя");
                    a.ShowDialog();
                }
            }
        }

        // обработчик нажатия на кнопку "Изменить значение"
        private void ChangeValueBtn_Click(object sender, EventArgs e)
        {
            string uniField = '[' + dataGridViews[selectedTab].Columns[0].Name + ']';
            string uniFieldValue = "'" + dataGridViews[selectedTab][0, dataGridViews[selectedTab].CurrentCell.RowIndex].Value.ToString() + "'";
            string changeField = '[' + dataGridViews[selectedTab].Columns[dataGridViews[selectedTab].CurrentCell.ColumnIndex].Name + ']';
            //string changeFieldValue = "'" + CellValueTB.Text + "'";

            //ChangeValueCell(currenttableName, uniField, uniFieldValue, changeField, changeFieldValue);
        }

        // обработчик нажатия на ячейку таблицы
        private void DG_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //CellValueTB.Text = DeleteSpaces((sender as DataGridView).CurrentCell.Value.ToString());
        }

        private void DG_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string uniField = '[' + dataGridViews[selectedTab].Columns[0].Name + ']';
            string uniFieldValue = "'" + dataGridViews[selectedTab][0, dataGridViews[selectedTab].CurrentCell.RowIndex].Value.ToString() + "'";
            string changeField = '[' + dataGridViews[selectedTab].Columns[dataGridViews[selectedTab].CurrentCell.ColumnIndex].Name + ']';

            string changeFieldValue = "'" + dataGridViews[selectedTab].CurrentCell.Value + "'";

            ChangeValueCell(currenttableName, uniField, uniFieldValue, changeField, changeFieldValue);

        }

        // обработчик нажатия на кнопку "Добавить строку"
        private void AddStringBtn_Click(object sender, EventArgs e)
        { 
            string fields = GetFields(currenttableName);
            string values = GetFieldsValues(selectedTab);
            AddString(currenttableName, fields, values);
        } 

        // обработчик нажатия на кнопку "Удалить строку"
        private void DeleteStringBtn_Click(object sender, EventArgs e)
        {
            string uniField = dataGridViews[selectedTab].Columns[0].Name;
            string uniFieldValue = dataGridViews[selectedTab][0, dataGridViews[selectedTab].CurrentCell.RowIndex].Value.ToString();
            DeleteString(currenttableName, uniField, uniFieldValue);
        }

        // обработчик выбора вкладки
        private void tabControl_Selecting(object sender, TabControlCancelEventArgs e)
        {
            currentTab = tabControl.SelectedTab;
            currenttableName = currentTab.AccessibilityObject.Name;
            selectedTab = tabControl.TabPages.IndexOf(tabControl.SelectedTab);

            switch (currenttableName)
            {
                case "AccessMatrix":
                    addBtn.Text = "Добавить пользователя";
                    delBtn.Text = "Удалить пользователя";
                    addBtn.Enabled = true; delBtn.Enabled = true;
                    codeCall = 1;
                    break;
                
                case "Consultants":
                    addBtn.Text = "Добавить консультанта";
                    delBtn.Text = "Удалить консультанта";
                    addBtn.Enabled = true; delBtn.Enabled = true;
                    codeCall = 2;
                    break;

                case "Depositors":
                    addBtn.Text = "Добавить вкладчика";
                    delBtn.Text = "Удалить вкладчика";
                    addBtn.Enabled = true; delBtn.Enabled = true;
                    codeCall = 3;
                    break;
                
                case "Deposits":
                    addBtn.Text = "Добавить вклад";
                    delBtn.Text = "Удалить вклад";
                    addBtn.Enabled = true; delBtn.Enabled = true;
                    codeCall = 4;
                    break;

                case "Req_Servs":
                    addBtn.Text = "Добавить услугу к заявке";
                    delBtn.Text = "Удалить заявку с услугой";
                    addBtn.Enabled = true; delBtn.Enabled = true;
                    codeCall = 5;
                    break;

                case "Requests":
                    addBtn.Text = "Добавить заявку";
                    delBtn.Text = "Удалить заявку";
                    addBtn.Enabled = true; delBtn.Enabled = true;
                    codeCall = 6;
                    break;

                case "Services":
                    addBtn.Text = "Добавить услугу";
                    delBtn.Text = "Удалить услугу";
                    addBtn.Enabled = true; delBtn.Enabled = true;
                    codeCall = 7;
                    break;

                default:
                    addBtn.Text = "Операция не предусмотрена";
                    delBtn.Text = "Операция не предусмотрена";
                    addBtn.Enabled = false; delBtn.Enabled = false;
                    break;
            }
        }

        private void updateBtn_Click(object sender, EventArgs e)
        {
            UpdateAllTables();
        }

        /* Метод вызова хранимых процедур из БД */
        void sp_Call(string nameSP, string[] parameters, string[] values)
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

                    FormBox a = new FormBox(this, "warning", cmd.Parameters["@Out"].Value.ToString());
                    a.ShowDialog();

                    UpdateAllTables();
                }
                catch (SqlException ex)
                {
                    FormBox a = new FormBox(this, "error", "Данная операция запрещена политикой безопасности");
                    a.ShowDialog();
                }
            }
        }

        private void delBtn_Click(object sender, EventArgs e)
        {
            string userName, uniField, uniFieldValue;
            string[] parameters = null;
            string[] values = null;
            switch (codeCall)
            {
                // AccessMatrix
                case 1:
                    userName = DeleteSpaces(dataGridViews[selectedTab][0, dataGridViews[selectedTab].CurrentCell.RowIndex].Value.ToString());
                    uniField = dataGridViews[selectedTab].Columns[0].Name;
                    uniFieldValue = dataGridViews[selectedTab][0, dataGridViews[selectedTab].CurrentCell.RowIndex].Value.ToString();
                    DeleteUser(userName, uniField, uniFieldValue);
                    break;

                // Consultants
                case 2:

                    uniField = string.Format("[{0}]", dataGridViews[selectedTab].Columns[0].Name);
                    uniFieldValue = string.Format("'{0}'", dataGridViews[selectedTab][0, dataGridViews[selectedTab].CurrentCell.RowIndex].Value.ToString());
                    Array.Resize(ref parameters, 1); parameters[parameters.Length - 1] = uniField;
                    Array.Resize(ref values, 1); values[values.Length - 1] = uniFieldValue;
                    sp_Call("DeleteConsultants", parameters, values);
                    break;

                // Depositors
                case 3:
                    uniField = string.Format("[{0}]", dataGridViews[selectedTab].Columns[0].Name);
                    uniFieldValue = string.Format("'{0}'", dataGridViews[selectedTab][0, dataGridViews[selectedTab].CurrentCell.RowIndex].Value.ToString());
                    Array.Resize(ref parameters, 1); parameters[parameters.Length - 1] = uniField;
                    Array.Resize(ref values, 1); values[values.Length - 1] = uniFieldValue;
                    sp_Call("DeleteDepositors", parameters, values);
                    break;

                // Deposits
                case 4:
                    uniField = string.Format("[{0}]", dataGridViews[selectedTab].Columns[0].Name);
                    uniFieldValue = string.Format("'{0}'", dataGridViews[selectedTab][0, dataGridViews[selectedTab].CurrentCell.RowIndex].Value.ToString());
                    Array.Resize(ref parameters, 1); parameters[parameters.Length - 1] = uniField;
                    Array.Resize(ref values, 1); values[values.Length - 1] = uniFieldValue;
                    sp_Call("DeleteDeposits", parameters, values);
                    break;

                // Req_Serv
                case 5:
                    //parameters = GetFields(currenttableName).Split(',');
                    //values = GetCurrentFieldsValues(selectedTab, dataGridViews[selectedTab].CurrentCell.RowIndex).Split(',');
                    uniField = string.Format("[{0}]", dataGridViews[selectedTab].Columns[0].Name);
                    uniFieldValue = string.Format("'{0}'", dataGridViews[selectedTab][0, dataGridViews[selectedTab].CurrentCell.RowIndex].Value.ToString());
                    Array.Resize(ref parameters, 1); parameters[parameters.Length - 1] = uniField;
                    Array.Resize(ref values, 1); values[values.Length - 1] = uniFieldValue;
                    sp_Call("DeleteReq_Servs", parameters, values);
                    break;

                // Requests
                case 6:
                    uniField = string.Format("[{0}]", dataGridViews[selectedTab].Columns[0].Name);
                    uniFieldValue = string.Format("'{0}'", dataGridViews[selectedTab][0, dataGridViews[selectedTab].CurrentCell.RowIndex].Value.ToString());
                    Array.Resize(ref parameters, 1); parameters[parameters.Length - 1] = uniField;
                    Array.Resize(ref values, 1); values[values.Length - 1] = uniFieldValue;
                    sp_Call("DeleteRequests", parameters, values);
                    break;

                // Services
                case 7:
                    uniField = string.Format("[{0}]", dataGridViews[selectedTab].Columns[0].Name);
                    uniFieldValue = string.Format("'{0}'", dataGridViews[selectedTab][0, dataGridViews[selectedTab].CurrentCell.RowIndex].Value.ToString());
                    Array.Resize(ref parameters, 1); parameters[parameters.Length - 1] = uniField;
                    Array.Resize(ref values, 1); values[values.Length - 1] = uniFieldValue;
                    sp_Call("DeleteServices", parameters, values);
                    break;
            }
        }

        private void backupBtn_Click(object sender, EventArgs e)
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

                    FormBox a = new FormBox(this, "success", "Резервное копирование базы данных успешно завершено!");
                    a.ShowDialog();
                    UpdateAllTables();
                }
                catch (Exception ex)
                {
                    FormBox a = new FormBox(this, "error", ex.Message);
                    a.ShowDialog();
                }
            }
        }

        private void restoreBtn_Click(object sender, EventArgs e)
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

                    FormBox a = new FormBox(this, "success", "Восстановление базы данных успешно завершено! Требуется повторная авторизация");
                    a.ShowDialog();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка");
                }
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            //FormBox a = new FormBox(this, "error");
            
            //a.ShowDialog();
        }

        private void addBtn_Click(object sender, EventArgs e)
        {
            string[] parameters;
            string[] values;
            switch (codeCall)
            {
                // AccessMatrix
                case 1:
                    FormBox a = new FormBox(this, "input", "Введите пароль для пользователя");
                    a.ShowDialog();
                    break;

                // Consultants
                case 2:
                    parameters = GetFields(currenttableName).Split(',');
                    values = GetFieldsValues(selectedTab).Split(',');
                    sp_Call("AddNewConsultants", parameters, values);
                    break;

                // Depositors
                case 3:
                    parameters = GetFields(currenttableName).Split(',');
                    values = GetFieldsValues(selectedTab).Split(',');
                    sp_Call("AddNewDepositors", parameters, values);
                    break;

                // Deposits
                case 4:
                    parameters = GetFields(currenttableName).Split(',');
                    values = GetFieldsValues(selectedTab).Split(',');
                    sp_Call("AddNewDeposits", parameters, values);
                    break;

                // Req_Serv
                case 5:
                    parameters = GetFields(currenttableName).Split(',');
                    values = GetFieldsValues(selectedTab).Split(',');
                    sp_Call("AddNewReq_Servs", parameters, values);
                    break;

                // Requests
                case 6:
                    parameters = GetFields(currenttableName).Split(',');
                    values = GetFieldsValues(selectedTab).Split(',');
                    sp_Call("AddNewRequests", parameters, values);
                    break;

                // Services
                case 7:
                    parameters = GetFields(currenttableName).Split(',');
                    values = GetFieldsValues(selectedTab).Split(',');
                    sp_Call("AddNewServices", parameters, values);
                    break;
            }
        }
    }
}
