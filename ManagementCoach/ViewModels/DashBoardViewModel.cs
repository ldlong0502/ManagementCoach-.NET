﻿using ManagementCoach.BE.Models;
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
        private DispatcherTimer dispatcherTimer = null;
        private ModelUser user = new RepoUser().GetUser(Thread.CurrentPrincipal.Identity.Name);
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
        public DispatcherTimer DispatcherTimerImage
        {
            get
            {
                return dispatcherTimer;
            }
            set
            {
                dispatcherTimer = value;
                OnPropertyChanged(nameof(DispatcherTimerImage));
            }
        }
        public DashBoardViewModel()
        {
            ShowHomeViewCommand = new ViewModelCommand(ExecuteShowHomeViewCommand);
            ShowManagerViewCommand = new ViewModelCommand(ExecuteShowManagerViewCommand);
            ShowAccountViewCommand = new ViewModelCommand(ExecuteShowAccountViewCommand);
            ExecuteShowHomeViewCommand(null);



        }

        private void ExecuteShowAccountViewCommand(object obj)
        {
            if (user.Role == "Employee")
            {
                MessageBox.Show( "You don't have permission", "Permission", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            DispatcherTimerImage.Stop();
            CurrentChildView = new UserViewModel();
            CurrentUserControl = new AccountUserControl();
            CurrentUserControl.DataContext = CurrentChildView;

        }

        private void ExecuteShowManagerViewCommand(object obj)
        {
            DispatcherTimerImage.Stop();
            CurrentChildView = new ManagerViewModel();
            CurrentUserControl = new ManagerUserControl();
            CurrentUserControl.DataContext = CurrentChildView;
        }

        private void ExecuteShowHomeViewCommand(object obj)
        {
            CurrentChildView = new AdminHomeViewModel();
            CurrentUserControl = new AdminHome();
            CurrentUserControl.DataContext = CurrentChildView;
            (CurrentChildView as AdminHomeViewModel).DisplayedImagePath = @"C:/Users/LENOVO/Downloads/ImageCoach/coach1.jpg";
            LoadImage();

        }

        public void LoadImage()
        {

            DispatcherTimerImage = new DispatcherTimer();
            DispatcherTimerImage.Tick += new EventHandler(dispatcherTimer_Tick);
            DispatcherTimerImage.Interval = new TimeSpan(0, 0, 1);
            DispatcherTimerImage.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            TimeSpan.FromSeconds(time).Duration();
            (CurrentChildView as AdminHomeViewModel).DisplayedImagePath = @"C:/Users/LENOVO/Downloads/ImageCoach/coach" + time + ".jpg";
            if (time == 4)
            {

                time = 1;
            }
            time++;
        }
    }
}
