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

namespace ManagementCoach.ViewModels
{
    public class CoachViewModel : ViewModelBase 
    {
        public CoachManContext context = new CoachManContext();
        private List<Coach> coachList;
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

        public CoachViewModel()
        {
            GetViewModel.coachViewModel = this;
            CoachList = context.Coaches.ToList();
        }

        public void Edit(Coach data)
        {
            var screen = new AddNewCoach(data);
            screen.ShowDialog();
        }
        public void Delete(Coach data)
        {
            DialogResult ret = System.Windows.Forms.MessageBox.Show("Do you want to delete this row?", "Delete row", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(ret == DialogResult.Cancel)
            {
                return;
            }
        }
        public void Load()
        {
            CoachList = context.Coaches.ToList();
        }
    }
}
