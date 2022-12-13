using OfficeOpenXml.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementCoach.BE.Entities
{
	public class CoachSeat : Models.ModelCoachSeat
	{
		[EpplusIgnore]
		public Coach Coach { get; set; }
	}
}
