using ManagementCoach.BE;
using ManagementCoach.BE.Data.Input;
using ManagementCoach.BE.Models;
using ManagementCoach.BE.Repositories;
using ManagementCoach.Views.Screens;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ManagementCoach.ViewModels
{ 
    public class LoginViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        private readonly ErrorsViewModel _errorsViewModel;
        private CoachManContext context = new CoachManContext();
        private List<ModelUser> listUsers = new List<ModelUser>();
        private string userName;
        private string password;

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
                _errorsViewModel.ClearErrors(nameof(UserName));
                if (string.IsNullOrEmpty(UserName))
                {
                    _errorsViewModel.AddError(nameof(UserName), "Field is required.");
                }
                else if(!listUsers.Any(c=> c.Username == UserName))
                {
                    _errorsViewModel.AddError(nameof(UserName), "UserName doesn't exist.");
                }
                OnPropertyChanged(nameof(UserName));
            }
        }
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
                _errorsViewModel.ClearErrors(nameof(Password));
                if (string.IsNullOrEmpty(Password))
                {
                    _errorsViewModel.AddError(nameof(Password), "Field is required.");
                }
                else if (!listUsers.Any(c => c.Username == UserName && c.Password ==MD5Helper.Encrypt(Password)))
                {
                    _errorsViewModel.AddError(nameof(Password), "Password doesn't exist.");
                }
                OnPropertyChanged(nameof(Password));
            }
        }


        public bool CanCreate => !HasErrors;
        public bool HasErrors => _errorsViewModel.HasErrors;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public ICommand LoginCommand { get; }
        public LoginViewModel()
        {
            _errorsViewModel = new ErrorsViewModel();
            _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;
            listUsers = new RepoUser().GetUsers("", 1, context.Users.Count()).Items;
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
            if ((UserName == null || Password == null ) || _errorsViewModel.HasErrors)
            {
                return false;
            }
            return true;
        }
        private void ErrorsViewModel_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
            OnPropertyChanged(nameof(CanCreate));
        }

        public IEnumerable GetErrors(string propertyName)
        {
            return _errorsViewModel.GetErrors(propertyName);
        }
    }
}
