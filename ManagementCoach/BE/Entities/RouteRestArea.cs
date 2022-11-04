using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementCoach.BE.Entities
{
	public class RouteRestArea : Models.RouteRestArea
	{
		public Route Route { get; set; }
		public RestArea	RestArea { get; set; }
	}
}
