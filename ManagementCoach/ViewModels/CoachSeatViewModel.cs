using ManagementCoach.BE.Models;
using ManagementCoach.BE.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementCoach.ViewModels
{
    public class CoachSeatViewModel : ViewModelBase
    {
        private List<ModelCoachSeat> listSeatDown;
        private List<ModelCoachSeat> listSeatUp;
        private int rows;
        public List<ModelCoachSeat> ListSeatUp
        {
            get
            {
                return listSeatUp;
            }
            set
            {
                listSeatUp = value;
                OnPropertyChanged(nameof(ListSeatUp));
            }
        }
        public List<ModelCoachSeat> ListSeatDown
        {
            get
            {
                return listSeatDown;
            }
            set
            {
                listSeatDown = value;
                OnPropertyChanged(nameof(ListSeatDown));
            }
        }
        public int Rows
        {
            get
            {
                return rows;
            }
            set
            {
                rows = value;
                OnPropertyChanged(nameof(Rows));
            }
        }
        public Action Close { get; set; }
        public CoachSeatViewModel()
        {

        }
        public CoachSeatViewModel(ModelCoach data)
        {
            ListSeatDown = new RepoCoachSeat().GetCoachSeats(data.Id).Where(c => c.Name.StartsWith("A")).ToList();
            ListSeatDown.Sort((a, b) => int.Parse(a.Name.Split('A')[1]).CompareTo(int.Parse(b.Name.Split('A')[1])));
            ListSeatUp = new RepoCoachSeat().GetCoachSeats(data.Id).Where(c => c.Name.StartsWith("B")).ToList();
            ListSeatUp.Sort((a, b) => int.Parse(a.Name.Split('B')[1]).CompareTo(int.Parse(b.Name.Split('B')[1])));
            Rows = ListSeatDown.Count() / 2;
        }
    }
}
