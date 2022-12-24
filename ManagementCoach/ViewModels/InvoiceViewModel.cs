using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementCoach.ViewModels
{
    public class InvoiceViewModel : ViewModelBase
    {
        private DateTime dateIssued;
        private string invoiceNo;
        private string provinceFrom;
        private string passengerName;
        private string provinceTo;
        private DateTime dateTrip;
        private int departureTime;
        private int total;
        private int numOfSeats;
        public DateTime DateTrip
        {
            get
            {
                return dateTrip;
            }
            set
            {
                dateTrip = value;
                OnPropertyChanged(nameof(DateTrip));
            }
        }
        private List<TempSeat> listSeats;
        public int NumOfSeats
        {
            get
            {
                return numOfSeats;
            }
            set
            {
                numOfSeats = value;
                OnPropertyChanged(nameof(NumOfSeats));
            }
        }
        public string PassengerName
        {
            get
            {
                return passengerName;
            }
            set
            {
                passengerName = value;
                OnPropertyChanged(nameof(PassengerName));
            }
        }
        public List<TempSeat> ListSeats
        {
            get
            {
                return listSeats;
            }
            set
            {
                listSeats = value;
                OnPropertyChanged(nameof(ListSeats));
            }
        }
         public int Total
        {
            get
            {
                return total;
            }
            set
            {
                total = value;
                OnPropertyChanged(nameof(Total));
            }
        }
        public int DepartureTime
        {
            get
            {
                return departureTime;
            }
            set
            {
                departureTime = value;
                OnPropertyChanged(nameof(DepartureTime));
            }
        }
        public string ProvinceTo
        {
            get
            {
                return provinceTo;
            }
            set
            {
                provinceTo = value;
                OnPropertyChanged(nameof(ProvinceTo));
            }
        }
        public string ProvinceFrom
        {
            get
            {
                return provinceFrom;
            }
            set
            {
                provinceFrom = value;
                OnPropertyChanged(nameof(provinceFrom));
            }
        }
        public string InvoiceNo
        {
            get
            {
                return invoiceNo;
            }
            set
            {
                invoiceNo = value;
                OnPropertyChanged(nameof(InvoiceNo));
            }
        }
        public DateTime DateIssued
        {
            get
            {
                return dateIssued;
            }
            set
            {
                dateIssued = value;
                OnPropertyChanged(nameof(DateIssued));
            }
        }

        public InvoiceViewModel()
        {

        }
        public InvoiceViewModel(Invoice invoice)
        {
            DateIssued = invoice.DateIssued;
            InvoiceNo = invoice.Id;
            ListSeats = invoice.ListSeatDetail;
            Total = invoice.Total;
            PassengerName = invoice.PassengerName;
            ProvinceFrom = invoice.FromProvince;
            ProvinceTo = invoice.ToProvince;
            DateTrip = invoice.DateTrip;
            DepartureTime = invoice.DepartureTime;
            NumOfSeats = ListSeats.Count();


        }
    }
}
