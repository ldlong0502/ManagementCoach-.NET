using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementCoach.BE.Models
{
	public class ModelUser
	{
		[Required]
		public int Id { get; set; }
		[Required]
		public string Username { get; set; }
		[Required]
		public string Password { get; set; }
		
		/// <summary>
		/// Là "Employee" hoặc "Admin"
		/// </summary>
		public string Role { get; set; }
		
		public string Name { get; set; }
		public string Email { get; set; }

		[Column(TypeName = "timestamptz")]
		public DateTimeOffset DateAdded { get; set; } = DateTimeOffset.Now;
	}
}
