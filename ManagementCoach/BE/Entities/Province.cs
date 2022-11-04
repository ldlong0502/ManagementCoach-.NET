using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementCoach.BE.Entities
{
	internal class Province : Models.Province
	{
		public RestArea RestArea { get; set; }
		public List<Station> Stations { get; set; }
	}
}
