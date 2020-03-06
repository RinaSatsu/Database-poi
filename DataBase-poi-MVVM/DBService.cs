using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase_poi_MVVM
{
    class DBService
    {
        #region Fields

        private static string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        SqlConnection connection;

        private SqlDataAdapter _departmentAdapter;
        private SqlDataAdapter _employeeAdapter;

        #endregion


        public DBService()
        {
            connection = new SqlConnection(connectionString);

            _departmentAdapter = new SqlDataAdapter("SELECT * FROM Departments;", connection);
            InsertDepartmentAdapter(connection, _departmentAdapter);

            _employeeAdapter = new SqlDataAdapter($"SELECT * FROM Employees WHERE Department = @DeparmentId;", connection);
            InsertEmployeeAdapter(connection, _employeeAdapter);
            UpdateEmployeeAdapter(connection, _employeeAdapter);
        }


        #region Public_Methods

        /// <summary>
        /// Заполняет таблицу департаментов значениями из базы данных
        /// </summary>
        /// <param name="dataSet">Заполняемый DataSet</param>
        public void FillDepartmentTable(DataSet dataSet)
        {
            _departmentAdapter.Fill(dataSet, "Departments");
        }

        /// <summary>
        /// Заполняет таблицу сотрудников значениями из базы данных
        /// </summary>
        /// <param name="dataSet">Исходный DataSet</param>
        /// <param name="tableName">Таблица сотрудников в DataSet</param>
        /// <param name="departmentId">Id департамента, сотрудники которого будут импортированы</param>
        public void FillEmployeeTable(DataSet dataSet, string tableName, int departmentId)
        {
            _employeeAdapter.SelectCommand.CommandText = $"SELECT * FROM Employees WHERE Department = {departmentId};";
            //employeeAdapter.SelectCommand.Parameters.Clear();

            //SqlParameter param = new SqlParameter("@DepartmentId", SqlDbType.Int, -1);
            //param.Value = Departments_comboBox.SelectedValue;
            //employeeAdapter.SelectCommand.Parameters.Add(param);

            //employeeAdapter.SelectCommand.Parameters.AddWithValue("@DepartmentId", Departments_comboBox.SelectedValue);
            _employeeAdapter.Fill(dataSet, tableName);
        }

        /// <summary>
        /// Приводит базу данных департаментов в соответствие с DataSet
        /// </summary>
        /// <param name="dataSet">Исходный DataSet</param>
        public void UpdateDepartmentsTable(DataSet dataSet)
        {
            _departmentAdapter.Update(dataSet, "Departments");
        }

        /// <summary>
        /// Приводит базу данных департаментов в соответствие с таблицей сотрудников в DataSet
        /// </summary>
        /// <param name="dataSet">Исходный DataSet</param>
        /// <param name="tableName">Таблица сотрудников в DataSet</param>
        public void UpdateEmployeesTable(DataSet dataSet, string tableName)
        {
            _employeeAdapter.Update(dataSet, tableName);
        }

        /// <summary>
        /// Меняет код и департамент в базе данных у выбранного сотрудника
        /// </summary>
        /// <param name="dataSet">Исходный DataSet</param>
        /// <param name="tableName">Таблица сотрудников в DataSe</param>
        /// <param name="index">Индекс сотрудника в таблице</param>
        public void ChangeDepartment(DataSet dataSet, string tableName, int index)
        {
            using (SqlConnection tempConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(@"UPDATE [Employees] SET [Code] = @Code, [Department] = @Department WHERE [Id] = @Id", tempConnection))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Code", dataSet.Tables[tableName].Rows[index].Field<Int32>("Code"));
                    cmd.Parameters.AddWithValue("@Department", dataSet.Tables[tableName].Rows[index].Field<Int32>("Department"));
                    cmd.Parameters.AddWithValue("@Id", dataSet.Tables[tableName].Rows[index].Field<Int32>("Id"));
                    tempConnection.Open();
                    int n = cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Удаляет сотрудника из базы данных
        /// </summary>
        /// <param name="dataSet">Исходный DataSet</param>
        /// <param name="tableName">Таблица сотрудников в DataSet</param>
        /// <param name="index">Индекс сотрудника в таблице</param>
        public void DeleteEmployee(DataSet dataSet, string tableName, int index)
        {
            using (SqlConnection tempConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("DELETE FROM Employees WHERE Id = @Id", tempConnection))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Id", dataSet.Tables[tableName].Rows[index].Field<Int32>("Id"));
                    tempConnection.Open();
                    int n = cmd.ExecuteNonQuery();
                }
            }
        }

        #endregion


        #region Adapter_commands

        /// <summary>
        /// Создает комманду Insert для таблицы департаментов
        /// </summary>
        private void InsertDepartmentAdapter(SqlConnection connection, SqlDataAdapter adapter)
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
        private void InsertEmployeeAdapter(SqlConnection connection, SqlDataAdapter adapter)
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
        private void UpdateEmployeeAdapter(SqlConnection connection, SqlDataAdapter adapter)
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
    }
}
