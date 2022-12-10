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
using System.Collections;
using Aspose.Cells;

namespace ManagementCoach.ViewModels
{
    public class CoachViewModel : ViewModelBase
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
            DeleteCommand = new ViewModelCommand(ExcuteDeleteCommand);
            NextPageCommand = new ViewModelCommand(ExcuteNextPageCommand, CanExcuteNextPageCommand);
            PreviousPageCommand = new ViewModelCommand(ExcutePreviousPageCommand, CanExcutePreviousPageCommand);
            UpLimitCommand = new ViewModelCommand(ExcuteUpLimitCommand, CanExcuteUpLimitCommand);
            DownLimitCommand = new ViewModelCommand(ExcuteDownLimitCommand, CanExcuteDownLimitCommand);
            FirstPageCommand = new ViewModelCommand(ExcuteFirstPageCommand, CanExcuteFirstPageCommand);
            EndPageCommand = new ViewModelCommand(ExcuteEndPageCommand, CanExcuteEndPageCommand);
        }
        public void ImportCoachesFromExcel()
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.DefaultExt = ".xlsx";
            openFileDialog.Filter = "Excel Workbook (.xlsx)|*.xlsx";



            if (false == openFileDialog.ShowDialog()) return;
            string filename = openFileDialog.FileName;

            Workbook workbook = new Workbook(filename);
            Worksheet worksheet = workbook.Worksheets[0];
            if (worksheet.Name == "Drivers" || worksheet.Name == "Driver")
            {
                int i = 2;
                int count = 0;
                while (worksheet.Cells[$"E{i}"].Value != null)
                {
                    if (new RepoDriver().EmailExists(worksheet.Cells[$"E{i}"].Value.ToString()))
                    {
                        i++;
                        continue;
                    }
                    try
                    {
                        var add = new RepoDriver().InsertDriver(new BE.Data.Input.InputDriver()
                        {
                            Name = worksheet.Cells[$"A{i}"].Value.ToString(),
                            IdCard = worksheet.Cells[$"B{i}"].Value.ToString(),
                            Gender = worksheet.Cells[$"C{i}"].Value.ToString(),
                            Dob = DateTime.Parse(worksheet.Cells[$"D{i}"].Value.ToString()),
                            Email = worksheet.Cells[$"E{i}"].Value.ToString(),
                            Phone = "0" + worksheet.Cells[$"F{i}"].Value.ToString(),
                            Address = worksheet.Cells[$"G{i}"].Value.ToString(),
                            DateJoined = DateTime.Parse(worksheet.Cells[$"H{i}"].Value.ToString()),
                            Active = bool.Parse(worksheet.Cells[$"I{i}"].Value.ToString()),
                            License = worksheet.Cells[$"J{i}"].Value.ToString(),
                            Notes = worksheet.Cells[$"K{i}"].Value.ToString(),
                        });
                        if (add.Success)
                        {
                            count++;
                        }
                        i++;
                    }
                    catch
                    {
                        i++;
                    }
                    System.Windows.MessageBox.Show("Add successfully {0} rows", count.ToString());
                    Load();
                    
                }
            
            }
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

			new RepoCoach().DeleteCoach((obj as ModelCoach).Id);
			Load();
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
            System.Windows.MessageBox.Show((obj as ModelCoach).Id.ToString());
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
