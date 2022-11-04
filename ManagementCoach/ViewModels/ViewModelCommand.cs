using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ManagementCoach.ViewModels
{
    public class ViewModelCommand : ICommand
    {

        private readonly Action<Object> _excuteAction;
        private readonly Predicate<object> _canExecuteAction;
        public ViewModelCommand(Action<Object> _excuteAction)
        {
            this._excuteAction = _excuteAction;
            this._canExecuteAction = null;
        }
        public ViewModelCommand(Action<Object> _excuteAction, Predicate<object> _canExecuteAction)
        {
            this._excuteAction = _excuteAction;
            this._canExecuteAction = _canExecuteAction;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public bool CanExecute(object parameter)
        {
            return _canExecuteAction == null ? true : _canExecuteAction(parameter);
        }

        public void Execute(object parameter)
        {
            _excuteAction(parameter);

        }
    }
}
