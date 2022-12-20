using ManagementCoach.BE.Data.Input;
using ManagementCoach.BE.Repositories;
using ManagementCoach.Views.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Principal;
using System.Text;
using System.Threading;
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
            
            string pass = MD5Helper.Encrypt(Password);
            if(new RepoUser().UserValid(UserName, pass))
            {
                Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(UserName), null);
                CurrentUser.currentUser = new RepoUser().GetUser(Thread.CurrentPrincipal.Identity.Name);
                DashBoard db = new DashBoard();
                CurrentUser.dashBoard = db;
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
