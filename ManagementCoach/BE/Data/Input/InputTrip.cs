using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ManagementCoach.BE.Data.Input
{
	public class InputTrip
	{
		public int RouteId { get; set; }
		public int CoachId { get; set; }
		public int DriverId { get; set; }
		public DateTimeOffset Date { get; set; } = DateTimeOffset.Now;
		public bool Cancelled { get; set; }
	}
}
