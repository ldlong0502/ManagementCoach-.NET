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
    public class StationViewModel : ViewModelBase, ILoadableViewModel
	{
        public CoachManContext context = new CoachManContext();
        private ICollectionView stationCollection;
        private string textSearch = "";
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
                Load();
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
        public ICollectionView StationCollection
        {
            get
            {
                return stationCollection;
            }
            set
            {
                stationCollection = value;
                OnPropertyChanged(nameof(StationCollection));
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
        public StationViewModel()
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
            var screen = new ProvinceScreen(this);
            screen.ShowDialog();
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
            var delAction = new RepoStation().DeleteStation((obj as ModelStation).Id);
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
            var objStation = new RepoStation().GetStation((obj as MergeStationAndProvinces).Id);
            var screen = new AddNewStation(this, objStation);
            screen.ShowDialog();
        }



        public void Load()
        {
            if (context.Stations.Count() == 0)
            {
                return;
            }
            var stationsPagination = new RepoStation().GetStations(TextSearch, CurrentPage, Limit);
            var listProvinces = new RepoProvince().GetProvinces("");
            var mergeList = from c in stationsPagination.Items
                            join lp in listProvinces
                            on c.ProvinceId equals lp.Id
                            select new MergeStationAndProvinces
                            {
                                Id = c.Id,
                                Name = c.Name,
                                District = c.District,
                                Address = c.Address,
                                NameProvince = lp.Name,
                            };
                                  
            StationCollection = CollectionViewSource.GetDefaultView(mergeList.ToList());
            NumOfPages = stationsPagination.PageCount;

            if (NumOfPages != 0 && CurrentPage > NumOfPages)
            {
                CurrentPage = 1;
                stationsPagination = new RepoStation().GetStations(TextSearch, CurrentPage, Limit);
                listProvinces = new RepoProvince().GetProvinces("");
                mergeList = from c in stationsPagination.Items
                                join lp in listProvinces
                                on c.Id equals lp.Id
                                select new MergeStationAndProvinces
                                {
                                    Id = c.Id,
                                    Name = c.Name,
                                    District = c.District,
                                    Address = c.Address,
                                    NameProvince = lp.Name,
                                };

                StationCollection = CollectionViewSource.GetDefaultView(mergeList.ToList());
            }

        }
       
    }
    public class MergeStationAndProvinces
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string District { get; set; }
        public string Address { get; set; }
        public string NameProvince { get; set; }
    }
}
