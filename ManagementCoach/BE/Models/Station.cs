using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementCoach.BE.Models
{
	public class Station
	{
		[Required]
		public int Id { get; set; }
		[Required]
		public string District { get; set; }
		[Required]
		public string Address { get; set; }
		[Required]
		public int ProvinceId { get; set; }
		
	}
}
