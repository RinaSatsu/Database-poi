using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase_poi
{
    class EditCompanyViewModel: INotifyPropertyChanged
    {
        #region Fields

        private readonly Action<string> _errorMessage;

        private readonly CompanyModel _model = new CompanyModel();
        private int? _departmentSelectedValue;
        private int? _employeeSelectedValue;

        private Employee _employeeData;

        private Command _addDepartmentCommand;

        private string _code = "A";
        public string Code
        {
            get { return _code; }
            set
            {
                _code = value;
                PropertyChanged("Code", new PropertyChangedEventArgs("Code"));
            }
        }

        #endregion


        #region Properties

        public DataTable DepartmentsDataTable => _model.CompanyDataSet.Tables["Departments"];
        public DataTable EmployeesDataTable => _model.CompanyDataSet.Tables["Employees"];

        public int? SelectedDepartment
        {
            get { return _departmentSelectedValue; }
            set
            {
                _departmentSelectedValue = value;
                PropertyChanged("SelectedDepartment", new PropertyChangedEventArgs("SelectedDepartment"));
                if (value == null)
                    return;
                _model.ImportEmployees("Employees", (int)_departmentSelectedValue);
            }
        }

        public int? SelectedEmployee
        {
            get { return _employeeSelectedValue; }
            set
            {
                if (value == null)
                    return;
                _employeeSelectedValue = value;
                //SelectedEmployeeData = new Employee(_model.GetEmployee("Employees", (int)_employeeSelectedValue));
                SelectedEmployeeData = new Employee(1, "AAA", 1, 1);
                PropertyChanged("SelectedEmployee", new PropertyChangedEventArgs("SelectedEmployee"));
                //Code = _employeeData.Code.ToString();
            }
        }

        public Employee SelectedEmployeeData
        {
            get { return _employeeData; }
            set
            {
                _employeeData = value;
                PropertyChanged("SelectedEmployeeData", new PropertyChangedEventArgs("SelectedEmployeeData"));
            }
        }


        public Command AddDepartmentCommand
        {
            get { return _addDepartmentCommand ?? (_addDepartmentCommand = new Command(AddDepartment)); }
        }


        #endregion


        public EditCompanyViewModel()
        {

        }

        public void AddDepartment(object departmentName)
        {
            try
            {
                _model.AddDepartment((string)departmentName);
            }
            catch (ArgumentException)
            {
                _errorMessage("Invalid department Id");
            }
            catch (Exception ex)
            {
                _errorMessage(ex.Message);
            }
        }

        public void AddEmployee(object employee)
        {
            if (SelectedDepartment == null) return;
            //if ((EmployeeName_textBox.Text == companyDataSet.Tables["Employees"].Rows[Employees_listView.SelectedIndex]["Name"].ToString()) &&
            //    (EmployeeAge_textBox.Text == companyDataSet.Tables["Employees"].Rows[Employees_listView.SelectedIndex]["Age"].ToString()) &&
            //    (EmployeeSalary_textBox.Text == companyDataSet.Tables["Employees"].Rows[Employees_listView.SelectedIndex]["Salary"].ToString())) return;
            try
            {
            //    DataRow newRow;
            //    int departmentId = Convert.ToInt32(Departments_comboBox.SelectedValue);
            //    int departmentCode = Convert.ToInt32((from dep in companyDataSet.Tables["Departments"].AsEnumerable()
            //                                          where dep.Field<Int32>("Id") == Convert.ToInt32(Departments_comboBox.SelectedValue)
            //                                          select dep["Code"]).First());
            //    if ((EmployeeName_textBox.Text == "") || (EmployeeAge_textBox.Text == "") || (EmployeeSalary_textBox.Text == ""))
            //        newRow = AddEmployee(departmentId, departmentCode);
            //    else
            //        newRow = AddEmployee(EmployeeName_textBox.Text, int.Parse(EmployeeAge_textBox.Text), double.Parse(EmployeeSalary_textBox.Text), departmentId, departmentCode);

            //    companyDataSet.Tables["Employees"].Rows.Add(newRow);
            //    employeeAdapter.Update(companyDataSet, "Employees");

            }
            catch (ArgumentOutOfRangeException)
            {
                _errorMessage("Invalid department Id");
            }
            catch (Exception ex)
            {
                _errorMessage(ex.Message);
            }
            //finally
            //{
            //    EmployeeCode_textBlock.Text = "";
            //    EmployeeName_textBox.Text = "";
            //    EmployeeAge_textBox.Text = "";
            //    EmployeeSalary_textBox.Text = "";
            //}
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        //public void OnPropertyChanged([CallerMemberName]string prop = "")
        //{
        //    if (PropertyChanged != null)
        //        PropertyChanged(this, new PropertyChangedEventArgs(prop));
        //}

        #endregion
    }
}
