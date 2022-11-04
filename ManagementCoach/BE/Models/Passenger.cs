using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementCoach.BE.Models
{
	public class Passenger
	{
		[Required]
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		public string IdCard { get; set; }
		public string Gender { get; set; }
		public DateTime Dob { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public string Address { get; set; }
		
		/// <summary>
		/// Mặc định là false
		/// </summary>
		public bool Blocked { get; set; }
		public string Notes { get; set; }

		[Column(TypeName = "timestamp with time zone")]
		public DateTime DateAdded { get; set; }

	}
}
