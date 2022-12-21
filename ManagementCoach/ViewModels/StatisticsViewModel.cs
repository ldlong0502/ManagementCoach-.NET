using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using ManagementCoach.BE;
using ManagementCoach.BE.Models;
using ManagementCoach.BE.Repositories;
using SkiaSharp;

namespace ManagementCoach.ViewModels
{
	[ObservableObject]
	public class StatisticsViewModel : ViewModelBase
    {
		private DateTime fromDate = DateTime.Today;
		private DateTime toDate = DateTime.Today;
        private List<string> listModeRoutes;
        private string modeRoutes;
        private CoachManContext context = new CoachManContext();
        private List<string> listNameRoute;
        private IEnumerable<ISeries> seriesRoutes;
        public IEnumerable<ISeries> SeriesRoutes 
        {
            get
            {
                return seriesRoutes;
            } 
           set 
            { 
                seriesRoutes = value;
                OnPropertyChanged(nameof(SeriesRoutes)); 
            }
        }
        public List<string> ListNameRoute
        {
            get
            {
                return listNameRoute;
            }
            set
            {
                listNameRoute = value;

                OnPropertyChanged(nameof(ListNameRoute));
            }
        }
        public List<string> ListModeRoutes
        {
            get
            {
                return listModeRoutes;
            }
            set
            {
                listModeRoutes = value;
              
                OnPropertyChanged(nameof(ListModeRoutes));
            }
        }
        public string ModeRoutes
        {
            get
            {
                return modeRoutes;
            }
            set
            {
                modeRoutes = value;
                ChartRoutes();

                OnPropertyChanged(nameof(ModeRoutes));
               
            }
        }
        public DateTime FromDate
		{
			get
			{
				return fromDate;
			}
			set
			{
				fromDate = value;
                ChartSales();
                OnPropertyChanged(nameof(FromDate));
                OnPropertyChanged(nameof(SeriesSales));
                OnPropertyChanged(nameof(XAxes));
            }
		}
		public DateTime ToDate
		{
			get
			{
				return toDate;
			}
			set
			{
				toDate = value;
                ChartSales();
				OnPropertyChanged(nameof(ToDate));
                OnPropertyChanged(nameof(SeriesSales));
                OnPropertyChanged(nameof(XAxes));
            }
		}
		public StatisticsViewModel()
        {
            ListModeRoutes = new List<string>() { "Amount of trips", "Amount of passengers" };
            ModeRoutes = ListModeRoutes.First();
            ChartSales();
            ChartRoutes();
        }
        private void ChartRoutes()
        {
            //Get  3 routes following amount of trips
            if (ModeRoutes == "Amount of trips")
            {
                var listTrips = new RepoTrip().GetTrips(1, context.Trips.Count()).Items;
                var q = from x in listTrips
                        group x.RouteId by x.RouteId into g
                        let count = g.Count()
                        orderby count descending
                        select new { IdRoute = g.Key, Count = count };
                var data = new List<int>();
                ListNameRoute = new List<string>();
                int i = 0;
                q.ToList().ForEach(item =>
                {
                    if (i < 3)
                    {
                        data.Add(item.Count);
                        var Route = new RepoRoute().GetRoute(item.IdRoute);
                        int originId = new RepoStation().GetStation(Route.OriginStationId).ProvinceId;
                        string originName = new RepoProvince().GetProvince(originId).Name;
                        int destinationId = new RepoStation().GetStation(Route.DestinationStationId).ProvinceId;
                        string destinationName = new RepoProvince().GetProvince(destinationId).Name;
                        string temp = originName + " -> " + destinationName;
                        ListNameRoute.Add(temp);
                        i++;
                    }

                });
                i = 0;
                SeriesRoutes = data.AsLiveChartsPieSeries((value, series) =>
                {
                    // here you can configure the series assigned to each value.
                    series.Name = ListNameRoute[i++];
                    series.DataLabelsPaint = new SolidColorPaint(new SKColor(30, 30, 30));
                    series.DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle;
                    series.DataLabelsFormatter = p => $"{p.StackedValue.Share:P2}";
                });
                 
    }
            else if (ModeRoutes == "Amount of passengers")
            {
                var listTrips = new RepoTrip().GetTrips(1, context.Trips.Count()).Items;
                var listTickets = new RepoTicket().GetTickets(1,context.Tickets.Count()).Items;  
                var mergeList = from t in listTrips
                            join p in listTickets
                            on t.Id equals p.TripId
                            select new
                            {
                                PassengerId = p.PassengerId,
                                RouteId = t.RouteId,
                            };
                var q = from x in mergeList
                        group x.PassengerId by x.RouteId into g
                        let count = g.Count()
                        orderby count descending
                        select new { IdRoute = g.Key, Count = count };
                var data = new List<int>();
                ListNameRoute = new List<string>();
                int i = 0;
                q.ToList().ForEach(item =>
                {
                    if (i < 3)
                    {
                        data.Add(item.Count);
                        var Route = new RepoRoute().GetRoute(item.IdRoute);
                        int originId = new RepoStation().GetStation(Route.OriginStationId).ProvinceId;
                        string originName = new RepoProvince().GetProvince(originId).Name;
                        int destinationId = new RepoStation().GetStation(Route.DestinationStationId).ProvinceId;
                        string destinationName = new RepoProvince().GetProvince(destinationId).Name;
                        string temp = originName + " -> " + destinationName;
                        ListNameRoute.Add(temp);
                        i++;
                    }

                });
                i = 0;
                SeriesRoutes = data.AsLiveChartsPieSeries((value, series) =>
                {
                    // here you can configure the series assigned to each value.
                    series.Name = ListNameRoute[i++];
                    series.DataLabelsPaint = new SolidColorPaint(new SKColor(30, 30, 30));
                    series.DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle;
                    series.DataLabelsFormatter = p => $"{p.StackedValue.Share:P2}";
                });

            }
        }

