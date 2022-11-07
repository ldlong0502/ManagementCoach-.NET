using ManagementCoach.BE;
using ManagementCoach.BE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ManagementCoach.ViewModels
{
    public class TripViewModel : ViewModelBase
    {
        public CoachManContext context = new CoachManContext();
        private List<Coach> tripList;
        public List<Coach> TripList
        {
            get
            {
                return tripList;
            }
            set
            {
                tripList = value;
                OnPropertyChanged(nameof(TripList));
            }
        }
        public TripViewModel()
        {
            TripList = context.Coaches.ToList();
        }
    }
}
