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
    /// Interaction logic for AddNewDriver.xaml
    /// </summary>
    public partial class AddNewDriver : Window
    {
        public AddNewDriver(DriverViewModel driverViewModel)
        {
            InitializeComponent();
            var vm = new AddDriverViewModel();
            this.DataContext = vm;
            if (vm.Close == null)
            {
                vm.Close = new Action(() => {
                    driverViewModel.Load();
                    this.Close();

                });
            }
        }
        public AddNewDriver(DriverViewModel driverViewModel, ModelDriver modelDriver)
        {
            InitializeComponent();
            var vm = new AddDriverViewModel(modelDriver);
            this.DataContext = vm;
            if (vm.Close == null)
            {
                vm.Close = new Action(() => {
                    driverViewModel.Load();
                    this.Close();

                });
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
    }
}
