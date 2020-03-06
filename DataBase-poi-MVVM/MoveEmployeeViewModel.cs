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
    class MoveEmployeeViewModel
    {
        #region Fields

        CompanyModel _model;

        private readonly Func<string, MessageBoxResult> _errorMessage;

        private int? _department1SelectedValue;
        private int? _department2SelectedValue;
        private int _employee1SelectedIndex;
        private int _employee2SelectedIndex;

        private Command _moveToCommand;
        private Command _moveBackCommand;

        #endregion


        #region Properties

        public DataTable DepartmentsDataTable => _model.CompanyDataSet.Tables["Departments"];
        public DataTable EmployeesMove1DataTable => _model.CompanyDataSet.Tables["EmployeesMove1"];
        public DataTable EmployeesMove2DataTable => _model.CompanyDataSet.Tables["EmployeesMove2"];

        public int? SelectedDepartment1
        {
            get { return _department1SelectedValue; }
            set
            {
                _department1SelectedValue = value;
                if (value == null)
                    return;
                _model.ImportEmployees("EmployeesMove1", (int)_department1SelectedValue);
                OnPropertyChanged("SelectedDepartment1");
            }
        }

        public int? SelectedDepartment2
        {
            get { return _department2SelectedValue; }
            set
            {
                _department2SelectedValue = value;
                if (value == null)
                    return;
                _model.ImportEmployees("EmployeesMove2", (int)_department2SelectedValue);
                OnPropertyChanged("SelectedDepartment2");
            }
        }

        public int SelectedEmployeeIndex1
        {
            get { return _employee1SelectedIndex; }
            set
            {
                _employee1SelectedIndex = value;
                OnPropertyChanged("SelectedEmployeeIndex1");
            }
        }

        public int SelectedEmployeeIndex2
        {
            get { return _employee2SelectedIndex; }
            set
            {
                _employee2SelectedIndex = value;
                OnPropertyChanged("SelectedEmployeeIndex2");
            }
        }

        #endregion


        public MoveEmployeeViewModel()
        {
        }

        public MoveEmployeeViewModel(Func<string, MessageBoxResult> errorMessage, CompanyModel model)
        {
            _model = model;
            _errorMessage = errorMessage;
        }


        #region Commands

        public Command MoveToCommand
        {
            get { return _moveToCommand ?? (_moveToCommand = new Command(MoveTo)); }
        }

        public Command MoveBackCommand
        {
            get { return _moveBackCommand ?? (_moveBackCommand = new Command(MoveBack)); }
        }

        #endregion


        #region Methods

        private void MoveTo(object index)
        {
            if (SelectedDepartment1 == null || SelectedDepartment2 == null) return;
            try
            {
                if (SelectedEmployeeIndex1 == -1)
                    return;
                _model.MoveEmployee("EmployeesMove1", "EmployeesMove2", (int)SelectedDepartment2, (int)index);
            }
            catch (ArgumentOutOfRangeException)
            {
                _errorMessage("Invalid department Id");
            }
            catch (KeyNotFoundException)
            {
                _errorMessage("Invalid employee Id");
            }
            //catch { }
            finally
            {

            }
        }

        private void MoveBack(object index)
        {
            if (SelectedDepartment1 == null || SelectedDepartment2 == null) return;
            try
            {
                if (SelectedEmployeeIndex2 == -1)
                    return;
                _model.MoveEmployee("EmployeesMove2", "EmployeesMove1", (int)SelectedDepartment1, (int)index);
            }
            catch (ArgumentOutOfRangeException)
            {
                _errorMessage("Invalid department Id");
            }
            catch (KeyNotFoundException)
            {
                _errorMessage("Invalid employee Id");
            }
            //catch { }
            finally
            {

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
