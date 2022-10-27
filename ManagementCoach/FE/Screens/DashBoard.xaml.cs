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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using FontAwesome.WPF;
using ManagementCoach.FE.UserControls;

namespace ManagementCoach.FE.Screens
{
    /// <summary>
    /// Interaction logic for DashBoard.xaml
    /// </summary>
    public partial class DashBoard : Window
    {
        bool isSlideClose = false;
        public DashBoard()
        {
            InitializeComponent();
            
        }
        void MoveSlide(object sender)
        {
            Grid grid = sender as Grid;
            slide.Margin = new Thickness(0.5, grid.Margin.Top, 0, 0);
        }
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Home_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MoveSlide(sender);
            frDashBoard.Content = new AdminHome();
        }

        private void Bus_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MoveSlide(sender);
            frDashBoard.Content = new ManagerUserControl();
        }

        private void Account_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MoveSlide(sender);
        }

        private void Info_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MoveSlide(sender);
        }

        private void Chart_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MoveSlide(sender);
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
            if (!isSlideClose)
            {
                Storyboard myStoryboard = FindResource("SlideClose") as Storyboard;
                myStoryboard.Begin();
            }
            else
            {
                Storyboard myStoryboard = FindResource("SlideOpen") as Storyboard;
                myStoryboard.Begin();
            }
            isSlideClose = !isSlideClose;

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            frDashBoard.Content = new AdminHome();
        }

    }
}
