using ManagementCoach.BE;
using ManagementCoach.BE.Repositories;
using ManagementCoach.Views.Screens;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace ManagementCoach.ViewModels
{
    [ObservableObject]
    public class AdminHomeViewModel : ViewModelBase
    {
       
        private CoachManContext context = new CoachManContext();
        private string welcomeText;
        private string displayedImagePath = "C:/Users/LENOVO/Downloads/ImageCoach/coach1.jpg";
        private string avatar;
        private string percentNewPassengersWeek;
        private int newPassengersWeek = 0;
        private int totalIncome = 0;
        private string percentTodayIncome;
        private int todayIncome = 0;
        
        public int TodayIncome
        {
            get
            {
                return todayIncome;
            }
            set
            {
                todayIncome = value;
                OnPropertyChanged(nameof(TodayIncome));
            }
        }
        public string PercentTodayIncome
        {
            get
            {
                return percentTodayIncome;
            }
            set
            {
                percentTodayIncome = value;
                OnPropertyChanged(nameof(PercentTodayIncome));
            }
        }
        public int NewPassengersWeek
        {
            get
            {
                return newPassengersWeek;
            }
            set
            {
                newPassengersWeek = value;
                OnPropertyChanged(nameof(NewPassengersWeek));
            }
        }
        public string PercentNewPassengersWeek
        {
            get
            {
                return percentNewPassengersWeek;
            }
            set
            {
                percentNewPassengersWeek = value;
                OnPropertyChanged(nameof(PercentNewPassengersWeek));
            }
        }
        public int TotalIncome
        {
            get
            {
                return totalIncome;
            }
            set
            {
                totalIncome = value;
                OnPropertyChanged(nameof(TotalIncome));
            }
        }
        private int totalTrips = 0;
        public int TotalTrips
        {
            get
            {
                return totalTrips;
            }
            set
            {
                totalTrips = value;
                OnPropertyChanged(nameof(TotalTrips));
            }
        }
        private int totalEmployees = 0;
        public int TotalEmployees
        {
            get
            {
                return totalEmployees;
            }
            set
            {
                totalEmployees = value;
                OnPropertyChanged(nameof(TotalEmployees));
            }
        }
        private int totalTicketsSelled = 0;
        public int TotalTicketsSelled
        {
            get
            {
                return totalTicketsSelled;
            }
            set
            {
                totalTicketsSelled = value;
                OnPropertyChanged(nameof(TotalTicketsSelled));
            }
        }
        public string Avatar
        {
            get
            {
                return avatar;
            }
            set
            {
                avatar = value;
                OnPropertyChanged(nameof(Avatar));
            }
        }
        public string DisplayedImagePath
        {
            get
            {
                return displayedImagePath;
            }
            set
            {
                displayedImagePath = value;
                OnPropertyChanged(nameof(DisplayedImagePath));
            }
        }
        public string WelcomeText
        {
            get
            {
                return welcomeText;
            }
            set
            {
                welcomeText = value;
                OnPropertyChanged(nameof(WelcomeText));
            }
        }
        public ICommand LogOutCommand { get; }
        public ICommand ChangePasswordCommand { get; }


        public AdminHomeViewModel()
        {
            Load();
            Chart();
            LogOutCommand = new ViewModelCommand(ExcuteLogOutCommand);
            ChangePasswordCommand = new ViewModelCommand(ExcuteChangePasswordCommand);
        }
        public ISeries[] SeriesCollection { get; set; }
        private List<string> list7Day = new List<string>();
        public Axis[] XAxes { get; set; } 
        private void Chart()
        {
            var today = DateTime.Today;
            var values = new List<int>();
            var last7day = DateTime.Today.AddDays(-6);
            while(last7day.CompareTo(today) <=0)
            {
                int moneyDay = 0;
                list7Day.Add(last7day.Date.ToString("dd/MM/yyyy"));
                var listTicketsDay = new RepoTicket().GetTickets(1, context.Tickets.Count()).Items.Where(c => getDate(c.DateBought).CompareTo(last7day) == 0).ToList();
                listTicketsDay.ForEach(ticket =>
                {
                    var trip = new RepoTrip().GetTrip(ticket.TripId);
                    var route = new RepoRoute().GetRoute(trip.RouteId);
                    moneyDay += route.Price;
                });
                values.Add(moneyDay);
                last7day = last7day.AddDays(1);
            }
            SeriesCollection = new ISeries[] { 
                new LineSeries<int> 
                {  
                    Values = values  
                } 
                
            };
            XAxes = new Axis[]
                {

                        new Axis
                        {
                            Labels = list7Day,
                            SeparatorsPaint = new SolidColorPaint(new SKColor(200, 200, 200)),
                            SeparatorsAtCenter = true,
                            TicksPaint = new SolidColorPaint(new SKColor(35, 35, 35)),
                            TicksAtCenter = true,
                            LabelsAlignment = LiveChartsCore.Drawing.Align.Start,
                            
                        }
                };

        }
    

        private void ExcuteChangePasswordCommand(object obj)
        {
            var screen = new ChangePassword();
            screen.ShowDialog();
        }

        private void ExcuteLogOutCommand(object obj)
        {
            CurrentUser.dashBoard.Close();
            var login = new LoginWindow();
            login.Show();
        }
        private DateTime getDate(DateTimeOffset offset)
        {
            return new DateTime(offset.Year,offset.Month,offset.Day);
        }
        private void Load()
        {
            //Welcome text
            var user = new RepoUser().GetUser(Thread.CurrentPrincipal.Identity.Name);
            var name = user.Name.Split(' ');
            WelcomeText = "Hello, " + name[name.Length-1];
            //Avtar
            Avatar = string.IsNullOrEmpty(CurrentUser.currentUser.ImageUrl) ? "Images/user.png" : CurrentUser.currentUser.ImageUrl;
            //Total Income
            var listTickets = new RepoTicket().GetTickets(1, context.Tickets.Count()).Items;
            listTickets.ForEach(ticket =>
            {
                var trip = new RepoTrip().GetTrip(ticket.TripId);
                var route = new RepoRoute().GetRoute(trip.RouteId);
                TotalIncome += route.Price;
            });
            //Total employees
            TotalEmployees = new RepoUser().GetUsers("",1,context.Users.Count()).Items.Where(u => u.Role == "Employee").Count();
            //Total Trips
            TotalTrips = context.Trips.Count();
            //Total tickets selled
            TotalTicketsSelled = context.Tickets.Count();
            //Today income
            var listTicketsToday = new RepoTicket().GetTickets(1, context.Tickets.Count()).Items.Where(c=> getDate(c.DateBought).CompareTo(CurrentUser.GetDateNow()) == 0).ToList();
            listTicketsToday.ForEach(ticket =>
            {
                var trip = new RepoTrip().GetTrip(ticket.TripId);
                var route = new RepoRoute().GetRoute(trip.RouteId);
                TodayIncome += route.Price;
            });
            //Check Percent 
            var percent = 0.0;
            if (TodayIncome == TotalIncome) percent = 1.0;
            else
                percent = (TotalIncome * 1.0 / (TotalIncome - TodayIncome)) - 1;
            PercentTodayIncome = "+" + (percent*100.0).ToString("0.##") + " %";
            //list date week
            DateTime startDate = DateTime.Today;
            startDate = startDate.AddDays(1 - (int)startDate.DayOfWeek);
            DateTime endDate = startDate.AddDays(6);
            while (startDate.Day <= endDate.Day)
            {
                var listPassengers = new RepoPassenger().GetPassengers("", 1, context.Passengers.Count()).Items.Where(p=> getDate(p.DateAdded).CompareTo(startDate) == 0).ToList();
                NewPassengersWeek += listPassengers.Count;
                startDate = startDate.AddDays(1);
            }
            //Check Percent 
            var percentPassengers = 0.0;
            var allPassengers = context.Passengers.Count();
            if (allPassengers == NewPassengersWeek) percentPassengers = 1.0;
            else
                percentPassengers = (allPassengers * 1.0 / (allPassengers - NewPassengersWeek)) - 1;
            PercentNewPassengersWeek = "+" + (percentPassengers * 100.0).ToString("0.##") + " %";
        }
        //Chart
       
    


    }
}
