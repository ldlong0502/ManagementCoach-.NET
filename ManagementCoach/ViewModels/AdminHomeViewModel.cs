using ManagementCoach.BE.Repositories;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace ManagementCoach.ViewModels
{
    public class AdminHomeViewModel : ViewModelBase
    {
        private string welcomeText;
        private string displayedImagePath;
        private string avatar;
        public string Avatar
        {
            get
            {
                return avatar;
            }
            set
            {
                avatar = value;
                OnPropertyChanged(nameof(Avatar));
            }
        }
        public string DisplayedImagePath
        {
            get
            {
                return displayedImagePath;
            }
            set
            {
                displayedImagePath = value;
                OnPropertyChanged(nameof(DisplayedImagePath));
            }
        }
        public string WelcomeText
        {
            get
            {
                return welcomeText;
            }
            set
            {
                welcomeText = value;
                OnPropertyChanged(nameof(WelcomeText));
            }
        }

        public AdminHomeViewModel()
        {
            Load();
        }

        private void Load()
        {
            //Welcome text
            var user = new RepoUser().GetUser(Thread.CurrentPrincipal.Identity.Name);
            var name = user.Name.Split(' ');
            WelcomeText = "Hello, " + name[name.Length-1];
            Avatar = string.IsNullOrEmpty(CurrentUser.currentUser.ImageUrl) ? "Images/user.png" : CurrentUser.currentUser.ImageUrl;


        }


        
        
    }
}
