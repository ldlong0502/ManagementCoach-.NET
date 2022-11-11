using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementCoach.ViewModels
{
    public class AddDriverViewModel: ViewModelBase
    {
        private readonly ErrorsViewModel _errorsViewModel;
        private int id;
		private string name;
		private string idcard;
		private string gender;
		private DateTime dob;
		private string email;
		private string phone;
		private string address;
		private DateTime datejoined;
		private bool active;
		private string license;
		private string notes;
		private DateTimeOffset dateadded;


        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
                _errorsViewModel.ClearErrors(nameof(Id));
                if (String.IsNullOrEmpty(Id.ToString()))
                {
                    _errorsViewModel.AddError(nameof(Id), "*Invalid Id.");
                }
                OnPropertyChanged(nameof(Id));
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
                _errorsViewModel.ClearErrors(nameof(Name));
                if (String.IsNullOrEmpty(Name))
                {
                    _errorsViewModel.AddError(nameof(Name), "*Invalid Name.");
                }
                OnPropertyChanged(nameof(Name));
            }
        }
        public string Idcard
        {
            get
            {
                return idcard;
            }
            set
            {
                idcard = value;
                _errorsViewModel.ClearErrors(nameof(Idcard));
                if (String.IsNullOrEmpty(Idcard) || Idcard.Length < 9)
                {
                    _errorsViewModel.AddError(nameof(Idcard), "*Invalid Idcard , Idcard has more 9 characters");
                }
                OnPropertyChanged(nameof(Idcard));
            }
        }
        public string Gender
        {
            get
            {
                return gender;
            }
            set
            {
                gender = value;
                OnPropertyChanged(nameof(Gender));
            }
        }
        public DateTime Dob
        {
            get
            {
                return dob;
            }
            set
            {
                dob = value;                
                OnPropertyChanged(nameof(Dob));
            }
        }
        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                email = value;
                OnPropertyChanged(nameof(Email));
            }
        }
        public string Phone
        {
            get
            {
                return phone;
            }
            set
            {
                phone = value;
                OnPropertyChanged(nameof(Phone));
            }
        }
        public string Address
        {
            get
            {
                return address;
            }
            set
            {
                address = value;
                OnPropertyChanged(nameof(Address));
            }
        }
        public string License
        {
            get
            {
                return license;
            }
            set
            {
                license = value;
                OnPropertyChanged(nameof(License));
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
        public bool Active
        {
            get
            {
                return active;
            }
            set
            {
                active = value;
                OnPropertyChanged(nameof(Active));
            }
        }
        public DateTime Datejoined
        {
            get
            {
                return datejoined;
            }
            set
            {
                datejoined = value;
                OnPropertyChanged(nameof(Datejoined));
            }
        }
        public DateTimeOffset Dateadded
        {
            get
            {
                return dateadded;
            }
            set
            {
                dateadded = value;
                OnPropertyChanged(nameof(Dateadded));
            }
        }
        public AddDriverViewModel()
        {

		
		}
    }
}
