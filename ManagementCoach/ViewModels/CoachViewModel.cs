using ManagementCoach.BE.Entities;
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

namespace ManagementCoach.ViewModels
{
    public class CoachViewModel : ViewModelBase
    {
        public CoachManContext context = new CoachManContext();
        private List<Coach> coachList;
        private string textSearch;
        private ICollectionView coachCollection;
        private object selectedItem;

		private int currentPage = 1;
		private int limit = 20;
       
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
				currentPage = 1;
				var pagination = new RepoCoach().GetCoaches(TextSearch, currentPage, limit);
				CoachCollection = CollectionViewSource.GetDefaultView(pagination.Items);
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
        public List<Coach> CoachList
        {
            get
            {
                return coachList;
            }
            set
            {
                coachList = value;
                OnPropertyChanged(nameof(CoachList));
            }
        }

        public ICommand OpenCoachSeatsCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public CoachViewModel()
        {
            GetViewModel.coachViewModel = this;
            Load();
            OpenCoachSeatsCommand = new ViewModelCommand(ExcuteOpenCoachSeatsCommand, CanExcuteOpenCoachSeatsCommand);
            EditCommand = new ViewModelCommand(ExcuteEditCommand);
            DeleteCommand = new ViewModelCommand(ExcuteDeleteCommand);
            
        }

        private void ExcuteDeleteCommand(object obj)
        {
            DialogResult ret = System.Windows.Forms.MessageBox.Show("Do you want to delete this row?", "Delete row", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (ret == DialogResult.Cancel || ret == DialogResult.No)
            {
                return;
            }

			new RepoCoach().DeleteCoach((obj as ModelCoach).Id);
			Load();
		}

		private void ExcuteEditCommand(object obj)
        {
            var screen = new AddNewCoach((obj as ModelCoach));
            screen.ShowDialog();
        }

        private bool CanExcuteOpenCoachSeatsCommand(object obj)
        {
            if ((SelectedItem as Coach).Status == "Out Of Service")
                return false;
            return true;
        }

        private void ExcuteOpenCoachSeatsCommand(object obj)
        {
            if (obj == null)
                return;
            System.Windows.MessageBox.Show((obj as Coach).Id.ToString());
        }

        private bool check(string data, string text)
        {
            if (!string.IsNullOrEmpty(data))
            {
                if(data.Contains(text))
                    return true;
                return false;
            }
            return false;

        }
        private bool Filter(object data)
        {
            if (!string.IsNullOrEmpty(TextSearch))
            {
                var coachDetail = data as ModelCoach;
                return coachDetail != null
                    && (check(coachDetail.Id.ToString(), TextSearch)
                    || check(coachDetail.Name, TextSearch));
            }
            return true;
           
        }       
        public void Load()
        {
			var coachesPagination = new RepoCoach().GetCoaches("", currentPage, limit);
			CoachCollection = CollectionViewSource.GetDefaultView(coachesPagination.Items);
        }
    }
}
