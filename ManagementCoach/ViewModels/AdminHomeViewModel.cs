using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace ManagementCoach.ViewModels
{
    public class AdminHomeViewModel : ViewModelBase
    {
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

        public ICommand Show { get;}
        public AdminHomeViewModel()
        {
            Show = new ViewModelCommand(ExcuteShow);
            //LoadImage();
        }

        private void ExcuteShow(object obj)
        {
            this.ToString();
        }

        
        
    }
}
