using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementCoach.BE.Models
{
	public class ModelTripCoachSeat : ModelCoachSeat
	{
		public int TripId { get; set; }
		/// <summary>
		/// Ghế này đã có người ngồi chưa
		/// </summary>
		public bool Taken { get; set; }
	}
}
