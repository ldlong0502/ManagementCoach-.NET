using ManagementCoach.BE;
using ManagementCoach.BE.Entities;
using ManagementCoach.BE.Models;
using ManagementCoach.BE.Repositories;
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
    public class RouteViewModel : ViewModelBase
    {
        public CoachManContext context = new CoachManContext();
        private ICollectionView routeCollection;
        private int originStationId;
        private int destinationStationId;
        private object selectedItem;
        private int currentPage = 1;
        private int limit = 2;
        private int numOfPages;
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
        public int OriginStationId
        {
            get
            {
                return originStationId;
            }
            set
            {
                originStationId = value;
                OnPropertyChanged(nameof(OriginStationId));
            }
        }
        public int DestinationStationId
        {
            get
            {
                return destinationStationId;
            }
            set
            {
                destinationStationId = value;
                OnPropertyChanged(nameof(DestinationStationId));
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
        public ICollectionView RouteCollection
        {
            get
            {
                return routeCollection;
            }
            set
            {
                routeCollection = value;
                OnPropertyChanged(nameof(RouteCollection));
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
        public ICommand OpenProvinceCommand { get; }
        public RouteViewModel()
        {
            Load();
            EditCommand = new ViewModelCommand(ExcuteEditCommand);
            DeleteCommand = new ViewModelCommand(ExcuteDeleteCommand);
            NextPageCommand = new ViewModelCommand(ExcuteNextPageCommand, CanExcuteNextPageCommand);
            PreviousPageCommand = new ViewModelCommand(ExcutePreviousPageCommand, CanExcutePreviousPageCommand);
            UpLimitCommand = new ViewModelCommand(ExcuteUpLimitCommand, CanExcuteUpLimitCommand);
            DownLimitCommand = new ViewModelCommand(ExcuteDownLimitCommand, CanExcuteDownLimitCommand);
            FirstPageCommand = new ViewModelCommand(ExcuteFirstPageCommand, CanExcuteFirstPageCommand);
            EndPageCommand = new ViewModelCommand(ExcuteEndPageCommand, CanExcuteEndPageCommand);
            OpenProvinceCommand = new ViewModelCommand(ExcuteOpenProvinceCommand);
        }

        private void ExcuteOpenProvinceCommand(object obj)
        {
            //var screen = new ProvinceScreen(this);
            //screen.ShowDialog();
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
            var delAction = new RepoRoute().DeleteRoute((obj as ModelRoute).Id);
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
            //var objRoute = new RepoRoute().GetRoute((obj as MergeRouteAndProvinces).Id);
            //var screen = new AddNewRoute(this, objRoute);
            //screen.ShowDialog();
        }



        public void Load()
        {
            if (context.Routes.Count() == 0)
            {
                return;
            }
            var routesPagination = new RepoRoute().GetRoutesFromStation(2,1,CurrentPage, Limit);
           

            RouteCollection = CollectionViewSource.GetDefaultView(routesPagination.Items);
            NumOfPages = routesPagination.PageCount;

            if (NumOfPages != 0 && CurrentPage > NumOfPages)
            {
                CurrentPage = 1;
                routesPagination = new RepoRoute().GetRoutes(CurrentPage, Limit);
                RouteCollection = CollectionViewSource.GetDefaultView(routesPagination.Items);
            }

        }

    }
}
