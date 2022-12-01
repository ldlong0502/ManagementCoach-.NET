using ManagementCoach.BE;
using ManagementCoach.BE.Models;
using ManagementCoach.BE.Repositories;
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
    public class UserViewModel : ViewModelBase
    {

        
        public CoachManContext context = new CoachManContext();
        private List<ModelUser> userList;
        private string textSearch = "";
        private ICollectionView userCollection;
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

        public ICollectionView UserCollection
        {
            get
            {
                return userCollection;
            }
            set
            {
                userCollection = value;
                OnPropertyChanged(nameof(UserCollection));
            }
        }
        public List<ModelUser> UserList
        {
            get
            {
                return userList;
            }
            set
            {
                userList = value;
                OnPropertyChanged(nameof(UserList));
            }
        }

        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand PreviousPageCommand { get; }
        public ICommand NextPageCommand { get; }
        public ICommand UpLimitCommand { get; }
        public ICommand DownLimitCommand { get; }
        public ICommand FirstPageCommand { get; }
        public ICommand EndPageCommand { get; }

        public UserViewModel()
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

            new RepoUser().DeleteUser((obj as ModelUser).Id);
            Load();
        }

        private void ExcuteEditCommand(object obj)
        {
            //var screen = new Add((obj as ModelCoach));
            //screen.ShowDialog();
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
        private int CountAllUserFilter()
        {
            List<ModelUser> modelUsers = new List<ModelUser>();
            context.Users.ToList().ForEach(x => modelUsers.Add(new RepoUser().GetUser(x.Id)));
            ICollectionView collection = CollectionViewSource.GetDefaultView(modelUsers);
            collection.Filter = Filter;
            return collection.Cast<ModelUser>().Count();

        }
        private bool Filter(object data)
        {
            if (!string.IsNullOrEmpty(TextSearch))
            {
                var userDetail = data as ModelUser;
                return userDetail != null
                    && (check(userDetail.Name, TextSearch));
            }
            return true;

        }
        public void Load()
        {
            UserCollection = CollectionViewSource.GetDefaultView(context.Users.ToList());
            //var usersPagination = new RepoUser().(TextSearch, CurrentPage, Limit);
            //CoachCollection = CollectionViewSource.GetDefaultView(coachesPagination.Items);
            //NumOfPages = coachesPagination.PageCount;
            //if (CurrentPage > NumOfPages)
            //{
            //    CurrentPage = 1;
            //    coachesPagination = new RepoUser().GetCoaches(TextSearch, CurrentPage, Limit);
            //    CoachCollection = CollectionViewSource.GetDefaultView(coachesPagination.Items);
            //}
        }
    }
   
}
