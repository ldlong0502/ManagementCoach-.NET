﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementCoach.BE.Models
{
	public class ModelTicket
	{
		[Required]
		public int Id { get; set; }
		[Required]
		public int TripId { get; set; }
		[Required]
		public int PassengerId { get; set; }

		[Required]
		public int CoachSeatId { get; set; }

		public int? Price { get; set; }

		[Column(TypeName = "timestamptz")]
		public DateTimeOffset DateBought { get; set; } = DateTimeOffset.Now;
	}
}
