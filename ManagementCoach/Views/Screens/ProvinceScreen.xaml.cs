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
    /// Interaction logic for ProvinceScreen.xaml
    /// </summary>
    public partial class ProvinceScreen : Window
    {
        public ProvinceScreen(StationViewModel stationViewModel)
        {
            InitializeComponent();
            var vm = new ProvinceViewModel();
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
            WindowState = WindowState.Minimized;
        }
    }
}
