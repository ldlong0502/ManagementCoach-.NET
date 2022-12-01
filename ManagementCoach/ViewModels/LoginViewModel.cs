using ManagementCoach.Views.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ManagementCoach.ViewModels
{ 
    public class LoginViewModel : ViewModelBase
    {
        private string userName;
        private String password;

        public Action CloseAction { get; set; }
        public string UserName
        {
            get
            {
                return userName;
            }
            set
            {
                userName = value;
               
                OnPropertyChanged(nameof(UserName));
            }
        }
        public String Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
                
                OnPropertyChanged(nameof(Password));
            }
        }
        public ICommand LoginCommand { get; }
        public LoginViewModel()
        {
            LoginCommand = new ViewModelCommand(ExcuteLoginCommand, CanExcuteLoginCommand);
        }

        private void ExcuteLoginCommand(object obj)
        {
            if(UserName == "admin" && Password == "admin")
            {
                DashBoard db = new DashBoard();
                db.Show();
                CloseAction();
            }
        }

        private bool CanExcuteLoginCommand(object obj)
        {
            return true;
        }
    }
}
