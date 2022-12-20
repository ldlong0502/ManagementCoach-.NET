using ManagementCoach.BE.Models;
using ManagementCoach.BE.Repositories;
using ManagementCoach.Views.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace ManagementCoach.ViewModels
{
    public class DashBoardViewModel :ViewModelBase
    {
        private ViewModelBase _currentChildView;
        private UserControl currentUserControl;
        private DispatcherTimer dispatcherTimerAdmin = null;
        private DispatcherTimer dispatcherTimerStaff = null;
        private int time = 1;
        public int Time
        {
            get
            {
                return time;
            }
            set
            {
                time = value;
                OnPropertyChanged(nameof(Time));
            }
        }
        public ViewModelBase CurrentChildView { 
            get 
            {
                return _currentChildView; 
            }
            set
            {
                _currentChildView = value;
                OnPropertyChanged(nameof(CurrentChildView));
            }
        }
        public UserControl CurrentUserControl { 
            get
            {
                return currentUserControl;
            }
            set
            {
                currentUserControl = value;
                OnPropertyChanged(nameof(CurrentUserControl));
            }
        }
        //ICommand
        public ICommand ShowHomeViewCommand { get; }
        public ICommand ShowManagerViewCommand { get; }
        public ICommand ShowAccountViewCommand { get; }
        public DispatcherTimer DispatcherTimerAdmin
        {
            get
            {
                return dispatcherTimerAdmin;
            }
            set
            {
                dispatcherTimerAdmin = value;
                OnPropertyChanged(nameof(DispatcherTimerAdmin));
            }
        }
        public DispatcherTimer DispatcherTimerStaff
        {
            get
            {
                return dispatcherTimerStaff;
            }
            set
            {
                dispatcherTimerStaff = value;
                OnPropertyChanged(nameof(DispatcherTimerStaff));
            }
        }
        public DashBoardViewModel()
        {
            ShowHomeViewCommand = new ViewModelCommand(ExecuteShowHomeViewCommand);
            ShowManagerViewCommand = new ViewModelCommand(ExecuteShowManagerViewCommand);
            ShowAccountViewCommand = new ViewModelCommand(ExecuteShowAccountViewCommand, CanExecuteShowAccountViewCommand);
            ExecuteShowHomeViewCommand(null);



        }

        private bool CanExecuteShowAccountViewCommand(object obj)
        {
            if (CurrentUser.currentUser.Role == "Admin")
            {
                return true;
            }
            return false;
        }

        private void ExecuteShowAccountViewCommand(object obj)
        {
            ResetTime();
            CurrentChildView = new UserViewModel();
            CurrentUserControl = new AccountUserControl();
            CurrentUserControl.DataContext = CurrentChildView;

        }

        private void ExecuteShowManagerViewCommand(object obj)
        {
            ResetTime();
            CurrentChildView = new ManagerViewModel();
            CurrentUserControl = new ManagerUserControl();
            CurrentUserControl.DataContext = CurrentChildView;
        }

        private void ExecuteShowHomeViewCommand(object obj)
        {
            if(CurrentUser.currentUser.Role == "Admin")
            {
                CurrentChildView = new AdminHomeViewModel();
                CurrentUserControl = new AdminHome();
                CurrentUserControl.DataContext = CurrentChildView;
                (CurrentChildView as AdminHomeViewModel).DisplayedImagePath = @"C:/Users/LENOVO/Downloads/ImageCoach/coach1.jpg";
                LoadImageAdmin();
            }
            else
            {
                CurrentChildView = new StaffHomeViewModel();
                CurrentUserControl = new StaffHome();
                CurrentUserControl.DataContext = CurrentChildView;
                (CurrentChildView as StaffHomeViewModel).DisplayedImagePath = @"C:/Users/LENOVO/Downloads/ImageCoach/coach1.jpg";
                LoadImageSatff();
            }
           

        }

        public void LoadImageAdmin()
        {

            DispatcherTimerAdmin = new DispatcherTimer();
            DispatcherTimerAdmin.Tick += new EventHandler(dispatcherTimerAdmin_Tick);
            DispatcherTimerAdmin.Interval = new TimeSpan(0, 0, 1);
            DispatcherTimerAdmin.Start();
        }
        public void LoadImageSatff()
        {

            DispatcherTimerStaff = new DispatcherTimer();
            DispatcherTimerStaff.Tick += new EventHandler(dispatcherTimerStaff_Tick);
            DispatcherTimerStaff.Interval = new TimeSpan(0, 0, 1);
            DispatcherTimerStaff.Start();
        }

        private void dispatcherTimerAdmin_Tick(object sender, EventArgs e)
        {
            TimeSpan.FromSeconds(time).Duration();
            (CurrentChildView as AdminHomeViewModel).DisplayedImagePath = @"C:/Users/LENOVO/Downloads/ImageCoach/coach" + time + ".jpg";            
            if (time == 4)
            {

                time = 1;
            }
            time++;
        }
        private void dispatcherTimerStaff_Tick(object sender, EventArgs e)
        {
            TimeSpan.FromSeconds(time).Duration();
            (CurrentChildView as StaffHomeViewModel).DisplayedImagePath = @"C:/Users/LENOVO/Downloads/ImageCoach/coach" + time + ".jpg";
            if (time == 4)
            {

                time = 1;
            }
            time++;
        }

        private void ResetTime()
        {
            if(DispatcherTimerAdmin != null)
                DispatcherTimerAdmin.Stop();
            if (DispatcherTimerStaff != null)
                DispatcherTimerStaff.Stop();
            
            time = 0;
        }
    }
}
