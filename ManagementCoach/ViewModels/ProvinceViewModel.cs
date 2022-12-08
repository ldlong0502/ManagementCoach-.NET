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
using System.Windows;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;

namespace ManagementCoach.ViewModels
{
    public class ProvinceViewModel : ViewModelBase
    {
        public CoachManContext context = new CoachManContext();
        private object selectedItem;
        private string textSearch = "";
        private string name;
        private bool visibleEdit = false;
        private ICollectionView provinceCollection; 
        public Action Close { get; set; }
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
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            
            }
        }
        public bool VisibleEdit
        {
            get
            {
                return visibleEdit;
            }
            set
            {
                visibleEdit = value;
                OnPropertyChanged(nameof(VisibleEdit));

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
        public ICollectionView ProvinceCollection
        {
            get
            {
                return provinceCollection;
            }
            set
            {
                provinceCollection = value;
                OnPropertyChanged(nameof(ProvinceCollection));
            }
        }


        //ICommand
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand CloseCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand CloseEditCommand { get; }
        public ProvinceViewModel()
        {
            Load();
            EditCommand = new ViewModelCommand(ExcuteEditCommand);
            DeleteCommand = new ViewModelCommand(ExcuteDeleteCommand);
            AddCommand = new ViewModelCommand(ExcuteAddCommand);
            CloseCommand = new ViewModelCommand(ExcuteCloseCommand);
            CloseEditCommand = new ViewModelCommand(ExcuteCloseEditCommand);
            SaveCommand = new ViewModelCommand(ExcuteSaveCommand);
        }

        private void ExcuteSaveCommand(object obj)
        {
            if (SelectedItem as ModelProvince == null)
            {
                var insert =  new RepoProvince().InsertProvince(Name);
                if(insert.Success == true)
                {
                    System.Windows.MessageBox.Show("Insert Province Successfully");
                }
                else
                {
                    System.Windows.MessageBox.Show(insert.ErrorMessage);
                    return;
                }
            }
            else
            {
                
                var update = new RepoProvince().UpdateProvince((SelectedItem as ModelProvince).Id, Name);
                if (update.Success == true)
                {
                    System.Windows.MessageBox.Show("Update Province Successfully");
                }
                else
                {
                    System.Windows.MessageBox.Show(update.ErrorMessage);
                    return;
                }
            }
            ExcuteCloseEditCommand(obj);
            Load();
        }

        private void ExcuteCloseEditCommand(object obj)
        {
            VisibleEdit = false;
            Name = "";
        }

        private void ExcuteCloseCommand(object obj)
        {
            Close();
        }

        private void ExcuteAddCommand(object obj)
        {
            SelectedItem = null;
            ExcuteCloseEditCommand(obj);
            VisibleEdit = true;
        }

        private void ExcuteDeleteCommand(object obj)
        {
            DialogResult ret = System.Windows.Forms.MessageBox.Show("Do you want to delete this row?", "Delete row", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (ret == DialogResult.Cancel || ret == DialogResult.No)
            {
                return;
            }
            var delAction = new RepoProvince().DeleteProvince((obj as ModelProvince).Id);
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
            ExcuteCloseEditCommand(obj);
            VisibleEdit = true;
            Name = (SelectedItem as ModelProvince).Name;

        }



        public void Load()
        {
            if (context.Provinces.Count() == 0)
            {
                return;
            }
            var provincesPagination = new RepoProvince().GetProvinces(TextSearch);
            ProvinceCollection = CollectionViewSource.GetDefaultView(provincesPagination.ToList());
        }
    }
}
