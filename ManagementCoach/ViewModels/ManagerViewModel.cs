using ManagementCoach.Views.Screens;
using ManagementCoach.Views.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ManagementCoach.ViewModels
{
    public class ManagerViewModel : ViewModelBase
    {
        private ViewModelBase _currentManagerView;
        private UserControl _currentControl;
        private string title;
        private string addAction;

        //borderbrush change 
        private SolidColorBrush borderBrushTrips = new SolidColorBrush(Colors.Transparent);
        private SolidColorBrush borderBrushPassengers = new SolidColorBrush(Colors.Transparent);
        private SolidColorBrush borderBrushCoaches = new SolidColorBrush(Colors.Transparent);
        private SolidColorBrush borderBrushDrivers = new SolidColorBrush(Colors.Transparent);
        private SolidColorBrush borderBrushStations = new SolidColorBrush(Colors.Transparent);


        public UserControl CurrentControl
        {
            get
            {
                return _currentControl;
            }
            set
            {
                _currentControl = value;
                OnPropertyChanged(nameof(CurrentControl));
            }
        }
        public ViewModelBase CurrentManagerView
        {
            get
            {
                return _currentManagerView;
            }
            set
            {
                _currentManagerView = value;
                OnPropertyChanged(nameof(CurrentManagerView));
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
        public string AddAction
        {
            get
            {
                return addAction;
            }
            set
            {
                addAction = value;
                OnPropertyChanged(nameof(AddAction));
            }
        }


        public SolidColorBrush BorderBrushTrips
        {
            get
            {
                return borderBrushTrips;
            }
            set
            {
                borderBrushTrips = value;
                OnPropertyChanged(nameof(BorderBrushTrips));
            }
        }
        public SolidColorBrush BorderBrushPassengers
        {
            get
            {
                return borderBrushPassengers;
            }
            set
            {
                borderBrushPassengers = value;
                OnPropertyChanged(nameof(BorderBrushPassengers));
            }
        }
        public SolidColorBrush BorderBrushCoaches
        {
            get
            {
                return borderBrushCoaches;
            }
            set
            {
                borderBrushCoaches = value;
                OnPropertyChanged(nameof(BorderBrushCoaches));
            }
        }
        public SolidColorBrush BorderBrushDrivers
        {
            get
            {
                return borderBrushDrivers;
            }
            set
            {
                borderBrushDrivers = value;
                OnPropertyChanged(nameof(BorderBrushDrivers));
            }
        }
        public SolidColorBrush BorderBrushStations
        {
            get
            {
                return BorderBrushStations;
            }
            set
            {
                borderBrushStations = value;
                OnPropertyChanged(nameof(BorderBrushStations));
            }
        }

        //Icommand
        public ICommand ShowTripsCommand { get; }
        public ICommand ShowPassengersCommand { get; }
        public ICommand ShowCoachesCommand { get; }
        public ICommand ShowDriversCommand { get; }
        public ICommand AddCommand { get; }

        public ManagerViewModel()
        {
            ShowTripsCommand = new ViewModelCommand(ExcuteShowTripsCommand);
            ShowPassengersCommand = new ViewModelCommand(ExcuteShowPassengersCommand);
            ShowCoachesCommand = new ViewModelCommand(ExcuteShowCoachesCommand);
            ShowDriversCommand = new ViewModelCommand(ExcuteShowDriversCommand);
            AddCommand = new ViewModelCommand(ExcuteAddCommand);
            ExcuteShowTripsCommand(null);
        }

        private void ExcuteShowDriversCommand(object obj)
        {
            Title = "Drivers";
            AddAction = "Add new driver";
            CurrentControl = new DriverUserControl();
            CurrentManagerView = new DriverViewModel();
            ChangeBrushColor();
        }

        private void ExcuteAddCommand(object obj)
        {
            if(Title == "Coaches")
            {
                var screen = new AddNewCoach((CurrentManagerView as CoachViewModel));
                
                screen.ShowDialog();
            }
            else if(Title == "Drivers")
            {
                var screen = new AddNewDriver();
                screen.ShowDialog();
            }
        }

        private void ExcuteShowCoachesCommand(object obj)
        {
            Title = "Coaches";
            AddAction = "Add new coach";
            CurrentControl = new CoachUserControl();
            CurrentManagerView = new CoachViewModel();
            CurrentControl.DataContext = CurrentManagerView;
            ChangeBrushColor();
        }

        private void ChangeBrushColor()
        {
            if(Title == "Trips")
            {
                BorderBrushTrips = new SolidColorBrush(Colors.Blue);
                BorderBrushPassengers = new SolidColorBrush(Colors.Transparent);
                BorderBrushCoaches =  new SolidColorBrush(Colors.Transparent);
                BorderBrushDrivers = new SolidColorBrush(Colors.Transparent);
                BorderBrushStations = new SolidColorBrush(Colors.Transparent);

            }
            else if (Title == "Passengers")
            {
                BorderBrushTrips = new SolidColorBrush(Colors.Transparent);
                BorderBrushPassengers = new SolidColorBrush(Colors.Blue);
                BorderBrushCoaches = new SolidColorBrush(Colors.Transparent);
                BorderBrushDrivers = new SolidColorBrush(Colors.Transparent);
                BorderBrushStations = new SolidColorBrush(Colors.Transparent);
            }
            else if(Title == "Coaches")
            {
                BorderBrushTrips = new SolidColorBrush(Colors.Transparent);
                BorderBrushPassengers = new SolidColorBrush(Colors.Transparent);
                BorderBrushCoaches = new SolidColorBrush(Colors.Blue);
                BorderBrushDrivers = new SolidColorBrush(Colors.Transparent);
                BorderBrushStations = new SolidColorBrush(Colors.Transparent);
            }
            else if (Title == "Drivers")
            {
                BorderBrushTrips = new SolidColorBrush(Colors.Transparent);
                BorderBrushPassengers = new SolidColorBrush(Colors.Transparent);
                BorderBrushCoaches = new SolidColorBrush(Colors.Transparent);
                BorderBrushDrivers = new SolidColorBrush(Colors.Blue);
                BorderBrushStations = new SolidColorBrush(Colors.Transparent);
            }
            else if (Title == "Stations")
            {
                BorderBrushTrips = new SolidColorBrush(Colors.Transparent);
                BorderBrushPassengers = new SolidColorBrush(Colors.Transparent);
                BorderBrushCoaches = new SolidColorBrush(Colors.Transparent);
                BorderBrushDrivers = new SolidColorBrush(Colors.Transparent);
                BorderBrushStations = new SolidColorBrush(Colors.Blue);
            }

        }

        private void ExcuteShowPassengersCommand(object obj)
        {
            Title = "Passengers";
            AddAction = "Add new passenger";

            ChangeBrushColor();
        }

        private void ExcuteShowTripsCommand(object obj)
        {
            Title = "Trips";
            AddAction = "Add new trip";
            CurrentControl = new TripsUserControl();
            CurrentManagerView = new TripViewModel();
            CurrentControl.DataContext = CurrentManagerView;
            ChangeBrushColor();
        }
    }
}
