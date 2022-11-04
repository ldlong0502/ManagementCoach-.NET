using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementCoach.BE.Entities
{
	public class RestArea : Models.RestArea
	{
		public List<RouteRestArea> RouteRestAreas { get; set; }
		public Province Province { get; set; }
	}
}
