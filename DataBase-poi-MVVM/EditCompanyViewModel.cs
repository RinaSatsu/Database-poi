using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DataBase_poi_MVVM
{
    class EditCompanyViewModel: INotifyPropertyChanged
    {
        #region Fields

        CompanyModel _model;

        private readonly Func<string, MessageBoxResult> _errorMessage;

        private int? _departmentSelectedValue;
        private int? _employeeSelectedValue;
        private Employee _employeeData;

        private Command _addDepartmentCommand;
        private Command _addEmployeeCommand;
        private Command _editEmployeeCommand;
        private Command _deleteEmployeeCommand;

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
                if (value == null)
                    return;
                _model.ImportEmployees("Employees", (int)_departmentSelectedValue);
                OnPropertyChanged("SelectedDepartment");
            }
        }

        public int? SelectedEmployee
        {
            get { return _employeeSelectedValue; }
            set
            {
                _employeeSelectedValue = value;
                if (value == null)
                {
                    SelectedEmployeeData = null;
                    return;
                }
                SelectedEmployeeData = _model.GetEmployee("Employees", (int)_employeeSelectedValue);
                OnPropertyChanged("SelectedEmployee");
            }
        }

        public Employee SelectedEmployeeData
        {
            get { return _employeeData; }
            set
            {
                _employeeData = value;
                OnPropertyChanged("SelectedEmployeeData");
            }
        }

        #endregion


        public EditCompanyViewModel()
        {
        }

        public EditCompanyViewModel(Func<string, MessageBoxResult> errorMessage, CompanyModel model)
        {
            _errorMessage = errorMessage;
            _model = model;
        }


        #region Commands

        public Command AddDepartmentCommand
        {
            get { return _addDepartmentCommand ?? (_addDepartmentCommand = new Command(AddDepartment)); }
        }

        public Command AddEmployeeCommand
        {
            get { return _addEmployeeCommand ?? (_addEmployeeCommand = new Command(AddEmployee)); }
        }

        public Command EditEmployeeCommand
        {
            get { return _editEmployeeCommand ?? (_editEmployeeCommand = new Command(EditEmployee)); }
        }

        public Command DeleteEmployeeCommand
        {
            get { return _deleteEmployeeCommand ?? (_deleteEmployeeCommand = new Command(DeleteEmployee)); }
        }

        #endregion


        #region Methods

        private void AddDepartment(object departmentName)
        {
            try
            {
                _model.AddDepartment((string)departmentName);
            }
            catch (ArgumentException)
            {
                //MessageBox.Show("Invalid department Id");
                _errorMessage("Invalid department Id");
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                _errorMessage(ex.Message);
            }
        }

        private void AddEmployee(object employee)
        {
            if (SelectedDepartment == null) return;
            try
            {
                Employee newEmpl;
                if (SelectedEmployee == null)
                {
                    newEmpl = new Employee();
                }
                else
                {
                    Employee empl = _model.GetEmployee("Employees", (int)SelectedEmployee);
                    if ((empl.Name == SelectedEmployeeData.Name) && (empl.Age == SelectedEmployeeData.Age) && (empl.Salary == SelectedEmployeeData.Salary)) return;
                    newEmpl = new Employee("0", SelectedEmployeeData.Name, SelectedEmployeeData.Age, SelectedEmployeeData.Salary);
                }
                int age;
                double salary;
                int.TryParse(newEmpl.Age, out age);
                double.TryParse(newEmpl.Salary, out salary);
                _model.AddEmployee("Employees", newEmpl.Name, age, salary, (int)SelectedDepartment);
            }
            catch (ArgumentOutOfRangeException)
            {
                //MessageBox.Show("Invalid department Id");
                _errorMessage("Invalid department Id");
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                _errorMessage(ex.Message);
            }
            finally
            {
                SelectedEmployee = null;
            }
        }

        private void EditEmployee(object index)
        {
            if (SelectedDepartment == null || SelectedEmployee == null || (int)index == -1) return;
            try
            {
                if ((SelectedEmployeeData.Name == "") || (SelectedEmployeeData.Age == "") || (SelectedEmployeeData.Salary == ""))
                    throw new Exception();
                int age;
                double salary;
                int.TryParse(SelectedEmployeeData.Age, out age);
                double.TryParse(SelectedEmployeeData.Salary, out salary);
                _model.EditEmployee("Employees", (int)index, SelectedEmployeeData.Name, age, salary);
            }
            catch (ArgumentOutOfRangeException)
            {
                //MessageBox.Show("Invalid department Id");
                _errorMessage("Invalid department Id");
            }
            catch (Exception)
            {
                //MessageBox.Show("Invalid employee data");
                _errorMessage("Invalid employee data");
            }
            finally
            {
                SelectedEmployee = null;
            }
        }

        private void DeleteEmployee(object index)
        {
            if (SelectedDepartment == null || SelectedEmployee == null || (int)index == -1) return;
            try
            {
                _model.DeleteEmployee("Employees", (int)index);
            }
            catch (ArgumentOutOfRangeException)
            {
                //MessageBox.Show("Invalid department Id");
                _errorMessage("Invalid department Id");
            }
            catch (Exception)
            {
                //MessageBox.Show("Invalid employee data");
                _errorMessage("Invalid employee data");
            }
            finally
            {
                SelectedEmployee = null;
            }
        }

        #endregion


        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        #endregion
    }
}
