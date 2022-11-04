using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementCoach.BE.Models
{
	public class Ticket
	{
		[Required]
		public int Id { get; set; }
		[Required]
		public int TripId { get; set; }
		[Required]
		public int CustomerId { get; set; }
		
		[Column(TypeName = "timestamp with time zone")]
		public DateTime DateBought { get; set; }
	}
}
