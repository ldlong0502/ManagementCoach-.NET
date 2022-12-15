using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementCoach.BE.Models
{
	public class ModelCoachSeat
	{
		[Required]
		public int Id { get; set; }
		[Required]
		public int CoachId { get; set; }
		public string Name { get; set; }

	}
}
