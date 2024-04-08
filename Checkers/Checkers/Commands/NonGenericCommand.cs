using Checkers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Checkers.Commands
{
    class NonGenericCommand:ICommand
    {
        private Action commandTask;
        private Predicate<object> canExecuteTask;

        public NonGenericCommand(Action toDo) : this(toDo, DefaultCanExecute)
        {
            commandTask = toDo;
        }

        public NonGenericCommand(Action toDo, Predicate<object> canExecute)
        {
            commandTask = toDo;
            canExecuteTask = canExecute;
        }

        private static bool DefaultCanExecute(object parameter)
        {
            return true;
        }
        public bool CanExecute(object parameter)
        {
            return canExecuteTask != null && canExecuteTask(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }

            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public void Execute(object parameter)
        {
            commandTask();
        }
    }
}
