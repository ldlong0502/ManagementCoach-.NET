using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementCoach.BE.Entities
{
	internal class Route : Models.Route
	{
		public List<RouteRestArea> RouteRestAreas { get; set; }
		public Station OriginStation { get; set; }
		public Station DestinationStation { get; set; }
		public Trip Trip { get; set; }
	}
}
