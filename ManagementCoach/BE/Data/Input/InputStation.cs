using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementCoach.BE.Data.Input
{
	public class InputStation
	{
		public string Name { get; set; }
		public string District { get; set; }
		public string Address { get; set; }
		public int ProvinceId { get; set; }
	}
}
