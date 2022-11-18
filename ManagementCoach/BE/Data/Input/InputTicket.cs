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
	public class InputTicket
	{
		public int TripId { get; set; }
		public int CustomerId { get; set; }
	}
}
