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
	public class InputCoach
	{
		public string Name { get; set; }
		public string RegNo { get; set; }
		public string Status { get; set; }
		public string Notes { get; set; }
	}
}