        private DateTime getDate(DateTimeOffset offset)
        {
            return new DateTime(offset.Year, offset.Month, offset.Day);
        }
        public ISeries[] SeriesSales { get; set; }
       
        private List<string> listDay = new List<string>();
        public Axis[] XAxes { get; set; }
        private void ChartSales()
        {
            if (FromDate == null || ToDate == null)
                return;
            if (FromDate.CompareTo(ToDate) > 0) return;
            var values = new List<int>();
            var pastDate = FromDate;
            while (pastDate.CompareTo(ToDate) <= 0)
            {
                int moneyDay = 0;
                listDay.Add(pastDate.Date.ToString("dd/MM/yyyy"));
                var listTicketsDay = new RepoTicket().GetTickets(1, context.Tickets.Count()).Items.Where(c => getDate(c.DateBought).CompareTo(pastDate) == 0).ToList();
                listTicketsDay.ForEach(ticket =>
                {
                    var trip = new RepoTrip().GetTrip(ticket.TripId);
                    var route = new RepoRoute().GetRoute(trip.RouteId);
                    moneyDay += route.Price;
                });
                values.Add(moneyDay);
                pastDate = pastDate.AddDays(1);
            }
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");
            SeriesSales = new ISeries[] {
                new LineSeries<int>
                {
                    Values = values,
                    TooltipLabelFormatter = p => $"{double.Parse(p.PrimaryValue.ToString()).ToString("#,###", cul.NumberFormat) + " VND"}"

                },
                

            };
            XAxes = new Axis[]
                {

                        new Axis
                        {
                            Labels = listDay,
                            SeparatorsPaint = new SolidColorPaint(new SKColor(200, 200, 200)),
                            SeparatorsAtCenter = true,
                            TicksPaint = new SolidColorPaint(new SKColor(35, 35, 35)),
                            TicksAtCenter = true,
                            LabelsAlignment = LiveChartsCore.Drawing.Align.Start,
                            
                        }
                };

        }
    }
}
