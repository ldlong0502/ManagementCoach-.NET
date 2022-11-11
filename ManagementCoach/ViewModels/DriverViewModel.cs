using ManagementCoach.BE;
using ManagementCoach.BE.Entities;
using ManagementCoach.Views.Screens;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;

namespace ManagementCoach.ViewModels
{
    public class DriverViewModel : ViewModelBase
    {
        public CoachManContext context = new CoachManContext();
        private ICollectionView driverCollection;
        private string textSearch;
        public string TextSearch
        {
            get
            {
                return textSearch;
            }
            set
            {
                textSearch = value;
                OnPropertyChanged(nameof(TextSearch));
                DriverCollection.Filter = Filter;
            }
        }

        private bool check(string data, string text)
        {
            if (!string.IsNullOrEmpty(data))
            {
                if (data.Contains(text))
                    return true;
                return false;
            }
            return false;

        }
        private bool Filter(object data)
        {
            if (!string.IsNullOrEmpty(TextSearch))
            {
                var driverDetail = data as Driver;
                return driverDetail != null
                    && (check(driverDetail.Id.ToString(), TextSearch)
                    || check(driverDetail.Name, TextSearch));
            }
            return true;

        }
        public ICollectionView DriverCollection
        {
            get
            {
                return driverCollection;
            }
            set
            {
                driverCollection = value;
                OnPropertyChanged(nameof(DriverCollection));
            }
        }
       

        //ICommand
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public DriverViewModel()
        {
            Load();
            EditCommand = new ViewModelCommand(ExcuteEditCommand);
            DeleteCommand = new ViewModelCommand(ExcuteDeleteCommand);
        }
        private void ExcuteDeleteCommand(object obj)
        {
            DialogResult ret = System.Windows.Forms.MessageBox.Show("Do you want to delete this row?", "Delete row", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (ret == DialogResult.Cancel)
            {
                return;
            }
        }

        private void ExcuteEditCommand(object obj)
        {
            var screen = new AddNewDriver(/*(obj as Driver)*/);
            screen.ShowDialog();

        }
        public void Load()
        {
            DriverCollection = CollectionViewSource.GetDefaultView(context.Drivers.ToList());
        }
    }
}
