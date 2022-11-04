using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementCoach.BE.Models
{
	public class RouteRestArea
	{
		[Required]
		public int RouteId { get; set; }
		[Required]
		public string RestAreaId { get; set; }
		[Required]
		public int StopOrder { get; set; }
	}
}
