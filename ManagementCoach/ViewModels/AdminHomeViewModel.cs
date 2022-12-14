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

        public ICommand Show { get;}
        public AdminHomeViewModel()
        {
            Load();
            Show = new ViewModelCommand(ExcuteShow);
            //LoadImage();
        }

        private void Load()
        {
            //Welcome text
            var user = new RepoUser().GetUser("admin");
            var name = user.Name.Split(' ');
            WelcomeText = "Hello, " + name[name.Length-1];
            
        }

        private void ExcuteShow(object obj)
        {
            this.ToString();
            
        }

        
        
    }
}
