using ManagementCoach.BE.Data.Input;
using ManagementCoach.BE.Entities;
using ManagementCoach.BE.Models;
using ManagementCoach.BE.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ManagementCoach.ViewModels
{
    public class AddCoachViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        private readonly ErrorsViewModel _errorsViewModel;
        private string name;
        private string regNo;
        private string status;
        private string notes;
        private int id;
        private DateTimeOffset dateAdded;


        public Action Close { get; set; }
        public string Name
        {
            get
            {
                _errorsViewModel.ClearErrors(nameof(Name));
                if (String.IsNullOrEmpty(name))
                {
                    _errorsViewModel.AddError(nameof(Name), "Field is required.");
                }
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public string RegNo
        {
            get
            {
                _errorsViewModel.ClearErrors(nameof(RegNo));
                if (String.IsNullOrEmpty(regNo))
                {
                    _errorsViewModel.AddError(nameof(RegNo), "Field is required");
                }
                return regNo;
            }
            set
            {
                regNo = value;
                OnPropertyChanged(nameof(RegNo));
            }
        }
        public string Status
        {
            get
            {
                _errorsViewModel.ClearErrors(nameof(Status));
                if (String.IsNullOrEmpty(status))
                {
                    _errorsViewModel.AddError(nameof(Status), "Field is required");
                }
                return status;
            }
            set
            {
                status = value;
               
                OnPropertyChanged(nameof(Status));
            }
        }
        public string Notes
        {
            get
            {
                return notes;
            }
            set
            {
                notes = value;              
                OnPropertyChanged(nameof(Notes));
            }
        }
        public DateTimeOffset DateAdded
        {
            get
            {
                return dateAdded;
            }
            set
            {
                dateAdded = value;
                OnPropertyChanged(nameof(DateAdded));
            }
        }
        private readonly List<string> listStatus = new List<string>() { "Active", "Out Of Service"};
        public IEnumerable<string> ListStatus
        {
            get { return listStatus; }
        }
        public bool CanCreate => !HasErrors;
        public bool HasErrors => _errorsViewModel.HasErrors;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;


        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public AddCoachViewModel()
        {
            _errorsViewModel = new ErrorsViewModel();
            _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;
            SaveCommand = new ViewModelCommand(ExcuteInsertCommand, CanExcuteSaveCommand);
            CancelCommand = new ViewModelCommand(ExcuteCancelCommand);
            DateAdded = DateTimeOffset.Now;
            Status = ListStatus.First();
            
        }
        public AddCoachViewModel(ModelCoach data)
        {
            _errorsViewModel = new ErrorsViewModel();
            _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;
            SaveCommand = new ViewModelCommand(ExcuteEditCommand, CanExcuteSaveCommand);
            CancelCommand = new ViewModelCommand(ExcuteCancelCommand);
            DateAdded = data.DateAdded;
            Status = data.Status;
            Name = data.Name;
            RegNo = data.RegNo;
            Notes = data.Notes;
            id = data.Id;
        }

        private void ExcuteEditCommand(object obj)
        {
			try
			{
				var coach = new RepoCoach();
				coach.UpdateCoach(
					id,
					new InputCoach()
					{
						Name = Name,
						Status = Status,
						RegNo = RegNo,
						Notes = string.IsNullOrEmpty(Notes) ? "" :Notes ,
					}
				);
				MessageBox.Show("Successfull");
                Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

        private void ExcuteCancelCommand(object obj)
        {
            var window = obj as Window;
            window.Close();
        }

        private bool CanExcuteSaveCommand(object obj)
        {
            if (_errorsViewModel.HasErrors)
                return false;
            return true;
        }

        private void ExcuteInsertCommand(object obj)
        {
            try
            {
                var coach = new RepoCoach();
                 if (coach.InsertCoach(
                    new InputCoach()
                    {
                        Name = Name,
                        Status = Status,
                        RegNo = RegNo,
                        Notes = string.IsNullOrEmpty(Notes) ? "" : Notes,
                    }
                ).Success == true)
                {
                  MessageBox.Show("Successfull");
                }
                else
                {
                   MessageBox.Show("The RegNo has alreaady existed!");
                    return;
                }
                
                Close();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
        }

        private void ErrorsViewModel_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
            OnPropertyChanged(nameof(CanCreate));
        }

        public IEnumerable GetErrors(string propertyName)
        {
            return _errorsViewModel.GetErrors(propertyName);
        }
    }
}
