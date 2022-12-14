using ManagementCoach.BE.Models;
using ManagementCoach.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ManagementCoach.Views.Screens
{
    /// <summary>
    /// Interaction logic for AddNewTrip.xaml
    /// </summary>
    public partial class AddNewTrip : Window
    {
        public AddNewTrip(TripViewModel tripViewModel)
        {
            InitializeComponent();
            var vm = new AddTripViewModel();
            this.DataContext = vm;
            if (vm.Close == null)
            {
                vm.Close = new Action(() => {
                    tripViewModel.Load();
                    this.Close();

                });
            }
        }
        public AddNewTrip(TripViewModel tripViewModel, ModelTrip modelTrip)
        {
            InitializeComponent();
            var vm = new AddTripViewModel(modelTrip);
            this.DataContext = vm;
            if (vm.Close == null)
            {
                vm.Close = new Action(() => {
                    tripViewModel.Load();
                    this.Close();

                });
            }
        }
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
