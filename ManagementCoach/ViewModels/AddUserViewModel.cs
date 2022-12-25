using ManagementCoach.BE.Data.Input;
using ManagementCoach.BE.Models;
using ManagementCoach.BE.Repositories;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

using System.Windows.Input;

namespace ManagementCoach.ViewModels
{
    public class AddUserViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        private readonly ErrorsViewModel _errorsViewModel;
        public Action Close { get; set; }
        private int id;
        private string name;
        private string username;
        private string password;
        private string email;
        private string role;
        private List<string> listRole = new List<string>() {  "Employee", "Admin"};
        private string title;
        private string imageUrl = "/Images/avatar.png";
        public string ImageUrl
        {
            get
            {
                return imageUrl;
            }
            set
            {
                imageUrl = value;
                OnPropertyChanged(nameof(ImageUrl));
            }
        }
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        public int Id
        {
            get => id; set
            {
                id = value;

                OnPropertyChanged(nameof(Id));
            }
        }
        public string Name
        {
            get
            {
                _errorsViewModel.ClearErrors(nameof(Name));
                if (String.IsNullOrEmpty(name))
                {
                    _errorsViewModel.AddError(nameof(Name), "Field is required.");
                }
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));

            }
        }
        public string Username
        {
            get
            {
                _errorsViewModel.ClearErrors(nameof(Username));
                if (String.IsNullOrEmpty(username))
                {
                    _errorsViewModel.AddError(nameof(Username), "Field is required.");
                }
                
                return username;
            }
            set
            {
                username = value;
                OnPropertyChanged(nameof(Username));
            }
        }
        public string Password
        {
            get
            {
                _errorsViewModel.ClearErrors(nameof(Password));
                if (String.IsNullOrEmpty(password))
                {
                    _errorsViewModel.AddError(nameof(Password), "Field is required.");
                }
                return password;
            }
            set
            {
                password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        public string Role
        {
            get
            {
                _errorsViewModel.ClearErrors(nameof(Role));
                if (String.IsNullOrEmpty(role))
                {
                    _errorsViewModel.AddError(nameof(Role), "Field is required.");
                }
                return role;
            }
            set
            {
                role = value;
                OnPropertyChanged(nameof(Role));
            }
        }
        public string Email
        {
            get
            {
                string strRegex = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
                Regex re = new Regex(strRegex);
                _errorsViewModel.ClearErrors(nameof(Email));

                if (String.IsNullOrEmpty(email))
                {
                    _errorsViewModel.AddError(nameof(Email), "Field is required.");
                }
                else if (!re.IsMatch(email))
                {
                    _errorsViewModel.AddError(nameof(Email), "Inavlid email.");
                }
                return email;
            }
            set
            {
                email = value;
                OnPropertyChanged(nameof(Email));
            }
        }
        
        public List<string> ListRole
        {
            get { return listRole; }
            set
            {
                listRole = value;
                OnPropertyChanged(nameof(ListRole));
            }
        }
        public bool CanCreate => !HasErrors;
        public bool HasErrors => _errorsViewModel.HasErrors;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;


        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand ChooseLicenseCommand { get; }
        public ICommand SeeLicenseCommand { get; }
        public ICommand ImageCommand { get; }

        public AddUserViewModel()
        {
            _errorsViewModel = new ErrorsViewModel();
            _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;
            SaveCommand = new ViewModelCommand(ExcuteSaveCommand, CanExcuteSaveCommand);
            CancelCommand = new ViewModelCommand(ExcuteCancelCommand);
            ImageCommand = new ViewModelCommand(ExcuteImageCommand);
            Title = "Add User";
            Role = ListRole.First();

        }
       

        public AddUserViewModel(ModelUser data)
        {
            _errorsViewModel = new ErrorsViewModel();
            _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;
            SaveCommand = new ViewModelCommand(ExcuteEditCommand, CanExcuteSaveCommand);
            CancelCommand = new ViewModelCommand(ExcuteCancelCommand);
            ImageCommand = new ViewModelCommand(ExcuteImageCommand);
            Id = data.Id;
            Name = data.Name;
            Role = data.Role;
            Password = MD5Helper.Decrypt(data.Password);
            Username = data.Username;
            Email =data.Email;
            Title = "Update User";
            ImageUrl = data.ImageUrl;

        }
        private void ExcuteImageCommand(object obj)
        {
            System.Windows.Forms.OpenFileDialog openFD = new System.Windows.Forms.OpenFileDialog();
            openFD.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif;...";

            if (openFD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ImageUrl = openFD.FileName;
            }
        }
        private void ExcuteEditCommand(object obj)
        {
            try
            {
                var editUser = new RepoUser().UpdateUser(Id, new InputUser()
                {
                    Id = Id,
                    Name = Name,
                    Username= Username,
                    Password = MD5Helper.Encrypt(Password),
                    Email = Email,
                    Role = Role,
                    ImageUrl = ImageUrl,

                });
                if (editUser.Success == true)
                {
                    MessageBox.Show("Successfully");
                    Close();
                }
                else
                {
                    MessageBox.Show(editUser.ErrorMessage);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ExcuteCancelCommand(object obj)
        {
            var window = obj as Window;
            window.Close();
        }

        private bool CanExcuteSaveCommand(object obj)
        {
            if (_errorsViewModel.HasErrors)
                return false;
            return true;
        }

        private void ExcuteSaveCommand(object obj)
        {
            try
            {
                var addUser = new RepoUser().InsertUser(new InputUser()
                {
                    Name = Name,
                    Username = Username,
                    Password = MD5Helper.Encrypt(Password),
                    Email = Email,
                    Role = Role,
                    ImageUrl = ImageUrl,

                });
                if (addUser.Success == true)
                {
                    MessageBox.Show("Successfully");
                    Close();
                }
                else
                {
                    MessageBox.Show(addUser.ErrorMessage);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

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
