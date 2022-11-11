using ManagementCoach.BE.Entities;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ManagementCoach.Views.UserControls
{
    /// <summary>
    /// Interaction logic for CoachUserControl.xaml
    /// </summary>
    public partial class CoachUserControl : UserControl
    {
        public CoachUserControl()
        {
            InitializeComponent();
        }

        //private void Edit_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        //get info Account
        //        Coach dataRowView = (Coach)((Button)e.Source).DataContext;

        //        GetViewModel.coachViewModel.Edit(dataRowView);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message.ToString());
        //    }
        //}

        //private void Delete_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        //get info Account
        //        Coach dataRowView = (Coach)((Button)e.Source).DataContext;

        //        GetViewModel.coachViewModel.Delete(dataRowView);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message.ToString());
        //    }
        //}
    }
}
