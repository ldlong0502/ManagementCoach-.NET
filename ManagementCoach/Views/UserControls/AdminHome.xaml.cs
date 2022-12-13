using ManagementCoach.BE;
using ManagementCoach.BE.Repositories;
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

namespace ManagementCoach.Views.UserControls
{
    /// <summary>
    /// Interaction logic for AdminHome.xaml
    /// </summary>
    public partial class AdminHome : UserControl
    {
        public AdminHome()
        {
			//var items = new RepoRoute().GetRoutes(1, 99).Items;
			//Excel.ExportAs(items);

			//var items = new RepoRoute().InsertRoute(new BE.Data.Input.InputRoute()
			//{
			//	DepartTime = 69,
			//	DestinationStationId = 1,
			//	EstimatedTime = 420,
			//	OriginStationId = 1,
			//	Price = 300000,
			//	RouteRestAreaIdList = new List<int>() { 2, 1 }
			//});


			InitializeComponent();
        }
    }
}
