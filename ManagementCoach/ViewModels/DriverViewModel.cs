using Aspose.Cells;
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
    public class DriverViewModel : ViewModelBase, ILoadableViewModel
	{
        public CoachManContext context = new CoachManContext();
        private ICollectionView driverCollection;
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
        public ICommand PreviousPageCommand { get; }
        public ICommand NextPageCommand { get; }
        public ICommand UpLimitCommand { get; }
        public ICommand DownLimitCommand { get; }
        public ICommand FirstPageCommand { get; }
        public ICommand EndPageCommand { get; }
        public DriverViewModel()
        {
            Load();
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
            var delAction = new RepoDriver().DeleteDriver((obj as ModelDriver).Id);
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
            var screen = new AddNewDriver(this, (obj as ModelDriver));
            screen.ShowDialog();
        }

        public void ImportDriversFromExcel()
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
                            Name = CheckObjectNull(worksheet.Cells[$"A{i}"].Value),
                            IdCard = CheckObjectNull(worksheet.Cells[$"B{i}"].Value),
                            Gender = CheckObjectNull(worksheet.Cells[$"C{i}"].Value),
                            Dob = DateTime.Parse(worksheet.Cells[$"D{i}"].Value.ToString()),
                            Email = CheckObjectNull(worksheet.Cells[$"E{i}"].Value),
                            Phone = "0" + CheckObjectNull(worksheet.Cells[$"F{i}"].Value),
                            Address = CheckObjectNull(worksheet.Cells[$"G{i}"].Value),
                            DateJoined = DateTime.Parse(worksheet.Cells[$"H{i}"].Value.ToString()),
                            Active = bool.Parse(worksheet.Cells[$"I{i}"].Value.ToString()),
                            License = CheckObjectNull(worksheet.Cells[$"J{i}"].Value),
                            Notes = CheckObjectNull(worksheet.Cells[$"K{i}"].Value),
                            
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
                   

                }
                System.Windows.MessageBox.Show("Add successfully rows: "+ count.ToString());
                Load();

            }
        }
        public void ExportDriverstoExcel()
        {
            DialogResult ret = System.Windows.Forms.MessageBox.Show("Do you want to export drivers to file excel?", "Exports", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (ret == DialogResult.No)
            {
                return;
            }
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.DefaultExt = ".xlsx";
            saveFileDialog.Filter = "Excel Workbook (.xlsx)|*.xlsx";

            if (false == saveFileDialog.ShowDialog()) return;
            string filename = saveFileDialog.FileName;

            Workbook workbook = new Workbook();
            Worksheet worksheet = workbook.Worksheets[0];
            worksheet.Name = "Drivers";

            worksheet.Cells["A1"].PutValue("Id");
            worksheet.Cells["B1"].PutValue("Name");
            worksheet.Cells["C1"].PutValue("IdCard");
            worksheet.Cells["D1"].PutValue("Gender");
            worksheet.Cells["E1"].PutValue("Dob");
            worksheet.Cells["F1"].PutValue("Email");
            worksheet.Cells["G1"].PutValue("Phone");
            worksheet.Cells["H1"].PutValue("Address");
            worksheet.Cells["I1"].PutValue("DateJoined");
            worksheet.Cells["J1"].PutValue("Active");
            worksheet.Cells["K1"].PutValue("License");
            worksheet.Cells["L1"].PutValue("Notes");
            worksheet.Cells["M1"].PutValue("DateAdded");
            var driversPagination = new RepoDriver().GetDrivers("", 1, context.Drivers.Count());
            for (int i = 0; i < driversPagination.Items.Count(); i++)
            {
                ModelDriver model = driversPagination.Items[i];
                worksheet.Cells[$"A{i + 2}"].PutValue(model.Id);
                worksheet.Cells[$"B{i + 2}"].PutValue(model.Name);
                worksheet.Cells[$"C{i + 2}"].PutValue(model.IdCard);
                worksheet.Cells[$"D{i + 2}"].PutValue(model.Gender);
                worksheet.Cells[$"E{i + 2}"].PutValue(model.Dob.ToString("dd-MM-yyyy"));
                worksheet.Cells[$"F{i + 2}"].PutValue(model.Email);
                worksheet.Cells[$"G{i + 2}"].PutValue(model.Phone);
                worksheet.Cells[$"H{i + 2}"].PutValue(model.Address);
                worksheet.Cells[$"I{i + 2}"].PutValue(model.DateJoined.ToString("dd-MM-yyyy"));
                worksheet.Cells[$"J{i + 2}"].PutValue(model.Active.ToString());
                worksheet.Cells[$"K{i + 2}"].PutValue(model.License);
                worksheet.Cells[$"L{i + 2}"].PutValue(model.Notes);
                worksheet.Cells[$"M{i + 2}"].PutValue(model.DateAdded.ToString("dd-MM-yyyy"));
            }
            worksheet.AutoFitColumns();
            workbook.Save(filename);
            System.Windows.Forms.MessageBox.Show("Export successfully", "Exports");
        }
        private string CheckObjectNull(Object obj)
        {
            if (obj == null) return "";
            return obj.ToString();
        }
        public void Load()
        {
            if (context.Drivers.Count() == 0)
            {
                return;
            }
            var coachesPagination = new RepoDriver().GetDrivers(TextSearch, CurrentPage, Limit);
            DriverCollection = CollectionViewSource.GetDefaultView(coachesPagination.Items);
            NumOfPages = coachesPagination.PageCount;

            if (NumOfPages != 0 && CurrentPage > NumOfPages)
            {
                CurrentPage = 1;
                coachesPagination = new RepoDriver().GetDrivers(TextSearch, CurrentPage, Limit);
                DriverCollection = CollectionViewSource.GetDefaultView(coachesPagination.Items);
            }

        }
    }
}
