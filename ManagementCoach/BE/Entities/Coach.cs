using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementCoach.BE.Entities
{
	public class Coach : Models.BaseCoach
	{
		public List<CoachSeat> CoachSeats { get; set; }
		public List<Trip> Trips { get; set; }
	}
}
