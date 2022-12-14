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
	public class InputDriver
	{
		public string Name { get; set; }
		public string IdCard { get; set; }
		public string Gender { get; set; }
		public DateTime Dob { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public string Address { get; set; }
		public DateTime DateJoined { get; set; }
		public bool Active { get; set; }
		public string License { get; set; }
		public string Notes { get; set; }
		public string ImageUrl { get; set; }
	}
}
