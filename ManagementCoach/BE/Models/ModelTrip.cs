using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementCoach.BE.Models
{
	public class ModelTrip
	{
		[Required]
		public int Id { get; set; }
		[Required]
		public int RouteId { get; set; }
		[Required]
		public int CoachId { get; set; }
		[Required]
		public int DriverId { get; set; }

		[Column(TypeName = "timestamptz")]
		public DateTimeOffset Date { get; set; }

		/// <summary>
		/// Mặc định là false
		/// </summary>
		public bool Cancelled { get; set; }
	}
}
