using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementCoach.BE.Entities
{
	public class Ticket : Models.ModelTicket
	{
		public Trip Trip { get; set; }
		public Passenger Passenger { get; set; }
	}
}
