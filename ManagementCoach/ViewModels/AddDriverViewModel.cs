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
using System.Threading.Tasks;
using System.Windows;

using System.Windows.Input;

namespace ManagementCoach.ViewModels
{
    public class AddDriverViewModel: ViewModelBase, INotifyDataErrorInfo
    {
        private readonly ErrorsViewModel _errorsViewModel;
        public Action Close { get ; set ; }
        private int id;
        private string name;
        private string idCard;
        private string gender;
        private DateTime dob;
        private string email;
        private string phone;
        private string address;
        private DateTime dateJoined;
        private bool active;
        private string license;
        private string notes;
        private List<string> listGender = new List<string>() { "Male", "Female" };
       
        public int Id { get => id; set {
                id = value;
                
                OnPropertyChanged(nameof(Id)); 
            } }
        public string Name { get => name; set { 
                name = value;
                _errorsViewModel.ClearErrors(nameof(Name));
                if (String.IsNullOrEmpty(Name))
                {
                    _errorsViewModel.AddError(nameof(Name), "*Invalid name.");
                }
                OnPropertyChanged(nameof(Name)); } }
        public string IdCard { get => idCard; set { 
                idCard = value;
                _errorsViewModel.ClearErrors(nameof(IdCard));
                if (String.IsNullOrEmpty(IdCard) || IdCard.Length < 9)
                {
                    _errorsViewModel.AddError(nameof(IdCard), "*Invalid IdCard.");
                }
                OnPropertyChanged(nameof(IdCard)); } }
        public string Gender { get => gender; set { gender = value;  OnPropertyChanged(nameof(Gender)); } }
        public DateTime Dob { get => dob; set { dob = value;
                _errorsViewModel.ClearErrors(nameof(Dob));
                if (DateTime.Compare(Dob, DateTime.Now) > 0)
                {
                    _errorsViewModel.AddError(nameof(Dob), "*Dob must earlier than now");
                }
                OnPropertyChanged(nameof(Dob)); } }
        public string Email { get => email; set { email = value; OnPropertyChanged(nameof(Email)); } }
        public string Phone { get => phone; set { phone = value; OnPropertyChanged(nameof(Phone)); } }
        public string Address { get => address; set { address = value; OnPropertyChanged(nameof(Address)); } }
        public DateTime DateJoined { get => dateJoined; set { dateJoined = value;
                _errorsViewModel.ClearErrors(nameof(DateJoined));
                if (DateTime.Compare(DateJoined, DateTime.Now) > 0)
                {
                    _errorsViewModel.AddError(nameof(DateJoined), "*DateJoined must earlier than now");
                }
                OnPropertyChanged(nameof(DateJoined)); } }
        public bool Active { get => active ; set { active = value; OnPropertyChanged(nameof(Active)); } }
        public string License { get => license; set { license = value; OnPropertyChanged(nameof(License)); } }
        public string Notes { get => notes; set { notes = value; OnPropertyChanged(nameof(Notes)); } }      
        public IEnumerable<string> ListGender
        {
            get { return listGender; }
        }
        public bool CanCreate => !HasErrors;
        public bool HasErrors => _errorsViewModel.HasErrors;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;


        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand ChooseLicenseCommand { get; }
        public ICommand SeeLicenseCommand { get; }

        public AddDriverViewModel()
        {
            _errorsViewModel = new ErrorsViewModel();
            _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;
            SaveCommand = new ViewModelCommand(ExcuteSaveCommand, CanExcuteSaveCommand);
            CancelCommand = new ViewModelCommand(ExcuteCancelCommand);
            ChooseLicenseCommand = new ViewModelCommand(ExcuteChooseLicenseCommand);
            SeeLicenseCommand = new ViewModelCommand(ExcuteSeeLicenseCommand, CanExcuteSeeLicenseCommand);
            DateJoined = DateTime.Now;
            Dob = DateTime.Now;
            Gender = ListGender.First();

        }
        private bool CanExcuteSeeLicenseCommand(object obj)
        {
            if (string.IsNullOrEmpty(License))
            {
                return false;
            }
            return true;
        }
        private void ExcuteSeeLicenseCommand(object obj)
        {
            System.Diagnostics.Process.Start("explorer.exe", License);
        }

        private void ExcuteChooseLicenseCommand(object obj)
        {
            System.Windows.Forms.OpenFileDialog openFD = new System.Windows.Forms.OpenFileDialog();
            openFD.Filter = "Bitmaps|*.bmp|jpeg|*.jpg";

            if (openFD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                License = openFD.FileName;
            }

        }

        public AddDriverViewModel(ModelDriver data)
        {
            _errorsViewModel = new ErrorsViewModel();
            _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;
            SaveCommand = new ViewModelCommand(ExcuteEditCommand, CanExcuteSaveCommand);
            CancelCommand = new ViewModelCommand(ExcuteCancelCommand);
            Id = data.Id;
            Name = data.Name;
            Active = data.Active;
            Address = data.Address;
            DateJoined = data.DateJoined;
            Dob = data.Dob;
            Email = data.Email;
            Gender = data.Gender;
            IdCard = data.IdCard;
            License = data.License;
            Notes = data.Notes;
            Phone = data.Phone;

        }
       
        private void ExcuteEditCommand(object obj)
        {
            try
            {
                var editDriver = new RepoDriver().UpdateDriver(Id, new InputDriver()
                {
                    Name = Name,
                    Active = Active,
                    Address = Address,
                    DateJoined = DateJoined,
                    Dob = Dob,
                    Email = Email,
                    Gender = Gender,
                    IdCard = IdCard,
                    License = License,
                    Notes = Notes,
                    Phone = Phone,
                   
                });
                if (editDriver.Success == true)
                {
                    MessageBox.Show("Successfully");
                    Close();
                }
                else
                {
                    MessageBox.Show(editDriver.ErrorMessage);
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
                var addDriver = new RepoDriver().InsertDriver(new InputDriver()
                {
                    Name = Name,
                    Active = Active,
                    Address = Address,
                    DateJoined = DateJoined,
                    Dob = Dob,
                    Email = Email,
                    Gender = Gender,
                    IdCard = IdCard,
                    License = License,
                    Notes = Notes,
                    Phone = Phone,
                });
                if (addDriver.Success == true)
                {
                    MessageBox.Show("Successfully");
                    Close();
                }
                else
                {
                    MessageBox.Show(addDriver.ErrorMessage);
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
