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
    /// Interaction logic for AddNewStation.xaml
    /// </summary>
    public partial class AddNewStation : Window
    {
        public AddNewStation(StationViewModel stationViewModel)
        {
            InitializeComponent();
            var vm = new AddStationViewModel();
            this.DataContext = vm;
            if (vm.Close == null)
            {
                vm.Close = new Action(() => {
                    stationViewModel.Load();
                    this.Close();

                });
            }
        }
        public AddNewStation(StationViewModel stationViewModel, ModelStation modelStation)
        {
            InitializeComponent();
            var vm = new AddStationViewModel(modelStation);
            this.DataContext = vm;
            if (vm.Close == null)
            {
                vm.Close = new Action(() => {
                    stationViewModel.Load();
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
