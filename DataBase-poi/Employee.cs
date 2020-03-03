using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase_poi
{
    public class Employee : INotifyPropertyChanged
    {
        private int _code;
        private string _name;
        private int _age;
        private double _salary;

        public int Code
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

        public int Age
        {
            get { return _age; }
            set
            {
                _age = value;
                OnPropertyChanged("Age");
            }
        }

        public double Salary
        {
            get { return _salary; }
            set
            {
                _salary = value;
                OnPropertyChanged("Salary");
            }
        }

        public Employee(Employee employee)
        {
            _code = employee.Code;
            _name = employee.Name;
            _age = employee.Age;
            _salary = employee.Salary;
        }

        public Employee(int code, string name, int age, double salary)
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
