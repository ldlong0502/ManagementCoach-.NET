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
	public class Trip : Models.ModelTrip
	{
		[EpplusIgnore]
		[Required] 
		public Route Route { get; set; }
		[EpplusIgnore]
		[Required] 
		public Coach Coach { get; set; }
		[EpplusIgnore]
		[Required] 
		public Driver Driver { get; set; }
		[EpplusIgnore]
		public List<Ticket> Tickets { get; set; }
	}
}
