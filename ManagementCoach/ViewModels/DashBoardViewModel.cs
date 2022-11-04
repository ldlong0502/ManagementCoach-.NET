using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ManagementCoach.ViewModels
{
    public class DashBoardViewModel :ViewModelBase
    {
        private ViewModelBase _currentChildView;
        public ViewModelBase CurrentChildView { 
            get 
            {
                return _currentChildView; 
            }
            set
            {
                _currentChildView = value;
                OnPropertyChanged(nameof(CurrentChildView));
            }
        }
        //ICommand
        public ICommand ShowHomeViewCommand { get; }
        public ICommand ShowManagerViewCommand { get; }
        public ICommand ShowAccountViewCommand { get; }

        public DashBoardViewModel()
        {
            ShowHomeViewCommand = new ViewModelCommand(ExecuteShowHomeViewCommand);
            ShowManagerViewCommand = new ViewModelCommand(ExecuteShowManagerViewCommand);
            ShowAccountViewCommand = new ViewModelCommand(ExecuteShowAccountViewCommand);
            ExecuteShowHomeViewCommand(null);



        }

        private void ExecuteShowAccountViewCommand(object obj)
        {
            CurrentChildView = new AccountViewModel();
            
        }

        private void ExecuteShowManagerViewCommand(object obj)
        {
            CurrentChildView = new ManagerViewModel();
        }

        private void ExecuteShowHomeViewCommand(object obj)
        {
            CurrentChildView = new HomeViewModel();         
            
        }
    }
}
