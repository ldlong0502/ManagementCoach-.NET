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
    public class AddTicketViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        private readonly ErrorsViewModel _errorsViewModel;
        private CoachManContext context = new CoachManContext();
        private ModelPassenger passenger;
        private ModelTrip trip;
        private List<ModelCoachSeat> listModelCoachSeats;
        private List<ModelPassenger> listPassengers;
        private List<ModelTrip> listTrips;
        private ModelCoachSeat coachSeat;
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
        public ModelTrip Trip
        {
            get
            {
                _errorsViewModel.ClearErrors(nameof(Trip));
                if (trip == null)
                {
                    _errorsViewModel.AddError(nameof(Trip), "Field is required");
                }
                return trip;
            }
            set
            {
                trip = value;
                OnPropertyChanged(nameof(Trip));
                if (Trip != null)
                     ListModelCoachSeats = new RepoCoachSeat().GetCoachSeats(Trip.CoachId).ToList();
               
            }
        }
        public ModelPassenger Passenger
        {
            get
            {
                _errorsViewModel.ClearErrors(nameof(Passenger));
                if (passenger == null)
                {
                    _errorsViewModel.AddError(nameof(Passenger), "Field is required");
                }
                return passenger;
            }
            set
            {
                passenger = value;

                OnPropertyChanged(nameof(Passenger));
            }
        }
        public List<ModelPassenger> ListPassengers
        {
            get
            {
                return listPassengers;
            }
            set
            {
                listPassengers = value;

                OnPropertyChanged(nameof(ListPassengers));
            }
        }
        public List<ModelTrip> ListTrips
        {
            get
            {
                
                return listTrips;

            }
            set
            {
                listTrips = value;
                OnPropertyChanged(nameof(ListTrips));
            }
        }
        public ModelCoachSeat CoachSeat
        {
            get
            {
                _errorsViewModel.ClearErrors(nameof(CoachSeat));
                if (coachSeat == null)
                {
                    _errorsViewModel.AddError(nameof(CoachSeat), "Field is required");
                }
                return coachSeat;

            }
            set
            {
                coachSeat = value;
                OnPropertyChanged(nameof(CoachSeat));
            }
        }
        public List<ModelCoachSeat> ListModelCoachSeats
        {
            get
            {
                return listModelCoachSeats;
            }
            set
            {
                listModelCoachSeats = value;

                OnPropertyChanged(nameof(ListModelCoachSeats));
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
        public AddTicketViewModel()
        {
            ListTrips = new RepoTrip().GetTrips(1, context.Trips.Count()).Items;           
            ListPassengers = new RepoPassenger().GetPassengers("", 1, context.Passengers.Count()).Items;
         
            _errorsViewModel = new ErrorsViewModel();
            _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;
            SaveCommand = new ViewModelCommand(ExcuteInsertCommand, CanExcuteSaveCommand);
            CancelCommand = new ViewModelCommand(ExcuteCancelCommand);
            Title = "Add Ticket";
        }

     
        public AddTicketViewModel(ModelTicket data)
        {
            ListTrips = new RepoTrip().GetTrips(1, context.Trips.Count()).Items;
            ListPassengers = new RepoPassenger().GetPassengers("", 1, context.Passengers.Count()).Items;
            _errorsViewModel = new ErrorsViewModel();
            _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;
            SaveCommand = new ViewModelCommand(ExcuteEditCommand, CanExcuteSaveCommand);
            CancelCommand = new ViewModelCommand(ExcuteCancelCommand);
            Title = "Update Ticket";
            id = data.Id;
            CoachSeat = new RepoCoachSeat().GetCoachSeat(data.CoachSeatId);
            Passenger = new RepoPassenger().GetPassenger(data.PassengerId);
            Trip = new RepoTrip().GetTrip(data.TripId);
        }

        private void ExcuteEditCommand(object obj)
        {
            try
            {
                var ticket = new RepoTicket().UpdateTicket(
                    id,
                    new InputTicket()
                    {
                        CoachSeatId = CoachSeat.Id,
                        PassengerId = Passenger.Id,
                        TripId = Trip.Id,

                    }
                );
                if (ticket.Success)
                {
                    MessageBox.Show("Successfull");
                }
                else
                {
                    MessageBox.Show(ticket.ErrorMessage);
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
                var ticket = new RepoTicket().InsertTicket(
                   new InputTicket()
                   {
                       CoachSeatId = CoachSeat.Id,
                       PassengerId = Passenger.Id,
                       TripId = Trip.Id,

                   }
               );
                if (ticket.Success == true)
                {
                    MessageBox.Show("Successfull");
                }
                else
                {
                    MessageBox.Show(ticket.ErrorMessage);
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
