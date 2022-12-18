using ManagementCoach.BE;
using ManagementCoach.BE.Entities;
using ManagementCoach.BE.Models;
using ManagementCoach.BE.Repositories;
using ManagementCoach.Views;
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
    public class TripViewModel : ViewModelBase, ILoadableViewModel
	{
   
        public CoachManContext context = new CoachManContext();
        private ICollectionView tripCollection;
        private object selectedItem;
        private int currentPage = 1;
        private int limit = 2;
        private int numOfPages;
        private string filterTrip;
        private List<string> listFilterTrip;
        private string textSearch = "";
        public string TextSearch
        {
            get
            {
                return textSearch;
            }
            set
            {

                if (FilterTrip == "None")
                    return;
                textSearch = value;
                OnPropertyChanged(nameof(TextSearch));
                Load();
            }
        }
        public object SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }
       

        public int CurrentPage
        {
            get
            {
                return currentPage;
            }
            set
            {
                currentPage = value;
                OnPropertyChanged(nameof(CurrentPage));
                Load();
            }
        }
        public int NumOfPages
        {
            get
            {
                return numOfPages;
            }
            set
            {
                numOfPages = value;
                OnPropertyChanged(nameof(NumOfPages));
            }
        }
        public int Limit
        {
            get
            {
                return limit;
            }
            set
            {
                limit = value;
                OnPropertyChanged(nameof(Limit));
                Load();
            }
        }
        public ICollectionView TripCollection
        {
            get
            {
                return tripCollection;
            }
            set
            {
                tripCollection = value;
                OnPropertyChanged(nameof(TripCollection));
            }
        }
        public string FilterTrip
        {
            get
            {
                return filterTrip;
            }
            set
            {
                filterTrip = value;
                OnPropertyChanged(nameof(FilterTrip));
            }
        }
        public List<string> ListFilterTrip
        {
            get
            {
                return listFilterTrip;
            }
            set
            {
                listFilterTrip = value;
                OnPropertyChanged(nameof(ListFilterTrip));
            }
        }


        //ICommand
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand PreviousPageCommand { get; }
        public ICommand NextPageCommand { get; }
        public ICommand UpLimitCommand { get; }
        public ICommand DownLimitCommand { get; }
        public ICommand FirstPageCommand { get; }
        public ICommand EndPageCommand { get; }
        public TripViewModel()
        {
            ListFilterTrip = new List<string>() { "None", "By Driver Id", "By Coach Id", "By Route Id" };
            FilterTrip = ListFilterTrip.First();
            Load();
            EditCommand = new ViewModelCommand(ExcuteEditCommand);
            DeleteCommand = new ViewModelCommand(ExcuteDeleteCommand);
            NextPageCommand = new ViewModelCommand(ExcuteNextPageCommand, CanExcuteNextPageCommand);
            PreviousPageCommand = new ViewModelCommand(ExcutePreviousPageCommand, CanExcutePreviousPageCommand);
            UpLimitCommand = new ViewModelCommand(ExcuteUpLimitCommand, CanExcuteUpLimitCommand);
            DownLimitCommand = new ViewModelCommand(ExcuteDownLimitCommand, CanExcuteDownLimitCommand);
            FirstPageCommand = new ViewModelCommand(ExcuteFirstPageCommand, CanExcuteFirstPageCommand);
            EndPageCommand = new ViewModelCommand(ExcuteEndPageCommand, CanExcuteEndPageCommand);
        }

        

        private bool CanExcuteEndPageCommand(object obj)
        {
            if (CurrentPage != NumOfPages)
                return true;
            return false;
        }

        private void ExcuteEndPageCommand(object obj)
        {
            CurrentPage = NumOfPages;
        }

        private bool CanExcuteFirstPageCommand(object obj)
        {
            if (CurrentPage != 1)
                return true;
            return false;
        }

        private void ExcuteFirstPageCommand(object obj)
        {
            CurrentPage = 1;
        }

        private bool CanExcuteDownLimitCommand(object obj)
        {
            if (Limit > 1)
                return true;
            return false;
        }

        private void ExcuteDownLimitCommand(object obj)
        {
            Limit--;
        }

        private bool CanExcuteUpLimitCommand(object obj)
        {
            if (Limit < 20)
                return true;
            return false;
        }

        private void ExcuteUpLimitCommand(object obj)
        {
            Limit++;
        }

        private bool CanExcuteNextPageCommand(object obj)
        {
            if (CurrentPage < NumOfPages)
                return true;
            return false;
        }

        private void ExcuteNextPageCommand(object obj)
        {
            CurrentPage++;
        }

        private bool CanExcutePreviousPageCommand(object obj)
        {
            if (CurrentPage > 1)
                return true;
            return false;
        }

        private void ExcutePreviousPageCommand(object obj)
        {
            CurrentPage--;
        }
        private void ExcuteDeleteCommand(object obj)
        {
            DialogResult ret = System.Windows.Forms.MessageBox.Show("Do you want to delete this row?", "Delete row", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (ret == DialogResult.Cancel || ret == DialogResult.No)
            {
                return;
            }
            var delAction = new RepoTrip().DeleteTrip((obj as ModelTrip).Id);
            if (delAction.Success == true)
            {
                MessageBox.Show("Successfully");
                Load();
            }
            else
            {
                MessageBox.Show(delAction.ErrorMessage);
            }

        }

        private void ExcuteEditCommand(object obj)
        {
            var objTrip = new RepoTrip().GetTrip((obj as ModelTrip).Id);
            var screen = new AddNewTrip(this, objTrip);
            screen.ShowDialog();
        }



        public void Load()
        {
            if (context.Trips.Count() == 0)
            {
                return;
            }
            var  tripsPagination = new RepoTrip().GetTrips(CurrentPage, Limit);
            if(FilterTrip == "By Driver Id" && int.TryParse(TextSearch, out _))
            {
                tripsPagination = new RepoTrip().GetTripsByDriver(int.Parse(TextSearch),CurrentPage, Limit);
            }
            else if (FilterTrip == "By Coach Id" && int.TryParse(TextSearch, out _))
            {
                tripsPagination = new RepoTrip().GetTripsByCoach(int.Parse(TextSearch), CurrentPage, Limit);
            }
            else if (FilterTrip == "By Route Id" && int.TryParse(TextSearch, out _))
            {
                tripsPagination = new RepoTrip().GetTripsByRoute(int.Parse(TextSearch), CurrentPage, Limit);
            }
            TripCollection = CollectionViewSource.GetDefaultView(tripsPagination.Items);
            NumOfPages = tripsPagination.PageCount;

            if (NumOfPages != 0 && CurrentPage > NumOfPages)
            {
                CurrentPage = 1;
                tripsPagination = new RepoTrip().GetTrips(CurrentPage, Limit);
                if (FilterTrip == "By Driver Id" && int.TryParse(TextSearch, out _))
                {
                    tripsPagination = new RepoTrip().GetTripsByDriver(int.Parse(TextSearch), CurrentPage, Limit);
                }
                else if (FilterTrip == "By Coach Id" && int.TryParse(TextSearch, out _))
                {
                    tripsPagination = new RepoTrip().GetTripsByCoach(int.Parse(TextSearch), CurrentPage, Limit);
                }
                else if (FilterTrip == "By Route Id" && int.TryParse(TextSearch, out _))
                {
                    tripsPagination = new RepoTrip().GetTripsByRoute(int.Parse(TextSearch), CurrentPage, Limit);
                }
                TripCollection = CollectionViewSource.GetDefaultView(tripsPagination.Items);
            }
        }

    }
}
