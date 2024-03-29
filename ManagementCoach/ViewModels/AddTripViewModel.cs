﻿using ManagementCoach.BE;
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
    public class AddTripViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        private readonly ErrorsViewModel _errorsViewModel;
        private CoachManContext context = new CoachManContext();
        public Action Close { get; set; }
        private int id;
        private ModelRoute route;
        private ModelCoach coach;
        private ModelDriver driver;
        private DateTime date;
        private string estimatedTime;
        private DateTime departTime;
        private bool cancelled;
        private List<ModelRoute> listRoute ;
        private List<ModelCoach> listCoach;
        private List<ModelDriver> listDriver;
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
        public ModelRoute Route
        {
            get
            {
                _errorsViewModel.ClearErrors(nameof(Route));
                if (route == null)
                {
                    _errorsViewModel.AddError(nameof(Route), "Field is required.");
                }
                return route;
            }
            set
            {
                route = value;
                OnPropertyChanged(nameof(Route));

            }
        }
        public List<ModelRoute> ListRoute
        {
            get
            {
              
                return listRoute;
            }
            set
            {
                listRoute = value;
                OnPropertyChanged(nameof(ListRoute));

            }
        }
        public List<ModelCoach> ListCoach
        {
            get
            {

                return listCoach;
            }
            set
            {
                listCoach = value;
                OnPropertyChanged(nameof(ListCoach));

            }
        }
        public List<ModelDriver> ListDriver
        {
            get
            {

                return listDriver;
            }
            set
            {
                listDriver = value;
                OnPropertyChanged(nameof(ListDriver));

            }
        }
        public ModelCoach Coach
        {
            get
            {
                _errorsViewModel.ClearErrors(nameof(Coach));
                if (coach == null)
                {
                    _errorsViewModel.AddError(nameof(Coach), "Field is required.");
                }
                return coach;
            }
            set
            {
                coach = value;
                OnPropertyChanged(nameof(Coach));

            }
        }
        public ModelDriver Driver
        {
            get
            {
                _errorsViewModel.ClearErrors(nameof(Driver));
                if (driver == null)
                {
                    _errorsViewModel.AddError(nameof(Driver), "Field is required.");
                }
                return driver;
            }
            set
            {
                driver = value;
                OnPropertyChanged(nameof(Driver));

            }
        }
        public DateTime Date
        {
            get
            {
                _errorsViewModel.ClearErrors(nameof(Date));
                if (DateTime.Compare(date, DateTime.Today) < 0)
                {
                    _errorsViewModel.AddError(nameof(Date), "Date must later than now");
                }
                              
                return date;
            }
            set
            {
                date = value;
                DepartTime = DepartTime;
                OnPropertyChanged(nameof(Date));
            }
        }
        public string EstimatedTime
        {
            get
            {
                _errorsViewModel.ClearErrors(nameof(EstimatedTime));
                if (string.IsNullOrEmpty(estimatedTime) || !int.TryParse(estimatedTime, out int a))
                {
                    _errorsViewModel.AddError(nameof(EstimatedTime), "Value not invalid");
                }
                
                return estimatedTime;
            }
            set
            {
                estimatedTime = value;
                OnPropertyChanged(nameof(EstimatedTime));
            }
        }
        public bool Cancelled
        {
            get
            {
               
                return cancelled;
            }
            set
            {
                cancelled = value;
                OnPropertyChanged(nameof(Cancelled));
            }
        }
        public DateTime DepartTime
        {
            get
            {
                _errorsViewModel.ClearErrors(nameof(DepartTime));
                if (departTime == null)
                {
                    _errorsViewModel.AddError(nameof(DepartTime), "Field is required.");
                }
                else if(DateTime.Compare(Date,DateTime.Today) == 0 && (departTime.Hour * 60 + departTime.Minute - 180 < DateTime.Now.Hour * 60 + DateTime.Now.Minute))
                {
                    _errorsViewModel.AddError(nameof(DepartTime), "Your trip must planed before 3 hours.");
                }
                return departTime;
            }
            set
            {
                departTime = value;
                OnPropertyChanged(nameof(DepartTime));
            }
        }
        public bool CanCreate => !HasErrors;
        public bool HasErrors => _errorsViewModel.HasErrors;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;


        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand ChooseLicenseCommand { get; }
        public ICommand SeeLicenseCommand { get; }

        public AddTripViewModel()
        {
            ListRoute = new RepoRoute().GetRoutes(1, context.Routes.Count()).Items;
            ListCoach= new RepoCoach().GetCoaches("",1, context.Coaches.Count()).Items;
            ListDriver = new RepoDriver().GetDrivers("",1, context.Drivers.Count()).Items;
            _errorsViewModel = new ErrorsViewModel();
            _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;
            SaveCommand = new ViewModelCommand(ExcuteSaveCommand, CanExcuteSaveCommand);
            CancelCommand = new ViewModelCommand(ExcuteCancelCommand);
            Date = DateTime.Today;
            Title = "Add Trip";
        }
       
        
        public AddTripViewModel(ModelTrip data)
        {
            ListRoute = new RepoRoute().GetRoutes(1, context.Routes.Count()).Items;
            ListCoach = new RepoCoach().GetCoaches("", 1, context.Coaches.Count()).Items;
            ListDriver = new RepoDriver().GetDrivers("", 1, context.Drivers.Count()).Items;
            _errorsViewModel = new ErrorsViewModel();
            _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;
            SaveCommand = new ViewModelCommand(ExcuteEditCommand, CanExcuteSaveCommand);
            CancelCommand = new ViewModelCommand(ExcuteCancelCommand);
            Id = data.Id;
            Route = new RepoRoute().GetRoute(data.RouteId);
            Coach = new RepoCoach().GetCoach(data.CoachId);
            Driver = new RepoDriver().GetDriver(data.DriverId);
            EstimatedTime = data.EstimatedTime.ToString();
            DepartTime = ConvertIntToDateTime(data.DepartTime);
            Cancelled = data.Cancelled;
            Date = data.Date;
            Title = "Update Trip";

           

        }

        private int ConvertTimeToInt()
        {
            return DepartTime.Hour * 60 + DepartTime.Minute;
        }
        private DateTime ConvertIntToDateTime(int x)
        {
            var hour = x / 60;
            var minute = x % 60;
            return new DateTime(1,1,1,hour, minute ,0);
        }
        private void ExcuteEditCommand(object obj)
        {
            try
            {
                var editTrip = new RepoTrip().UpdateTrip(Id, new InputTrip()
                {
                    RouteId = Route.Id,
                    DriverId = Driver.Id,
                    CoachId = Coach.Id,
                    DepartTime = ConvertTimeToInt(),
                    Date = Date,
                    Cancelled = Cancelled,
                    EstimatedTime = int.Parse(EstimatedTime),

                }); ; ; ;
                if (editTrip.Success == true)
                {
                    MessageBox.Show("Successfully");
                    Close();
                }
                else
                {
                    MessageBox.Show(editTrip.ErrorMessage);
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
                var time = ConvertTimeToInt();
                var y = context.Trips.Any(
                    x => (x.CoachId == Coach.Id || x.DriverId == Driver.Id) 
                && ( time <= x.DepartTime + x.EstimatedTime && time >= x.DepartTime ) 
                && x.Date.CompareTo(Date) == 0 && x.RouteId == Route.Id );
                if (y)
                {
                    MessageBox.Show("Same time with an other trip");
                    return;
                }
                var addTrip = new RepoTrip().InsertTrip(new InputTrip()
                {
                    RouteId = Route.Id,
                    DriverId = Driver.Id,
                    CoachId = Coach.Id,
                    DepartTime = ConvertTimeToInt(),
                    Date = Date,
                    Cancelled = Cancelled,
                    EstimatedTime = int.Parse(EstimatedTime),
                });
                if (addTrip.Success == true)
                {
                    MessageBox.Show("Successfully");
                    Close();
                }
                else
                {
                    MessageBox.Show(addTrip.ErrorMessage);
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
