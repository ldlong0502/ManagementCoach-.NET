using ManagementCoach.BE;
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
        private CoachManContext context = new CoachManContext();
        private ModelStation originStation;
        private ModelStation destinationStation;
        private string price;
        private object departTime;
        private int estimatedTime;
        private List<int> routeRestAreas;
        private List<ModelStation> listStations ;
        private List<ModelRestArea> listRestAreas ;
        private string textRestAreas;
        private ModelRestArea restArea;
        private bool visibleListRestArea = false;
        private int id;
        private int count = 0;
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
        public string TextRestAreas
        {
            get
            {
                return textRestAreas;
            }
            set
            {
                textRestAreas = value;

                OnPropertyChanged(nameof(TextRestAreas));
            }
        }
        public ModelRestArea RestArea
        {
            get
            {
                return restArea;
            }
            set
            {
                restArea = value;

                OnPropertyChanged(nameof(RestArea));
            }
        }
        public bool VisibleListRestArea
        {
            get
            {
                return visibleListRestArea;
            }
            set
            {
                visibleListRestArea = value;

                OnPropertyChanged(nameof(VisibleListRestArea));
            }
        }
        public List<ModelRestArea> ListRestAreas
        {
            get
            {
                return listRestAreas;
            }
            set
            {
                listRestAreas = value;

                OnPropertyChanged(nameof(ListRestAreas));
            }
        }
        public ModelStation OriginStation
        {
            get
            {
                _errorsViewModel.ClearErrors(nameof(OriginStation));
                if (originStation == null)
                {
                    _errorsViewModel.AddError(nameof(OriginStation), "Field is required.");
                }
                else if (destinationStation == originStation)
                {
                    _errorsViewModel.AddError(nameof(OriginStation), "The value sames with destinationStation.");
                }
                return originStation;
                
            }
            set
            {
                originStation = value;
                OnPropertyChanged(nameof(OriginStation));
            }
        }
        public ModelStation DestinationStation
        {
            get
            {
                _errorsViewModel.ClearErrors(nameof(DestinationStation));
                if (destinationStation == null)
                {
                    _errorsViewModel.AddError(nameof(DestinationStation), "Field is required.");
                }
                else if(destinationStation == originStation)
                {
                    _errorsViewModel.AddError(nameof(DestinationStation), "The value sames with originStation.");
                }
                return destinationStation;
                
            }
            set
            {
                destinationStation = value;
                OnPropertyChanged(nameof(DestinationStation));
            }
        }
        public string Price
        {
            get
            {
                _errorsViewModel.ClearErrors(nameof(Price));
                if (string.IsNullOrEmpty(price) || !int.TryParse(price, out int a))
                {
                    _errorsViewModel.AddError(nameof(Price), "Value not invalid");
                }
                return price;
            }
            set
            {
                price = value;
                OnPropertyChanged(nameof(Price));
            }
        }
        public List<ModelStation> ListStations
        {
            get
            {
                return listStations;
            }
            set
            {
                listStations = value;

                OnPropertyChanged(nameof(ListStations));
            }
        }
        public object DepartTime
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

        public bool CanCreate => !HasErrors;
        public bool HasErrors => _errorsViewModel.HasErrors;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;


        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand ChooseRestAreaCommand { get; }
        public ICommand AddRestAreaCommand { get; }
        public ICommand RemoveRestAreaCommand { get; }
        public AddRouteViewModel()
        {
            ListStations = new RepoStation().GetStations("", 1, context.Stations.Count()).Items;
            ListRestAreas = new RepoRestArea().GetRestAreas("", 1, context.RestAreas.Count()).Items;
            RouteRestAreas = new List<int>();
            _errorsViewModel = new ErrorsViewModel();
            _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;
            SaveCommand = new ViewModelCommand(ExcuteInsertCommand, CanExcuteSaveCommand);
            CancelCommand = new ViewModelCommand(ExcuteCancelCommand);
            ChooseRestAreaCommand = new ViewModelCommand(ExcuteChooseRestAreaCommand);
            AddRestAreaCommand = new ViewModelCommand(ExcuteAddRestAreaCommand);
            RemoveRestAreaCommand = new ViewModelCommand(ExcuteRemoveRestAreaCommand);
            Title = "Add Route";
        }

        private void ExcuteAddRestAreaCommand(object obj)
        {
           
            if (RestArea == null || RouteRestAreas.Exists(id => id == RestArea.Id)) return;
            TextRestAreas = "";
            count = 0;
            RouteRestAreas.Add(RestArea.Id);
            RouteRestAreas.ForEach((route) =>
            {
                if (count == 0)
                {
                    TextRestAreas += route.ToString() + " ";

                }
                else
                {
                    TextRestAreas += "->" + route.ToString() + " ";
                }
                count++;
            });

        }
        private void ExcuteRemoveRestAreaCommand(object obj)
        {
            if (RestArea == null || !RouteRestAreas.Exists(id => id == RestArea.Id)) return;
            TextRestAreas = "";
            count = 0;
            RouteRestAreas.Remove(RestArea.Id);
            RouteRestAreas.ForEach((route) =>
            {
                if (count == 0)
                {
                    TextRestAreas += route.ToString() + " ";

                }
                else
                {
                    TextRestAreas += "->" + route.ToString() + " ";
                }
                count++;
            });

        }

        private void ExcuteChooseRestAreaCommand(object obj)
        {
            count = 0;
            RestArea = null;
            VisibleListRestArea = !VisibleListRestArea;
        }

        public AddRouteViewModel(ModelRoute data)
        {
            ListStations = new RepoStation().GetStations("", 1, context.Stations.Count()).Items;
            ListRestAreas = new RepoRestArea().GetRestAreas("", 1, context.RestAreas.Count()).Items;
            RouteRestAreas = new List<int>();
            _errorsViewModel = new ErrorsViewModel();
            _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;
            SaveCommand = new ViewModelCommand(ExcuteEditCommand, CanExcuteSaveCommand);
            CancelCommand = new ViewModelCommand(ExcuteCancelCommand);
            ChooseRestAreaCommand = new ViewModelCommand(ExcuteChooseRestAreaCommand);
            AddRestAreaCommand = new ViewModelCommand(ExcuteAddRestAreaCommand);
            RemoveRestAreaCommand = new ViewModelCommand(ExcuteRemoveRestAreaCommand);
            Price = data.Price.ToString();
            DestinationStation = new RepoStation().GetStation(data.DestinationStationId);
            OriginStation = new RepoStation().GetStation(data.OriginStationId);
            id = data.Id;
            Title = "Update Route";
            data.RestAreas.ForEach(route => {
            {
              RouteRestAreas.Add(route.Id);
            } });
            count = 0;
            RouteRestAreas.ForEach((route) =>
            {
                if (count == 0)
                {
                    TextRestAreas += route.ToString() + " ";

                }
                else
                {
                    TextRestAreas += "-> " + route.ToString() + " ";
                }
                count++;
            });
        }

        private void ExcuteEditCommand(object obj)
        {
            try
            {
                var route = new RepoRoute().UpdateRoute(
                    id,
                    new InputRoute()
                    {
                       DepartTime = 0,
                       DestinationStationId = DestinationStation.Id,
                       OriginStationId = OriginStation.Id,
                       EstimatedTime = 0,
                       Price = int.Parse(Price),
                       RouteRestAreaIdList = RouteRestAreas

                    }
                );
                if (route.Success)
                {
                    MessageBox.Show("Successfull");
                }
                else
                {
                    MessageBox.Show(route.ErrorMessage);
                }
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
                var route = new RepoRoute().InsertRoute(
                   new InputRoute()
                   {
                       DepartTime = 0,
                       DestinationStationId = DestinationStation.Id,
                       OriginStationId = OriginStation.Id,
                       EstimatedTime = 0,
                       Price = int.Parse(Price),
                       RouteRestAreaIdList = RouteRestAreas,
                   }
               );
                if (route.Success == true)
                {
                    MessageBox.Show("Successfull");
                }
                else
                {
                    MessageBox.Show(route.ErrorMessage);
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
