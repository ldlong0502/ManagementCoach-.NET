using OfficeOpenXml.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementCoach.BE.Entities
{
	public class Route : Models.BaseRoute
	{
		[EpplusIgnore]
		public List<RouteRestArea> RouteRestAreas { get; set; }
		[EpplusIgnore]
		public Station OriginStation { get; set; }
		[EpplusIgnore]
		public Station DestinationStation { get; set; }
		[EpplusIgnore]
		public Trip Trip { get; set; }
	}
}
