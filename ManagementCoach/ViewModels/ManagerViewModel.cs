﻿using ManagementCoach.BE;
using ManagementCoach.BE.Entities;
using ManagementCoach.Views;
using ManagementCoach.Views.Screens;
using ManagementCoach.Views.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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



		//Icommand
		public ICommand ShowTripsCommand { get; }
		public ICommand ShowPassengersCommand { get; }
		public ICommand ShowCoachesCommand { get; }
		public ICommand ShowDriversCommand { get; }
		public ICommand ShowStationsCommand { get; }
		public ICommand ShowRoutesCommand { get; }
		public ICommand ShowRestAreasCommand { get; }
        public ICommand ShowTicketsCommand { get; }

        public ICommand AddCommand { get; }
		public ICommand ImportCommand { get; }
		public ICommand ExportCommand { get; }

		public ManagerViewModel()
        {
            ShowTripsCommand = new ViewModelCommand(ExcuteShowTripsCommand);
            ShowPassengersCommand = new ViewModelCommand(ExcuteShowPassengersCommand);
            ShowCoachesCommand = new ViewModelCommand(ExcuteShowCoachesCommand);
            ShowDriversCommand = new ViewModelCommand(ExcuteShowDriversCommand);
            ShowStationsCommand = new ViewModelCommand(ExcuteShowStationsCommand);
            ShowRoutesCommand = new ViewModelCommand(ExcuteShowRoutesCommand);
            ShowRestAreasCommand = new ViewModelCommand(ExcuteShowRestAreasCommand);
            ShowTicketsCommand = new ViewModelCommand(ExcuteShowTicketsCommand);
            AddCommand = new ViewModelCommand(ExcuteAddCommand);
            ImportCommand = new ViewModelCommand(ExcuteImportCommand);
            ExportCommand = new ViewModelCommand(ExcuteExportCommand);
            ExcuteShowTripsCommand(null);
        }

        private void ExcuteShowTicketsCommand(object obj)
        {
            Title = "Tickets";
            AddAction = "Add new ticket";
            CurrentControl = new TicketUserControl();
            CurrentManagerView = new TicketViewModel();
            CurrentControl.DataContext = CurrentManagerView;
        }

		private static Dictionary<string, Action<string, CoachManContext>> _exportExcelHandlerByTitle =
			new Dictionary<string, Action<string, CoachManContext>>() 
		{
				{ "Tickets", (sheetName, context) => ExcelHelper.ExportSingleSheetAs(sheetName, context.Tickets) },
				{ "Trips", (sheetName, context) => ExcelHelper.ExportSingleSheetAs(sheetName, context.Trips) },
				{ "Coaches", (sheetName, context) => {
					var filePath = ExcelHelper.ExportSingleSheetAs(sheetName, context.Coaches);
					if (filePath != null)
					{
						ExcelHelper.Export(filePath, "CoachSeats", context.CoachSeats);
					}
				} },
				{ "Drivers", (sheetName, context) => ExcelHelper.ExportSingleSheetAs(sheetName, context.Drivers) },
				{ "Passengers", (sheetName, context) => ExcelHelper.ExportSingleSheetAs(sheetName, context.Passengers) },
				{ "Stations", (sheetName, context) => ExcelHelper.ExportSingleSheetAs(sheetName, context.Stations) },
				{ "RestAreas", (sheetName, context) => ExcelHelper.ExportSingleSheetAs(sheetName, context.RestAreas) },
				{ "Routes", (sheetName, context) => ExcelHelper.ExportSingleSheetAs(sheetName, context.Routes) },
				{ "Users", (sheetName, context) => ExcelHelper.ExportSingleSheetAs(sheetName, context.Users) },
		};

		private void ExcuteExportCommand(object obj)
        {
			var context = new CoachManContext();
			_exportExcelHandlerByTitle[Title](Title, context);
        }

		private static Dictionary<string, Action<string>> _importExcelHandlerByTitle =
			new Dictionary<string, Action<string>>()
		{
				{ "Tickets", (sheetName) => ExcelHelper.ImportFromFile<Ticket>(sheetName) },
				{ "Trips", (sheetName) => ExcelHelper.ImportFromFile<Trip>(sheetName) },
				{ "Coaches", (sheetName) => {
					var (filePath, dropData) = ExcelHelper.ImportFromFile<Coach>(sheetName);
					if (filePath != null)
					{
						ExcelHelper.Import<CoachSeat>(filePath, "CoachSeats", dropData);
					}	
				} 
				},
				{ "Drivers", (sheetName) => ExcelHelper.ImportFromFile<Driver>(sheetName) },
				{ "Passengers", (sheetName) => ExcelHelper.ImportFromFile<Passenger>(sheetName) },
				{ "Stations", (sheetName) => ExcelHelper.ImportFromFile<Station>(sheetName) },
				{ "RestAreas", (sheetName) => ExcelHelper.ImportFromFile<RestArea>(sheetName) },
				{ "Routes", (sheetName) => ExcelHelper.ImportFromFile<Route>(sheetName) },
				{ "Users", (sheetName) => ExcelHelper.ImportFromFile<User>(sheetName) },
		};

		private void ExcuteImportCommand(object obj)
        {
			_importExcelHandlerByTitle[Title](Title);
			if (CurrentManagerView is ILoadableViewModel loadableViewModel)
			{
				loadableViewModel.Load();
			}
		}

        private void ExcuteShowRestAreasCommand(object obj)
        {
            Title = "RestAreas";
            AddAction = "Add new rest area";
            CurrentControl = new RestAreaUserControl();
            CurrentManagerView = new RestAreaViewModel();
            CurrentControl.DataContext = CurrentManagerView;
        }

        private void ExcuteShowRoutesCommand(object obj)
        {
            Title = "Routes";
            AddAction = "Add new route";
            CurrentControl = new RouteUserControl();
            CurrentManagerView = new RouteViewModel();
            CurrentControl.DataContext = CurrentManagerView;
        }

        private void ExcuteShowStationsCommand(object obj)
        {
            Title = "Stations";
            AddAction = "Add new station";
            CurrentControl = new StationUserControl();
            CurrentManagerView = new StationViewModel();
            CurrentControl.DataContext = CurrentManagerView;
        }

        private void ExcuteShowDriversCommand(object obj)
        {
            Title = "Drivers";
            AddAction = "Add new driver";
            CurrentControl = new DriverUserControl();
            CurrentManagerView = new DriverViewModel();
            CurrentControl.DataContext = CurrentManagerView;
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
                var screen = new AddNewDriver((CurrentManagerView as DriverViewModel));
                screen.ShowDialog();
            }
            else if (Title == "Passengers")
            {
                var screen = new AddNewPassenger((CurrentManagerView as PassengerViewModel));
                screen.ShowDialog();
            }
            else if (Title == "Stations")
            {
                var screen = new AddNewStation((CurrentManagerView as StationViewModel ));
                screen.ShowDialog();
            }
            else if (Title == "RestAreas")
            {
                var screen = new AddNewRestArea((CurrentManagerView as RestAreaViewModel));
                screen.ShowDialog();
            }
            else if (Title == "Routes")
            {
                var screen = new AddNewRoute((CurrentManagerView as RouteViewModel));
                screen.ShowDialog();
            }
            else if (Title == "Trips")
            {
                var screen = new AddNewTrip((CurrentManagerView as TripViewModel));
                screen.ShowDialog();
            }
            else if (Title == "Tickets")
            {
                var screen = new AddNewTicket((CurrentManagerView as TicketViewModel));
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
        }

        private void ExcuteShowPassengersCommand(object obj)
        {
            Title = "Passengers";
            AddAction = "Add new passenger";
            CurrentControl = new PassengerUserControl();
            CurrentManagerView = new PassengerViewModel();
            CurrentControl.DataContext = CurrentManagerView;
        }

        private void ExcuteShowTripsCommand(object obj)
        {
            Title = "Trips";
            AddAction = "Add new trip";
            CurrentControl = new TripsUserControl();
            CurrentManagerView = new TripViewModel();
            CurrentControl.DataContext = CurrentManagerView;
        }
    }
}
