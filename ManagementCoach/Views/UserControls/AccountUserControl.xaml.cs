using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
    /// Interaction logic for AccountUserControl.xaml
    /// </summary>
    public partial class AccountUserControl : UserControl
    {

        public AccountUserControl()
        {
            InitializeComponent();

        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    //get info Account
            //    Account accountdataRowView = (Account)((Button)e.Source).DataContext;
            //    MessageBoxResult result =  MessageBox.Show("Do you want to remove this row?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Question);
            //    if(result == MessageBoxResult.Yes)
            //    {
            //        listAccounts.Remove(accountdataRowView);
            //    }
                
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message.ToString());
            //}

        }

        private void Edit_CLick(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    //get info Account
            //    Account dataRowView = (Account)((Button)e.Source).DataContext;

            //    String ID = dataRowView.ID;
            //    MessageBox.Show("You Clicked : " + ID);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message.ToString());
            //}
        }
    }
    public class Account
    {
        public string ID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string DateCreated { get; set; }
    }
}