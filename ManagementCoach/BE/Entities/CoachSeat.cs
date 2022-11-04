using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementCoach.BE.Entities
{
	internal class CoachSeat : Models.CoachSeat
	{
		public Coach Coach { get; set; }
	}
}
