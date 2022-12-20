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
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[Required]
		public int RouteId { get; set; }
		[Required]
		public int CoachId { get; set; }
		[Required]
		public int DriverId { get; set; }

		/// <summary>
		/// Ngày khởi hành
		/// </summary>
		[Column(TypeName = "date")]
		public DateTime Date { get; set; }

		/// <summary>
		/// Mặc định là false
		/// </summary>
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
