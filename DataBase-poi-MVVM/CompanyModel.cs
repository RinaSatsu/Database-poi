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
    class CompanyModel
    {
        #region Fields

        private DataSet _companyDataSet;
        private DBService _dataBase = new DBService();


        #endregion


        #region Properties

        public DataSet CompanyDataSet { get => _companyDataSet; }

        #endregion


        public CompanyModel()
        {
            _companyDataSet = new DataSet("CompanyEdit");

            _dataBase.FillDepartmentTable(_companyDataSet);

            _companyDataSet.Tables.Add("Employees");
            _companyDataSet.Tables.Add("EmployeesMove1");
            _companyDataSet.Tables.Add("EmployeesMove2");
        }


        #region Public_Methods

        /// <summary>
        /// Добавляет сотрудников одного департамента в таблицу в DataSet
        /// </summary>
        /// <param name="tableName">Имя таблицы</param>
        /// <param name="departmentId">Id департамента, сотрудников которого нужно добавить</param>
        public void ImportEmployees(string tableName, int departmentId)
        {
            if (!_companyDataSet.Tables.Contains(tableName))
                throw new ArgumentOutOfRangeException();

            _companyDataSet.Tables[tableName].Rows.Clear();
            _dataBase.FillEmployeeTable(_companyDataSet, tableName, departmentId);
        }

        /// <summary>
        /// Получает данные сотрудника из таблицы
        /// </summary>
        /// <param name="tableName">Имя таблицы</param>
        /// <param name="employeeId">Id нужного сотрудника</param>
        /// <returns>Данные о сотруднике - код, имя, возраст и зарплату</returns>
        public Employee GetEmployee(string tableName, int employeeId)
        {
            if (!_companyDataSet.Tables.Contains(tableName))
                throw new ArgumentOutOfRangeException();

            var employeeData = (from emp in _companyDataSet.Tables[tableName].AsEnumerable()
                                where emp.Field<Int32>("Id") == employeeId
                                select new { Code = emp["Code"], Name = emp["Name"], Age = emp["Age"], Salary = emp["Salary"] }).First();
            return new Employee(Convert.ToInt32(employeeData.Code), employeeData.Name.ToString(), Convert.ToInt32(employeeData.Age), Convert.ToDouble(employeeData.Salary));
        }

        /// <summary>
        /// Добавляет департамент с заданным именем в DataSet
        /// </summary>
        /// <param name="departmentName">Имя добавляемого департамента</param>
        public void AddDepartment(string departmentName)
        {
            DataRow departmentData = CreateDepartment(departmentName);
            _companyDataSet.Tables["Departments"].Rows.Add(departmentData);
            _dataBase.UpdateDepartmentsTable(_companyDataSet);
        }

        /// <summary>
        /// Добавляет нового сотрудника в таблицу в DataSet
        /// </summary>
        /// <param name="tableName">Имя таблицы</param>
        /// <param name="name">Имя сотрудника (генерируется, если нет)</param>
        /// <param name="age">Возраст сотрудника (генерируется, если 0)</param>
        /// <param name="salary">Зарплата сотрудника (генерируется, если 0)</param>
        /// <param name="departmentId">Id департамента, в который добавляется сотрудник</param>
        public void AddEmployee(string tableName, string name, int age, double salary, int departmentId)
        {
            if (!_companyDataSet.Tables.Contains(tableName))
                throw new ArgumentOutOfRangeException();

            int departmentCode = Convert.ToInt32((from dep in _companyDataSet.Tables["Departments"].AsEnumerable()
                                                  where dep.Field<Int32>("Id") == departmentId
                                                  select dep["Code"]).First());
            DataRow employeeData = CreateEmployee(name, age, salary, departmentId, departmentCode);

            _companyDataSet.Tables[tableName].Rows.Add(employeeData);
            _dataBase.UpdateEmployeesTable(_companyDataSet, tableName);
        }

        /// <summary>
        /// Изменяет данные сотрудника
        /// </summary>
        /// <param name="tableName">Имя таблицы, в которой содержится сотрудник</param>
        /// <param name="index">Индекс сотрудника в таблице</param>
        /// <param name="name">Имя сотрудника</param>
        /// <param name="age">Возраст сотрудника</param>
        /// <param name="salary">Зарплата сотрудника</param>
        public void EditEmployee(string tableName, int index, string name, int age, double salary)
        {
            if (!_companyDataSet.Tables.Contains(tableName))
                throw new ArgumentOutOfRangeException();

            _companyDataSet.Tables[tableName].Rows[index]["Name"] = name;
            _companyDataSet.Tables[tableName].Rows[index]["Age"] = age;
            _companyDataSet.Tables[tableName].Rows[index]["Salary"] = salary;
        }

        /// <summary>
        /// Удаляет заданного сотрудника
        /// </summary>
        /// <param name="tableName">Имя таблицы</param>
        /// <param name="index">Индекс сотрудника в таблице</param>
        public void DeleteEmployee(string tableName, int index)
        {
            if (!_companyDataSet.Tables.Contains(tableName))
                throw new ArgumentOutOfRangeException();

            _dataBase.DeleteEmployee(_companyDataSet, tableName, index);
            _companyDataSet.Tables[tableName].Rows.RemoveAt(index);
        }

        /// <summary>
        /// Перемещает сотрудника из одного департамента (и соответствующей ему таблицы) в другой
        /// </summary>
        /// <param name="fromTableName">Имя таблицы, из которой убирается сотрудник</param>
        /// <param name="toTableName">Имя таблицы, в которую добавляется сотрудник</param>
        /// <param name="toDepartmentId">Id департамента, в который добавляется сотрудник</param>
        /// <param name="employeeIndex">Индекс перемещаемого сотрудника в изначальной таблице</param>
        public void MoveEmployee(string fromTableName, string toTableName, int toDepartmentId, int employeeIndex)
        {
            int toDepartmentCode = Convert.ToInt32((from dep in _companyDataSet.Tables["Departments"].AsEnumerable()
                                                  where dep.Field<Int32>("Id") == toDepartmentId
                                                  select dep["Code"]).First());
            ChangeDepartment(_companyDataSet.Tables[fromTableName].Rows[employeeIndex], toTableName, toDepartmentId, toDepartmentCode);

            _dataBase.UpdateEmployeesTable(_companyDataSet, fromTableName);

            DataRow newRow = _companyDataSet.Tables[fromTableName].Rows[employeeIndex];
            _companyDataSet.Tables[toTableName].Rows.Add(newRow.ItemArray);
            _companyDataSet.Tables[fromTableName].Rows.Remove(newRow);
        }

        #endregion


        #region Private_Methods

        /// <summary>
        /// Создает департамент с заданным именем, или генерирует, если имя не задано
        /// </summary>
        /// <param name="name">Имя департамента</param>
        private DataRow CreateDepartment(string name)
        {
            var codeList = CreateSetFromColumn<int>(_companyDataSet.Tables["Departments"], "Code");
            var newCode = codeList.Count == 0 ? 1 : codeList.Max() + 1;
            DataRow newDepartment = _companyDataSet.Tables["Departments"].NewRow();
            newDepartment["Code"] = newCode;
            newDepartment["Name"] = name == "" ? $"Department_{newCode}" : name;
            return newDepartment;
        }

        /// <summary>
        /// Создает сотрудника
        /// </summary>
        /// <param name="name">Имя сотрудника (генерируется, если не задано)</param>
        /// <param name="age">Возраст сотрудника (генерируется, если 0)</param>
        /// <param name="salary">Зврплата сотрудника (генерируется, если 0)</param>
        /// <param name="departmentId">Id департамента, в который нужно добавить сотрудника</param>
        /// <param name="departmentCode">Код департамента, в который нужно добавить сотрудника</param>
        /// <returns>Строку для записи в базу данных</returns>
        private DataRow CreateEmployee(string name, int age, double salary, int departmentId, int departmentCode)
        {
            var rnd = new Random();
            var newCode = GenerateNewCode("Employees", departmentId, departmentCode);

            DataRow newEmployee = _companyDataSet.Tables["Employees"].NewRow();
            newEmployee["Code"] = newCode;
            newEmployee["Name"] = name == "" ? $"Employee_{newCode}" : name;
            newEmployee["Age"] = age == 0 ? rnd.Next(20, 65) : age;
            newEmployee["Salary"] = salary == 0 ? rnd.Next(5, 45) * 1000 : salary;
            newEmployee["Department"] = departmentId;

            return newEmployee;
        }

        /// <summary>
        /// Меняет департамент у указанного сотрудника
        /// </summary>
        /// <param name="employeeData">Строка с информацией о сотруднике</param>
        /// <param name="dataTableName">Имя таблицы</param>
        /// <param name="departmentId">Id целевого департамента</param>
        /// <param name="departmentCode">Код целевого департамента</param>
        private void ChangeDepartment(DataRow employeeData, string dataTableName, int departmentId, int departmentCode)
        {
            var newCode = GenerateNewCode(dataTableName, departmentId, departmentCode);
            employeeData["Code"] = newCode;
            employeeData["Department"] = departmentId;
        }

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
        /// Генерирует новый код для задщанного сотрудника в соответствии с его департаментом
        /// </summary>
        /// <param name="employeedataTableName">Таблица с сотрудниками</param>
        /// <param name="departmentId">Id департамента, в соответствии с которым генерируется код сотрудника</param>
        /// <param name="departmentCode">Код департамента, в соответствии с которым генерируется код сотрудника</param>
        /// <returns></returns>
        private int GenerateNewCode(string employeedataTableName, int departmentId, int departmentCode)
        {
            if (!_companyDataSet.Tables.Contains(employeedataTableName))
                throw new ArgumentOutOfRangeException();
            var employeeCodeList = CreateSetFromColumn<int>(_companyDataSet.Tables[employeedataTableName], "Code");
            var departmentIdList = CreateSetFromColumn<int>(_companyDataSet.Tables["Departments"], "Id");
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
