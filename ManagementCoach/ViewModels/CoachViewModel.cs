﻿using ManagementCoach.BE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManagementCoach.BE.Repositories;
using ManagementCoach.BE;
using System.Windows.Input;
using System.Windows;
using ManagementCoach.Views.Screens;
using System.Windows.Forms;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Media;
using ManagementCoach.BE.Models;
using System.Collections;
using Aspose.Cells;
using ManagementCoach.Views;

namespace ManagementCoach.ViewModels
{
    public class CoachViewModel : ViewModelBase, ILoadableViewModel
	{
        public CoachManContext context = new CoachManContext();
        private string textSearch = "";
        private ICollectionView coachCollection;
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

        public ICollectionView CoachCollection
        {
            get
            {
                return coachCollection;
            }
            set
            {
                coachCollection = value;
                OnPropertyChanged(nameof(CoachCollection));
            }
        }

        public ICommand OpenCoachSeatsCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand PreviousPageCommand { get; }
        public ICommand NextPageCommand { get; }
        public ICommand UpLimitCommand { get; }
        public ICommand DownLimitCommand { get; }
        public ICommand FirstPageCommand { get; }
        public ICommand EndPageCommand { get; }

        public CoachViewModel()
        {
            
            Load();
            OpenCoachSeatsCommand = new ViewModelCommand(ExcuteOpenCoachSeatsCommand, CanExcuteOpenCoachSeatsCommand);
            EditCommand = new ViewModelCommand(ExcuteEditCommand);
            DeleteCommand = new ViewModelCommand(ExcuteDeleteCommand, CanExcuteDeleteCommand);
            NextPageCommand = new ViewModelCommand(ExcuteNextPageCommand, CanExcuteNextPageCommand);
            PreviousPageCommand = new ViewModelCommand(ExcutePreviousPageCommand, CanExcutePreviousPageCommand);
            UpLimitCommand = new ViewModelCommand(ExcuteUpLimitCommand, CanExcuteUpLimitCommand);
            DownLimitCommand = new ViewModelCommand(ExcuteDownLimitCommand, CanExcuteDownLimitCommand);
            FirstPageCommand = new ViewModelCommand(ExcuteFirstPageCommand, CanExcuteFirstPageCommand);
            EndPageCommand = new ViewModelCommand(ExcuteEndPageCommand, CanExcuteEndPageCommand);
        }
        private bool CanExcuteDeleteCommand(object obj)
        {
            if (CurrentUser.currentUser.Role == "Admin")
                return true;
            return false;
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
            if (Limit > 1 )
                return true;
            return false;
        }

        private void ExcuteDownLimitCommand(object obj)
        {
            Limit--;
        }

        private bool CanExcuteUpLimitCommand(object obj)
        {
            if (Limit < 20 )
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

			var delAction = new RepoCoach().DeleteCoach((obj as ModelCoach).Id);
            if (delAction.Success == true)
            {
                System.Windows.MessageBox.Show("Successfully");
                Load();
            }
            else
            {
                System.Windows.MessageBox.Show(delAction.ErrorMessage);
            }
		}

		private void ExcuteEditCommand(object obj)
        {
            var screen = new AddNewCoach((obj as ModelCoach) , this);
            screen.ShowDialog();
        }

        private bool CanExcuteOpenCoachSeatsCommand(object obj)
        {
            if (SelectedItem == null || (SelectedItem as ModelCoach).Status == "Out Of Service")
                return false;
            return true;
        }

        private void ExcuteOpenCoachSeatsCommand(object obj)
        {
            if (obj == null)
                return;
            var x = new CoachSeatWindow();
            var context = new CoachSeatViewModel((obj as ModelCoach));
            x.DataContext = context;
            x.ShowDialog();
        }     
        public void Load()
        {
            if(context.Coaches.Count() == 0)
            {
                return;
            }
			var coachesPagination = new RepoCoach().GetCoaches(TextSearch, CurrentPage, Limit);
			CoachCollection = CollectionViewSource.GetDefaultView(coachesPagination.Items);
            NumOfPages = coachesPagination.PageCount;

            if (NumOfPages != 0 && CurrentPage > NumOfPages)
            {
                CurrentPage = 1;
                coachesPagination = new RepoCoach().GetCoaches(TextSearch, CurrentPage, Limit);
                CoachCollection = CollectionViewSource.GetDefaultView(coachesPagination.Items);
            }
        }
    }
}
