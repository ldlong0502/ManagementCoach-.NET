using ManagementCoach.BE;
using ManagementCoach.BE.Models;
using ManagementCoach.BE.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;

namespace ManagementCoach.ViewModels
{
    public class TripShowsViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        private List<TripShow> listTrips;
        private readonly ErrorsViewModel _errorsViewModel;
        private CoachManContext context = new CoachManContext();
        private ModelRoute route;
        private int idCoach;
        private ModelPassenger passenger;
        private List<ModelPassenger> listPassenger;
        private string title;
        private int totalPrice;
        private string textSeats;
        private List<string> chooseSeats = new List<string>();
        private bool visible = false;
        private List<Seat> listSeatDown = new List<Seat>();
        private List<Seat> listSeatUp = new List<Seat>();
        private int rows;
        private ModelTrip chooseTrip;
        private DateTime dateFilter;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        public bool CanCreate => !HasErrors;
        public bool HasErrors => _errorsViewModel.HasErrors;

        public List<string> ChooseSeats
        {
            get
            {
                return chooseSeats;
            }
            set
            {
                chooseSeats = value;
               
                OnPropertyChanged(nameof(ChooseSeats));                
            }
        }
        public DateTime DateFilter
        {
            get
            {
                return dateFilter;
            }
            set
            {
                dateFilter = value;
                _errorsViewModel.ClearErrors(nameof(DateFilter));
                if (dateFilter.CompareTo(CurrentUser.GetDateNow()) < 0)
                {
                    _errorsViewModel.AddError(nameof(DateFilter), "Can't filter the day in past.");
                    return;
                }
                Load();
                OnPropertyChanged(nameof(DateFilter));
            }
        }
        public ModelTrip ChooseTrip
        {
            get
            {
                return chooseTrip;
            }
            set
            {
                chooseTrip = value;
                OnPropertyChanged(nameof(ChooseTrip));
            }
        }
        public int IdCoach
        {
            get
            {
                return idCoach;
            }
            set
            {
                idCoach = value;
                OnPropertyChanged(nameof(IdCoach));
            }
        }
        public int TotalPrice
        {
            get
            {
                return totalPrice;
            }
            set
            {
                totalPrice = value;
                OnPropertyChanged(nameof(TotalPrice));
            }
        }
        public string TextSeats
        {
            get
            {
                return textSeats;
            }
            set
            {
                textSeats = value;
                OnPropertyChanged(nameof(TextSeats));
            }
        }
        public List<ModelPassenger> ListPassenger
        {
            get
            {
                return listPassenger;
            }
            set
            {
                listPassenger = value;
                OnPropertyChanged(nameof(ListPassenger));
            }
        }
        public ModelPassenger Passenger
        {
            get
            {
                _errorsViewModel.ClearErrors(nameof(Passenger));
                if (passenger == null)
                {
                    _errorsViewModel.AddError(nameof(Passenger), "Field is required.");
                }
                return passenger;
            }
            set
            {
                passenger = value;
                OnPropertyChanged(nameof(Passenger));
            }
        }
        public List<Seat> ListSeatUp
        {
            get
            {
                return listSeatUp;
            }
            set
            {
                listSeatUp = value;
                OnPropertyChanged(nameof(ListSeatUp));
            }
        }
        public List<Seat> ListSeatDown
        {
            get
            {
                return listSeatDown;
            }
            set
            {
                listSeatDown = value;
                OnPropertyChanged(nameof(ListSeatDown));
            }
        }
        public int Rows
        {
            get
            {
                return rows;
            }
            set
            {
                rows = value;
                OnPropertyChanged(nameof(Rows));
            }
        }
        public bool Visible
        {
            get
            {
                return visible;
            }
            set
            {
                visible = value;
                OnPropertyChanged(nameof(Visible));
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
        public ModelRoute Route
        {
            get
            {
                return route;
            }
            set
            {
                route = value;
                OnPropertyChanged(nameof(Route));
            }
        }
        public List<TripShow> ListTrips
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
        public ICommand ChooseCommand { get; }
        public ICommand ChooseSeatCommand { get; }
        public ICommand PaymentCommand { get; }
        public TripShowsViewModel()
        {
            _errorsViewModel = new ErrorsViewModel();
            _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;
            ListSeatUp = new List<Seat>() { new Seat() };
            ListSeatDown = new List<Seat>() { new Seat() };
        }
        public TripShowsViewModel(ModelRoute data, DateTime date)
        {
            _errorsViewModel = new ErrorsViewModel();
            _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;
            DateFilter = date;
            ChooseSeats = new List<string>();
            Route = data;
            ListSeatUp = new List<Seat>() { new Seat() };
            ListSeatDown = new List<Seat>() { new Seat() };
            Title = new RepoProvince().GetProvince(new RepoStation().GetStation(Route.OriginStationId).ProvinceId).Name + " -> " + new RepoProvince().GetProvince(new RepoStation().GetStation(Route.DestinationStationId).ProvinceId).Name;
            Load();
            ChooseCommand = new ViewModelCommand(ExcuteChooseCommand);
            ChooseSeatCommand = new ViewModelCommand(ExcuteChooseSeatCommand);
            PaymentCommand = new ViewModelCommand(ExcutePaymentCommand, CanExcutePaymentCommand);
        }

        private bool CanExcutePaymentCommand(object obj)
        {
           if(_errorsViewModel.HasErrors)
           {
                return false;
           }
           return true;
        }

        private void ExcutePaymentCommand(object obj)
        {
            if(ChooseSeats.Count() == 0)
            {
                MessageBox.Show("Please choose at least 1 seat");
                return;
            }
            DialogResult ret = MessageBox.Show("Do you choose this option?", "Payment", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if(ret == DialogResult.OK)
            {

                    ChooseSeats.ForEach(seat =>
                    {
                        var modelSeat = new RepoCoachSeat().GetCoachSeats(ChooseTrip.CoachId).Find(c=> c.Name == seat);
                        new RepoTicket().InsertTicket(new BE.Data.Input.InputTicket()
                        {
                            CoachSeatId = modelSeat.Id,
                            TripId = ChooseTrip.Id,
                            PassengerId = Passenger.Id,                         
                        });
                    });
                    MessageBox.Show("Payment Successfully", "Payment", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ExcuteChooseCommand(ChooseTrip.Id);
 
               
               
            }
        }

        private void ExcuteChooseSeatCommand(object obj)
        {

            System.Windows.Controls.Button btn = (System.Windows.Controls.Button)obj;
            if(btn.BorderBrush == Brushes.Black)
            {
                btn.BorderBrush = Brushes.Yellow;
                ChooseSeats.Add(btn.Content.ToString());
                TotalPrice = Route.Price * ChooseSeats.Count;
                TextSeats = "";
                int i = 0;
                chooseSeats.ForEach(item => {
                    if (i == 0)
                    {
                        TextSeats += item;
                        i++;
                    }
                    else
                    {
                        TextSeats += ", " + item;
                    }

                });
            }
                
            else
            {
                btn.BorderBrush = Brushes.Black;
                ChooseSeats.Remove(btn.Content.ToString());
                TotalPrice = Route.Price * ChooseSeats.Count;
                TextSeats = "";
                int i = 0;
                chooseSeats.ForEach(item => {
                    if (i == 0)
                    {
                        TextSeats += item;
                        i++;
                    }
                    else
                    {
                        TextSeats += ", " + item;
                    }

                });
            }
        }

        private void ExcuteChooseCommand(object obj)
        {
            Passenger = null;
            Visible = true;
            ChooseTrip = new RepoTrip().GetTrip((int)obj);
            ListSeatUp.Clear();
            ListSeatDown.Clear();
            TextSeats = "";
            TotalPrice = 0;
            ChooseSeats.Clear();
            var listDown = new RepoCoachSeat().GetCoachSeats(ChooseTrip.CoachId).Where(c => c.Name.StartsWith("A")).ToList();
            var temp = new List<Seat>();
            listDown.Sort((a, b) => int.Parse(a.Name.Split('A')[1]).CompareTo(int.Parse(b.Name.Split('A')[1])));
            listDown.ForEach(seat =>
            {
                if (context.Tickets.Any(c => c.CoachSeatId == seat.Id))
                {
                    temp.Add(new Seat()
                    {
                        ModelCoachSeat = seat,
                        IsEnabled = false,
                        Brush = Brushes.Black,
                        
                    });
                }
                else
                {
                    temp.Add(new Seat()
                    {
                        ModelCoachSeat = seat,
                        IsEnabled = true,
                        Brush = Brushes.Black,
                    });
                }
            });
            ListSeatDown = temp;
            var listUp = new RepoCoachSeat().GetCoachSeats(ChooseTrip.CoachId).Where(c => c.Name.StartsWith("B")).ToList();
            listUp.Sort((a, b) => int.Parse(a.Name.Split('B')[1]).CompareTo(int.Parse(b.Name.Split('B')[1])));
            temp = new List<Seat>();
            listUp.ForEach(seat =>
            {
                if (context.Tickets.Any(c => c.CoachSeatId == seat.Id))
                {
                    temp.Add(new Seat()
                    {
                        ModelCoachSeat = seat,
                        IsEnabled = false,
                        Brush = Brushes.Black,
                    });
                }
                else
                {
                    temp.Add(new Seat()
                    {
                        ModelCoachSeat = seat,
                        IsEnabled = true,
                        Brush = Brushes.Black,
                    });
                }
            });
            ListSeatUp = temp;
            Rows = ListSeatDown.Count() / 2;
        }
        public  DateTime GetDateTrip(ModelTrip trip)
        {
            return new DateTime(trip.Date.Year, trip.Date.Month, trip.Date.Day, trip.DepartTime / 60 , trip.DepartTime % 60, 0);
        }
        private void Load()
        {
            if (Route == null) return;
            ListPassenger = new RepoPassenger().GetPassengers("",1,context.Passengers.Count()).Items;
            Passenger = null;
            Visible = true;
            ListSeatUp = new List<Seat>();
            ListSeatDown = new List<Seat>();
            TextSeats = "";
            TotalPrice = 0;
            ChooseSeats.Clear();
            //Get list Trips by id route
            ListTrips = new List<TripShow>();
            var listTrip = new List<ModelTrip>();
            listTrip = new RepoTrip().GetTripsByRoute(Route.Id).Items.Where(trip => trip.Date.CompareTo(DateFilter) == 0).ToList();
            listTrip.Sort((a, b) => a.DepartTime.CompareTo(b.DepartTime));
            var temp = new List<TripShow>();
            listTrip.ForEach(trip => { 
                var coach = new RepoCoach().GetCoach(trip.CoachId);
                var originStation = new RepoStation().GetStation(Route.OriginStationId);
                var desStation = new RepoStation().GetStation(Route.DestinationStationId);
                var listTicket = new RepoTicket().GetTickets(1, context.Tickets.Count()).Items.Where(ticket => ticket.TripId == trip.Id).ToList();
                var listCoachSeat = new RepoCoachSeat().GetCoachSeats(coach.Id);
                var routeRestArea = new RepoRoute().GetRestAreasOfRoute(Route.Id);
                var list = new List<string>();
               
                int i = 0;
                routeRestArea.ForEach(route => {
                    list.Add("Rest Area " + ++i + ": " + route.Name);
                });
                if(list.Count() == 0)
                {
                    list.Add("Don't have rest areas");
                }
                temp.Add(new TripShow()
                {
                    Id = trip.Id,
                    NameCoach = coach.Name,
                    DepartureTime = trip.DepartTime,
                    Destinationtime = trip.DepartTime + trip.EstimatedTime,
                    OriginStationName = originStation.Name,
                    DestinationStationName = desStation.Name,
                    ImageUrl = string.IsNullOrEmpty(coach.ImageUrl) ? "/Images/coach.jpg" : coach.ImageUrl,
                    AvailableSeats = "Only " + (listCoachSeat.Count() - listTicket.Count()) + " seats available",
                    Price = Route.Price,
                    ListRestAreas = list,
                    Date = trip.Date
                }) ;
            
            });
            ListTrips = temp;
            // 


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

    public class TripShow
    {
        public int Id { get; set; }
        public string NameCoach { get; set; }
        public string OriginStationName { get; set; }
        public string DestinationStationName { get; set; }
        public int DepartureTime { get; set; }
        public int Destinationtime { get; set; }
        public string AvailableSeats { get; set; }
        public DateTime Date { get; set; }
        public int Price { get; set; }
        public string ImageUrl { get; set; }
        public List<string> ListRestAreas { get; set; }
    }

    public class Seat
    {
        public ModelCoachSeat ModelCoachSeat { get; set; }
        public bool IsEnabled { get; set; }
        public Brush Brush { get; set; }
    }
}
