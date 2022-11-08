using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementCoach.BE.Models
{
	public class ModelCoach : BaseCoach
	{
		public List<ModelCoachSeat> CoachSeats { get; set; }
	}

	public class BaseCoach
	{
		[Required]
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }

		[Required]
		public string RegNo { get; set; }


		/// <summary>
		/// Mặc định là "Active"
		/// </summary>
		public string Status { get; set; }
		public string Notes { get; set; }
		[Column(TypeName = "timestamptz")]
		public DateTimeOffset DateAdded { get; set; }
	}
}
