using ManagementCoach.BE.Data.Input;
using ManagementCoach.BE.Entities;
using ManagementCoach.BE.Models;
using ManagementCoach.BE.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ManagementCoach.ViewModels
{
    public class AddRestAreaViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        private readonly ErrorsViewModel _errorsViewModel;
        private string name;
        private string district;
        private string address;
        private ModelProvince province;
        private int id;
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

        public Action Close { get; set; }
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
        public string Address
        {
            get
            {
                _errorsViewModel.ClearErrors(nameof(Address));
                if (String.IsNullOrEmpty(address))
                {
                    _errorsViewModel.AddError(nameof(Address), "Field is required.");
                }
                return address;
            }
            set
            {
                address = value;
                OnPropertyChanged(nameof(Address));
            }
        }
        public ModelProvince Province
        {
            get
            {
                _errorsViewModel.ClearErrors(nameof(Province));
                if (province == null)
                {
                    _errorsViewModel.AddError(nameof(Province), "Field is required.");
                }
                return province;
            }
            set
            {
                province = value;
                OnPropertyChanged(nameof(Province));
            }
        }

        private List<ModelProvince> listProvinces = new RepoProvince().GetProvinces("");
        public List<ModelProvince> ListProvinces
        {
            get
            {
                return listProvinces;

            }
            set
            {
                listProvinces = value;
                OnPropertyChanged(nameof(ListProvinces));
            }

        }
        public bool CanCreate => !HasErrors;
        public bool HasErrors => _errorsViewModel.HasErrors;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;


        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public AddRestAreaViewModel()
        {
            _errorsViewModel = new ErrorsViewModel();
            _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;
            SaveCommand = new ViewModelCommand(ExcuteInsertCommand, CanExcuteSaveCommand);
            CancelCommand = new ViewModelCommand(ExcuteCancelCommand);
            Province = ListProvinces.First();
            Title = "Add Rest Area";

        }
        public AddRestAreaViewModel(ModelRestArea data)
        {
            _errorsViewModel = new ErrorsViewModel();
            _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;
            SaveCommand = new ViewModelCommand(ExcuteEditCommand, CanExcuteSaveCommand);
            CancelCommand = new ViewModelCommand(ExcuteCancelCommand);
            id = data.Id;
            Name = data.Name;
            Address = data.Address;
            Province = ListProvinces.Where(e => e.Id == new RepoProvince().GetProvince(data.Id).Id).FirstOrDefault();
            Title = "Update Rest Area";
        }

        private void ExcuteEditCommand(object obj)
        {
            try
            {
                var coach = new RepoRestArea();
                coach.UpdateRestArea(
                    id,
                    new InputRestArea()
                    {
                        Name = Name,
                        Address = Address,
                        ProvinceId = Province.Id,
                    }
                );
                MessageBox.Show("Successfull");
                Close();
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

        private void ExcuteInsertCommand(object obj)
        {
            try
            {
                var coach = new RepoRestArea();
                if (coach.InsertRestArea(
                   new InputRestArea()
                   {
                       Name = Name,
                       Address = Address,
                       ProvinceId = Province.Id,
                   }
               ).Success == true)
                {
                    MessageBox.Show("Successfull");
                }
                else
                {
                    MessageBox.Show("The RegNo has alreaady existed!");
                    return;
                }

                Close();


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
