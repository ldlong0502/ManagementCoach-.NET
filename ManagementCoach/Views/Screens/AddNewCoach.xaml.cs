using ManagementCoach.BE.Entities;
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
    /// Interaction logic for AddNewCoach.xaml
    /// </summary>
    public partial class AddNewCoach : Window
    {
        public AddNewCoach()
        {
            InitializeComponent();
            GetViewModel.addNewCoach = this;
        }
        public AddNewCoach(ModelCoach data)
        {
            InitializeComponent();
            GetViewModel.addNewCoach = this;
            this.DataContext = new AddCoachViewModel(data);
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
            
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        
    }
}
