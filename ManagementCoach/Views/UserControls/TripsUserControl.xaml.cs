using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace ManagementCoach.Views.UserControls
{
    /// <summary>
    /// Interaction logic for TripsUserControl.xaml
    /// </summary>
    public partial class TripsUserControl : UserControl
    {
        ObservableCollection<Trips> listTrips = new ObservableCollection<Trips>();
        public TripsUserControl()
        {
            InitializeComponent();
            var converter = new BrushConverter();

            listTrips = GetTrips();            
            TripsDataGrid.ItemsSource = listTrips;
        }
        //create data for trips   
        ObservableCollection<Trips> GetTrips()
        {
            ObservableCollection<Trips> trips = new ObservableCollection<Trips>();
            for (int i = 1; i < 10; i++)
            {
                trips.Add(new Trips
                {
                    IdTrip = i.ToString(),
                    Name = "sfagg" + i.ToString(),
                    Status = "schelduled",
                    DepartureDate = DateTime.Now.ToString("yyyy-MM-dd"),
                    DepartureTime = DateTime.Now.ToString("h:mm:ss tt"),
                    ETA = "0:00",
                    Fare = "500$",
                    Booking = "0|40",
                    Amount = "0|20000$",

                });
            }
            return trips;
        }
        bool SearchData(Trips trips , string data)
        {
            if (trips.IdTrip.Contains(data) || 
                trips.Status.Contains(data) ||  
                trips.DepartureDate.Contains(data) ||
                trips.DepartureTime.Contains(data) ||
                trips.Amount.Contains(data) ||
                trips.Booking.Contains(data) ||
                trips.ETA.Contains(data) ||
                trips.Fare.Contains(data) ||
                trips.Name.Contains(data))
            {
                return true;
            }
            return false;
        }
        private void Search_Event(object sender, TextChangedEventArgs e)
        {
            try
            {
                listTrips.Clear();
                if (txtSearch.Text.Length == 0)
                {
                    listTrips = GetTrips();
                    TripsDataGrid.ItemsSource = listTrips;
                    return;
                }
                foreach (Trips tr in GetTrips())
                {
                    if (SearchData(tr, txtSearch.Text))
                    {
                        listTrips.Add(tr);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            
           
        }
    }
    public class Trips
    {
        public string IdTrip { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string DepartureDate { get; set; }
        public string DepartureTime { get; set; }
        public string ETA { get; set; }
        public string Fare { get; set; }
        public string Booking { get; set; }
        public string Amount { get; set; }
    }

}
