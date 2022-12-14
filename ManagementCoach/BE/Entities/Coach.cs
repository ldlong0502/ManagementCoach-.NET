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
	public class Coach : Models.BaseCoach
	{
		[EpplusIgnore]
		public List<CoachSeat> CoachSeats { get; set; }
		[EpplusIgnore]
		public List<Trip> Trips { get; set; }
	}
}
