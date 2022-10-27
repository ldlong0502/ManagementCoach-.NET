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

namespace ManagementCoach.FE.UserControls
{
    /// <summary>
    /// Interaction logic for TripsUserControl.xaml
    /// </summary>
    public partial class TripsUserControl : UserControl
    {
        public TripsUserControl()
        {
            InitializeComponent();
            var converter = new BrushConverter();
            ObservableCollection<Trips> trips = new ObservableCollection<Trips>();
            //create data for trips

            for(int i = 1; i < 10; i++)
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
                    BgColor = (Brush)converter.ConvertFromString("#1098ad"),

                });
            }
            TripsDataGrid.ItemsSource = trips;
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
        public Brush BgColor { get; set; }
    }

}
