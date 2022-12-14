using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementCoach.BE.Models
{
	public class ModelDriver
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
		public DateTime DateJoined { get; set; }

		/// <summary>
		/// Mặc định là true
		/// </summary>
		public bool Active { get; set; }
		public string License { get; set; }
		public string Notes { get; set; }

		[Column(TypeName = "timestamptz")]
		public DateTimeOffset DateAdded { get; set; } = DateTimeOffset.Now;

		public string ImageUrl { get; set; }
	}
}
