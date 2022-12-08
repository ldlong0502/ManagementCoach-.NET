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
    public class AddRouteViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        private readonly ErrorsViewModel _errorsViewModel;
        public int originStationId { get; set; }
        public int destinationStationId { get; set; }
        public int price { get; set; }
        public int departTime { get; set; }
        public int estimatedTime { get; set; }
        public List<int> routeRestAreas { get; set; }
        private int id;


        public Action Close { get; set; }
        public int OriginStationId
        {
            get
            {
                return originStationId;
            }
            set
            {
                originStationId = value;
                
                OnPropertyChanged(nameof(OriginStationId));
            }
        }
        public int DestinationStationId
        {
            get
            {
                return destinationStationId;
            }
            set
            {
                destinationStationId = value;

                OnPropertyChanged(nameof(DestinationStationId));
            }
        }
        public int Price
        {
            get
            {
                return price;
            }
            set
            {
                price = value;

                OnPropertyChanged(nameof(Price));
            }
        }
        public int DepartTime
        {
            get
            {
                return departTime;
            }
            set
            {
                departTime = value;

                OnPropertyChanged(nameof(DepartTime));
            }
        }
        public int EstimatedTime
        {
            get
            {
                return estimatedTime;
            }
            set
            {
                estimatedTime = value;

                OnPropertyChanged(nameof(EstimatedTime));
            }
        }
        public List<int> RouteRestAreas
        {
            get
            {
                return routeRestAreas;
            }
            set
            {
                routeRestAreas = value;

                OnPropertyChanged(nameof(RouteRestAreas));
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
        public AddRouteViewModel()
        {
            _errorsViewModel = new ErrorsViewModel();
            _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;
            SaveCommand = new ViewModelCommand(ExcuteInsertCommand, CanExcuteSaveCommand);
            CancelCommand = new ViewModelCommand(ExcuteCancelCommand);


        }
        public AddRouteViewModel(ModelRoute data)
        {
            //_errorsViewModel = new ErrorsViewModel();
            //_errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;
            //SaveCommand = new ViewModelCommand(ExcuteEditCommand, CanExcuteSaveCommand);
            //CancelCommand = new ViewModelCommand(ExcuteCancelCommand);
            //id = data.Id;
            //Name = data.Name;
            //Address = data.Address;
            //District = data.District;
            //Province = ListProvinces.Where(e => e.Id == new RepoProvince().GetProvince(data.Id).Id).FirstOrDefault();

        }

        private void ExcuteEditCommand(object obj)
        {
            try
            {
                var route = new RepoRoute();
                route.UpdateRoute(
                    id,
                    new InputRoute()
                    {
                       
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
                var route = new RepoRoute();
                if (route.InsertRoute(
                   new InputRoute()
                   {
                      
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
