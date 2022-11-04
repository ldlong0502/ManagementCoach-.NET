using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementCoach.BE.Entities
{
	internal class Ticket : Models.Ticket
	{
		public Trip Trip { get; set; }
		public Passenger Passenger { get; set; }
	}
}
