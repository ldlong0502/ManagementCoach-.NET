using OfficeOpenXml.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementCoach.BE.Entities
{
	public class RouteRestArea : Models.ModelRouteRestArea
	{
		[EpplusIgnore]
		public Route Route { get; set; }
		[EpplusIgnore]
		public RestArea	RestArea { get; set; }
	}
}
