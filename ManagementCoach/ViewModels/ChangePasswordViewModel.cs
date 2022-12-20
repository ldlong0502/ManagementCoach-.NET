using ManagementCoach.BE.Data.Input;
using ManagementCoach.BE.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;

namespace ManagementCoach.ViewModels
{
    public class ChangePasswordViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        private readonly ErrorsViewModel _errorsViewModel;
        public bool CanCreate => !HasErrors;
        public bool HasErrors => _errorsViewModel.HasErrors;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        private string currentPassword;
        public string CurrentPassword
        {
            get
            {
                _errorsViewModel.ClearErrors(nameof(CurrentPassword));
                if (String.IsNullOrEmpty(currentPassword))
                {
                    _errorsViewModel.AddError(nameof(CurrentPassword), "Field is required");
                }
                else if(currentPassword != MD5Helper.Decrypt(CurrentUser.currentUser.Password))
                {
                    _errorsViewModel.AddError(nameof(CurrentPassword), "Password is failed.");
                }
                return currentPassword;
            }
            set
            {
                currentPassword = value;

                OnPropertyChanged(nameof(CurrentPassword));
            }
        }
        private string newPassword;
        public string NewPassword
        {
            get
            {
                _errorsViewModel.ClearErrors(nameof(NewPassword));
                if (String.IsNullOrEmpty(newPassword))
                {
                    _errorsViewModel.AddError(nameof(NewPassword), "Field is required");
                }
                return newPassword;
            }
            set
            {
                newPassword = value;

                OnPropertyChanged(nameof(NewPassword));
            }
        }
        private string confirmNewPassword;
        public string ConfirmNewPassword
        {
            get
            {
                _errorsViewModel.ClearErrors(nameof(ConfirmNewPassword));
                if (String.IsNullOrEmpty(confirmNewPassword))
                {
                    _errorsViewModel.AddError(nameof(ConfirmNewPassword), "Field is required");
                }
                else if(confirmNewPassword != NewPassword)
                {
                    _errorsViewModel.AddError(nameof(ConfirmNewPassword), "Confirm failed.");
                }
                return confirmNewPassword;
            }
            set
            {
                confirmNewPassword = value;

                OnPropertyChanged(nameof(ConfirmNewPassword));
            }
        }
        public ICommand CurrentPasswordCommand { get; }
        public ICommand NewPasswordCommand { get; }
        public ICommand ConfirmNewPasswordCommand { get; }
        public ICommand SaveCommand { get; }
        public ChangePasswordViewModel()
        {
            _errorsViewModel = new ErrorsViewModel();
            _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;
            CurrentPasswordCommand = new ViewModelCommand(ExcuteCurrentPasswordCommand);
            NewPasswordCommand = new ViewModelCommand(ExcuteNewPasswordCommand);
            ConfirmNewPasswordCommand = new ViewModelCommand(ExcuteConfirmNewPasswordCommand);
            SaveCommand = new ViewModelCommand(ExcuteSaveCommand, CanExcuteSaveCommand);
        }

        private bool CanExcuteSaveCommand(object obj)
        {
            if (_errorsViewModel.HasErrors) return false;
            return true;
        }

        private void ExcuteSaveCommand(object obj)
        {
            var window = (System.Windows.Window)obj;
            new RepoUser().UpdateUser(CurrentUser.currentUser.Id,new InputUser()
            {
                Id = CurrentUser.currentUser.Id,
                Email = CurrentUser.currentUser.Email,
                ImageUrl = CurrentUser.currentUser.ImageUrl,
                Name = CurrentUser.currentUser.Name,
                Role = CurrentUser.currentUser.Role,
                Username = CurrentUser.currentUser.Username,
                Password = MD5Helper.Encrypt(NewPassword),
            });
            CurrentUser.currentUser = new RepoUser().GetUser(Thread.CurrentPrincipal.Identity.Name);
            MessageBox.Show("Change Password succesfully", "Change Password", MessageBoxButtons.OK, MessageBoxIcon.Information);
            window.Close();
        }

        private void ExcuteConfirmNewPasswordCommand(object obj)
        {
            var pass = (PasswordBox)obj;
            ConfirmNewPassword = pass.Password;
        }

        private void ExcuteNewPasswordCommand(object obj)
        {
            var pass = (PasswordBox)obj;
            NewPassword = pass.Password;
        }

        private void ExcuteCurrentPasswordCommand(object obj)
        {
            var pass = (PasswordBox)obj;
            CurrentPassword = pass.Password;            
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
