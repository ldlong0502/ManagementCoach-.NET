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
using ManagementCoach.Views.UserControls;

namespace ManagementCoach.Views.Screens
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
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
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
    }
}
