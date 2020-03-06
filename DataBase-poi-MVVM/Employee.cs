using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase_poi_MVVM
{
    public class Employee : INotifyPropertyChanged
    {
        //private int _code;
        private string _code;
        private string _name;
        //private int _age;
        private string _age;
        //private double _salary;
        private string _salary;

        //public int Code
        public string Code
        {
            get { return _code; }
            set
            {
                _code = value;
                OnPropertyChanged("Code");
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        //public int Age
        public string Age
        {
            get { return _age; }
            set
            {
                _age = value;
                OnPropertyChanged("Age");
            }
        }

        //public double Salary
        public string Salary
        {
            get { return _salary; }
            set
            {
                _salary = value;
                OnPropertyChanged("Salary");
            }
        }

        public Employee()
        {
            _code = "0";
            _name = "";
            _age = "0";
            _salary = "0";
        }

        public Employee(Employee employee)
        {
            _code = employee.Code;
            _name = employee.Name;
            _age = employee.Age;
            _salary = employee.Salary;
        }

        //public Employee(int code, string name, int age, double salary)
        public Employee(string code, string name, string age, string salary)
        {
            _code = code;
            _name = name;
            _age = age;
            _salary = salary;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
