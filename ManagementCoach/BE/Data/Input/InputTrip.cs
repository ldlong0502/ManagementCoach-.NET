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

		/// <summary>
		/// Ngày khởi hành
		/// </summary>
		public DateTime Date { get; set; }
		public bool Cancelled { get; set; }

		/// <summary>
		/// Giờ khởi hành, đơn bị là phút, VD: 7h30 --> 450, 15h --> 900
		/// </summary>
		public int DepartTime { get; set; }


		/// <summary>
		/// Đơn vị là phút
		/// </summary>
		public int EstimatedTime { get; set; }
	}
}
