using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementCoach.BE.Entities
{
	public class Trip : Models.Trip
	{
		public Route Route { get; set; }
		public Coach Coach { get; set; }
		public Driver Driver { get; set; }
		public List<Ticket> Tickets { get; set; }
	}
}
