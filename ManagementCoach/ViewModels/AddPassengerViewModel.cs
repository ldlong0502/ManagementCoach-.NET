﻿using ManagementCoach.BE.Data.Input;
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
    public class AddPassengerViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        private readonly ErrorsViewModel _errorsViewModel;
        public Action Close { get; set; }
        private int id;
        private string name;
        private string idCard;
        private string gender;
        private DateTime dob;
        private string email;
        private string phone;
        private string address;
        private DateTime dateJoined;
        private bool block;
        private string license;
        private string notes;
        private List<string> listGender = new List<string>() { "Male", "Female" };
        private string title;

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
        public string IdCard
        {
            get
            {
                _errorsViewModel.ClearErrors(nameof(IdCard));
                if (String.IsNullOrEmpty(idCard))
                {
                    _errorsViewModel.AddError(nameof(IdCard), "Field is required.");
                }
                else if (idCard.Length < 9)
                {
                    _errorsViewModel.AddError(nameof(IdCard), "Value length >= 9 characters.");
                }
                return idCard;
            }
            set
            {
                idCard = value;
                OnPropertyChanged(nameof(IdCard));
            }
        }
        public string Gender
        {
            get
            {
                _errorsViewModel.ClearErrors(nameof(Gender));
                if (String.IsNullOrEmpty(gender))
                {
                    _errorsViewModel.AddError(nameof(Gender), "Field is required.");
                }
                return gender;
            }
            set
            {
                gender = value;
                OnPropertyChanged(nameof(Gender));
            }
        }
        public DateTime Dob
        {
            get
            {
                _errorsViewModel.ClearErrors(nameof(Dob));
                if (DateTime.Compare(dob, DateTime.Now) > 0)
                {
                    _errorsViewModel.AddError(nameof(Dob), "*Dob must earlier than now");
                }
                return dob;
            }
            set
            {
                dob = value;
                OnPropertyChanged(nameof(Dob));
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
        public string Phone
        {
            get
            {
                string strRegex = @"(84|0[3|5|7|8|9])+([0-9]{8})\b";
                Regex re = new Regex(strRegex);

                // The IsMatch method is used to validate
                // a string or to ensure that a string
                // conforms to a particular pattern.

                _errorsViewModel.ClearErrors(nameof(Phone));
                if (String.IsNullOrEmpty(phone))
                {
                    _errorsViewModel.AddError(nameof(Phone), "Field is required.");
                }
                else if (!re.IsMatch(phone))
                {
                    _errorsViewModel.AddError(nameof(Phone), "Inavlid phone number.");
                }
                return phone;
            }
            set
            {
                phone = value;
                OnPropertyChanged(nameof(Phone));
            }
        }
        public string Address { get => address; set { address = value; OnPropertyChanged(nameof(Address)); } }
        public DateTime DateJoined
        {
            get => dateJoined; set
            {
                dateJoined = value;
                _errorsViewModel.ClearErrors(nameof(DateJoined));
                if (DateTime.Compare(DateJoined, DateTime.Now) > 0)
                {
                    _errorsViewModel.AddError(nameof(DateJoined), "*DateJoined must earlier than now");
                }
                OnPropertyChanged(nameof(DateJoined));
            }
        }
        public bool Block { get => block; set { block = value; OnPropertyChanged(nameof(Block)); } }
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


        public AddPassengerViewModel()
        {
            _errorsViewModel = new ErrorsViewModel();
            _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;
            SaveCommand = new ViewModelCommand(ExcuteSaveCommand, CanExcuteSaveCommand);
            CancelCommand = new ViewModelCommand(ExcuteCancelCommand);
            DateJoined = DateTime.Now;
            Dob = DateTime.Now;
            Gender = ListGender.First();
            Title = "Add Passenger";

        }
        public AddPassengerViewModel(ModelPassenger data)
        {
            _errorsViewModel = new ErrorsViewModel();
            _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;
            SaveCommand = new ViewModelCommand(ExcuteEditCommand, CanExcuteSaveCommand);
            CancelCommand = new ViewModelCommand(ExcuteCancelCommand);
            Id = data.Id;
            Name = data.Name;
            block = data.Blocked;
            Address = data.Address;
            Dob = data.Dob;
            Email = data.Email;
            Gender = data.Gender;
            IdCard = data.IdCard;
            Notes = data.Notes;
            Phone = data.Phone;
            Title = "Update Passenger";

        }

        private void ExcuteEditCommand(object obj)
        {
            try
            {
                var editPassenger = new RepoPassenger().UpdatePassenger(Id, new InputPassenger()
                {
                    Name = Name,
                    Blocked = Block,
                    Address = Address,
                    Dob = Dob,
                    Email = Email,
                    Gender = Gender,
                    IdCard = IdCard,
                    Notes = Notes,
                    Phone = Phone,

                });
                if (editPassenger.Success == true)
                {
                    MessageBox.Show("Successfully");
                    Close();
                }
                else
                {
                    MessageBox.Show(editPassenger.ErrorMessage);
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
                var addPassenger = new RepoPassenger().InsertPassenger(new InputPassenger()
                {
                    Name = Name,
                    Blocked = Block,
                    Address = Address,
                    Dob = Dob,
                    Email = Email,
                    Gender = Gender,
                    IdCard = IdCard,
                    Notes = Notes,
                    Phone = Phone,
                });
                if (addPassenger.Success == true)
                {
                    MessageBox.Show("Successfully");
                    Close();
                }
                else
                {
                    MessageBox.Show(addPassenger.ErrorMessage);
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
