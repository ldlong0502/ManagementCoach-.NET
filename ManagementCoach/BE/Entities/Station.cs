using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementCoach.BE.Entities
{
	public class Station : Models.ModelStation
	{
		public List<Route> Routes { get; set; }
		public Province Province { get; set; }
	}
}
