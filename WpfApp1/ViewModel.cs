using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class ViewModel : INotifyPropertyChanged
    {
        private string _txt;

        public string Txt
        {
            get { return _txt; }
            set
            {
                _txt = value;
                PropertyChanged("Txt", new PropertyChangedEventArgs("Txt"));
                SelectedEmployeeData = new Employee(1, "AAA", 2, 3);
                SelectedEmployeeData.Name = _txt;
            }
        }


        private Employee _employeeData;

        public Employee SelectedEmployeeData
        {
            get { return _employeeData; }
            set
            {
                _employeeData = value;
                PropertyChanged("SelectedEmployeeData", new PropertyChangedEventArgs("SelectedEmployeeData"));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
    }
}
