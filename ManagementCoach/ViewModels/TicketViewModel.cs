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
    public class TicketViewModel : ViewModelBase, ILoadableViewModel
	{

        public CoachManContext context = new CoachManContext();
        private ICollectionView ticketCollection;
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
        public ICollectionView TicketCollection
        {
            get
            {
                return ticketCollection;
            }
            set
            {
                ticketCollection = value;
                OnPropertyChanged(nameof(TicketCollection));
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
        public TicketViewModel()
        {
            Load();
            EditCommand = new ViewModelCommand(ExcuteEditCommand);
            DeleteCommand = new ViewModelCommand(ExcuteDeleteCommand);
            NextPageCommand = new ViewModelCommand(ExcuteNextPageCommand, CanExcuteNextPageCommand);
            PreviousPageCommand = new ViewModelCommand(ExcutePreviousPageCommand, CanExcutePreviousPageCommand);
            UpLimitCommand = new ViewModelCommand(ExcuteUpLimitCommand, CanExcuteUpLimitCommand);
            DownLimitCommand = new ViewModelCommand(ExcuteDownLimitCommand, CanExcuteDownLimitCommand);
            FirstPageCommand = new ViewModelCommand(ExcuteFirstPageCommand, CanExcuteFirstPageCommand);
            EndPageCommand = new ViewModelCommand(ExcuteEndPageCommand, CanExcuteEndPageCommand);
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
            var delAction = new RepoTicket().DeleteTicket((obj as ModelTicket).Id);
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
            var objTicket = new RepoTicket().GetTicket((obj as MergeTicketsAndPassengers).Id);
            var screen = new AddNewTicket(this, objTicket);
            screen.ShowDialog();
        }



        public void Load()
        {
            if (context.Tickets.Count() == 0)
            {
                return;
            }
            var ticketsPagination = new RepoTicket().GetTickets(CurrentPage, Limit);
            var listPassengers = new RepoPassenger().GetPassengers("", 1, context.Passengers.Count());
            var mergeList = from t in ticketsPagination.Items
                            join lp in listPassengers.Items
                            on t.PassengerId equals lp.Id
                            select new MergeTicketsAndPassengers
                            {
                                Id = t.Id,
                                TripId = t.TripId,
                                PassengerId = t.PassengerId,
                                PassengerName = lp.Name,
                                DateBought = t.DateBought
                            };
            TicketCollection = CollectionViewSource.GetDefaultView(mergeList.ToList());
            NumOfPages = ticketsPagination.PageCount;

            if (NumOfPages != 0 && CurrentPage > NumOfPages)
            {
                CurrentPage = 1;
                ticketsPagination = new RepoTicket().GetTickets(CurrentPage, Limit);
                listPassengers = new RepoPassenger().GetPassengers("", 1, context.Passengers.Count());
                mergeList = from t in ticketsPagination.Items
                                join lp in listPassengers.Items
                                on t.PassengerId equals lp.Id
                                select new MergeTicketsAndPassengers
                                {
                                    Id = t.Id,
                                    TripId = t.TripId,
                                    PassengerId = t.PassengerId,
                                    PassengerName = lp.Name,
                                    DateBought = t.DateBought
                                };
                TicketCollection = CollectionViewSource.GetDefaultView(mergeList.ToList());
            }
        }

    }

    public class MergeTicketsAndPassengers
    {
        public int Id { get; set; }
        public int TripId { get; set; }
        public int PassengerId { get; set; }
        public string PassengerName { get; set; }
        public DateTimeOffset DateBought { get; set; }
    }
}
