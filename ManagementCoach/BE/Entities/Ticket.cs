using OfficeOpenXml.Attributes;
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
		[EpplusIgnore]
		public Trip Trip { get; set; }
		[EpplusIgnore]
		public Passenger Passenger { get; set; }
	}
}
