using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DataBase_poi_MVVM
{
    class Command : ICommand
    {
        #region Fields

        protected Action<object> action = null;
        private bool canExecute = false;

        #endregion

        public Command(Action<object> action, bool canExecute = true)
        {
            this.action = action;
            this.canExecute = canExecute;
        }

        #region ICommand

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return canExecute;
        }

        public void Execute(object parameter)
        {
            action(parameter);
        }

        #endregion
    }
}
