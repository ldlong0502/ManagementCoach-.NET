using ManagementCoach.BE;
using ManagementCoach.BE.Models;
using ManagementCoach.BE.Repositories;
using ManagementCoach.Views.Screens;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace ManagementCoach.ViewModels
{
    public class StaffHomeViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        private CoachManContext context = new CoachManContext();
        private string welcomeText;
        private string displayedImagePath = "C:/Users/LENOVO/Downloads/ImageCoach/coach1.jpg";
        private string avatar;
        private ModelProvince departure;
        private ModelProvince destination;
        private List<ModelProvince> listProvinces;
        private List<RoutesShow> listRoutes;
        private DateTime date;

        private readonly ErrorsViewModel _errorsViewModel;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        public bool CanCreate => !HasErrors;
        public bool HasErrors => _errorsViewModel.HasErrors;

        public ModelProvince Departure
        {
            get
            {
                return departure;
            }
            set
            {
                departure = value;
                OnPropertyChanged(nameof(Departure));
            }
        }
        public ModelProvince Destination
        {
            get
            {
                return destination;
            }
            set
            {
                destination = value;
                OnPropertyChanged(nameof(Destination));
            }
        }
        public List<RoutesShow> ListRoutes
        {
            get
            {
                return listRoutes;
            }
            set
            {
                listRoutes = value;
                OnPropertyChanged(nameof(ListRoutes));
            }
        }
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
        public DateTime Date
        {
            get
            {
                return date;
            }
            set
            {
                date = value;
                _errorsViewModel.ClearErrors(nameof(Date));
                if (date.CompareTo(CurrentUser.GetDateNow()) < 0)
                {
                    _errorsViewModel.AddError(nameof(Date), "Can't filter the day in past.");
                    return;
                }
                OnPropertyChanged(nameof(Date));
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
        public Action Close { get; set; }
        public ICommand SwapCommand { get; }
        public ICommand ReserveCommand { get;  } 
        public ICommand SearchRouteCommand { get; }
        public ICommand LogOutCommand { get; }
        public ICommand ChangePasswordCommand { get; }
        public StaffHomeViewModel()
        {
            _errorsViewModel = new ErrorsViewModel();
            _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;
            Date = DateTime.Now;

            ListProvinces = new RepoProvince().GetProvinces("");
            Load();
            SwapCommand = new ViewModelCommand(ExcuteSwapCommand);
            ReserveCommand = new ViewModelCommand(ExcuteReserveCommand, CanExcuteReserveCommand);
            SearchRouteCommand = new ViewModelCommand(ExcuteSearchRouteCommand, CanExcuteSearchRouteCommand);
            LogOutCommand = new ViewModelCommand(ExcuteLogOutCommand);
            ChangePasswordCommand = new ViewModelCommand(ExcuteChangePasswordCommand);
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

        private bool CanExcuteSearchRouteCommand(object obj)
        {
            if(Departure == null || Destination == null || Departure == Destination || _errorsViewModel.HasErrors) { return false; }
            return true;
        }

        private void ExcuteSearchRouteCommand(object obj)
        {
            //tìm route theo điểm đến, đi nếu k có => return
            var originStation = new RepoStation().GetStations("", 1, context.Stations.Count()).Items.Find(st => st.ProvinceId == Departure.Id);
            var destinationStation = new RepoStation().GetStations("", 1, context.Stations.Count()).Items.Find(st => st.ProvinceId == Destination.Id);
            if(originStation == null || destinationStation == null) 
            {
                MessageBox.Show("Don't have this route", "Routes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            var listRoutes = new RepoRoute().GetRoutesFromStation(originStation.Id,destinationStation.Id,1,1).Items;
            if(listRoutes.Count() == 0)
            {
                MessageBox.Show("Your route is empty", "Routes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //tìm trip theo ngày đi
            var route = listRoutes[0];
            var screen = new TripShows();
            var dataContext = new TripShowsViewModel(new RepoRoute().GetRoute(route.Id), Date);
            screen.DataContext = dataContext;
            screen.ShowDialog();

        }

        private bool CanExcuteReserveCommand(object obj)
        {
            int id = (int)obj;
            var route = new RepoRoute().GetRoute(id);
            var listTrip = new RepoTrip().GetTripsByRoute(route.Id).Items.Where(trip => trip.Date.CompareTo( DateTime.Today) >= 0).ToList();
            if (listTrip.Count == 0)
                return false;
            return true;
        }

        private void ExcuteReserveCommand(object obj)
        {
            int id = (int)obj;
            
            var screen = new TripShows();
            var context = new TripShowsViewModel(new RepoRoute().GetRoute(id), CurrentUser.GetDateNow());
            screen.DataContext = context;
            screen.ShowDialog();
        }

        private void ExcuteSwapCommand(object obj)
        {
            if (Destination == null || Departure == null) return;
            var temp = Departure;
            Departure = Destination;
            Destination = temp;
        }

        private void Load()
        {
            //Welcome text

            var user = new RepoUser().GetUser(Thread.CurrentPrincipal.Identity.Name);
            if(user != null)
            {
                var name = user.Name.Split(' ');
                WelcomeText = "Hello, " + name[name.Length - 1];
            }
            
            Avatar = string.IsNullOrEmpty(CurrentUser.currentUser.ImageUrl) ? "/Images/avatar.png" : CurrentUser.currentUser.ImageUrl;
            //Get 4 routes popular 
            ListRoutes = new List<RoutesShow>();
            var list = new RepoRoute().GetRoutes().Items;
            foreach (var item in list)
            {

                string title = new RepoProvince().GetProvince(new RepoStation().GetStation(item.OriginStationId).ProvinceId).Name + " -> " + new RepoProvince().GetProvince(new RepoStation().GetStation(item.DestinationStationId).ProvinceId).Name;
                string price = item.Price.ToString();
                string imageUrl = string.IsNullOrEmpty(item.ImageUrl) ? "/Images/coach.jpg" : item.ImageUrl;
                var listTrip = new RepoTrip().GetTripsByRoute(item.Id).Items.Where(trip => trip.Date == DateTime.Today).ToList();
                listTrip.Sort((a, b) => a.DepartTime.CompareTo(b.DepartTime));
                List<int> listTime = new List<int>();
                listTrip.ForEach(trip => {
                    if(trip.DepartTime > GetTimeNow() + 60)
                    {
                        if (listTime.Count != 3)
                        {
                            listTime.Add(trip.DepartTime);
                        }
                    }
                });
                var count  = listTime.Count;
                for(int i = 3 - count; i > 0; i--)
                {
                    listTime.Add(0);
                }
                ListRoutes.Add(new RoutesShow() { 
                    Id = item.Id,
                    Title = title,
                    Price = price,
                    ImageUrl = imageUrl,
                    ListTime = listTime
                });
            }

        }
        private int GetTimeNow()
        {
            return DateTime.Now.Hour * 60 + DateTime.Now.Minute;
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

    public class RoutesShow
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Price { get; set; }
        public string ImageUrl { get; set; }
        public List<int> ListTime { get; set; }
    }
}
