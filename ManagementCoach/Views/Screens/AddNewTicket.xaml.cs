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
    /// Interaction logic for AddNewTicket.xaml
    /// </summary>
    public partial class AddNewTicket : Window
    {
        public AddNewTicket(TicketViewModel TicketViewModel)
        {
            InitializeComponent();
            var vm = new AddTicketViewModel();
            this.DataContext = vm;
            if (vm.Close == null)
            {
                vm.Close = new Action(() => {
                    TicketViewModel.Load();
                    this.Close();

                });
            }
        }
        public AddNewTicket(TicketViewModel TicketViewModel, ModelTicket modelTicket)
        {
            InitializeComponent();
            var vm = new AddTicketViewModel(modelTicket);
            this.DataContext = vm;
            if (vm.Close == null)
            {
                vm.Close = new Action(() => {
                    TicketViewModel.Load();
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
