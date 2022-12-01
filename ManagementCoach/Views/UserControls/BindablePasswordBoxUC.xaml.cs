using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
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
    /// Interaction logic for BindablePasswordBoxUC.xaml
    /// </summary>
    public partial class BindablePasswordBoxUC : UserControl
    {
       
        private static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(String), typeof(BindablePasswordBoxUC));

        public String Password
        {
            get { return (String)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }
        public BindablePasswordBoxUC()
        {
            InitializeComponent();
            pwdPassword.PasswordChanged += PwdPassword_PasswordChanged;
        }

        private void PwdPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            Password = pwdPassword.Password;
        }
    }
}
