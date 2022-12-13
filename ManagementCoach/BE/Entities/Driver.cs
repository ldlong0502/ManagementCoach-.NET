using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml.Attributes;

namespace ManagementCoach.BE.Entities
{
	public class Driver : Models.ModelDriver
	{
		[EpplusIgnore]
		public List<Trip> Trips { get; set; }
	}
}
