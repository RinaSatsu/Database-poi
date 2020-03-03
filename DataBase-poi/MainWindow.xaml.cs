using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace DataBase_poi
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Fields

        static string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        SqlConnection connection;

        private DataSet companyDataSet;
        private SqlDataAdapter departmentAdapter;
        private SqlDataAdapter employeeAdapter;

        #endregion


        public MainWindow()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            textBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
        }

        private void MainWind_Loaded(object sender, RoutedEventArgs e)
        {

            //connection = new SqlConnection(connectionString);

            //companyDataSet = new DataSet("CompanyEdit");

            //departmentAdapter = new SqlDataAdapter("SELECT * FROM Departments;", connection);
            //InsertDepartmentAdapter(connection, departmentAdapter);
            //departmentAdapter.Fill(companyDataSet, "Departments");

            //employeeAdapter = new SqlDataAdapter($"SELECT * FROM Employees WHERE Department = @DeparmentId;", connection);
            //InsertEmployeeAdapter(connection, employeeAdapter);
            //UpdateEmployeeAdapter(connection, employeeAdapter);
            //companyDataSet.Tables.Add("Employees");
            //companyDataSet.Tables.Add("EmployeesMove1");
            //companyDataSet.Tables.Add("EmployeesMove2");

            //Departments_comboBox.ItemsSource = companyDataSet.Tables["Departments"].DefaultView;
            //DepartmentsMove1_comboBox.ItemsSource = companyDataSet.Tables["Departments"].DefaultView;
            //DepartmentsMove2_comboBox.ItemsSource = companyDataSet.Tables["Departments"].DefaultView;
        }


    #region ViewEditTab

    #region View

    private void Departments_comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //companyDataSet.Tables["Employees"].Rows.Clear();

            //employeeAdapter.SelectCommand.CommandText = $"SELECT * FROM Employees WHERE Department = {Departments_comboBox.SelectedValue};";
            ////employeeAdapter.SelectCommand.Parameters.Clear();

            ////SqlParameter param = new SqlParameter("@DepartmentId", SqlDbType.Int, -1);
            ////param.Value = Departments_comboBox.SelectedValue;
            ////employeeAdapter.SelectCommand.Parameters.Add(param);

            ////employeeAdapter.SelectCommand.Parameters.AddWithValue("@DepartmentId", Departments_comboBox.SelectedValue);
            //employeeAdapter.Fill(companyDataSet, "Employees");
            //Employees_listView.ItemsSource = companyDataSet.Tables["Employees"].DefaultView;
        }

        #endregion


        #region Buttons

        private void AddDepartment_button_Click(object sender, RoutedEventArgs e)
        {
            
            //try
            //{
            //    DataRow newRow;
            //    if (Department_textBox.Text == "")
            //        newRow = AddDepartment();
            //    else
            //        newRow = AddDepartment(Department_textBox.Text);
            //    companyDataSet.Tables["Departments"].Rows.Add(newRow);
            //    departmentAdapter.Update(companyDataSet, "Departments");
            //}
            //catch (ArgumentException)
            //{
            //    MessageBox.Show("Invalid department Id");
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }

        private void AddEmployee_button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(EmployeeName_textBox.Text);
            //if (Departments_comboBox.SelectedValue == null) return;
            //if ((EmployeeName_textBox.Text == companyDataSet.Tables["Employees"].Rows[Employees_listView.SelectedIndex]["Name"].ToString()) && 
            //    (EmployeeAge_textBox.Text == companyDataSet.Tables["Employees"].Rows[Employees_listView.SelectedIndex]["Age"].ToString()) && 
            //    (EmployeeSalary_textBox.Text == companyDataSet.Tables["Employees"].Rows[Employees_listView.SelectedIndex]["Salary"].ToString())) return;
            //try
            //{
            //    DataRow newRow;
            //    int departmentId = Convert.ToInt32(Departments_comboBox.SelectedValue);
            //    int departmentCode = Convert.ToInt32((from dep in companyDataSet.Tables["Departments"].AsEnumerable()
            //                  where dep.Field<Int32>("Id") == Convert.ToInt32(Departments_comboBox.SelectedValue)
            //                  select dep["Code"]).First());
            //    if ((EmployeeName_textBox.Text == "") || (EmployeeAge_textBox.Text == "") || (EmployeeSalary_textBox.Text == ""))
            //        newRow = AddEmployee(departmentId, departmentCode);
            //    else
            //        newRow = AddEmployee(EmployeeName_textBox.Text, int.Parse(EmployeeAge_textBox.Text), double.Parse(EmployeeSalary_textBox.Text), departmentId, departmentCode);

            //    companyDataSet.Tables["Employees"].Rows.Add(newRow);
            //    employeeAdapter.Update(companyDataSet, "Employees");

            //}
            //catch (ArgumentOutOfRangeException)
            //{
            //    MessageBox.Show("Invalid department Id");
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
            //finally
            //{
            //    EmployeeCode_textBlock.Text = "";
            //    EmployeeName_textBox.Text = "";
            //    EmployeeAge_textBox.Text = "";
            //    EmployeeSalary_textBox.Text = "";
            //}
        }

        private void EditEmployee_button_Click(object sender, RoutedEventArgs e)
        {
            if (Departments_comboBox.SelectedValue == null) return;
            try
            {
                if ((EmployeeName_textBox.Text == "") || (EmployeeAge_textBox.Text == "") || (EmployeeSalary_textBox.Text == ""))
                    throw new Exception();
                EditEmployee(companyDataSet.Tables["Employees"].Rows[Employees_listView.SelectedIndex],
                                EmployeeName_textBox.Text,
                                Convert.ToInt32(EmployeeAge_textBox.Text),
                                Convert.ToInt32(EmployeeSalary_textBox.Text));
                employeeAdapter.Update(companyDataSet, "Employees");
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Invalid department Id");
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid employee data");
            }
            finally
            {
                EmployeeCode_textBlock.Text = "";
                EmployeeName_textBox.Text = "";
                EmployeeAge_textBox.Text = "";
                EmployeeSalary_textBox.Text = "";
            }
        }

        private void DeleteEmployee_button_Click(object sender, RoutedEventArgs e)
        {
            if (Departments_comboBox.SelectedValue == null) return;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("DELETE FROM Employees WHERE Id = @Id", connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@Id", companyDataSet.Tables["Employees"].Rows[Employees_listView.SelectedIndex].Field<Int32>("Id"));
                        connection.Open();
                        int n = cmd.ExecuteNonQuery();
                        Console.WriteLine(n);
                    }
                }
                companyDataSet.Tables["Employees"].Rows.Remove(companyDataSet.Tables["Employees"].Rows[Employees_listView.SelectedIndex]);
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Invalid department Id");
            }
            catch (KeyNotFoundException)
            {
                MessageBox.Show("Invalid employee Id");
            }
            catch { }
            finally
            {
                EmployeeCode_textBlock.Text = "";
                EmployeeName_textBox.Text = "";
                EmployeeAge_textBox.Text = "";
                EmployeeSalary_textBox.Text = "";
            }
        }

        #endregion

        #endregion


        #region MoveTab

        #region View

        private void DepartmentsMove1_comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            companyDataSet.Tables["EmployeesMove1"].Rows.Clear();

            employeeAdapter.SelectCommand.CommandText = $"SELECT * FROM Employees WHERE Department = {DepartmentsMove1_comboBox.SelectedValue};";
            employeeAdapter.Fill(companyDataSet, "EmployeesMove1");
            EmployeesMove1_listView.ItemsSource = companyDataSet.Tables["EmployeesMove1"].DefaultView;
        }

        private void DepartmentsMove2_comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            companyDataSet.Tables["EmployeesMove2"].Rows.Clear();
            employeeAdapter.SelectCommand.CommandText = $"SELECT * FROM Employees WHERE Department = {DepartmentsMove2_comboBox.SelectedValue};";
            employeeAdapter.Fill(companyDataSet, "EmployeesMove2");
            EmployeesMove2_listView.ItemsSource = companyDataSet.Tables["EmployeesMove2"].DefaultView;
        }

        #endregion

        #region Buttons
        private void MoveTo_button_Click(object sender, RoutedEventArgs e)
        {
            if (DepartmentsMove1_comboBox.SelectedValue == null || DepartmentsMove2_comboBox.SelectedValue == null) return;
            try
            {
                if (DepartmentsMove2_comboBox.SelectedValue == DepartmentsMove1_comboBox.SelectedValue)
                    return;
                int departmentId = Convert.ToInt32(DepartmentsMove2_comboBox.SelectedValue);
                int departmentCode = Convert.ToInt32((from dep in companyDataSet.Tables["Departments"].AsEnumerable()
                                                      where dep.Field<Int32>("Id") == Convert.ToInt32(DepartmentsMove2_comboBox.SelectedValue)
                                                      select dep["Code"]).First());
                ChangeDepartment(companyDataSet.Tables["EmployeesMove1"].Rows[EmployeesMove1_listView.SelectedIndex], "EmployeesMove2", departmentId, departmentCode);

                employeeAdapter.Update(companyDataSet, "EmployeesMove1");

                DataRow newRow = companyDataSet.Tables["EmployeesMove1"].Rows[EmployeesMove1_listView.SelectedIndex];
                companyDataSet.Tables["EmployeesMove2"].Rows.Add(newRow.ItemArray);
                companyDataSet.Tables["EmployeesMove1"].Rows.Remove(newRow);
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Invalid department Id");
            }
            catch (KeyNotFoundException)
            {
                MessageBox.Show("Invalid employee Id");
            }
            catch { }
            finally
            {
                EmployeeCode_textBlock.Text = "";
                EmployeeName_textBox.Text = "";
                EmployeeAge_textBox.Text = "";
                EmployeeSalary_textBox.Text = "";
            }
        }

        private void MoveBack_button_Click(object sender, RoutedEventArgs e)
        {
            if (DepartmentsMove1_comboBox.SelectedValue == null || DepartmentsMove2_comboBox.SelectedValue == null) return;
            try
            {
                if (DepartmentsMove2_comboBox.SelectedValue == DepartmentsMove1_comboBox.SelectedValue)
                    return;
                int departmentId = Convert.ToInt32(DepartmentsMove1_comboBox.SelectedValue);
                int departmentCode = Convert.ToInt32((from dep in companyDataSet.Tables["Departments"].AsEnumerable()
                                                      where dep.Field<Int32>("Id") == Convert.ToInt32(DepartmentsMove1_comboBox.SelectedValue)
                                                      select dep["Code"]).First());
                ChangeDepartment(companyDataSet.Tables["EmployeesMove2"].Rows[EmployeesMove2_listView.SelectedIndex], "EmployeesMove1", departmentId, departmentCode);

                employeeAdapter.Update(companyDataSet, "EmployeesMove2");

                DataRow newRow = companyDataSet.Tables["EmployeesMove2"].Rows[EmployeesMove2_listView.SelectedIndex];
                companyDataSet.Tables["EmployeesMove1"].Rows.Add(newRow.ItemArray);
                companyDataSet.Tables["EmployeesMove2"].Rows.Remove(newRow);
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Invalid Id");
            }
            catch (KeyNotFoundException)
            {
                MessageBox.Show("Invalid employee Id");
            }
            catch { }
            finally
            {
                EmployeeCode_textBlock.Text = "";
                EmployeeName_textBox.Text = "";
                EmployeeAge_textBox.Text = "";
                EmployeeSalary_textBox.Text = "";
            }
        }

        #endregion

        #endregion


        #region Adapter_commands

        /// <summary>
        /// Создает комманду Insert для таблицы департаментов
        /// </summary>
        public static void InsertDepartmentAdapter(SqlConnection connection, SqlDataAdapter adapter)
        {
            SqlCommand command = new SqlCommand(@"INSERT INTO Departments (Code, Name) VALUES (@Code, @Name); SET @Id = @@IDENTITY;", connection);

            command.Parameters.Add("@Code", SqlDbType.Int, -1, "Code");
            command.Parameters.Add("@Name", SqlDbType.NVarChar, -1, "Name");

            SqlParameter param = command.Parameters.Add("@Id", SqlDbType.Int, 0, "Id");
            param.Direction = ParameterDirection.Output;

            adapter.InsertCommand = command;
        }

        /// <summary>
        /// Создает комманду Insert для таблицы сотрудников
        /// </summary>
        public static void InsertEmployeeAdapter(SqlConnection connection, SqlDataAdapter adapter)
        {
            SqlCommand command = new SqlCommand(@"INSERT INTO [Employees] ([Code], [Name], [Age], [Salary], [Department]) VALUES 
                                                                        (@Code, @Name, @Age, @Salary, @Department); SET @Id = @@IDENTITY;", connection);

            command.Parameters.Add("@Code", SqlDbType.Int, -1, "Code");
            command.Parameters.Add("@Name", SqlDbType.NVarChar, -1, "Name");
            command.Parameters.Add("@Age", SqlDbType.Int, -1, "Age");
            command.Parameters.Add("@Salary", SqlDbType.Decimal, -1, "Salary");
            command.Parameters.Add("@Department", SqlDbType.Int, -1, "Department");

            SqlParameter param = command.Parameters.Add("@Id", SqlDbType.Int, 0, "Id");
            param.Direction = ParameterDirection.Output;

            adapter.InsertCommand = command;
        }

        /// <summary>
        /// Создает комманду Update для таблицы сотрудников 
        /// </summary>
        public static void UpdateEmployeeAdapter(SqlConnection connection, SqlDataAdapter adapter)
        {
            SqlCommand command = new SqlCommand(@"UPDATE [Employees] SET [Code] = @Code, [Name] = @Name, [Age] = @Age, [Salary] = @Salary, [Department] = @Department WHERE [Id] = @Id", connection);

            command.Parameters.Add("@Code", SqlDbType.Int, -1, "Code");
            command.Parameters.Add("@Name", SqlDbType.NVarChar, -1, "Name");
            command.Parameters.Add("@Age", SqlDbType.Int, -1, "Age");
            command.Parameters.Add("@Salary", SqlDbType.Decimal, -1, "Salary");
            command.Parameters.Add("@Department", SqlDbType.Int, -1, "Department");

            SqlParameter param = command.Parameters.Add("@Id", SqlDbType.Int, 0, "Id");
            param.SourceVersion = DataRowVersion.Original;

            adapter.UpdateCommand = command;
        }

        #endregion


        #region Methods
        
        /// <summary>
        /// Создает множество значений определенного столбца таблицы
        /// </summary>
        /// <typeparam name="T">Тип значений в столбце</typeparam>
        /// <param name="sourceTable">Исходная таблица</param>
        /// <param name="columnName">Название столбца</param>
        private HashSet<T> CreateSetFromColumn<T>(DataTable sourceTable, string columnName)
        {
            if (!sourceTable.Columns.Contains(columnName))
                return null;
            HashSet<T> result = new HashSet<T>();
            for (int i = 0; i < sourceTable.Rows.Count; i++)
            {
                result.Add((T)sourceTable.Rows[i][columnName]);
            }
            return result;
        }

        /// <summary>
        /// Добавляет департамент и генерирует ему имя
        /// </summary>
        public DataRow AddDepartment()
        {
            return AddDepartment("");
        }

        /// <summary>
        /// Добавляет департамент с заданным именем
        /// </summary>
        /// <param name="name">Имя департамента</param>
        public DataRow AddDepartment(string name)
        {
            var codeList = CreateSetFromColumn<int>(companyDataSet.Tables["Departments"], "Code");
            var newCode = codeList.Count == 0 ? 1 : codeList.Max() + 1;
            DataRow newDepartment = companyDataSet.Tables["Departments"].NewRow();
            newDepartment["Code"] = newCode;
            newDepartment["Name"] = name == "" ? $"Department_{newCode}" : name;
            return newDepartment;
        }

        /// <summary>
        /// Добавляет сотрудника и генерирует ему имя, возраст и зарплату
        /// </summary>
        /// <param name="departmentId">Id департамента, в который нужно добавить сотрудника</param>
        /// <param name="departmentCode">Код департамента, в который нужно добавить сотрудника</param>
        /// <returns>Строку для записи в базу данных</returns>
        public DataRow AddEmployee(int departmentId, int departmentCode)
        {
            var rnd = new Random();
            return AddEmployee("", rnd.Next(20, 65), rnd.Next(5, 45) * 1000, departmentId, departmentCode);
        }

        /// <summary>
        /// Добавляет сотрудника
        /// </summary>
        /// <param name="name">Имя сотрудника</param>
        /// <param name="age">Возраст сотрудника</param>
        /// <param name="salary">Зврплата сотрудника</param>
        /// <param name="departmentId">Id департамента, в который нужно добавить сотрудника</param>
        /// <param name="departmentCode">Код департамента, в который нужно добавить сотрудника</param>
        /// <returns>Строку для записи в базу данных</returns>
        public DataRow AddEmployee(string name, int age, double salary, int departmentId, int departmentCode)
        {
            var newCode = GenerateNewCode("Employees", departmentId, departmentCode);

            DataRow newEmployee = companyDataSet.Tables["Employees"].NewRow();
            newEmployee["Code"] = newCode;
            newEmployee["Name"] = name == "" ? $"Employee_{newCode}" : name;
            newEmployee["Age"] = age;
            newEmployee["Salary"] = salary;
            newEmployee["Department"] = departmentId;

            return newEmployee;
        }

        /// <summary>
        /// Изменяет данные сотрудника
        /// </summary>
        /// <param name="row">Строка с данными сотрудника</param>
        /// <param name="name">Имя сотрудника</param>
        /// <param name="age">Возраст сотрудника</param>
        /// <param name="salary">Зврплата сотрудника</param>
        public void EditEmployee(DataRow row, string name, int age, double salary)
        {
            row["Name"] = name;
            row["Age"] = age;
            row["Salary"] = salary;
        }

        public void ChangeDepartment(DataRow employeeData, string dataTableName, int departmentId, int departmentCode)
        {
            var newCode = GenerateNewCode(dataTableName, departmentId, departmentCode);
            employeeData["Code"] = newCode;
            employeeData["Department"] = departmentId;
        }

        public int GenerateNewCode(string employeedataTableName, int departmentId, int departmentCode)
        {
            if (!companyDataSet.Tables.Contains(employeedataTableName))
                throw new ArgumentOutOfRangeException();
            var employeeCodeList = CreateSetFromColumn<int>(companyDataSet.Tables[employeedataTableName], "Code");
            var departmentIdList = CreateSetFromColumn<int>(companyDataSet.Tables["Departments"], "Id");
            var newCode = departmentCode * 1000 + 1;
            if (employeeCodeList.Count > 0)
            {
                newCode = employeeCodeList.Max() + 1;
                if (newCode / 1000 == employeeCodeList.Max() / 1000 + 1)
                {
                    var tempCode = departmentCode * 1000 + 1;
                    while (employeeCodeList.Contains(tempCode))
                    {
                        tempCode++;
                    }
                    newCode = tempCode;
                }
            }
            if (newCode / 1000 != departmentCode || !departmentIdList.Contains(departmentId))
                throw new ArgumentException();

            return newCode;
        }

        #endregion
    }
}
